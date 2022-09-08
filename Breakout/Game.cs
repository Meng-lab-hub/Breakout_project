using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Game
    {
        public static Game Instance { get; } = new Game();
        // for having only one instance of the Game class
        private Game() { }

        Stopwatch gameTime;
        TimeSpan lastUpdate;
        TimeSpan stepInterval = TimeSpan.FromMilliseconds(10);
        // last time will start at 0 if nothing is assigned
        TimeSpan lastSend;
        TimeSpan sendInterval = TimeSpan.FromMilliseconds(500);

        public Vector2 MapSize { get; private set; } = new Vector2(1132, 800);
        List<GameObject> Objects = new List<GameObject>();
        
        // for storing the ball
        Ball ball;
        Bat bat;
        /*
        // Header genious collection
        List<Brick> Bricks = new List<Brick>();
        List<Ball> Balls = new List<Ball>();
        List<Bat> Bats = new List<Bat>();
        
        Deleted while adding list<GameObject>
        */

        public void Start()
        {

            Objects.Add(new Wall { P1 = new Vector2(0, MapSize.Y), P2 = new Vector2() });
            Objects.Add(new Wall
            {
                P1 = new Vector2(MapSize.X, MapSize.Y),
                P2 = new Vector2(0, MapSize.Y)
            });
            Objects.Add(new Wall
            {
                P1 = new Vector2(MapSize.X, 0),
                P2 = new Vector2(MapSize.X, MapSize.Y)
            });

            // Add ball here
            var gap = new Vector2(12, 12);
            var size = new Vector2(100, 30);
            var pos = new Vector2(size.X * 0.5f + gap.X, MapSize.Y - size.Y * 0.5f - gap.Y);
            for (int i = 0; i < 100; i++)
            {
                Objects.Add(new SimpleBrick { Id = i, Position = pos, Size = size, CornerRadius = 10 }); ;
                pos.X += size.X + gap.X;
                if (pos.X + size.X * 0.5f + gap.X > MapSize.X)
                {
                    pos.X = size.X * 0.5f + gap.X;
                    pos.Y -= size.Y + gap.Y;
                }
            }

            Objects.Add(ball = new Ball 
            { 
                Position = new Vector2(MapSize.X * 0.5f, 10), 
                Radius = 5, 
                Velocity = new Vector2(0, 225) 
            });

            gameTime = Stopwatch.StartNew();

            Network.Instance.Start();
        }

        void SendState()
        {
            var state = new GameState { Ball = ball, Bat = bat, Bricks = new int[100], Name = "Ma_meng", Score  = Score};
            foreach (var obj in Objects)
                if (obj is Brick brick)
                    state.Bricks[brick.Id] = brick.PackState();
            Network.Instance.Send(state);
        }


        // this method allow us to add anything to the game
        public void Add(GameObject obj)
        {

            Objects.Add(obj);
            if (bat == null && obj is Bat b)
                bat = b;

        }


        void ProcessState(GameState state, Session session)
        {
            session.Name = state.Name;
            session.Score = state.Score;

            if (session.Ball == null)
            {
                var rng = new Random();
                var r = rng.Next(128);
                var g = rng.Next(128);
                session.Brush = new SolidBrush(Color.FromArgb(255, r, g, Math.Max(0, 128 - r - g)));
                session.Ball = new Ball { Owner = session };
                session.Bat = new Bat { Owner = session };
                Objects.Add(session.Ball);
                Objects.Add(session.Bat);
            }
            session.Ball.Radius = state.Ball.Radius;
            session.Ball.Position = state.Ball.Position;
            session.Ball.Velocity = state.Ball.Velocity;
            session.Bat.X = state.Bat.X;
            session.Bat.Speed = state.Bat.Speed;
            session.Bat.Width = state.Bat.Width;


            foreach (var obj in Objects)
                if (obj is Brick brick)
                {
                    brick.UpdateFromNetwork(state.Bricks[brick.Id]);
                }
        }


        public void Update()
        {

            GameState state;
            while ((state = Network.Instance.Receive(out var session)) != null)
            {
                ProcessState(state, session);
            }

            while (lastUpdate + stepInterval <= gameTime.Elapsed)
            {
                if (GameOver)
                {
                    return;
                }

                lastUpdate += stepInterval;
                var dT = (float)stepInterval.TotalSeconds;
                foreach (var obj in Objects)
                    if (!obj.Deleted && obj.Update(dT))
                        foreach (var other in Objects)
                            if (other != obj && !other.Deleted && !obj.Deleted)
                                obj.DetectAndResolveCollision(other);
                // remove all deleted here
                Objects.RemoveAll(x => x.Deleted);
                if (ball.Position.Y < -60)
                {
                    GameOver = true;
                }

                if (lastSend + sendInterval <= gameTime.Elapsed)
                {
                    lastSend = gameTime.Elapsed;
                    SendState();
                }

            }
        }
        public void Draw(Graphics g)
        {
            foreach (var item in Objects)
            { 
                item.Draw(g);
            }
        }

        public event Action ScoreChanged;
        private int score;
        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    ScoreChanged?.Invoke();
                }
            }
        }

        public event Action GameOverChanged;
        private bool gameOver;
        public bool GameOver
        {
            get => gameOver;
            set
            {
                if (gameOver != value)
                {
                    gameOver = value;
                    GameOverChanged?.Invoke();
                }
            }
        }

        /*
        public void DestroyBrick(Brick brick)
        {
            Bricks.Remove(brick);
        }

        Deleted while adding List<GameObject>
        */
    }

}

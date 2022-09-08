using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Ball : GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Radius { get; set; }
        public override bool Update(float dT)
        {
            Position += Velocity * dT;
            return true;
        }
        public override void Draw(Graphics g)
        {

            // Draw a ball on the canves, a ball will just move upward  
            g.FillEllipse(Owner?.Brush ?? Brushes.White, Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2);
        }


        // A detect and resolve collision is implemented here or can be on the brick, bat
        public override bool DetectAndResolveCollision(GameObject other)
        {
            float d;
            Vector2 n;
            bool cd;
            switch (other)
            {
                case Brick brick:
                    cd = DetectCollision(brick, out d, out n);
                    break;
                case Bat bat:
                    if (bat.Owner == Owner)
                        cd = DetectCollision(bat, out d, out n);
                    else
                        return false;
                    cd = DetectCollision(bat, out d, out n);
                    break;
                case Ball ball: 
                    return false;
                case Wall wall:
                    cd = DetectCollision(wall, out d, out n);
                    break;
                default:
                    throw new NotSupportedException();
            }
            if (cd)
            {
                Position += n * d;
                var dot = Vector2.Dot(Velocity, n);
                if (dot < 0)
                    Velocity -= n * 2 * dot; // reflect velocity
                other.OnHit();
                if (Owner == null)
                    other.OnHit();

            }
            return cd;
        }

        // Detect collision with brick
        bool DetectCollision(Brick brick, out float d, out Vector2 n)
        {
            var increasedHalfSizeByRadius = new Vector2(brick.Size.X * 0.5f + Radius, brick.Size.Y *
            0.5f + Radius);
            return Collision.PointVsRoundedRectangle(Position, brick.Position,
            increasedHalfSizeByRadius, brick.CornerRadius + Radius, out d, out n);
        }

        // Detect collision with the bat
        bool DetectCollision(Bat bat, out float d, out Vector2 n)
        {
            var cR = 40;
            return Collision.PointVsRoundedRectangle(Position, new Vector2(bat.X, -cR),
            new Vector2((bat.Width + cR) * 0.5f + Radius, cR + Radius), cR + Radius, out d, out n);
        }

        // Detect collision with the wall
        bool DetectCollision(Wall wall, out float d, out Vector2 n)
        {
            return Collision.PointVsHalfPlane(Position, wall.P1 + wall.Normal * Radius, wall.Normal,
            out d, out n);
        }


    }
}

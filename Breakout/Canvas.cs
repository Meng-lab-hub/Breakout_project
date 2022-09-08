using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Canvas : Control
    {
        bool designTime;
        Bat bat = new Bat { Width = 100, X = Game.Instance.MapSize.X / 2};

        // max speed of the bat to the left or the right
        float maxSpeed = 150;

        // for mouse control
        bool mouseControlled = false;
        float lastMouseX;

        // solving pressing a and d together
        bool leftDown = false;
        bool rightDown = false;



        public Canvas()
        {
            DoubleBuffered = true;
            BackColor = Color.Black;
        }

        public void Start()
        {
            if (DesignMode)
                return;
            Game.Instance.Start();
            Game.Instance.Add(bat);
            Application.Idle += (s, e) => Invalidate();

            // keyboard event handler
            KeyDown += Canvas_KeyDown;
            KeyUp += Canvas_KeyUp;

            // mouse event handler
            MouseMove += Canvas_MouseMove;
        }

        // added when we handle mouse event
        private void Canvas_MouseMove(object? sender, MouseEventArgs e)
        {
            // e. contain almost every properties of the mouse.
            lastMouseX = e.X;
            mouseControlled = true;
        }



        // added when we handle keyboard event
        // control by arrow key is not possible yet
        private void Canvas_KeyDown(object? sender, KeyEventArgs e)
        {
            mouseControlled = false;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                //bat.Speed = -maxSpeed;
                leftDown = true;
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                //bat.Speed = maxSpeed;
                rightDown = true;
            //else if (e.KeyCode == Keys.S)
            //    bat.Speed = 0;
        }

        private void Canvas_KeyUp(object? sender, KeyEventArgs e)
        {
            mouseControlled = false;
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                //bat.Speed = -maxSpeed;
                leftDown = false;
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                //bat.Speed = maxSpeed;
                rightDown = false;
            bat.Speed = 0;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
                return;

            if (mouseControlled)
            {
                var x = lastMouseX / Width * (Game.Instance.MapSize.X + 20) - 10;
                var speed = x < bat.X - 1 ? -maxSpeed : x > bat.X + 1 ? maxSpeed : 0;
                bat.Speed = speed;
            } 
            else
            {
                bat.Speed = (leftDown ? -maxSpeed : 0) + (rightDown ? maxSpeed : 0);
            }

            Game.Instance.Update();
            e.Graphics.Transform = new Matrix(
                new RectangleF(-10, -50, Game.Instance.MapSize.X + 20, Game.Instance.MapSize.Y + 60),
                new PointF[] {
                new PointF( 0, Bounds.Height ),
                new PointF( Bounds.Width, Bounds.Height ),
                new PointF( )
                });
            Game.Instance.Draw(e.Graphics);
        }


    }
}

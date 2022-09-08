using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    abstract class Brick : GameObject
    {

        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float CornerRadius { get; set; }

        // this is how we serialize it
        public abstract int PackState();
        public abstract void UpdateFromNetwork(int state);


        // Draw a rounded-edge brick
        protected void Draw(Graphics g, Brush brush)
        {
            var cR = CornerRadius;
            var cR2 = cR * 2;
            var hS = Size * 0.5f;
            g.FillRectangle(brush, Position.X - hS.X, Position.Y - hS.Y + cR, Size.X, Size.Y - cR2);
            g.FillRectangle(brush, Position.X - hS.X + cR, Position.Y - hS.Y, Size.X - cR2, Size.Y);
            g.FillEllipse(brush, Position.X - hS.X, Position.Y - hS.Y, cR2, cR2);
            g.FillEllipse(brush, Position.X - hS.X, Position.Y + hS.Y - cR2, cR2, cR2);
            g.FillEllipse(brush, Position.X + hS.X - cR2, Position.Y - hS.Y, cR2, cR2);
            g.FillEllipse(brush, Position.X + hS.X - cR2, Position.Y + hS.Y - cR2, cR2, cR2);
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Bat : GameObject
    {
        public float X { get; set; }
        public float Width { get; set; }
        public float Speed { get; set; }
        public override bool Update(float dT)
        {
            X += Speed * dT;

            // prevent bat to go outside the frame
            X = Math.Max(Math.Min(X, Game.Instance.MapSize.X), 0);
            return Speed != 0;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Owner?.Brush ?? Brushes.White, X - Width * 0.5f, -6, Width, 6);
        }
    }

}

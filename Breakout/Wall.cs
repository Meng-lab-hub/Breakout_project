using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class Wall : GameObject
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }
        public Vector2 Normal => Vector2.Rotate90(Vector2.Normalize(P2 - P1));
        public override void Draw(Graphics g)
        {
            g.DrawLine(Pens.White, P1.X, P1.Y, P2.X, P2.Y);
        }
    }

}

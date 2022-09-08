using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class HardBrick : Brick
    {
        public int Lives { get; private set; } = 3;
        public override void Draw(Graphics g)
        {
            // Draw brick
            Draw(g, Brushes.Pink);
        }
        public override void OnHit()
        {
            if (--Lives == 0)
            {
                // Game.Instance.DestroyBrick(this);
                Game.Instance.Score += 2;
                Deleted = true;
            }
            else
            {
                Game.Instance.Score += 1;
            }
        }

        public override int PackState() => Lives;

        public override void UpdateFromNetwork(int state)
        {
            if (state < Lives)
                Lives = state;
            if (Lives == 0)
                Deleted = true;
        }

    }

}

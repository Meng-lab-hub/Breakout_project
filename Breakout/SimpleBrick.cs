using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class SimpleBrick : Brick
    {
        public override void Draw(Graphics g)
        {
            //Draw brick
            Draw(g, Brushes.BurlyWood);
        }
        public override void OnHit()
        {
            // Game.Instance.DestroyBrick(this);
            Deleted = true;
            Game.Instance.Score += 2;
        }

        public override int PackState()
        {
            return Deleted ? 0 : 1;
        }
        // public override int PackState() => Deleted ? 0 : 1; if the body of method has only 1 line

        public override void UpdateFromNetwork(int state)
        {
            Deleted = state == 0;
        }
    }
}

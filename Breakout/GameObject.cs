using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    abstract class GameObject
    {
        public bool Deleted { get; set; }
        public Session Owner { get; set; }
        public virtual bool Update(float dT) { return false; }
        public virtual bool DetectAndResolveCollision(GameObject other) { return false; }
        public virtual void OnHit() { }
        public abstract void Draw(Graphics g);
    }

}

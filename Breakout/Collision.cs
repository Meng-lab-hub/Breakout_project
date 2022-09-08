using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    internal class Collision
    {
        public static bool PointVsHalfPlane(Vector2 p, Vector2 pointOn, Vector2 n, out float d, out Vector2 collisionNormal)
        {
            d = Vector2.Dot(n, pointOn - p);
            collisionNormal = n;
            return d > 0;
        }

        public static bool PointVsRectangle(Vector2 p, Vector2 center, Vector2 halfSize, Vector2 n, out float d, out Vector2 collisionNormal)
        {
            var p0 = p - center;
            d = Vector2.Dot(n, new Vector2(n.X * halfSize.X, n.Y * halfSize.Y) - p0);
            collisionNormal = n;
            return Math.Abs(p0.X) < halfSize.X && Math.Abs(p0.Y) < halfSize.Y;
        }

        public static bool PointVsCircle(Vector2 p, Vector2 center, float r, out float d, out Vector2 collisionNormal)
        {
            var v = p - center;
            collisionNormal = Vector2.Normalize(v);
            d = r - v.Length;
            return d > 0;
        }

        public static bool PointVsRoundedRectangle(Vector2 p, Vector2 c, Vector2 hS, float cR, out float d, out Vector2 n)
        {
            var qS = hS * 0.5f;
            return
            PointVsRectangle(p, new Vector2(c.X, c.Y - qS.Y), new Vector2(hS.X - cR, qS.Y), new Vector2(0, -1), out d, out n) ||
            PointVsRectangle(p, new Vector2(c.X, c.Y + qS.Y), new Vector2(hS.X - cR, qS.Y), new Vector2(0, 1), out d, out n) ||
            PointVsRectangle(p, new Vector2(c.X - qS.X, c.Y), new Vector2(qS.X, hS.Y - cR), new Vector2(-1, 0), out d, out n) ||
            PointVsRectangle(p, new Vector2(c.X + qS.X, c.Y), new Vector2(qS.X, hS.Y - cR), new Vector2(1, 0), out d, out n) ||
            PointVsCircle(p, c + hS + new Vector2(-cR, -cR), cR, out d, out n) ||
            PointVsCircle(p, c - hS + new Vector2(cR, cR), cR, out d, out n) ||
            PointVsCircle(p, c + new Vector2(hS.X, -hS.Y) + new Vector2(-cR, cR), cR, out d, out n) ||
            PointVsCircle(p, c + new Vector2(-hS.X, hS.Y) + new Vector2(cR, -cR), cR, out d, out n);
        }

    }

   
}

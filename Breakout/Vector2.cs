using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        /*
         old style
         public static Vector2 Rotate90(Vector2 v) { 
               new Vector2(-v.Y, v.X);
         }
        */
        public static Vector2 Rotate90(Vector2 v) => new (-v.Y, v.X);

        public Vector2(float x, float y) { X = x; Y = y; }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        // implemented during class
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2 operator *(Vector2 v1, float f)
        {
            return new Vector2(v1.X * f, v1.Y * f);
        }
        public static Vector2 operator /(Vector2 v1, float f)
        {
            return new Vector2(v1.X / f, v1.Y / f);
        }
        public static Vector2 Normalize(Vector2 v1)
        {
            var len = v1.Length;
            return new Vector2(v1.X / len, v1.Y / len);
        }
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }


        

    }

}

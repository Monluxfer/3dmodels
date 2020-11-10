using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Point2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point2D()
        { }

        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static bool operator == (Point2D p1, Point2D p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator != (Point2D p1, Point2D p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }
    }
}

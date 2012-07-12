namespace Poisson.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    class MathUtils
    {
        public static Point VectorToPoint(Vector2 v)
        {
            return new Point(Convert.ToInt32(v.X), Convert.ToInt32(v.Y));
        }

        public static Vector2 PointToVector(Point p)
        {
            return new Vector2((float) p.X, (float) p.Y);
        }
    }
}

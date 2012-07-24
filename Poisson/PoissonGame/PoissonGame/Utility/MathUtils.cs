namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    class MathUtils
    {
        public const float CIRCLE = MathHelper.Pi * 2;
        public const float HALF_CIRCLE = MathHelper.Pi;
        public const float QUARTER_CIRCLE = MathHelper.Pi / 2;

        public static Point VectorToPoint(Vector2 v)
        {
            return new Point(Convert.ToInt32(v.X), Convert.ToInt32(v.Y));
        }

        public static Vector2 PointToVector(Point p)
        {
            return new Vector2((float) p.X, (float) p.Y);
        }

        public static float RadToDeg(float r)
        {
            return 180.0f * r / MathHelper.Pi;
        }

        public static float DegToRad(float d)
        {
            return MathHelper.Pi * d / 180.0f;
        }
    }
}

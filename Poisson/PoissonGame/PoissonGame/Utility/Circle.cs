using Microsoft.Xna.Framework;
namespace Poisson
{
    class Circle
    {
        public Vector2 Pos { get; set; }
        public float Radius { get; set; }

        float Squared(float f)
        {
            return f * f;
        }

        public bool IsCollidedCircle(Circle circle)
        {
            return Squared(circle.Pos.X - this.Pos.X) + Squared(circle.Pos.Y - this.Pos.Y) <
                Squared(circle.Radius + this.Radius);
        }
    }
}

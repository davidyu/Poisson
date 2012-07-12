namespace Poisson.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    class Fish : Entity
    {
        public Fish() : base()
        {
        }

        public Fish(Vector2 pos, float orient) : base(pos, orient)
        {
        }
    }
}

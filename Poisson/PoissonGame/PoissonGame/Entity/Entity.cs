namespace Poisson.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    abstract class Entity
    {
        public Vector2 Pos
        {
            get;
            set;
        }

        public Vector2 Vel
        {
            get;
            set;
        }

        public abstract void Update(List<Entity> entities, Entity player);

        public abstract void Render(SpriteBatch batch);

        
    }
}

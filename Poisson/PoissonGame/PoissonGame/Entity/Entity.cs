namespace Poisson.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Poisson.Utility;

    abstract class Entity
    {
        public Vector2 Pos { get; set; }  
        public Vector2 Vel { get; set; }
        public float Orient { get; set; }
        public float AngVel { get; set; }

        public Texture2D SpriteTexture { get; set; }
        public Rectangle SpriteRect { get; set; }
        public Rectangle BoundingRect { get; set; }

        public Entity() { }
        public Entity(Vector2 pos, float orient)
        {
            this.Pos = pos;
            this.Orient = orient;
        }


        public abstract void Update(GameTime gameTime, List<Entity> entities, Entity player);

        public abstract void Render(GameTime gameTime, SpriteBatch batch);

        
    }
}

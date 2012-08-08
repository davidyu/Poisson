namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class Entity
    {
        public Vector2 Pos { get; set; }
        public Vector2 Vel { get; set; }
        public float Orient { get; set; }
        public float AngVel { get; set; }
        public bool FacingLeft { get; set; }

        public Texture2D SpriteTexture { get; set; }
        public Rectangle SpriteRect { get; set; }
       
        public Rectangle bRect { get; private set; }

        public Rectangle BoundingRect {
            get { return (new Rectangle((int)Pos.X + this.bRect.X, (int)Pos.Y+this.bRect.Y, this.bRect.Width, this.bRect.Height)); }
            set { this.bRect = value; }
        }

        public Entity() { }
        public Entity(Vector2 pos, float orient)
        {
            this.Pos = pos;
            this.Orient = orient;
        }

        public virtual void Initialise(Game game) {}

        public virtual void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam) { }

        public virtual void Render(GameTime gameTime, SpriteBatch batch, Camera cam) {}

        
    }
}

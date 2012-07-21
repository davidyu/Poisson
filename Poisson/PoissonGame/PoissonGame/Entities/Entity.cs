namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Poisson.Utility;

    class Entity
    {
        public Vector2 Pos { get; set; }  
        public Vector2 Vel { get; set; }
        public float Orient { get; set; }
        public float AngVel { get; set; }
        public bool FacingLeft { get; set; }

        public Texture2D SpriteTexture { get; set; }
        public Rectangle SpriteRect { get; set; }
<<<<<<< HEAD
       
        public Rectangle bRect { get; set; }
=======

        private Rectangle bRect;
>>>>>>> c8ed20d8917b0c403585cef5e398161d0749984f

        public Rectangle BoundingRect { 
            get { return (new Rectangle((int)Pos.X, (int)Pos.Y, bRect.Width, bRect.Height)); }
            set { bRect = value; }
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

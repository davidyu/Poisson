namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Poisson.Utility;

    class Ship : Entity
    {
        const float FRICTION = 0.99f;

        Rectangle hookRect;
        Vector2 hookPos;

        public Ship() : base()
        {
        }

        public Ship(Vector2 pos, float orient)
            : base(pos, orient)
        {
        }

        public override void Initialise(Game game)
        {
            SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 0, 256, 164);
            this.hookRect = new Rectangle(0, 0, 40, 40);
            this.hookPos = new Vector2(this.Pos.X, this.Pos.Y);
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            this.Vel *= FRICTION;


        }

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, destRect, 
                this.SpriteRect, Color.White,
                this.Orient, this.Pos, SpriteEffects.None, 0.0f);
                
        }
    }
}

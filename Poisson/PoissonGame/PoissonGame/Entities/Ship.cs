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
        enum EHookState { UPDOWN = 0 }
        const float FRICTION = 0.99f;

        Rectangle hookRect;
        Vector2 hookPos;
        EHookState hookState;
        
        

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
            this.Vel = new Vector2(3.0f, 0.0f);
            this.Facing = true;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            //this.Vel *= FRICTION;

            if (this.Pos.X < 0.0f || this.Pos.X > 600) {
                this.Vel *= -1.0f;
                this.Facing = !this.Facing;
            }

            switch (this.hookState) {
                case EHookState.UPDOWN: 

                    break;
                default:
                    break;
            }
        }

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            SpriteEffects spriteEffects = new SpriteEffects();

            if (!Facing)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            //batch.Draw(this.SpriteTexture, this.Pos, Color.White);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.0f);
                
        }
    }
}

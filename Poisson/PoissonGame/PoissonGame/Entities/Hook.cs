namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class Hook : Entity
    {
        public enum EHookState {
            MOVING,
            WAITING,
            GRABBED,
            RETRACTED
        } //maybe different hook types later

        public EHookState HookState { get; set; }
        int stateticks;
        bool hasFish;

        public Rectangle HookRect
        {
            get
            {
                return new Rectangle((int)this.Pos.X + (int)Pos.X,
                                     (int)Pos.Y + (int)this.Pos.Y,
                                      this.HookRect.Width,
                                      this.HookRect.Width);
            }
        }

        public Hook()
            : base()
        {
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 255, 16, 22);
            this.bRect = new Rectangle(0, 0, 40, 40);
            this.HookState = EHookState.RETRACTED;
            
            this.stateticks = 0;
            this.hasFish = false;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            switch (this.HookState) {
                case EHookState.RETRACTED:
                    break;
                case EHookState.MOVING:
                    break;
                case EHookState.WAITING:
                    break;
            }
            //this.Pos += this.Vel;

            if (!this.hasFish) {
                foreach (Fish fish in entities) {
                    if (this.IsCollided(fish)) {
                        this.HookState = EHookState.GRABBED;
                        fish.Hooked();
                    }
                }
            }

            if (this.IsCollided(player)) {
                ((Fish)player).Hooked();
            }
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            SpriteEffects spriteEffects = new SpriteEffects();

            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.4f);
        }

        private bool IsCollided(Entity entity)
        {
            if (this.BoundingRect.Intersects(entity.BoundingRect)) {
                return true;
            }
            return false;
        }
    }
}

namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Diagnostics;

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
        Fish hookedFish;
        public float HookDepthDestination { get; set; }

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
            this.HookDepthDestination = 500f;
            this.Vel = Vector2.Zero;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            switch (this.HookState) {
                case EHookState.RETRACTED:
                    break;
                case EHookState.MOVING:
                    this.Vel = new Vector2(0f, 3f);
                    if (this.Pos.Y > HookDepthDestination) {
                        this.HookState = EHookState.WAITING;
                        this.Vel = Vector2.Zero;
                    }
                    break;
                case EHookState.WAITING:
                    break;
            }
            //Debug.WriteLine(this.HookState.ToString());
            this.Pos += this.Vel;

            if (!this.hasFish)
            {
                foreach (Fish fish in entities)
                {
                    if (this.IsCollided(fish))
                    {
                        this.HookState = EHookState.MOVING;
                        fish.Hooked(this);
                        this.hasFish = true;
                        this.hookedFish = fish;
                        //this.HookState = EHookState.GRABBED;
                    }
                }

                if (this.IsCollided(player))
                {
                    ((Fish)player).Hooked(this);
                    this.hasFish = true;
                    this.hookedFish = (Fish)player;
                    //this.HookState = EHookState.GRABBED;
                }
            }
            else //this.hasFish
            {
                this.hookedFish.Hooked(this);
            }


            
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            SpriteEffects spriteEffects = new SpriteEffects();

            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.2f);
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

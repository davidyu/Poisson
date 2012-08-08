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
            MOVINGDOWN,
            MOVINGUP,
            WAITING,
            GRABBED,
            RETRACTED
        } //maybe different hook types later

        public EHookState HookState { get; set; }
        double waitTime;
        bool hasFish;
        Fish hookedFish;
        public float HookDepthDestination { get; set; }
        public Ship Ship { get; set; }

        public Texture2D rect;

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

        public Hook(Ship ship)
            : base()
        {
            this.Ship = ship;
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 255, 16, 22);
            this.BoundingRect = new Rectangle(0, 0, 40, 40);
            this.HookState = EHookState.RETRACTED;
            
            this.hasFish = false;
            this.HookDepthDestination = 500f;
            this.Vel = Vector2.Zero;

            this.rect = new Texture2D(game.GraphicsDevice, bRect.Width, bRect.Height);
            Color[] data = new Color[bRect.Width * bRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            this.rect.SetData(data);
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            switch (this.HookState) {
                case EHookState.RETRACTED:
                    if (this.hookedFish != null) {
                        this.hookedFish.Kill();
                        this.hasFish = false;
                        this.hookedFish = null;
                    }
                    this.HookState = EHookState.MOVINGDOWN;
                    break;
                case EHookState.MOVINGDOWN:
                    this.Vel = new Vector2(0f, 3f);
                    if (this.Pos.Y > HookDepthDestination) {
                        this.HookState = EHookState.WAITING;
                        this.waitTime = gameTime.TotalGameTime.TotalSeconds + 5.0;
                        this.Vel = Vector2.Zero;
                    }
                    break;
                case EHookState.MOVINGUP:
                    this.Vel = new Vector2(0f, -3f);
                    if (this.Pos.Y < 40) {
                        this.HookState = EHookState.RETRACTED;
                        this.Vel = Vector2.Zero;
                    }
                    break;
                case EHookState.WAITING:
                    if (this.waitTime > gameTime.TotalGameTime.TotalSeconds)
                        this.HookState = EHookState.MOVINGUP;
                    break;
            }
            //Debug.WriteLine(this.HookState.ToString());
            this.Pos += this.Vel;

            if (!this.hasFish)
            {
                foreach (Fish fish in entities)
                {
                    if (this.IsCollided(fish) && fish.State != Fish.EFishState.HOOKED)
                    {
                        this.HookState = EHookState.MOVINGUP;
                        fish.Hooked(this);
                        this.hasFish = true;
                        this.hookedFish = fish;
                        break;
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

            batch.Draw(this.rect, this.Pos, Color.White);
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

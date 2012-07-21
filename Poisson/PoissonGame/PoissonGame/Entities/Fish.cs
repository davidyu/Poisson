namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input.Touch;
    using Poisson.Utility;
    using System.Diagnostics;

    class Fish : Entity
    {
        //not yet implemented: random AI behavior for non-human fishes
        enum EFishState {
            STEERING,     //follow Poisson! Poisson is perpetually in this mode
            SPONTANEOUS,  //random movement
            HOOKED,       //Jimmy John got you, you poor thing
            DEAD,          //just before being removed from screen
            WAIT
        }

        private bool          flipX   = false;
        private SpriteEffects effects = SpriteEffects.None;
        private float         rotation = 0.0f;
        private EFishState    state = EFishState.STEERING;
        private Vector2       target = new Vector2(0, 0);

        const float FRICTION = 0.90f;
        public bool inRoutine { get; set;}
        GameTime routineStartTime {get; set;}
        Vector2 targetPoint {get; set; }
        
        public bool isHuman { get; set; }

        public Fish()
            : base()
        {
        }

        public Fish(Vector2 pos, float orient, bool isHuman)
            : base(pos, orient)
        {
            this.isHuman = isHuman;
            inRoutine = true;
        }

        public override void Initialise(Game game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold | GestureType.HorizontalDrag | GestureType.VerticalDrag;
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");

            if (this.isHuman) {
                this.SpriteRect = new Rectangle(0, 164, 111, 64);
                this.state = EFishState.STEERING;
            } else {
                this.SpriteRect = new Rectangle(0, 228, 45, 28);
                this.state = EFishState.SPONTANEOUS;
            }

            this.BoundingRect = new Rectangle(0, 0, 164, 64);
        }

        private void PollInput(Camera cam)
        {
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc) {
                if (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved) {
                    Vector2 touchPos = cam.ScreenToWorld(tl.Position); //converted to camera pos
                    this.rotation = (float)Math.Atan2(Pos.Y - touchPos.Y, Pos.X - touchPos.X) % MathUtils.circle;
                    //refactor later for angular velocity
                    this.flipX = (Pos.X < touchPos.X);
                    Vector2 delta = touchPos - Pos;
                    delta.Normalize();
                    this.Vel = delta * 10;
                }
            }
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            if (isHuman) {
                PollInput(cam);
            } else {
                Autonomous(player);
            }
            
            //common update code at the end
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            this.Vel *= FRICTION;
        }

        public void Routine(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - routineStartTime.TotalGameTime.TotalSeconds < 10) //DON'T MOVE FISHY
            {
                this.Vel = new Vector2(0,0);
            }
            else
            {
                Random random = new Random();
            }

        }

        public void Hooked() //do-me: implement actions when hooked
        {
            
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            this.effects = this.flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.Orient  = this.flipX ? this.rotation - MathUtils.circle/2 : this.rotation;
            
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(SpriteRect.Width/2, SpriteRect.Height/2), 1.0f, this.effects, 0.0f);
        }

        private void Autonomous(Entity player)
        {
            Vector2 delta  = player.Pos - Pos;
            if (this.state == EFishState.STEERING)
            {
                if (delta.Length() >= 200.0)
                {
                    this.state = EFishState.SPONTANEOUS;
                    Random random = new Random();
                    this.target = new Vector2((float)random.NextDouble() * 800.0f, (float)random.NextDouble() * 480.0f);
                }
                else
                {
                    this.rotation = (float)Math.Atan2(player.Pos.Y - Pos.Y, player.Pos.X - Pos.X) % MathUtils.circle;
                    this.flipX = (Pos.X < player.Pos.X);
                    delta = -delta;
                    delta.Normalize();
                    this.Vel = delta * 10;
                }
            }
            else if (this.state == EFishState.SPONTANEOUS)
            {
                Vector2 dprime = target - Pos;
                if (delta.Length() <= 50.0)
                {
                    this.state = EFishState.STEERING;
                }
                else if (dprime.Length() <= 20.0)
                {
                    Random random = new Random();
                    this.target = new Vector2((float)random.NextDouble() * 800.0f, (float)random.NextDouble() * 480.0f);
                }
                else
                {
                    this.rotation = (float)Math.Atan2(Pos.Y - target.Y, Pos.X - target.X) % MathUtils.circle;
                    this.flipX = (Pos.X < target.X);
                    dprime.Normalize();
                    this.Vel = dprime * 3;
                }
            }
        }
    }
}

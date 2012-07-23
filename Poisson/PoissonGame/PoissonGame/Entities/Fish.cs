namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input.Touch;
    using System.Diagnostics;

    class Fish : Entity
    {
        //not yet implemented: random AI behavior for non-human fishes
        enum EFishState {
            STEERING,     //follow Poisson! Poisson is perpetually in this mode
            SPONTANEOUS,  //random movement
            HOOKED,       //Jimmy John got you, you poor thing
            DEAD,         //just before being removed from screen
            WAIT
        }

        private bool          flipX   = false;
        private SpriteEffects effects = SpriteEffects.None;
        private float         rotation = 0.0f;
        private EFishState    state = EFishState.STEERING;
        private Vector2       target = new Vector2(0, 0);

        const float FRICTION = 0.90f;
        const float ANG_FRICTION = 0.75f;
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
            this.AngVel = 0.0f;
        }

        private void Thrust()
        {
            this.Vel = 10 * new Vector2((float)Math.Cos(this.rotation), (float)Math.Sin(this.rotation));
        }

        private void PollInput(Camera cam)
        {
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc) {
                if (tl.State == TouchLocationState.Moved || tl.State == TouchLocationState.Pressed) {
                    Vector2 touchPos = cam.ScreenToWorld(tl.Position); //converted to camera pos
                    MoveTowards(touchPos, 10);
                    Thrust();
                }
            }
        }

        private void Autonomous(Entity player)
        {
            Vector2 delta = player.Pos - Pos;
            if (this.state == EFishState.STEERING)
            {
                if (delta.Length() >= 200.0)
                {
                    this.state = EFishState.SPONTANEOUS;
                    FindNewTarget();
                }
                else
                {
                    MoveAwayFrom(player.Pos, 10);
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
                    FindNewTarget();
                }
                else
                {
                    MoveTowards(target, 3);
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
            this.AngVel *= ANG_FRICTION;
            this.rotation += this.AngVel;
            this.rotation %= 2 * MathUtils.circle;
            
            this.Pos += this.Vel;
            this.Vel *= FRICTION;
        }

        private void FindNewTarget()
        {
            Random random = new Random();
            this.target = new Vector2((float)random.NextDouble() * 800.0f, (float)random.NextDouble() * 480.0f);
        }

        //do-me: MoveTowards and MoveAwayFrom are too similar! Refactor into one method!
        private void MoveTowards(Vector2 loc, float intensity)
        {
            Vector2 delta = loc - Pos;
            delta.X = (float)(Math.Cos(-rotation) * delta.X - Math.Sin(-rotation) * delta.Y);
            delta.Y = (float)(Math.Sin(-rotation) * delta.X + Math.Cos(-rotation) * delta.Y);
            delta.Normalize();
            this.AngVel += Math.Sign(delta.Y) * 0.1f;

            this.flipX = (Pos.X < loc.X);
            this.Orient = this.flipX ? (float)Math.Atan2(loc.Y - Pos.Y, loc.X - Pos.X) : (float)Math.Atan2(Pos.Y - loc.Y, Pos.X - loc.X);
            //this.Vel = delta * intensity;
        }

        private void MoveAwayFrom(Vector2 loc, float intensity)
        {
            Vector2 delta = Pos - loc;
            this.AngVel = ((float)Math.Atan2(loc.Y - Pos.Y, loc.X - Pos.X) % MathUtils.circle - this.Orient) / 10;
            this.flipX = (Pos.X < loc.X);
            delta.Normalize();
            this.Vel = delta * intensity;
        }

        public void Hooked() //do-me: implement actions when hooked
        {
            this.Orient = 90f;
            this.Pos = new Vector2(0.0f, 0.0f);
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            this.effects = this.flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2((float) SpriteRect.Width/2, SpriteRect.Height/2), 1.0f, this.effects, 0.0f);
        }
    }
}

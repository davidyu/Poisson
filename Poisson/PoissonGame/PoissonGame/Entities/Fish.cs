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
        public enum EFishState {
            STEERING,     //follow Poisson! Poisson is perpetually in this mode
            SPONTANEOUS,  //random movement
            HOOKED,       //Jimmy John got you, you poor thing
            DEAD,         //just before being removed from screen
            WAIT
        }

        public EFishState State { get; set; }

        private const float SEA_LEVEL = 205f;
        public float THRUST_SPEED = 3.5f;
        private bool          flipY   = false;
        private SpriteEffects effects = SpriteEffects.None; 
        private Vector2       target = new Vector2(0, 0);

        const float FRICTION = 0.90f;
        const float ANG_FRICTION = 0.75f;
        public bool inRoutine { get; set;}
        GameTime routineStartTime {get; set;}
        Vector2 targetPoint {get; set; }

        Texture2D rect;

        private bool hooked = false;
        
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
                this.State = EFishState.STEERING;
            } else {
                this.SpriteRect = new Rectangle(0, 228, 45, 28);
                this.State = EFishState.SPONTANEOUS;
            }

            if (this.isHuman)
                this.BoundingRect = new Rectangle(-this.SpriteRect.Width / 2, -this.SpriteRect.Height / 2, 100, 50);
            else
                this.BoundingRect = new Rectangle(-this.SpriteRect.Width/2, -this.SpriteRect.Height/2, 50, 35);

            this.AngVel = 0.0f;

            this.rect = new Texture2D(game.GraphicsDevice, bRect.Width, bRect.Height);
            Color[] data = new Color[bRect.Width * bRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            this.rect.SetData(data);
        }

        private void Thrust()
        {
            if (this.Pos.Y < SEA_LEVEL)
                return;
            this.Vel = THRUST_SPEED * new Vector2((float)Math.Cos(this.Orient + MathUtils.CIRCLE / 2), 
                                        (float)Math.Sin(this.Orient + MathUtils.CIRCLE / 2));
        }

        private void PollInput(Camera cam)
        {
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc) {
                if (tl.State == TouchLocationState.Moved || tl.State == TouchLocationState.Pressed) {
                    Vector2 touchPos = cam.ScreenToWorld(tl.Position); //converted to camera pos
                    SteerToward(touchPos, 0.1f);
                    Thrust();
                }
            }
        }

        private void Autonomous(Entity player, Camera cam)
        {
            Vector2 delta = player.Pos - Pos;
            if (this.State == EFishState.STEERING)
            {
                if (delta.Length() >= 400.0) {
                    this.State = EFishState.SPONTANEOUS;
                    FindNewTarget(cam);
                } else {
                    SteerToward(player.Pos, 0.5f);
                    Thrust();
                }
            }
            else if (this.State == EFishState.SPONTANEOUS)
            {
                Vector2 dprime = target - Pos;
                if (delta.Length() <= 300.0) {
                    this.State = EFishState.STEERING;
                } else if (dprime.Length() <= 20.0) {
                    FindNewTarget(cam);
                } else {
                    SteerToward(target, 0.2f);
                    Thrust();
                }
            }
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            if (this.hooked || this.State == EFishState.DEAD)
                return;

            if (isHuman) {
                PollInput(cam);
            } else {
                Autonomous(player, cam);
            }
            
            this.Orient = (this.Orient + MathUtils.CIRCLE) % MathUtils.CIRCLE; //clamp to 0 < Orient < 2pi
            this.flipY = (this.Orient > MathUtils.QUARTER_CIRCLE && this.Orient < 3 * MathUtils.QUARTER_CIRCLE);
            this.AngVel *= ANG_FRICTION;
            this.Vel *= FRICTION;

            this.Orient += this.AngVel;
            if (this.Pos.Y < SEA_LEVEL)
                this.Vel += new Vector2(0f, 2f);
            this.Pos += this.Vel;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            this.effects = this.flipY ? SpriteEffects.FlipVertically : SpriteEffects.None;
            
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2((float) SpriteRect.Width/2, SpriteRect.Height/2), 1.0f, this.effects, 0.0f);
            batch.Draw(this.rect, new Vector2(this.BoundingRect.X, this.BoundingRect.Y), Color.White);
        }

        private void FindNewTarget(Camera cam)
        {
            Random random = new Random();
            this.target = cam.ScreenToWorld(new Vector2((float)random.NextDouble() * 800.0f, (float)random.NextDouble() * 480.0f));
        }

        private void SteerToward(Vector2 loc, float intensity)
        {
            Vector2 delta = loc - Pos;
            delta.Normalize();

            //get delta degrees (how much should I rotate?)
            float ddeg = ((float)Math.Atan2(delta.Y, delta.X) + MathUtils.HALF_CIRCLE) % MathUtils.CIRCLE - this.Orient;

            if (Math.Abs(ddeg) < MathUtils.HALF_CIRCLE) {
                this.AngVel += ddeg * intensity;
            }
            else {
                //take care of edge cases such as rotating at the edge from 0 to 360 or 360 to 0
                float sign = Math.Sign(ddeg);
                this.AngVel += -sign * (MathUtils.CIRCLE - sign * ddeg) * intensity;
            }
        }

        public void Hooked(Entity h) //do-me: implement actions when hooked
        {
            if (this.State != EFishState.DEAD)
                this.State = EFishState.HOOKED;
            this.Orient = MathUtils.QUARTER_CIRCLE;
            this.Pos = h.Pos;
            this.hooked = true;
        }

        public void Kill()
        {
            this.State = EFishState.DEAD;
        }
    }
}

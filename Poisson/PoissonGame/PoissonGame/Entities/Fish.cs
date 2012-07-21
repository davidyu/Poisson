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
            DEAD          //just before being removed from screen
        }

        private bool          flipX   = false;
        private SpriteEffects effects = SpriteEffects.None;


        const float FRICTION = 0.99f;
        public bool isTouching { get; set; }
        Vector2 userTouch { get; set; }
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
            } else {
                this.SpriteRect = new Rectangle(0, 228, 45, 28);
            }

            this.BoundingRect = new Rectangle(0, 0, 164, 64);
        }

        private void PollInput()
        {
            TouchCollection tc = TouchPanel.GetState();
            foreach (TouchLocation tl in tc) {
                if (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved) {
                    this.flipX = (Pos.X < tl.Position.X);
                    Vector2 delta = tl.Position - Pos;
                    delta.Normalize();
                    this.Orient = (float) Math.Atan2(Pos.Y - tl.Position.Y, Pos.X - tl.Position.X) % MathUtils.circle;
                    this.Vel = delta * 10;
                    this.Orient += 0.1f;
                } else {
                    this.Vel *= 0.1f; //dampen vel
                }
            }
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            if (isHuman) {
                PollInput();
            } else {
                //if they're within the radius of the human fishy, change attributes accordingly
                if (Math.Abs(this.Pos.X - player.Pos.X) < 50 && Math.Abs(this.Pos.Y - player.Pos.Y) < 50)
                {
                    inRoutine = false;
                    Vector2 Vdif = new Vector2(player.Pos.X - this.Pos.X, player.Pos.Y - this.Pos.Y);
                    Vdif.Normalize();
                    Vdif = new Vector2(Vdif.X * 3, Vdif.Y * 3);
                    this.Vel = Vdif;
                }
                else
                {
                    if (inRoutine)
                    {
                        if (routineStartTime != null)
                        {
                            if (gameTime.TotalGameTime.TotalSeconds - routineStartTime.TotalGameTime.TotalSeconds < 10) //DON'T MOVE FISHY
                            {
                                this.Vel = new Vector2(0, 0);
                            }
                            else //time to move!
                            {
                                Random random = new Random();
                                if ((gameTime.TotalGameTime.TotalSeconds - routineStartTime.TotalGameTime.TotalSeconds < 30) || this.Pos == targetPoint)
                                {
                                    this.Vel = new Vector2(0, 0);
                                    inRoutine = false;
                                }
                                else if (this.Vel.X.Equals(0) || this.Vel.Y.Equals(0)) //freshly moving, set the target move point
                                {
                                    targetPoint = new Vector2(random.Next(800), random.Next(480));
                                    Vector2 newVel = targetPoint;
                                    newVel.Normalize();
                                    this.Vel = new Vector2(newVel.X, newVel.Y);
                                }
                                else //move to target location
                                {
                                }
                            }
                        }

                    }
                    else //start the AI routine
                    {
                        inRoutine = true;
                        routineStartTime = gameTime;
                        this.Vel = new Vector2(0, 0);
                    }
                }
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

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            this.effects = this.flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            this.Orient  = this.flipX ? this.Orient - MathUtils.circle/2 : this.Orient;
            
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(SpriteRect.Width/2, SpriteRect.Height/2), 1.0f, this.effects, 0.0f);
        }
    }
}

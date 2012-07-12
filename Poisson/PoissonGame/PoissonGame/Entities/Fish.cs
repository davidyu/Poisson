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

        //public Fish(Vector2 pos, float orient)
        //    : base(pos, orient)
        //{
        //}

        public Fish(Vector2 pos, float orient, bool isHuman)
            : base(pos, orient)
        {
<<<<<<< HEAD
            this.isHuman = isHuman;
            inRoutine = true;
=======
            
>>>>>>> e75e7f41e8e0577c01d7a0a228d21bd94a7dc4fd
        }

        public override void Initialise(Game game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold | GestureType.HorizontalDrag | GestureType.VerticalDrag;
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
<<<<<<< HEAD
            if (this.isHuman)
            { 
                this.SpriteRect = new Rectangle(0, 164, 111, 64); 
            }
            else
            {
                this.SpriteRect = new Rectangle(0, 228, 45, 28);
            }
=======
            this.SpriteRect = new Rectangle(0, 164, 111, 64);
            this.BoundingRect = new Rectangle(0, 0, 164, 64);
>>>>>>> e75e7f41e8e0577c01d7a0a228d21bd94a7dc4fd
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            if (isHuman) //control the human fish
            {
                TouchCollection tc = TouchPanel.GetState();
                foreach (TouchLocation tl in tc)
                {
                    if ((tl.State == TouchLocationState.Pressed) || (tl.State == TouchLocationState.Moved)) //if the user has touched the screen 
                    {
                        
                        isTouching = true;
                        Debug.WriteLine(tl.Position.X.ToString() + " " + tl.Position.Y.ToString()); //debug line
                        //if (this.SpriteRect.Contains((int)(this.Pos.X), (int)(this.Pos.Y)) || (tl.Position.X.CompareTo(this.Pos.X) == 0 && tl.Position.X.CompareTo(this.Pos.Y) == 0))
                        //{
                        //    this.Vel = new Vector2(0, 0);
                        //}
                        //else
                        //{
                            Vector2 Vdif = new Vector2(tl.Position.X - this.Pos.X, tl.Position.Y - this.Pos.Y);
                            Vdif.Normalize(); //NORMALIZE THE VECTOR, HURRAH
                            Vdif = new Vector2(Vdif.X * 10, Vdif.Y * 10);
                            this.Vel = Vdif;
                        //}
                    }
                    else //no touch on screen, reset all userfish velocities
                    {
                        this.Vel = new Vector2(0, 0);
                    }
                }
            }
            else //other little fishies
            {
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

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            //batch.Draw(this.SpriteTexture, this.Pos, Color.White);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}

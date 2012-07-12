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

        public bool isHuman { get; set; }

        public Fish()
            : base()
        {
        }

        public Fish(Vector2 pos, float orient)
            : base(pos, orient)
        {
        }

        public override void Initialise(Game game)
        {

            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold | GestureType.HorizontalDrag | GestureType.VerticalDrag;
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 164, 111, 64);
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
                        if (this.SpriteRect.Contains((int)(this.Pos.X), (int)(this.Pos.Y)) || (tl.Position.X.CompareTo(this.Pos.X) == 0 && tl.Position.X.CompareTo(this.Pos.Y) == 0))
                        {
                            this.Vel = new Vector2(0, 0);
                        }

                        else
                        {
                            Vector2 Vdif = new Vector2(tl.Position.X - this.Pos.X, tl.Position.Y - this.Pos.Y);
                            Vdif.Normalize(); //NORMALIZE THE VECTOR, HURRAH
                            Vdif = new Vector2(Vdif.X * 10, Vdif.Y * 10);
                            this.Vel = Vdif;
                        }
                    }
                    else //no touch on screen, reset all userfish velocities
                    {
                        this.Vel = new Vector2(0, 0);
                    }
                }
            }
            else //other fish
            {
                //if they're within the radius of the human fishy, change attributes accordingly
                //if (this.Pos.X - 

                //else, do regular updates

                
            }
            //common update code at the end
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            this.Vel *= FRICTION;
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

namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Poisson.Utility;
    using Microsoft.Xna.Framework.Input.Touch;
    using System.Diagnostics;

    class Fish : Entity
    {
        const float FRICTION = 0.99f;
        bool isHuman = false;

        public Fish() : base()
        {
        }

        public Fish(Vector2 pos, float orient) : base(pos, orient)
        {
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 164, 111, 64);
        }

        public void HandleInput()
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection) {
                if ((tl.State == TouchLocationState.Pressed)
                        || (tl.State == TouchLocationState.Moved)) {
                    //Vector2 targetOffset = tl.Position - this.Pos;
                    //Vector2 targetRelative = MathUtils.RotateVector2(targetOffset, this.Orient);
                    //Vector2 xAxis = new Vector2(1.0f, 0.0f);

                    //if (targetRelative.Y > 0) {
                    //    Debug.WriteLine("TOP");
                    //}
                    //else {
                    //    Debug.WriteLine("BOT");
                    //}

                    //targetRelative.Normalize();
                    //AngleBetweenVector2(targetRelative, xAxis)

                }
            }
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            this.Vel *= FRICTION;

            if (isHuman) {
                HandleInput();
            } else {
                HandleInput();
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

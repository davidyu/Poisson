namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    class Sea : Entity
    {
        float depth;

        public Sea(Vector2 pos, Vector2 vel, float depth) : base(pos, 0.0f)
        {
            this.Vel = vel;
            this.depth = depth;
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/environment");
            this.SpriteRect = new Rectangle(0, 0, 799, 360);
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            Vector2 tempPos = this.Pos;

            this.Pos += this.Vel;
            if (this.Pos.X > this.SpriteRect.Width*2) {
                tempPos.X -= this.SpriteRect.Width;
                this.Pos = tempPos;
            } else if (this.Pos.X < 0) {
                tempPos.X += this.SpriteRect.Width;
                this.Pos = tempPos;
            }
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 2.0f, SpriteEffects.None, this.depth);
            batch.Draw(this.SpriteTexture, this.Pos- cam.ScreenToWorld(new Vector2(this.SpriteRect.Width, 0.0f)),
               this.SpriteRect, Color.White,
               this.Orient, new Vector2(0f, 0f), 2.0f, SpriteEffects.None, this.depth);
        }

    }
}

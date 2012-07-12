namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    class Sea : Entity
    {

        public Sea(Vector2 pos, Vector2 vel) : base(pos, 0.0f)
        {
            this.Vel = vel;
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/environment");
            this.SpriteRect = new Rectangle(0, 0, 799, 360);
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            this.Pos += this.Vel;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}

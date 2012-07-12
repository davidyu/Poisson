namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Poisson.Utility;

    class Fish : Entity
    {
        const float FRICTION = 0.99f;

        public Fish() : base()
        {
        }

        public Fish(Vector2 pos, float orient) : base(pos, orient)
        {
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            this.Pos += this.Vel;
            this.Orient += this.AngVel;
            this.Vel *= FRICTION;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(this.SpriteTexture, new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height),
                this.SpriteRect, Color.White,
                this.Orient, MathUtils.PointToVector(this.SpriteRect.Center), SpriteEffects.None, 0.0f);

        }
    }
}

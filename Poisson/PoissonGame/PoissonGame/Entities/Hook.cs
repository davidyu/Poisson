namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class Hook : Entity
    {
        public Hook()
            : base()
        {
        }

        public override void Initialise(Game game)
        {
            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 255, 16, 22);
            this._hookRect = new Rectangle(0, 0, 40, 40);
            this.hookPos = new Vector2(ROD_OFFSET, 0);
            this.hookState = EHookState.RETRACTED;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
        }
    }
}

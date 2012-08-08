namespace Poisson.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input.Touch;

    class MainMenuScreen : GameScreen
    {
        private Texture2D bgTex;
        public override void LoadContent()
        {
            bgTex = this.ScreenManager.Game.Content.Load<Texture2D>("Art/menu");
        }

        public override void UnloadContent() { }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.ScreenExit();
            }
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count != 0 && tc[0].State == TouchLocationState.Pressed) {
                this.ScreenManager.PushScreen(new GameplayScreen());
            }

        }

        public override void Draw(GameTime gameTime)
        {
            this.ScreenManager.Game.GraphicsDevice.Clear(new Color(219, 237, 212));
            this.ScreenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null);

            this.ScreenManager.SpriteBatch.Draw(this.bgTex, new Rectangle(0, 0, 480, 360),Color.White);

            this.ScreenManager.SpriteBatch.End();
        }

    }
}

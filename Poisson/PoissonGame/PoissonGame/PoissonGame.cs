namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;
    using Microsoft.Xna.Framework.Media;
    using System.Xml.Linq;
    using Poisson.Screens;

    public class PoissonGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Camera camera;

        int score = 0;
        SpriteFont hudFont;

        List<Entity> fishes;
        List<Entity> ships;
        List<Entity> seas;

        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();


        Fish player; //player will also be fishes[0] upon init

        public PoissonGame()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.screenManager = new ScreenManager(this);
            this.screenManager.PushScreen(new MainMenuScreen());
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            this.screenManager.Update(gameTime);
        }

        //private void CheckCollisions()
        //{
        //}

        protected override void Draw(GameTime gameTime)
        {
            this.screenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}

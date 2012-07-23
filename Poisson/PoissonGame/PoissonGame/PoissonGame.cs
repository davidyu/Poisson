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

    public class PoissonGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
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
            // Init GFX
            float SCREEN_WIDTH = (float)graphics.GraphicsDevice.Viewport.Width;
            float SCREEN_HEIGHT = (float)graphics.GraphicsDevice.Viewport.Height;

            camera = new Camera(0.5f);

            //this.projection = Matrix.CreateScale(SCREEN_WIDTH / 2560f, SCREEN_HEIGHT / 2400f, 1f);
            Random random = new Random();
            fishes = new List<Entity>();
            hudFont = Content.Load<SpriteFont>("HUDFont");

            ships = new List<Entity>();

            player = new Fish(new Vector2(400f, 0f), 0.0f, true); //player is Poisson and has different graphic than regular fishies
            fishes.Add(player); //player is always index 0

            seas = new List<Entity>();

            ships.Add(new Ship(new Vector2(100f, 50f), 0.0f));
            
            seas.Add(new Sea(new Vector2(0.0f, 150.0f), new Vector2(-10.0f, 0.0f), 0.39f));

            for (int i = 0; i < 3; i++) {
                fishes.Add(new Fish(new Vector2(random.Next(800), random.Next(480)), 0.0f, false)); //NEED TO INCLUDE MIniMUMS FOR THE SEA LATER
            }
                
            foreach (Fish fish in fishes) {
                fish.Initialise(this);
            }

            foreach (Ship ship in ships) {
                ship.Initialise(this);
            }

            foreach (Sea sea in seas) {
                sea.Initialise(this);
            }

            player.Initialise(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }

            score = (int)gameTime.TotalGameTime.TotalMilliseconds * 7;

            CheckCollisions();

            foreach (Fish fish in fishes) {
                fish.Update(gameTime, ships, player, this.camera);
            }

            foreach (Ship ship in ships) {
                ship.Update(gameTime, fishes, player, this.camera);
            }

            foreach (Sea sea in this.seas) {
                sea.Update(gameTime, fishes, player, this.camera);
            }

            base.Update(gameTime);
        }

        private void CheckCollisions()
        {
            // right now we only use one ship. Consider refactoring and using SAP when time comes to use multiple ships
            //foreach (Fish fish in fishes) {
            //    if (fish.BoundingRect.Intersects((ships[0] as Ship).hookRect)) {
                    
            //    }
            //}
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(new Color(219, 237, 212));
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.camera.Projection);

            this.spriteBatch.DrawString(hudFont, score.ToString(), new Vector2(0f, 0f), Color.Black);

            foreach (Fish fish in fishes) {
                fish.Render(gameTime, this.spriteBatch, this.camera);
            }

            foreach (Ship ship in ships) {
                ship.Render(gameTime, this.spriteBatch, this.camera);
            }

            foreach (Sea sea in this.seas) {
                sea.Render(gameTime, this.spriteBatch, this.camera);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

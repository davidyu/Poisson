namespace Poisson
{
    using System;
    using System.Collections.Generic;
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
    using Poisson.Entities;

    public class PoissonGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<string, Animation> animations = new Dictionary<string,Animation>(); 

        // Game Properties
        List<Entity> fishes;
        List<Entity> ships;
        Fish player;


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
            fishes = new List<Entity>();

            fishes.Add(new Fish(new Vector2(400f, 0f), 0.0f));

            ships = new List<Entity>();
            player = new Fish(); //player is Poisson and has different graphic than regular fishies

            

            ships.Add(new Ship(new Vector2(100f, 100f), 0.0f));

            foreach (Fish fish in fishes) {
                fish.Initialise(this);
            }

            foreach (Ship ship in ships) {
                ship.Initialise(this);
            }

            player.Initialise(this);

            base.Initialize();
        }

        Texture2D SpriteTexture;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Fish fish in fishes) {
                fish.Update(gameTime, ships, player);
            }

            foreach (Ship ship in ships) {
                ship.Update(gameTime, fishes, player);
            }

            player.Update(gameTime, ships, player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

                foreach (Fish fish in fishes) {
                    fish.Render(gameTime, this.spriteBatch);
                }

                foreach (Ship ship in ships) {
                    ship.Render(gameTime, this.spriteBatch);
                }

                player.Render(gameTime, this.spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

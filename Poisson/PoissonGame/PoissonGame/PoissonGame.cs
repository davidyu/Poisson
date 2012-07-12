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
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        protected override void Initialize()
        {
            fishes = new List<Entity>();
            ships = new List<Entity>();
            player = new Fish();

            base.Initialize();
        }

        Texture2D SpriteTexture;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteTexture = Content.Load<Texture2D>("Textures/Ammo/rock_ammo");

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Fish fish in fishes) {

            }

            foreach (Ship ship in ships) {

            }

            player.Update(gameTime, ships, player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
           {
                graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                    Vector2 pos = new Vector2(0.0f, 10.0f);
                    spriteBatch.Draw(SpriteTexture, pos, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}

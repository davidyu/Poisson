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
    using Poisson.Entities;

    public class PoissonGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        

        // Game Properties
        int score = 0;
        SpriteFont hudFont;

        List<Entity> fishes;
        List<Entity> ships;
        List<Entity> seas;
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
            hudFont = Content.Load<SpriteFont>("HUDFont");

            ships = new List<Entity>();

            player = new Fish(new Vector2(400f, 0f), 1.72f); //player is Poisson and has different graphic than regular fishies
            seas = new List<Entity>();
            player = new Fish(new Vector2(400f, 200f), 1.72f); //player is Poisson and has different graphic than regular fishies
            fishes.Add(player);

            ships.Add(new Ship(new Vector2(100f, 50f), 0.0f));
            seas.Add(new Sea(new Vector2(0.0f, 50.0f), new Vector2(10.0f, 0.0f), 0.5f));
            seas.Add(new Sea(new Vector2(0.0f, 150.0f), new Vector2(-10.0f, 0.0f), 0.39f));

            foreach (Fish fish in fishes)
            {
                fish.Initialise(this);
                fish.isHuman = false;
            }

            foreach (Ship ship in ships)
            {
                ship.Initialise(this);
            }

            foreach (Sea sea in seas) {
                sea.Initialise(this);
            }

            player.Initialise(this);
            player.isHuman = true;

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            score = (int)gameTime.TotalGameTime.TotalMilliseconds * 7;
            


            foreach (Fish fish in fishes) {
                fish.Update(gameTime, ships, player);
                
                /*
                 * COLLISION DETECTION IS HERE
                 */

                foreach (Ship ship in ships) {
                    if (fish.BoundingRect.Intersects(ship.hookRect))
                    {
                        //hit! do stuff here
                    }
                }
            }

            foreach (Ship ship in ships) {
                ship.Update(gameTime, fishes, player);
            }

            foreach (Sea sea in this.seas) {
                sea.Update(gameTime, fishes, player);
            }

            player.Update(gameTime, ships, player);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            this.spriteBatch.DrawString(hudFont, score.ToString(), new Vector2(0f, 0f), Color.Black);

            foreach (Fish fish in fishes)
            {
                fish.Render(gameTime, this.spriteBatch);
            }

            foreach (Ship ship in ships)
            {
                ship.Render(gameTime, this.spriteBatch);
            }
            foreach (Sea sea in this.seas) {
                sea.Render(gameTime, this.spriteBatch);
            }

            player.Render(gameTime, this.spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

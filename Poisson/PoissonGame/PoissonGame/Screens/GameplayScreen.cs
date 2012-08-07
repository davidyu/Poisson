using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Poisson.Screens
{
    class GameplayScreen : GameScreen
    {
        Camera camera;

        int score = 0;
        SpriteFont hudFont;

        List<Entity> fishes;
        List<Entity> ships;
        List<Entity> seas;

        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();


        Fish player; //player will also be fishes[0] upon init

        public GameplayScreen()
        {
        }

        public override void LoadContent() 
        {
            camera = new Camera(0.5f);

            //this.projection = Matrix.CreateScale(SCREEN_WIDTH / 2560f, SCREEN_HEIGHT / 2400f, 1f);
            Random random = new Random();
            fishes = new List<Entity>();
            hudFont = this.ScreenManager.Game.Content.Load<SpriteFont>("HUDFont");

            ships = new List<Entity>();

            player = new Fish(new Vector2(0f, 0f), 0.0f, true); //player is Poisson and has different graphic than regular fishies
            fishes.Add(player); //player is always index 0

            seas = new List<Entity>();

            ships.Add(new Ship(new Vector2(100f, 50f), 0.0f));
            
            seas.Add(new Sea(new Vector2(0.0f, 150.0f), new Vector2(-10.0f, 0.0f), 0.39f));

            for (int i = 0; i < 9; i++) {
                fishes.Add(new Fish(new Vector2(random.Next(800), random.Next(480)), 0.0f, false)); //NEED TO INCLUDE MIniMUMS FOR THE SEA LATER
            }
                
            foreach (Fish fish in fishes) {
                fish.Initialise(this.ScreenManager.Game);
            }

            foreach (Ship ship in ships) {
                ship.Initialise(this.ScreenManager.Game);
            }

            foreach (Sea sea in seas) {
                sea.Initialise(this.ScreenManager.Game);
            }

            player.Initialise(this.ScreenManager.Game);
        }

        public override void UnloadContent() { }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                //this.ScreeExit();
            }

            score = (int)gameTime.TotalGameTime.TotalMilliseconds * 7;

            CheckCollisions();

            List<Fish> toRemove = new List<Fish>();
            foreach (Fish fish in this.fishes) {  
                fish.Update(gameTime, ships, player, this.camera);
                if (fish.State == Fish.EFishState.DEAD)
                    toRemove.Add(fish);
            }
            foreach (Fish fish in toRemove) {
                this.fishes.Remove(fish);
            }
            

            foreach (Ship ship in ships) {
                ship.Update(gameTime, fishes, player, this.camera);
            }

            foreach (Sea sea in this.seas) {
                sea.Update(gameTime, fishes, player, this.camera);
            }
    
        }

        public override void Draw(GameTime gameTime) 
        {
            this.ScreenManager.Game.GraphicsDevice.Clear(new Color(219, 237, 212));
            this.ScreenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.camera.Projection);

            this.ScreenManager.SpriteBatch.DrawString(hudFont, score.ToString(), new Vector2(0f, 0f), Color.Black);

            foreach (Fish fish in fishes) {
                fish.Render(gameTime, this.ScreenManager.SpriteBatch, this.camera);
            }

            foreach (Ship ship in ships) {
                ship.Render(gameTime, this.ScreenManager.SpriteBatch, this.camera);
            }

            foreach (Sea sea in this.seas) {
                sea.Render(gameTime, this.ScreenManager.SpriteBatch, this.camera);
            }

            this.ScreenManager.SpriteBatch.End();
        }

        private void CheckCollisions()
        {
            // right now we only use one ship. Consider refactoring and using SAP when time comes to use multiple ships
            //foreach (Fish fish in fishes) {
            //    if (fish.BoundingRect.Intersects((ships[0] as Ship).hookRect)) {

            //    }
            //}
        }


    }
}
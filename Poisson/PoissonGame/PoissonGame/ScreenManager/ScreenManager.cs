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
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.IO;


namespace Poisson
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        Stack<GameScreen> screens = new Stack<GameScreen>();

        bool isInitialized;
        bool traceEnabled;

        #endregion

        #region Properties

        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {
            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
        }


        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            this.screens.Peek().Update(gameTime);

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();
        }

        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            //Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }


        public override void Draw(GameTime gameTime)
        {
           screens.Peek().Draw(gameTime);
        }


        #endregion

        #region Public Methods

        public void PushScreen(GameScreen screen)
        {
            this.screens.Push(screen);
            screen.LoadContent();
        }

        public void PopScreen()
        {
            this.screens.Peek().UnloadContent();
            this.screens.Pop();

        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }


        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            //spriteBatch.Begin();

            //spriteBatch.Draw(blankTexture,
            //                 new Rectangle(0, 0, viewport.Width, viewport.Height),
            //                 Color.Black * alpha);

            //spriteBatch.End();
        }

        #endregion
    }
}

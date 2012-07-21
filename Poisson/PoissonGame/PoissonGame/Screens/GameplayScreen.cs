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


namespace Poisson
{
    /// <summary>
    /// This is the class the handle the entire game
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields/Properties


        SpriteFont font16px;
        SpriteFont font36px;

        Texture2D arrowTexture;
        Texture2D background;
        Texture2D controlstickBoundary;
        Texture2D controlstick;
        Texture2D beehiveTexture;
        Texture2D smokeButton;
        //ScoreBar smokeButtonScorebar;

        Vector2 controlstickStartupPosition;
        Vector2 controlstickBoundaryPosition;
        Vector2 smokeButtonPosition;
        Vector2 lastTouchPosition;

        bool isSmokebuttonClicked;
        bool drawArrow;
        bool drawArrowInterval;
        bool isInMotion;
        bool isAtStartupCountDown;
        bool isLevelEnd;
        bool levelEnded;
        bool isUserWon;
        bool userTapToExit;

        Dictionary<string, Animation> animations;

        int amountOfSoldierBee;
        int amountOfWorkerBee;
        int arrowCounter;

        //List<Beehive> beehives = new List<Beehive>();
        //List<Bee> bees = new List<Bee>();

        const string SmokeText = "Smoke";

        TimeSpan gameElapsed;
        TimeSpan startScreenTime;

        //BeeKeeper beeKeeper;
        //HoneyJar jar;
        //Vat vat;

        //DifficultyMode gameDifficultyLevel;

        public bool IsStarted
        {
            get
            {
                return !isAtStartupCountDown && !levelEnded;
            }
        }
       #endregion
    }
}
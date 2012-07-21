namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    class Ship : Entity
    {
        enum EHookState
        {
            UP,
            DOWN,
            RETRACTED
        } //maybe different hook types later

        enum EShipState
        {
            SEEKING, //movin' around
            WAITING, //moving before hooking
            HOOKING,  //throw dat bait Jimmy!
        }

        EHookState hookState;
        EShipState shipState;

        int timeToNextHook;

        const float movingVel = 5f;

        const float FRICTION = 0.99f;
        const float BORING_HOOK_VEL = 5f;
        const int BOTTOM_OF_SCREEN = 400; //worst constant ever.

        const int TOP_OF_ROD = 20;        //ugly constants to keep the hook in the right place.
        const int ROD_OFFSET = 227;
        const int ROD_OFFSET_FLIP = 10;

        private Rectangle _hookSpriteRect;
        private Rectangle _hookRect;

        public Rectangle hookRect
        {
            get
            {
                return new Rectangle((int)hookPos.X + (int)Pos.X,
                                     (int)Pos.Y + (int)hookPos.Y,
                                      this._hookRect.Width,
                                      this._hookRect.Width); //gross
            }
        }

        Vector2 hookPos;
        Texture2D hookSprite;
        Random rseed; //used for random interval hooking

        Vector2 shipDestination;

        public Ship()
            : base()
        {
        }

        public Ship(Vector2 pos, float orient)
            : base(pos, orient)
        {
        }

        public override void Initialise(Game game)
        {
            SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            hookSprite = game.Content.Load<Texture2D>("Art/spritesheet");
            this._hookSpriteRect = new Rectangle(0, 255, 16, 22);
            this.SpriteRect = new Rectangle(0, 0, 256, 164);
            this._hookRect = new Rectangle(0, 0, 40, 40);
            this.hookPos = new Vector2(ROD_OFFSET, 0);
            this.hookState = EHookState.RETRACTED;
            this.Vel = new Vector2(3.0f, 0.0f);
            this.Facing = true;
            this.timeToNextHook = 4000;
            this.rseed = new Random();
        }

        //need gameTime to reset timeToNextHook
        private void setShipToSeek(GameTime gameTime)
        {
            //while newDestination is not too close to this.Pos.X, generate newDest value
            float newDestination = rseed.Next(1, 4) * 200 - 100;
            //determine ships destination
            //if (newDestination < this.Pos.X) //FLIP THE SHIP TO FACE LEFT
            //{
            //    spriteEffects = SpriteEffects.FlipHorizontally;
            //    hookPos = new Vector2(ROD_OFFSET_FLIP, hookPos.Y);
            //}
            if (newDestination < this.Pos.X) //SET VELOCITY
            {
                this.Vel = new Vector2(-3.0f, 0.0f);
            }
            else
            {
                this.Vel = new Vector2(3.0f, 0.0f);
            }
            this.shipDestination = new Vector2((float)(newDestination), this.Pos.Y); //SET DESTINATION

            this.shipState = EShipState.SEEKING;
            //this.timeToNextHook = (int)gameTime.TotalGameTime.TotalMilliseconds + rseed.Next(0, 4000);
        }

        private void setShipToWait(GameTime gameTime)
        {
            this.Vel = new Vector2(0.0f, 0.0f);
            this.shipState = EShipState.WAITING;
            this.timeToNextHook = (int)gameTime.TotalGameTime.TotalMilliseconds + 100; //set the time to send the hook down!
        }

        private void setShipToHook()
        {
            float newHookDestination = (float)(200/3*(rseed.Next(0,3)+200));
            this.hookDestination = new Vector2((float)(newHookDestination), this.Pos.Y);
            this.shipState = EShipState.HOOKING;
            this.hookState = EHookState.DOWN;
        }



        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            switch (this.shipState) {
                case EShipState.SEEKING:
                    break;
                case EShipState.WAITING:
                    break;
                case EShipState.HOOKING:
                    break;
            }

            if (shipState == EShipState.SEEKING) {
                if (Math.Abs(this.Pos.X - this.shipDestination.X) < 10.0)
                {
                    setShipToWait(gameTime);
                }
            }

            if ((shipState == EShipState.WAITING) && (gameTime.TotalGameTime.TotalMilliseconds >= this.timeToNextHook)) {
                setShipToHook();
            }

            if (this.Pos.X < 0.0f || this.Pos.X > 600) { //turn around if hit edge
                this.Vel *= -1.0f;
                this.Facing = !this.Facing;
            }

            switch (this.hookState) {
                case EHookState.UP:
                    this.hookPos.Y -= BORING_HOOK_VEL;
                    if (this.hookPos.Y <= TOP_OF_ROD) {
                        this.hookState = EHookState.RETRACTED;
                        setShipToSeek(gameTime);
                    }
                    break;
                case EHookState.DOWN:
                    this.hookPos.Y += BORING_HOOK_VEL;
                    if (this.hookPos.Y >= BOTTOM_OF_SCREEN)
                        this.hookState = EHookState.UP;
                    break;
                case EHookState.RETRACTED:
                    break;
            }

            this.Pos += this.Vel;
            this.Vel *= FRICTION;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            SpriteEffects spriteEffects = new SpriteEffects();

            if (!Facing)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                hookPos = new Vector2(ROD_OFFSET_FLIP, hookPos.Y);
            }
            else
            {
                hookPos = new Vector2(ROD_OFFSET, hookPos.Y);
            }

            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            //batch.Draw(this.SpriteTexture, this.Pos, Color.White);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.4f);

            batch.Draw(this.hookSprite, this.Pos + this.hookPos,
                _hookSpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.3f);
        }
    }
}

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
        enum EHookState {
            UP,
            DOWN,
            RETRACTED
        } //maybe different hook types later

        enum EShipState {
            SEEKING, //movin' around
            HOOKING  //throw dat bait Jimmy!
        }

        EHookState hookState;
        EShipState shipState;

        int timeToNextHook;

        const float FRICTION = 0.99f;
        const float BORING_HOOK_VEL = 5f;
        const int   BOTTOM_OF_SCREEN = 400; //worst constant ever.

        const int TOP_OF_ROD = 20;        //ugly constants to keep the hook in the right place.
        const int ROD_OFFSET = 227;
        const int ROD_OFFSET_FLIP = 10;

        private Rectangle _hookSpriteRect;
        private Rectangle _hookRect;

        public Rectangle hookRect
        {
            get {
                return new Rectangle((int) hookPos.X + (int) Pos.X,
                                     (int) Pos.Y + (int) hookPos.Y,
                                      this._hookRect.Width,
                                      this._hookRect.Width); //gross
            }
        }

        Vector2 hookPos;
        Texture2D hookSprite;
        Random rseed; //used for random interval hooking

        public Ship() : base()
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
            this.shipState = EShipState.SEEKING;
            this.timeToNextHook = (int) gameTime.TotalGameTime.TotalMilliseconds + rseed.Next(0, 4000);
        }

        private void setShipToHook()
        {
            this.shipState = EShipState.HOOKING;
            this.hookState = EHookState.DOWN;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player)
        {
            if (shipState == EShipState.SEEKING) {
                this.Pos += this.Vel;
                this.Orient += this.AngVel;
            }

            if ((shipState == EShipState.SEEKING) && (gameTime.TotalGameTime.TotalMilliseconds >= this.timeToNextHook)) {
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
        }

        public override void Render(GameTime gameTime, SpriteBatch batch)
        {
            SpriteEffects spriteEffects = new SpriteEffects();

            if (!Facing) {
                spriteEffects = SpriteEffects.FlipHorizontally;
                hookPos = new Vector2(ROD_OFFSET_FLIP, hookPos.Y);
            } else {
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

namespace Poisson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Poisson.Entities;

    class Ship : Entity
    {
        enum EShipState
        {
            SEEKING, //movin' around
            WAITING, //moving before hooking
            HOOKING,  //throw dat bait Jimmy!
        }

        EShipState shipState;

        int timeToNextHook;

        const float movingVel = 6f;

        const float FRICTION = 0.99f;
        const float BORING_HOOK_VEL = 5f;
        const int BOTTOM_OF_SCREEN = 400; //worst constant ever.

        const int TOP_OF_ROD = 20;        //ugly constants to keep the hook in the right place.
        const int ROD_OFFSET = 227;
        const int ROD_OFFSET_FLIP = 10;

        private Hook hook;

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
            this.hook = new Hook();
            this.hook.Initialise(game);
            this.hook.HookState = Hook.EHookState.MOVING;

            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 0, 256, 164);

            this.Vel = new Vector2(3.0f, 0.0f);
            this.FacingLeft = true;
            //this.timeToNextHook = 4000;
            this.rseed = new Random();
            this.setShipToSeek();
        }

        //need gameTime to reset timeToNextHook
        private void setShipToSeek()
        {
            
            float newDestination = (rseed.Next(0, 4) * 300 + 200); //select 4 locaons (includes min value)
            while (Math.Abs(newDestination - this.Pos.X) < 10)
            {
                newDestination = (rseed.Next(0, 4) * 300 + 200);
            }
            Debug.WriteLine("Destination " + newDestination);
            //determine ships destination
            if (newDestination < this.Pos.X) //FLIP THE SHIP TO FACE LEFT
            {
                if (FacingLeft) //ACTUALLY FACING RIGHT
                {
                    FacingLeft = false;
                }
            }
            else
            {
                if (!FacingLeft)
                {
                    FacingLeft = true;
                }
            }

            if (newDestination < this.Pos.X) //SET VELOCITY
            {
                this.Vel = new Vector2(-movingVel, 0.0f);
            }
            else
            {
                this.Vel = new Vector2(movingVel, 0.0f);
            }
            this.shipDestination = new Vector2((float)(newDestination), this.Pos.Y); //SET DESTINATION

            this.shipState = EShipState.SEEKING;
        }

        private void setShipToWait(GameTime gameTime)
        {
            this.Vel = new Vector2(0.0f, 0.0f);
            this.shipState = EShipState.WAITING;
            this.timeToNextHook = (int)gameTime.TotalGameTime.TotalMilliseconds + 1000; //set the time to send the hook down!
        }

        private void setShipToHook()
        {
            float newHookDestination = (float)(200/3*(rseed.Next(0,4)+200)); //select 3 locations
            //this.hookDestination = new Vector2((float)(newHookDestination), this.Pos.Y);
            //this.hookVel = 
            this.shipState = EShipState.HOOKING;
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            if (!this.FacingLeft) {
                this.hook.Pos = new Vector2((float)ROD_OFFSET_FLIP+this.Pos.X, this.hook.Pos.Y);
            }
            else {
                this.hook.Pos = new Vector2((float)ROD_OFFSET+this.Pos.X, this.hook.Pos.Y);
            } 
            this.hook.Update(gameTime, entities, player, cam);

            switch (this.shipState) {
                case EShipState.SEEKING:
                    //this.Vel = new Vector2(1.0f, 0.0f);
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
                setShipToSeek();
                //setShipToHook();
            }

            //if (this.Pos.X < 0.0f || this.Pos.X > 1300) { //turn around if hit edge
            //    this.Vel *= -1.0f;
            //    this.FacingLeft = !this.FacingLeft;
            //}

<<<<<<< HEAD
=======
            if (!this.FacingLeft) {
                this.hook.Pos = new Vector2(ROD_OFFSET_FLIP, this.hook.Pos.Y);
            }
            else {
                this.hook.Pos = new Vector2(ROD_OFFSET, this.hook.Pos.Y);
            } 
            switch (this.hook.HookState) {
                case Hook.EHookState.MOVING:
                    Vector2 pos = this.hook.Pos;
                    pos.Y -= BORING_HOOK_VEL;
                    this.hook.Pos = pos;
                    if (pos.Y <= TOP_OF_ROD) {
                        this.hook.HookState = Hook.EHookState.RETRACTED;
                        setShipToSeek();
                    }
                    break;
                //case Hook.EHookState.DOWN:
                //    this.hookPos.Y += BORING_HOOK_VEL;
                //    if (this.hookPos.Y >= BOTTOM_OF_SCREEN)
                //        this.hookState = EHookState.UP;
                //    break;
                case Hook.EHookState.RETRACTED:
                    break;
            }

>>>>>>> a40f4d66d5b81f73305b44fc57654c39ab808f54
            this.Pos += this.Vel;
            //this.Vel *= FRICTION;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            this.hook.Render(gameTime, batch, cam);
            SpriteEffects spriteEffects = new SpriteEffects();

            if (!this.FacingLeft) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else {
            }
            if (!FacingLeft) {
                spriteEffects = SpriteEffects.FlipHorizontally;
                this.hook.Pos = new Vector2(ROD_OFFSET_FLIP, this.hook.Pos.Y);
            } else {
                this.hook.Pos = new Vector2(ROD_OFFSET, this.hook.Pos.Y);
            }

            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.4f);
        }
    }
}

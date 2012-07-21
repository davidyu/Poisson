namespace Poisson.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Poisson.Utility;

    class Ship : Entity
    {

        enum EShipState {
            SEEKING, //movin' around
            HOOKING  //throw dat bait Jimmy!
        }

        EShipState shipState;

        int timeToNextHook;

        const float FRICTION = 0.99f;
        const float BORING_HOOK_VEL = 5f;
        const int   BOTTOM_OF_SCREEN = 400; //worst constant ever.

        const int TOP_OF_ROD = 20;        //ugly constants to keep the hook in the right place.
        const int ROD_OFFSET = 227;
        const int ROD_OFFSET_FLIP = 10;

        private Entity hook;

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
            this.hook = new Hook();
            this.hook.Initialise(game);

            this.SpriteTexture = game.Content.Load<Texture2D>("Art/spritesheet");
            this.SpriteRect = new Rectangle(0, 0, 256, 164);

            this.Vel = new Vector2(3.0f, 0.0f);
            this.FacingLeft = true;
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
        }

        public override void Update(GameTime gameTime, List<Entity> entities, Entity player, Camera cam)
        {
            this.hook.Update(gameTime, entities, player, cam);

            switch (this.shipState) {
                case EShipState.SEEKING:
                    break;
                case EShipState.HOOKING:
                    break;
            }

            if ((shipState == EShipState.SEEKING) && (gameTime.TotalGameTime.TotalMilliseconds >= this.timeToNextHook)) {
                setShipToHook();
            }

            if (this.Pos.X < 0.0f || this.Pos.X > 600) { //turn around if hit edge
                this.Vel *= -1.0f;
                this.FacingLeft = !this.FacingLeft;
            }

            //switch (this.hookState) {
            //    case EHookState.UP:
            //        this.hookPos.Y -= BORING_HOOK_VEL;
            //        if (this.hookPos.Y <= TOP_OF_ROD) {
            //            this.hookState = EHookState.RETRACTED;
            //            setShipToSeek(gameTime);
            //        }
            //        break;
            //    case EHookState.DOWN:
            //        this.hookPos.Y += BORING_HOOK_VEL;
            //        if (this.hookPos.Y >= BOTTOM_OF_SCREEN)
            //            this.hookState = EHookState.UP;
            //        break;
            //    case EHookState.RETRACTED:
            //        break;
            //}

            this.Pos += this.Vel;
            this.Vel *= FRICTION;
        }

        public override void Render(GameTime gameTime, SpriteBatch batch, Camera cam)
        {
            this.hook.Render(gameTime, batch, cam);
            SpriteEffects spriteEffects = new SpriteEffects();

            if (!FacingLeft) {
                spriteEffects = SpriteEffects.FlipHorizontally;
                this.hook.Pos = new Vector2(ROD_OFFSET_FLIP, this.hook.Pos.Y);
            } else {
                this.hook.Pos = new Vector2(ROD_OFFSET, this.hook.Pos.Y);
            }

            Rectangle destRect = new Rectangle((int)this.Pos.X, (int)this.Pos.Y, (int)SpriteRect.Width, (int)SpriteRect.Height);
            //batch.Draw(this.SpriteTexture, this.Pos, Color.White);
            batch.Draw(this.SpriteTexture, this.Pos,
                this.SpriteRect, Color.White,
                this.Orient, new Vector2(0f, 0f), 1.0f, spriteEffects, 0.4f);
        }
    }
}

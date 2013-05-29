using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniXNAft.Items;

namespace MiniXNAft.Entities {
    public class Player : Mob, IInteractive, IMotionInteractive, IUpdateable {


        // Vector2 position { get; set; }
        private float _X;
        private float _Y;
        public float Velocity { get; set; }

        // Dimensions
        public int Width { get; set; }
        public int Height { get; set; }

        private Movement orientation;
        private bool moving = false;

        //
        public Item attackItem;
        public Item activeItem;
        public int stamina;
        public int staminaRecharge;
        public int staminaRechargeDelay;
        public int score;
        public int maxStamina = 10;
        private int onStairDelay;
        public int invulnerableTime = 0;

        // touch zones 
        private const int TouchZoneWidth = 80;
        private Rectangle leftTouchZone;
        private Rectangle topTouchZone;
        private Rectangle rightTouchZone;
        private Rectangle bottomTouchZone;

        // animation
        private double elapsedSinceLastFrame = 0;
        int CurrentFrame = 0;
        int TotalFrames = 2;
        double FrameDelay = .1D;

        enum Movement { None = 0, Down, Up, Right, Left };

        private Player() {
            Velocity = 50.0F;
            _X = _Y = x = y = 64 * 8;
            stamina = maxStamina;
        }


        public Player(Drawer drawer)
            : this() {
            this.Height = drawer.Height * GamePage.ScaleFactor; this.Width = drawer.Width * GamePage.ScaleFactor;
            // _X = x = Width / 2;
            //_Y = y = Height / 2;

            leftTouchZone = new Rectangle(0, 0, TouchZoneWidth, Height);
            topTouchZone = new Rectangle(0, 0, Width, TouchZoneWidth);
            rightTouchZone = new Rectangle(Width - TouchZoneWidth, 0, Width, Height);
            bottomTouchZone = new Rectangle(0, Height - TouchZoneWidth, Width, Height);
        }

        public void Interact(GamePadState gamePadState, TouchCollection touches) {
            foreach (TouchLocation tl in touches) {
                switch (tl.State) {
                    case TouchLocationState.Pressed:
                        orientation = guessMovement(tl);
                        break;
                    case TouchLocationState.Released:
                        moving = false;
                        break;
                }
            }
        }

        private Movement guessMovement(TouchLocation tl) {
            float x = tl.Position.X;
            float y = tl.Position.Y;
            Point p = new Point((int)x, (int)y);
            // System.Diagnostics.Debug.WriteLine("x:" + x + "y:" + y);
            if (leftTouchZone.Contains(p)) {
                // System.Diagnostics.Debug.WriteLine("Touch: Left");
                moving = true;
                return Movement.Left;
            }
            if (topTouchZone.Contains(p)) {
                // System.Diagnostics.Debug.WriteLine("Touch: Up");
                moving = true;
                return Movement.Up;
            }
            if (rightTouchZone.Contains(p)) {
                //  System.Diagnostics.Debug.WriteLine("Touch: Right");
                moving = true;
                return Movement.Right;
            }
            if (bottomTouchZone.Contains(p)) {
                //    System.Diagnostics.Debug.WriteLine("Touch: Down");
                moving = true;
                return Movement.Down;
            }
            // System.Diagnostics.Debug.WriteLine("Touch: None");
            return orientation;
        }

        public void InteractMotion(MotionReading motion, DisplayOrientation orientation) {

        }

        private void Animate(double elapsedTime) {

            elapsedSinceLastFrame += elapsedTime;

            while (elapsedSinceLastFrame > this.FrameDelay) {
                elapsedSinceLastFrame -= this.FrameDelay;
                this.CurrentFrame++;
                // Are we at the end?
                if (this.CurrentFrame == this.TotalFrames) {
                    this.CurrentFrame = 0;
                }
            }

        }

        public void Update(GameTime gameTime) {

            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (moving) {
                Animate(elapsedTime);
            }

            int xa = 0;
            int ya = 0;
            if (moving) {
                switch (orientation) {
                    case Movement.Left:
                        xa--;
                        _X -= (float)elapsedTime * Velocity;
                        break;
                    case Movement.Up:
                        ya--;
                        _Y -= (float)elapsedTime * Velocity;
                        break;
                    case Movement.Right:
                        xa++;
                        _X += (float)elapsedTime * Velocity;
                        break;
                    case Movement.Down:
                        ya++;
                        _Y += (float)elapsedTime * Velocity;
                        break;
                }
                if (isSwimming() && tickTime % 60 == 0) {
                    if (stamina > 0) {
                        stamina--;
                    } else {
                        hurt(this, 1, dir ^ 1);
                    }
                }

                if (staminaRechargeDelay % 2 == 0) {
                    move(xa, ya);
                }

                /*
                if (_X < 0) {
                    _X = 0F;
                }
                if (_X > Width - (16 * GamePage.ScaleFactor)) {
                    _X = Width - (16 * GamePage.ScaleFactor);
                }
                if (_Y < 0) {
                    _Y = 0;
                }
                if (_Y > Height - (16 * GamePage.ScaleFactor)) {
                    _Y = Height - (16 * GamePage.ScaleFactor);
                }
                */

                // update inherited from Entity
                x = (int)_X;
                y = (int)_Y;
               // System.Diagnostics.Debug.WriteLine("x:" + x + " _X:" + _X);
               // System.Diagnostics.Debug.WriteLine("y:" + y + " _Y:" + _Y);
            }
        }

        new public void Draw(Drawer drawer) {
            int spriteOffset = 0;
            switch (orientation) {
                case Movement.Left:
                    spriteOffset = 6;
                    break;
                case Movement.Up:
                    spriteOffset = 2;
                    break;
                case Movement.Right:
                    spriteOffset = 4;
                    break;
                case Movement.Down: break;
            }

            int flip1 = (walkDist >> 3) & 1;
            int flip2 = (walkDist >> 3) & 1;


            int xo = x - 8;
            int yo = y - 11;
            if (isSwimming()) {
                yo += 4;
                //int waterColor = Color.get(-1, -1, 115, 335);
                //if (tickTime / 8 % 2 == 0) {
                //waterColor = Color.get(-1, 335, 5, 115);
                //}
                //screen.render(xo + 0, yo + 3, 5 + 13 * 32, waterColor, 0);
                //screen.render(xo + 8, yo + 3, 5 + 13 * 32, waterColor, 1);
            }

            /*
		    if (attackTime > 0 && attackDir == 1) {
			    screen.render(xo + 0, yo - 4, 6 + 13 * 32,
					    Color.get(-1, 555, 555, 555), 0);
			    screen.render(xo + 8, yo - 4, 6 + 13 * 32,
					    Color.get(-1, 555, 555, 555), 1);
			    if (attackItem != null) {
				    attackItem.renderIcon(screen, xo + 4, yo - 4);
			    }
		    }
             * */



            //screen.render(xo + 8 * flip1, yo + 0, xt + yt * 32, col, flip1);
            //screen.render(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col, flip1);

            /*if (!isSwimming()) {
                screen.render(xo + 8 * flip2, yo + 8, xt + (yt + 1) * 32, col,
                        flip2);
                screen.render(xo + 8 - 8 * flip2, yo + 8, xt + 1 + (yt + 1) * 32,
                        col, flip2);
            }*/

            /*
            if (attackTime > 0 && attackDir == 2) {
                screen.render(xo - 4, yo, 7 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 1);
                screen.render(xo - 4, yo + 8, 7 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 3);
                if (attackItem != null) {
                    attackItem.renderIcon(screen, xo - 4, yo + 4);
                }
            }
            if (attackTime > 0 && attackDir == 3) {
                screen.render(xo + 8 + 4, yo, 7 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 0);
                screen.render(xo + 8 + 4, yo + 8, 7 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 2);
                if (attackItem != null) {
                    attackItem.renderIcon(screen, xo + 8 + 4, yo + 4);
                }
            }
            if (attackTime > 0 && attackDir == 0) {
                screen.render(xo + 0, yo + 8 + 4, 6 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 2);
                screen.render(xo + 8, yo + 8 + 4, 6 + 13 * 32,
                        Color.get(-1, 555, 555, 555), 3);
                if (attackItem != null) {
                    attackItem.renderIcon(screen, xo + 4, yo + 8 + 4);
                }
            }
            */

            /*
		    if (activeItem instanceof FurnitureItem) {
			    Furniture furniture = ((FurnitureItem) activeItem).furniture;
			    furniture.x = x;
			    furniture.y = yo;
			    furniture.render(screen);

		    }*/

            // drawer.Draw(xo + 8, yo + 8, 0 + spriteOffset, 14 + CurrentFrame * 2, 16, 16, Color.Pink);
            drawer.SetScaling(false);
            drawer.Draw(Width / 2 - 8, Height / 2 - 8, 0 + spriteOffset, 14 + CurrentFrame * 2, 16, 16, Color.Pink);
            drawer.SetScaling(true);
        }

        public bool payStamina(int cost) {
            if (cost > stamina)
                return false;
            stamina -= cost;
            return true;
        }
    }
}

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

namespace MiniXNAft.Entities {
    public class Player : IInteractive, IMotionInteractive, IUpdateable {

        private Drawer drawer;

        // Vector2 position { get; set; }
        float X { get; set; }
        float Y { get; set; }
        float Velocity { get; set; }

        // Dimensions
        public int Width { get; set; }
        public int Height { get; set; }

        private Movement orientation;
        private bool moving = false;

        // touch zones 
        private const int TouchZoneWidth = 80;
        private Rectangle leftTouchZone;
        private Rectangle topTouchZone;
        private Rectangle rightTouchZone;
        private Rectangle bottomTouchZone;

        // animation
        private double elapsedSinceLastFrame = 0;
        private bool playingAnimation = false;
        int CurrentFrame = 0;
        int TotalFrames = 2;
        double FrameDelay = .1D;

        enum Movement { None = 0, Down, Up, Right, Left };

        private Player() {
            Velocity = 50.0F;
            X = Y = 0;
        }


        public Player(Drawer drawer)
            : this() {
            this.drawer = drawer;
            this.Height = drawer.Width; this.Width = drawer.Height;
            X = Width / 2;
            Y = Height / 2;

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
            System.Diagnostics.Debug.WriteLine("x:" + x + "y:" + y);
            if (leftTouchZone.Contains(p)) {
                System.Diagnostics.Debug.WriteLine("Touch: Left");
                moving = true;
                return Movement.Left;
            }
            if (topTouchZone.Contains(p)) {
                System.Diagnostics.Debug.WriteLine("Touch: Up");
                moving = true;
                return Movement.Up;
            }
            if (rightTouchZone.Contains(p)) {
                System.Diagnostics.Debug.WriteLine("Touch: Right");
                moving = true;
                return Movement.Right;
            }
            if (bottomTouchZone.Contains(p)) {
                System.Diagnostics.Debug.WriteLine("Touch: Down");
                moving = true;
                return Movement.Down;
            }
            System.Diagnostics.Debug.WriteLine("Touch: None");
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

            if (moving) {
                switch (orientation) {
                    case Movement.Left:
                        X -= (float)elapsedTime * Velocity;
                        break;
                    case Movement.Up:
                        Y -= (float)elapsedTime * Velocity;
                        break;
                    case Movement.Right:
                        X += (float)elapsedTime * Velocity;
                        break;
                    case Movement.Down:
                        Y += (float)elapsedTime * Velocity;
                        break;
                }
                if (X < 0) {
                    X = 0F;
                }
                if (X > Width - (16 * GamePage.ScaleFactor)) {
                    X = Width - (16 * GamePage.ScaleFactor);
                }
                if (Y < 0) {
                    Y = 0;
                }
                if (Y > Height - (16 * GamePage.ScaleFactor)) {
                    Y = Height - (16 * GamePage.ScaleFactor);
                }
            }
        }

        public void Draw(SharedGraphicsDeviceManager graphics, SpriteBatch spriteBatch) {
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
            drawer.Draw((int)X, (int)Y, 0 + spriteOffset, 14 + CurrentFrame * 2, 16, 16, spriteBatch, Color.Pink);
        }
    }
}

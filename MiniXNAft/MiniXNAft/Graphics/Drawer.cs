using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Graphics {
    public class Drawer {
        private Microsoft.Xna.Framework.SharedGraphicsDeviceManager graphics;
        public Microsoft.Xna.Framework.Graphics.Texture2D spriteSheet;
        public int Height, Width;
        private int xoffset;
        private int yoffset;
        private bool doScaling = true;
        public SpriteFont font { get; set; }

        public Drawer(SharedGraphicsDeviceManager graphics, Texture2D spriteSheet) {
            this.graphics = graphics;
            this.spriteSheet = spriteSheet;
            Height = graphics.PreferredBackBufferHeight / GamePage.ScaleFactor;
            Width = graphics.PreferredBackBufferWidth / GamePage.ScaleFactor;
            ResetOffset();


        }

        public void Draw(int x, int y, int spritex, int spritey, SpriteBatch spriteBatch, Color color) {
            Draw(x, y, spritex, spritey, 8, 8, spriteBatch, color);
        }

        public void Draw(int x, int y, int sprite, SpriteBatch spriteBatch, Color color) {
            Draw(x, y, (sprite % 32), (sprite / 32), spriteBatch, color);
        }

        public void Draw(int x, int y, int sprite, SpriteBatch spriteBatch, Color color, SpriteEffects se) {
            Draw(x, y, (sprite % 32), (sprite / 32), 8, 8, spriteBatch, color, se);
        }

        public void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, SpriteBatch spriteBatch, Color color) {
            Draw(x, y, spritex, spritey, spritew, spriteh, spriteBatch, color, SpriteEffects.None);
        }

        public void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, SpriteBatch spriteBatch, Color color, SpriteEffects se) {
            // Disable smoothing
            // graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            if (doScaling) {
                spriteBatch.Draw(spriteSheet, new Rectangle((x - xoffset) * GamePage.ScaleFactor, (y - yoffset) * GamePage.ScaleFactor,
                    spritew * GamePage.ScaleFactor, spriteh * GamePage.ScaleFactor),
                    new Rectangle(spritex * 8, spritey * 8, spritew, spriteh), color, 0, new Vector2(), se, 0);
            } else {
                spriteBatch.Draw(spriteSheet, new Rectangle(x - xoffset, y - yoffset,
                    spritew * GamePage.ScaleFactor, spriteh * GamePage.ScaleFactor),
                    new Rectangle(spritex * 8, spritey * 8, spritew, spriteh), color, 0, new Vector2(), se, 0);
            }
        }

        internal void SetOffset(Vector2 off) {
            SetOffset((int)off.X, (int)off.Y);
        }

        internal Vector2 GetOffset() {
            return new Vector2(this.xoffset, this.yoffset);
        }

        internal void SetOffset(int xScroll, int yScroll) {
            xoffset = xScroll;
            yoffset = yScroll;
        }

        internal void ResetOffset() {
            xoffset = yoffset = 0;
        }

        public void SetScaling(bool val) {
            doScaling = val;
        }


        internal void DrawString(SpriteBatch sb, string p, Vector2 vector2, Color color) {
            sb.DrawString(font, p, new Vector2((vector2.X - xoffset) * GamePage.ScaleFactor, (vector2.Y - yoffset) * GamePage.ScaleFactor), color);
        }

        internal void DrawString(SpriteBatch spriteBatch, string p) {
            DrawString(spriteBatch, p, new Vector2(0, 0), Microsoft.Xna.Framework.Color.White);
        }
    }
}

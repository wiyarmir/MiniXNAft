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
        protected SharedGraphicsDeviceManager graphics;
        public Texture2D spriteSheet;
        public int Height, Width;
        protected int xoffset;
        protected int yoffset;
        protected bool doScaling = true;
        protected SpriteBatch spriteBatch;
        public SpriteFont font { get; set; }

        public Drawer(SharedGraphicsDeviceManager graphics, Texture2D spriteSheet) {
            this.graphics = graphics;
            this.spriteSheet = spriteSheet;
            Height = graphics.PreferredBackBufferHeight / GamePage.ScaleFactor;
            Width = graphics.PreferredBackBufferWidth / GamePage.ScaleFactor;
            ResetOffset();


        }

        public void Draw(int x, int y, int spritex, int spritey, Color color) {
            Draw(x, y, spritex, spritey, 8, 8, color);
        }

        public void Draw(int x, int y, int sprite, Color color) {
            Draw(x, y, (sprite % 32), (sprite / 32), color);
        }

        public void Draw(int x, int y, int sprite, Color color, SpriteEffects se) {
            Draw(x, y, (sprite % 32), (sprite / 32), 8, 8, color, se);
        }

        public void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, Color color) {
            Draw(x, y, spritex, spritey, spritew, spriteh, color, SpriteEffects.None);
        }

        virtual public void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, Color color, SpriteEffects se) {
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

        virtual internal void StartDrawing() {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            // FIXME use SpriteSortMode.Defered when not debugging 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

        }

        virtual internal void Init() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        virtual internal void EndDrawing() {
            spriteBatch.End();
        }

    }
}

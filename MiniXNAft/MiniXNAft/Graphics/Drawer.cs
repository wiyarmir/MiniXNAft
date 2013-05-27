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

        public Drawer(SharedGraphicsDeviceManager graphics, Texture2D spriteSheet) {
            this.graphics = graphics;
            this.spriteSheet = spriteSheet;
            Height = graphics.PreferredBackBufferHeight / GamePage.ScaleFactor;
            Width = graphics.PreferredBackBufferWidth / GamePage.ScaleFactor;
        }

        public void Draw(int x, int y, int spritex, int spritey, SpriteBatch spriteBatch, Color color) {
            Draw(x, y, spritex, spritey, 8, 8, spriteBatch, color);
        }

        public void Draw(int x, int y, int sprite, SpriteBatch spriteBatch, Color color) {
            Draw(x, y, sprite * 8, (sprite / 32) * 8, spriteBatch, color);
        }

        public void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, SpriteBatch spriteBatch, Color color) {
            spriteBatch.Draw(spriteSheet, new Rectangle(x, y, spritew * GamePage.ScaleFactor, spriteh * GamePage.ScaleFactor),
                new Rectangle(spritex * 8, spritey * 8, spritew, spriteh), color);
        }
    }
}

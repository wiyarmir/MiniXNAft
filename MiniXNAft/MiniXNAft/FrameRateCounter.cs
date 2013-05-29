using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MiniXNAft {
    public class FrameRateCounter   {

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FrameRateCounter() {
        }

        public  void Update(GameTime gameTime) {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1)) {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public  void Draw(GameTime gameTime,SpriteBatch spriteBatch,SpriteFont spriteFont) {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);


            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);

        }

        internal void Print(GameTime gameTime) {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);
            System.Diagnostics.Debug.WriteLine(fps);
        }
    }
}

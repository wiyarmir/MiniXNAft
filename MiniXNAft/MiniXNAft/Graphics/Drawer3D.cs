using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Graphics {
    public class Drawer3D : Drawer {


        public Drawer3D(SharedGraphicsDeviceManager graphics, Texture2D spriteSheet)
            : base(graphics, spriteSheet) {

        }

        public override void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, Color color, SpriteEffects se) {
            base.Draw(x, y, spritex, spritey, spritew, spriteh, color, se);
        }
    }
}

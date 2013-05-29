using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Levels.Tiles {
    class GrassTile : Tile {

        public GrassTile(int p)
            : base(p) {
            connectsToGrass = true;

        }

        public override void Draw(Drawer drawer,  Level level, int x, int y) {
            bool u = !level.getTile(x, y - 1).connectsToGrass;
            bool d = !level.getTile(x, y + 1).connectsToGrass;
            bool l = !level.getTile(x - 1, y).connectsToGrass;
            bool r = !level.getTile(x + 1, y).connectsToGrass;

            Color col = Color.LightGreen;
            Color transitionColor = col;//Color.LightGoldenrodYellow;

            if (!u && !l) {
                //screen.render(x * 16 + 0, y * 16 + 0, 0, col, 0);
                drawer.Draw(x * 16 + 0, y * 16 + 0, 0, Color.LightGreen);
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 0, (l ? 11 : 12) + (u ? 0 : 1) * 32, transitionColor);
            }

            if (!u && !r) {
                //screen.render(x * 16 + 8, y * 16 + 0, 1, col, 0);
                drawer.Draw(x * 16 + 8, y * 16 + 0, 0, Color.LightGreen);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 0, (r ? 13 : 12) + (u ? 0 : 1) * 32, transitionColor);
            }
            if (!d && !l) {
                //screen.render(x * 16 + 0, y * 16 + 8, 2, col, 0);
                drawer.Draw(x * 16 + 0, y * 16 + 8, 2, Color.LightGreen);
            } else {
                //screen.render(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1)
                //        * 32, transitionColor, 0);
                drawer.Draw(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1) * 32, transitionColor);
            }
            if (!d && !r) {
                //screen.render(x * 16 + 8, y * 16 + 8, 3, col, 0);
                drawer.Draw(x * 16 + 8, y * 16 + 8, 3, Color.LightGreen);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 8, (r ? 13 : 12) + (d ? 2 : 1) * 32, transitionColor);
            }


            //drawer.DrawString(spriteBatch, "(" + x + "," + y + ")", new Vector2(x * 16, y * 16), Color.Red);
            //drawer.Draw(x * 8 * GamePage.ScaleFactor, y * 8 * GamePage.ScaleFactor, 0, Color.LightGreen);
        }

    }
}

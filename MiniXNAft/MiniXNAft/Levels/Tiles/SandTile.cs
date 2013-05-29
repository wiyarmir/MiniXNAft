using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Levels.Tiles {
    class SandTile : Tile {
        public SandTile(int p)
            : base(p) {
            connectsToSand = true;
        }

        public override void Draw(Graphics.Drawer drawer, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Level level, int x, int y) {
            Color col = Color.Yellow;

            Color transitionColor = Color.Yellow;

            bool u = !level.getTile(x, y - 1).connectsToSand;
            bool d = !level.getTile(x, y + 1).connectsToSand;
            bool l = !level.getTile(x - 1, y).connectsToSand;
            bool r = !level.getTile(x + 1, y).connectsToSand;

            bool steppedOn = level.getData(x, y) > 0;

            if (!u && !l) {
                if (!steppedOn) {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 0, spriteBatch, col);
                } else {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 3 + 1 * 32, spriteBatch, col);
                }
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 0, (l ? 11 : 12) + (u ? 0 : 1) * 32, spriteBatch, transitionColor);
            }

            if (!u && !r) {
                drawer.Draw(x * 16 + 8, y * 16 + 0, 1, spriteBatch, col);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 0, (r ? 13 : 12) + (u ? 0 : 1) * 32, spriteBatch, transitionColor);
            }

            if (!d && !l) {
                drawer.Draw(x * 16 + 0, y * 16 + 8, 2, spriteBatch, col);
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1) * 32, spriteBatch, transitionColor);
            }

            if (!d && !r) {
                if (!steppedOn) {
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 3, spriteBatch, col);
                } else {
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 3 + 1 * 32, spriteBatch, col);
                }

            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 8, (r ? 13 : 12) + (d ? 2 : 1) * 32, spriteBatch, transitionColor);

            }

            /*
            drawer.Draw(x * 16 + 0, y * 16 + 0, 0, spriteBatch, col);
            drawer.Draw(x * 16 + 8, y * 16 + 0, 3, spriteBatch, col);
            drawer.Draw(x * 16 + 0, y * 16 + 8, 2, spriteBatch, col);
            drawer.Draw(x * 16 + 8, y * 16 + 8, 1, spriteBatch, col);
             * */
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniXNAft.Graphics;

namespace MiniXNAft.Levels.Tiles {
    class SandTile : Tile {
        public SandTile(int p)
            : base(p) {
            connectsToSand = true;
        }

        public override void Draw(Drawer drawer, Level level, int x, int y) {
            Color col = Color.Yellow;

            Color transitionColor = Color.Yellow;

            bool u = !level.getTile(x, y - 1).connectsToSand;
            bool d = !level.getTile(x, y + 1).connectsToSand;
            bool l = !level.getTile(x - 1, y).connectsToSand;
            bool r = !level.getTile(x + 1, y).connectsToSand;

            bool steppedOn = level.getData(x, y) > 0;

            if (!u && !l) {
                if (!steppedOn) {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 0, col);
                } else {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 3 + 1 * 32, col);
                }
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 0, (l ? 11 : 12) + (u ? 0 : 1) * 32, transitionColor);
            }

            if (!u && !r) {
                drawer.Draw(x * 16 + 8, y * 16 + 0, 1, col);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 0, (r ? 13 : 12) + (u ? 0 : 1) * 32, transitionColor);
            }

            if (!d && !l) {
                drawer.Draw(x * 16 + 0, y * 16 + 8, 2, col);
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1) * 32, transitionColor);
            }

            if (!d && !r) {
                if (!steppedOn) {
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 3, col);
                } else {
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 3 + 1 * 32, col);
                }

            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 8, (r ? 13 : 12) + (d ? 2 : 1) * 32, transitionColor);

            }

            /*
            drawer.Draw(x * 16 + 0, y * 16 + 0, 0, col);
            drawer.Draw(x * 16 + 8, y * 16 + 0, 3, col);
            drawer.Draw(x * 16 + 0, y * 16 + 8, 2, col);
            drawer.Draw(x * 16 + 8, y * 16 + 8, 1, col);
             * */
        }
    }
}

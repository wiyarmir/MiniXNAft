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

        }

        new public void Draw(Drawer drawer, SpriteBatch spriteBatch, Level level, int x, int y) {
            bool u = !level.getTile(x, y - 1).connectsToGrass;
            bool d = !level.getTile(x, y + 1).connectsToGrass;
            bool l = !level.getTile(x - 1, y).connectsToGrass;
            bool r = !level.getTile(x + 1, y).connectsToGrass;


            drawer.Draw(x, y, 0, spriteBatch, Color.Green);
        }

    }
}

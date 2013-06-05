using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniXNAft.Graphics;
using MiniXNAft.Entities;

namespace MiniXNAft.Levels.Tiles {
    class WaterTile : Tile {


        public WaterTile(int p)
            : base(p) {

            connectsToSand = true;
            connectsToWater = true;
        }

        Random wRandom = new Random();

        public override void Draw(Drawer drawer, Level level, int x, int y) {
            Color col = Color.Turquoise;

            drawer.Draw(x * 16 + 0, y * 16 + 0, wRandom.Next(3), col);
            drawer.Draw(x * 16 + 8, y * 16 + 0, 37, col);
            drawer.Draw(x * 16 + 0, y * 16 + 8, 37, col);
            drawer.Draw(x * 16 + 8, y * 16 + 8, wRandom.Next(3), col);
        }
        override public bool mayPass(Level level, int x, int y, Entity e) {
            return e.canSwim();
        }
    }
}

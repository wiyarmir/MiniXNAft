using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniXNAft.Levels.Tiles;

namespace MiniXNAft.Items.Resources {
    class PlantableResource : Resource {


        public PlantableResource(string name, int sprite, Color color, Tile targetTile, params Tile[] sourceTiles1)
            : base(name, sprite, color) {
        }

    }
}

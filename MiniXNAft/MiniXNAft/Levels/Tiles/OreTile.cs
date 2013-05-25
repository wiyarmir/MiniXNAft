using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniXNAft.Items.Resources;

namespace MiniXNAft.Levels.Tiles {
    class OreTile : Tile {
        private Resource resource;

        public OreTile(int p, Resource resource)
            : base(p) {
            this.resource = resource;
        }
    }
}

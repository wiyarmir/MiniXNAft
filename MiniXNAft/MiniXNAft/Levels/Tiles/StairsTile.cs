using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniXNAft.Levels.Tiles {
    class StairsTile:Tile {
        private bool p_2;

        public StairsTile(int p, bool p_2)
            : base(p) {
            this.p_2 = p_2;
        }
    }
}

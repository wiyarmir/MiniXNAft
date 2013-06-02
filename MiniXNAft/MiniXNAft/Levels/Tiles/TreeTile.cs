using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniXNAft.Entities;
using MiniXNAft.Items;

namespace MiniXNAft.Levels.Tiles {
    class TreeTile : GrassTile {
        public TreeTile(int p) : base(p) {
        }

        public override void Draw(Graphics.Drawer drawer, Level level, int x, int y) {
            base.Draw(drawer, level, x, y);

            Color col = Color.GreenYellow;
            Color barkCol1 = col;
            Color barkCol2 = col;

            bool u = level.getTile(x, y - 1) == this;
            bool l = level.getTile(x - 1, y) == this;
            bool r = level.getTile(x + 1, y) == this;
            bool d = level.getTile(x, y + 1) == this;
            bool ul = level.getTile(x - 1, y - 1) == this;
            bool ur = level.getTile(x + 1, y - 1) == this;
            bool dl = level.getTile(x - 1, y + 1) == this;
            bool dr = level.getTile(x + 1, y + 1) == this;

            if (u && ul && l) {
                drawer.Draw(x * 16 + 0, y * 16 + 0, 10 + 1 * 32, col, 0);
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 0, 9 + 0 * 32, col, 0);
            }
            if (u && ur && r) {
                drawer.Draw(x * 16 + 8, y * 16 + 0, 10 + 2 * 32, barkCol2, 0);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 0, 10 + 0 * 32, col, 0);
            }
            if (d && dl && l) {
                drawer.Draw(x * 16 + 0, y * 16 + 8, 10 + 2 * 32, barkCol2, 0);
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 8, 9 + 1 * 32, barkCol1, 0);
            }
            if (d && dr && r) {
                drawer.Draw(x * 16 + 8, y * 16 + 8, 10 + 1 * 32, col, 0);
            } else {
                drawer.Draw(x * 16 + 8, y * 16 + 8, 10 + 3 * 32, barkCol2, 0);
            }
        }
        override public bool mayPass(Level level, int x, int y, Entity e) {
            return false;
        }

        new public void hurt(Level level, int x, int y, Mob source, int dmg,
                int attackDir) {
            hurt(level, x, y, dmg);
        }

        new public bool interact(Level level, int xt, int yt, Player player, Item item, int attackDir) {
            /*
                if (item is ToolItem) {
                ToolItem tool = (ToolItem) item;
                if (tool.type == ToolType.axe) {
                    if (player.payStamina(4 - tool.level)) {
                        hurt(level, xt, yt, random.nextInt(10) + (tool.level) * 5
                                + 10);
                        return true;
                    }
                }
            }
             */
            return false;
        }

        private void hurt(Level level, int x, int y, int dmg) {
        }
    }
}

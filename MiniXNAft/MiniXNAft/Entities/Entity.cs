using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using MiniXNAft.Levels;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Items;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MiniXNAft.Entities {
    public class Entity {
        protected readonly Random random = new Random();
        public int x, y;
        public int xr = 6;
        public int yr = 6;
        public bool removed;
        public Level level;

        public void Draw(Drawer drawer) {
        }

        public void Update() {
        }

        public void remove() {
            removed = true;
        }

        public void init(Level level) {
            this.level = level;
        }

        public bool intersects(int x0, int y0, int x1, int y1) {
            return !(x + xr < x0 || y + yr < y0 || x - xr > x1 || y - yr > y1);
        }

        public bool blocks(Entity e) {
            return false;
        }

        public void hurt(Mob mob, int dmg, int attackDir) {
        }

        public void hurt(Tile tile, int x, int y, int dmg) {
        }

        public bool move(int xa, int ya) {
            if (xa != 0 || ya != 0) {
                bool stopped = true;
                if (xa != 0 && move2(xa, 0))
                    stopped = false;
                if (ya != 0 && move2(0, ya))
                    stopped = false;
                if (!stopped) {
                    int xt = x >> 4;
                    int yt = y >> 4;
                    level.getTile(xt, yt).steppedOn(level, xt, yt, this);
                }
                return !stopped;
            }
            return true;
        }

        protected bool move2(int xa, int ya) {
            if (xa != 0 && ya != 0)
                throw new NotSupportedException(
                        "Move2 can only move along one axis at a time!");

            int xto0 = ((x) - xr) >> 4;
            int yto0 = ((y) - yr) >> 4;
            int xto1 = ((x) + xr) >> 4;
            int yto1 = ((y) + yr) >> 4;

            int xt0 = ((x + xa) - xr) >> 4;
            int yt0 = ((y + ya) - yr) >> 4;
            int xt1 = ((x + xa) + xr) >> 4;
            int yt1 = ((y + ya) + yr) >> 4;
            bool blocked = false;
            for (int yt = yt0; yt <= yt1; yt++)
                for (int xt = xt0; xt <= xt1; xt++) {
                    if (xt >= xto0 && xt <= xto1 && yt >= yto0 && yt <= yto1)
                        continue;
                    level.getTile(xt, yt).bumpedInto(level, xt, yt, this);
                    if (!level.getTile(xt, yt).mayPass(level, xt, yt, this)) {
                        blocked = true;
                        return false;
                    }
                }
            if (blocked)
                return false;

            List<Entity> wasInside = level.getEntities(x - xr, y - yr, x + xr, y
                    + yr);
            List<Entity> isInside = level.getEntities(x + xa - xr, y + ya - yr, x
                    + xa + xr, y + ya + yr);
            for (int i = 0; i < isInside.Count; i++) {
                Entity e = isInside[i];
                if (e == this)
                    continue;

                e.touchedBy(this);
            }

            //FIXME
            // isInside.removeAll(wasInside);

            for (int i = 0; i < isInside.Count; i++) {
                Entity e = isInside[i];
                if (e == this)
                    continue;

                if (e.blocks(this)) {
                    return false;
                }
            }

            x += xa;
            y += ya;
            return true;
        }

        protected void touchedBy(Entity entity) {
        }

        public bool isBlockableBy(Mob mob) {
            return true;
        }

        public void touchItem(ItemEntity itemEntity) {
        }

        public bool canSwim() {
            return false;
        }

        public bool interact(Player player, Item item, int attackDir) {
            return item.interact(player, this, attackDir);
        }

        public bool use(Player player, int attackDir) {
            return false;
        }

        public int getLightRadius() {
            return 0;
        }
    }
}

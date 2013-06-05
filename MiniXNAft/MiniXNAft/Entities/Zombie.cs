using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniXNAft.Entities;
using Microsoft.Xna.Framework;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Graphics;
 
namespace MiniXNAft.Entities {
    public class Zombie : Mob {
        private int xa, ya;
        private int lvl;
        private int randomWalkTime = 0;

        public Zombie(int lvl) {
            this.lvl = lvl;
            x = random.Next(64 * 16);
            y = random.Next(64 * 16);
            health = maxHealth = lvl * lvl * 10;
        }

        override public void Update() {
            base.Update();

            if (level.player != null && randomWalkTime == 0) {
                int xd = level.player.x - x;
                int yd = level.player.y - y;
                if (xd * xd + yd * yd < 50 * 50) {
                    xa = 0;
                    ya = 0;
                    if (xd < 0)
                        xa = -1;
                    if (xd > 0)
                        xa = +1;
                    if (yd < 0)
                        ya = -1;
                    if (yd > 0)
                        ya = +1;
                }
            }

            int speed = tickTime & 1;
            if (!move(xa * speed, ya * speed) || random.Next(200) == 0) {
                randomWalkTime = 60;
                xa = (random.Next(3) - 1) * random.Next(2);
                ya = (random.Next(3) - 1) * random.Next(2);
            }
            if (randomWalkTime > 0)
                randomWalkTime--;
        }

        override public void Draw(Drawer drawer) {
            int xt = 0;
            int yt = 14;

            int flip1 = (walkDist >> 3) & 1;
            int flip2 = (walkDist >> 3) & 1;

            if (dir == 1) {
                xt += 2;
            }
            if (dir > 1) {

                flip1 = 0;
                flip2 = ((walkDist >> 4) & 1);
                if (dir == 2) {
                    flip1 = 1;
                }
                xt += 4;
                yt += ((walkDist >> 3) & 1) * 2;
            }

            int xo = x - 8;
            int yo = y - 11;

            Color col = new Color(50f / 555, 252f / 555, 10f / 555, 1f);
            if (hurtTime > 0) {
                col = new Color(1f, 1f, 1f, 1f);
            }

            drawer.Draw(xo + 8 * flip1, yo + 0, xt + yt * 32, col, (flip1 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            drawer.Draw(xo + 8 - 8 * flip1, yo + 0, xt + 1 + yt * 32, col, (flip1 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            drawer.Draw(xo + 8 * flip2, yo + 8, xt + (yt + 1) * 32, col, (flip2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            drawer.Draw(xo + 8 - 8 * flip2, yo + 8, xt + 1 + (yt + 1) * 32, col, (flip2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }

        override protected void touchedBy(Entity entity) {
            if (entity is Player) {
                entity.hurt(this, lvl + 1, dir);
            }
        }

        override protected void die() {
            base.die();

            int count = random.Next(2) + 1;
            for (int i = 0; i < count; i++) {
                //  level.add(new ItemEntity(new ResourceItem(Resource.cloth), x
                //          + random.Next(11) - 5, y + random.Next(11) - 5));
            }

            if (level.player != null) {
                level.player.score += 50 * lvl;
            }

        }
    }
}

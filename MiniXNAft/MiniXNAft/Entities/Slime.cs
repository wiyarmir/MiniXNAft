﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniXNAft.Entities;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiniXNAft.Entities {
    public class Slime : Mob {
        private int xa, ya;
        private int jumpTime = 0;
        private int lvl;

        public Slime(int lvl) {
            this.lvl = lvl;
            x = random.Next(64 * 16);
            y = random.Next(64 * 16);
            health = maxHealth = lvl * lvl * 5;

            col = slimeColors[random.Next(slimeColors.Count())];
        }

        private static Color[] slimeColors = { new Color(555F / 555F, 522F / 555F, 100F / 555F, 1F), Color.PaleVioletRed, Color.LawnGreen };

        Color col;

        override public void Update() {

            base.Update();


            int speed = 1;
            if (!move(xa * speed, ya * speed) || random.Next(40) == 0) {
                if (jumpTime <= -10) {
                    xa = (random.Next(3) - 1);
                    ya = (random.Next(3) - 1);

                    if (level.player != null) {
                        int xd = level.player.x - x;
                        int yd = level.player.y - y;
                        if (xd * xd + yd * yd < 50 * 50) {
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

                    if (xa != 0 || ya != 0)
                        jumpTime = 10;
                }
            }

            jumpTime--;
            if (jumpTime == 0) {
                xa = ya = 0;
            }
        }
        override protected void die() {
            base.die();

            int count = random.Next(2) + 1;
            for (int i = 0; i < count; i++) {
                // level.add(new ItemEntity(new ResourceItem(Resource.slime), x
                //         + random.nextInt(11) - 5, y + random.nextInt(11) - 5));
            }

            if (level.player != null) {
                level.player.score += 25 * lvl;
            }

        }

        override public void Draw(Drawer drawer) {
            int xt = 0;
            int yt = 20;

            int xo = x - 8;
            int yo = y - 11;

            if (jumpTime > 0) {
                xt += 2;
                yo -= 4;
            }

            /*if (lvl == 2)
                col = Color.get(-1, 100, 522, 555);
            if (lvl == 3)
                col = Color.get(-1, 111, 444, 555);
            if (lvl == 4)
                col = Color.get(-1, 000, 111, 224);

            if (hurtTime > 0) {
                col = Color.get(-1, 555, 555, 555);
            }*/

            drawer.Draw(xo, yo, xt, yt, 16, 16, col);

            /*
            drawer.Draw(xo + 0, yo + 0, xt + yt * 32, col, 0);
            drawer.Draw(xo + 8, yo + 0, xt + 1 + yt * 32, col, 0);
            drawer.Draw(xo + 0, yo + 8, xt + (yt + 1) * 32, col, 0);
            drawer.Draw(xo + 8, yo + 8, xt + 1 + (yt + 1) * 32, col, 0);
            */
        }

        override protected void touchedBy(Entity entity) {
            entity.hurt(this, lvl, dir);

        }
    }
}

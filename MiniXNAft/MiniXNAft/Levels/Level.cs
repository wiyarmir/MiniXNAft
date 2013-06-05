using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MiniXNAft.Entities;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Levels;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Levels {
    public class Level {
        Random random = new Random();
        public int w;
        public int h;
        public int monsterDensity;
        public List<List<Entity>> entitiesInTiles;

        public Player player;

        public byte[] tiles;
        public byte[] data;

        public int mobCount = 0;

        public List<Entity> entities = new List<Entity>();

        public Level(int w, int h) {

            this.w = w;
            this.h = h;
            byte[][] maps;
            //int level = 3;

            maps = LevelGen.createAndValidateTopMap(w, h);
            monsterDensity = 8;


            tiles = maps[0];
            data = maps[1];


            entitiesInTiles = new List<List<Entity>>(w * h);
            for (int i = 0; i < w * h; i++) {
                entitiesInTiles.Insert(i, new List<Entity>());
            }
        }


        public void renderBackground(Drawer drawer, int xScroll, int yScroll) {
            int xo = xScroll >> 4;
            int yo = yScroll >> 4;


            int w = (drawer.Width + 15) >> 4;
            int h = (drawer.Height + 15) >> 4;

            drawer.SetOffset(xScroll, yScroll);
            for (int y = yo; y <= h + yo; y++) {
                for (int x = xo; x <= w + xo; x++) {
                    getTile(x, y).Draw(drawer, this, x, y);
                }
            }
            drawer.ResetOffset();


            // drawer.DrawString(spriteBatch, "xs:" + xScroll + "ys:" + yScroll + "xo:" + xo + "yo:" + yo + "w:" + w + "h:" + h);
        }

        private List<Entity> rowSprites = new List<Entity>();

        public void renderSprites(Drawer drawer, int xScroll, int yScroll) {
            int xo = xScroll >> 4;
            int yo = yScroll >> 4;
            int w = (drawer.Width + 15) >> 4;
            int h = (drawer.Height + 15) >> 4;

            drawer.SetOffset(xScroll, yScroll);
            for (int y = yo; y <= h + yo; y++) {
                for (int x = xo; x <= w + xo; x++) {
                    if (x < 0 || y < 0 || x >= this.w || y >= this.h)
                        continue;
                    rowSprites.AddRange(entitiesInTiles[x + y * this.w]);
                }
                if (rowSprites.Count > 0) {
                    sortAndRender(drawer, rowSprites);
                }
                rowSprites.Clear();

            }
            drawer.ResetOffset();
        }

        private void sortAndRender(Drawer drawer, List<Entity> list) {
            // Collections.sort(list, spriteSorter);

            for (int i = 0; i < list.Count; i++) {
                list[i].Draw(drawer);
            }
        }

        public Tile getTile(int x, int y) {
            if (x < 0 || y < 0 || x >= w || y >= h)
                return Tile.rock;
            Tile ret = Tile.tiles[tiles[x + y * w]];
            return ret;
        }

        public void setTile(int x, int y, Tile t, int dataVal) {
            if (x < 0 || y < 0 || x >= w || y >= h)
                return;
            tiles[x + y * w] = t.id;
            data[x + y * w] = (byte)dataVal;
        }

        public int getData(int x, int y) {
            if (x < 0 || y < 0 || x >= w || y >= h)
                return 0;
            return data[x + y * w] & 0xff;
        }

        public void setData(int x, int y, int val) {
            if (x < 0 || y < 0 || x >= w || y >= h)
                return;
            data[x + y * w] = (byte)val;
        }

        public void add(Entity entity) {
            if (entity is Player) {
                player = (Player)entity;
            }
            entity.removed = false;
            entities.Add(entity);
            entity.init(this);

            insertEntity(entity.x >> 4, entity.y >> 4, entity);
        }

        public void remove(Entity e) {
            entities.Remove(e);
            int xto = e.x >> 4;
            int yto = e.y >> 4;
            removeEntity(xto, yto, e);
        }

        private void insertEntity(int x, int y, Entity e) {
            if (x < 0 || y < 0 || x >= w || y >= h) {
                return;
            }
            mobCount++;
            entitiesInTiles[x + y * w].Add(e);
        }

        private void removeEntity(int x, int y, Entity e) {
            if (x < 0 || y < 0 || x >= w || y >= h) {
                return;
            }
            mobCount--;
            entitiesInTiles[x + y * w].Remove(e);
        }

        public void trySpawn(int count) {
            for (int i = 0; i < count; i++) {
                Mob mob;

                int minLevel = 1;
                int maxLevel = 3;

                int lvl = random.Next(maxLevel - minLevel + 1) + minLevel;
                if (random.Next(2) == 0) {
                    mob = new Slime(lvl);
                } else {
                    mob = new Zombie(lvl);
                }

                if (mob.findStartPos(this)) {
                    System.Diagnostics.Debug.WriteLine("New mob:" + mob + " at " + mob.x + "," + mob.y);
                    mobCount++;
                    this.add(mob);
                }
            }
        }

        public void Update() {
            trySpawn(1);

            for (int i = 0; i < w * h / 50; i++) {
                int xt = random.Next(w);
                int yt = random.Next(h);
                getTile(xt, yt).Update(this, xt, yt);
            }

            for (int i = 0; i < entities.Count; i++) {
                Entity e = entities[i];
                int xto = e.x >> 4;
                int yto = e.y >> 4;

                e.Update();

                if (e.removed) {
                    entities.RemoveAt(i--);
                    removeEntity(xto, yto, e);
                } else {
                    int xt = e.x >> 4;
                    int yt = e.y >> 4;

                    if (xto != xt || yto != yt) {
                        removeEntity(xto, yto, e);
                        insertEntity(xt, yt, e);
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("Mob count:" + mobCount);
        }

        public List<Entity> getEntities(int x0, int y0, int x1, int y1) {
            List<Entity> result = new List<Entity>();
            int xt0 = (x0 >> 4) - 1;
            int yt0 = (y0 >> 4) - 1;
            int xt1 = (x1 >> 4) + 1;
            int yt1 = (y1 >> 4) + 1;
            for (int y = yt0; y <= yt1; y++) {
                for (int x = xt0; x <= xt1; x++) {
                    if (x < 0 || y < 0 || x >= w || y >= h)
                        continue;
                    List<Entity> entities = entitiesInTiles[x + y * this.w];
                    for (int i = 0; i < entities.Count; i++) {
                        Entity e = entities[i];
                        if (e.intersects(x0, y0, x1, y1))
                            result.Add(e);
                    }
                }
            }
            return result;
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using MiniXNAft.Entities;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Items;
using MiniXNAft.Graphics;
using MiniXNAft.Items.Resources;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Levels.Tiles {
    public class Tile {
        public static int tickCount = 0;
        protected Random random = new Random();

        public static Tile[] tiles = new Tile[256];
        public static Tile grass = new GrassTile(0);
        public static Tile rock = new RockTile(1);


        public static Tile water = new WaterTile(2);
        public static Tile flower = new FlowerTile(3);
        public static Tile tree = new TreeTile(4);
        public static Tile dirt = new DirtTile(5);
        public static Tile sand = new SandTile(6);
        public static Tile cactus = new CactusTile(7);
        public static Tile hole = new HoleTile(8);
        public static Tile treeSapling = new SaplingTile(9, grass, tree);
        public static Tile cactusSapling = new SaplingTile(10, sand, cactus);
        public static Tile farmland = new FarmTile(11);
        public static Tile wheat = new WheatTile(12);
        public static Tile lava = new LavaTile(13);
        public static Tile stairsDown = new StairsTile(14, false);
        public static Tile stairsUp = new StairsTile(15, true);
        public static Tile infiniteFall = new InfiniteFallTile(16);
        public static Tile cloud = new CloudTile(17);
        public static Tile hardRock = new HardRockTile(18);
        public static Tile ironOre = new OreTile(19, Resource.ironOre);
        public static Tile goldOre = new OreTile(20, Resource.goldOre);
        public static Tile gemOre = new OreTile(21, Resource.gem);
        public static Tile cloudCactus = new CloudCactusTile(22);



        public readonly byte id;

        public bool connectsToGrass = false;
        public bool connectsToSand = false;
        public bool connectsToLava = false;
        public bool connectsToWater = false;

        public Tile(int id) {
            this.id = (byte)id;
            if (tiles[id] != null)
                throw new NotImplementedException("Duplicate tile ids!");
            tiles[id] = this;
        }

        public virtual void Draw(Drawer drawer, SpriteBatch spriteBatch, Level level, int x, int y) {
            drawer.Draw(x * 16 + 0, y * 16 + 0, 21, spriteBatch, Color.White);
            drawer.Draw(x * 16 + 8, y * 16 + 0, 21, spriteBatch, Color.White);
            drawer.Draw(x * 16 + 0, y * 16 + 8, 21, spriteBatch, Color.White);
            drawer.Draw(x * 16 + 8, y * 16 + 8, 21, spriteBatch, Color.White);
        }

        virtual public bool mayPass(Level level, int x, int y, Entity e) {
            return true;
        }

        public int getLightRadius(Level level, int x, int y) {
            return 0;
        }

        public void hurt(Level level, int x, int y, Mob source, int dmg,
                int attackDir) {
        }

        public void bumpedInto(Level level, int xt, int yt, Entity entity) {
        }

        public void tick(Level level, int xt, int yt) {
        }

        public void steppedOn(Level level, int xt, int yt, Entity entity) {
        }

        public bool interact(Level level, int xt, int yt, Player player,
                Item item, int attackDir) {
            return false;
        }

        public bool use(Level level, int xt, int yt, Player player, int attackDir) {
            return false;
        }

        public bool connectsToLiquid() {
            return connectsToWater || connectsToLava;
        }
    }
}

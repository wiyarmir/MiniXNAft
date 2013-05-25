using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MiniXNAft.Entities;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Levels;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Items.Resources {
    public class Resource {
        public static Resource wood = new Resource("Wood", 1 + 4 * 32,
            //Color.get(-1, 200, 531, 430)
            Color.White
            );
        public static Resource stone = new Resource("Stone", 2 + 4 * 32,
            //Color.get(-1, 111, 333, 555)
            Color.White
                );
        public static Resource flower = new PlantableResource("Flower", 0 + 4 * 32,
            //Color.get(-1, 10, 444, 330)
                Color.White
                , Tile.flower, Tile.grass);
        public static Resource acorn = new PlantableResource("Acorn", 3 + 4 * 32,
            // Color.get(-1, 100, 531, 320)
               Color.White
                , Tile.treeSapling, Tile.grass);
        public static Resource dirt = new PlantableResource("Dirt", 2 + 4 * 32,
            //Color.get(-1, 100, 322, 432)
                Color.White
                , Tile.dirt, Tile.hole, Tile.water, Tile.lava);
        public static Resource sand = new PlantableResource("Sand", 2 + 4 * 32,
            //Color.get(-1, 110, 440, 550)
                Color.White
                , Tile.sand, Tile.grass, Tile.dirt);
        public static Resource cactusFlower = new PlantableResource("Cactus", 4 + 4 * 32,
            //Color.get(-1, 10, 40, 50)
                Color.White
                , Tile.cactusSapling, Tile.sand);
        public static Resource seeds = new PlantableResource("Seeds", 5 + 4 * 32,
            //Color.get(-1, 10, 40, 50)
                Color.White
                , Tile.wheat, Tile.farmland);
        public static Resource wheat = new Resource("Wheat", 6 + 4 * 32,
            //Color.get( -1, 110, 330, 550)
            Color.White
            );
        public static Resource bread = new FoodResource("Bread", 8 + 4 * 32,
            //Color.get(-1, 110, 330, 550)
                Color.White
                , 2, 5);
        public static Resource apple = new FoodResource("Apple", 9 + 4 * 32,
            //Color.get(-1, 100, 300, 500)
                Color.White
                , 1, 5);

        public static Resource coal = new Resource("COAL", 10 + 4 * 32,
            // Color.get(-1, 000, 111, 111)
             Color.White
            );
        public static Resource ironOre = new Resource("I.ORE", 10 + 4 * 32,
            //    Color.get(-1, 100, 322, 544)
                 Color.White
                );
        public static Resource goldOre = new Resource("G.ORE", 10 + 4 * 32,
            //  Color.get(-1, 110, 440, 553)
                 Color.White
                );
        public static Resource ironIngot = new Resource("IRON", 11 + 4 * 32,
            //   Color.get(-1, 100, 322, 544)
                 Color.White
                );
        public static Resource goldIngot = new Resource("GOLD", 11 + 4 * 32,
            //  Color.get(-1, 110, 330, 553)
                 Color.White
                );

        public static Resource slime = new Resource("SLIME", 10 + 4 * 32,
            //   Color.get(-1, 10, 30, 50)
                 Color.White
                );
        public static Resource glass = new Resource("glass", 12 + 4 * 32,
            // Color.get(-1, 555, 555, 555)
                 Color.White
                );
        public static Resource cloth = new Resource("cloth", 1 + 4 * 32,
            //  Color.get(-1, 25, 252, 141)
             Color.White
                );
        public static Resource cloud = new PlantableResource("cloud", 2 + 4 * 32,
            // Color.get(-1, 222, 555, 444)
                 Color.White
                , Tile.cloud, Tile.infiniteFall);
        public static Resource gem = new Resource("gem", 13 + 4 * 32,
            // Color.get(-1, 101, 404, 545)
             Color.White
            );

        public readonly String name;
        public readonly int sprite;
        public readonly Color color;

        public Resource(String name, int sprite, Color color) {
            if (name.Length > 6)
                throw new NotSupportedException("Name cannot be longer than six characters!");
            this.name = name;
            this.sprite = sprite;
            this.color = color;
        }

        public bool interactOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir) {
            return false;
        }
    }
}

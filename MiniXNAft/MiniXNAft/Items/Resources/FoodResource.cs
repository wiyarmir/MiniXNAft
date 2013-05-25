using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MiniXNAft.Levels.Tiles;
using MiniXNAft.Levels;
using MiniXNAft.Entities;

namespace MiniXNAft.Items.Resources {
    class FoodResource : Resource {
        private int heal;
        private int staminaCost;
        public FoodResource(String name, int sprite, Color color, int heal,
            int staminaCost)
            : base(name, sprite, color) {
            
            this.heal = heal;
            this.staminaCost = staminaCost;
        }

        new public bool interactOn(Tile tile, Level level, int xt, int yt,
            Player player, int attackDir) {
            if (player.health < player.maxHealth && player.payStamina(staminaCost)) {
                player.heal(heal);
                return true;
            }
            return false;
        }
    }
}

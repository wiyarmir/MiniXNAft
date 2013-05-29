using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniXNAft.Levels.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniXNAft.Entities;

namespace MiniXNAft.Levels.Tiles {
    class RockTile : Tile {

        public RockTile(int p)
            : base(p) {

        }

        public override void Draw(Graphics.Drawer drawer, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Level level, int x, int y) {
            Color col = Color.SandyBrown;
            Color transitionColor = Color.SandyBrown;

            bool u = level.getTile(x, y - 1) != this;
            bool d = level.getTile(x, y + 1) != this;
            bool l = level.getTile(x - 1, y) != this;
            bool r = level.getTile(x + 1, y) != this;

            bool ul = level.getTile(x - 1, y - 1) != this;
            bool dl = level.getTile(x - 1, y + 1) != this;
            bool ur = level.getTile(x + 1, y - 1) != this;
            bool dr = level.getTile(x + 1, y + 1) != this;

            if (!u && !l) {
                if (!ul) {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 0, spriteBatch, col);
                } else {
                    drawer.Draw(x * 16 + 0, y * 16 + 0, 7 + 0 * 32, spriteBatch, transitionColor, 
                        SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically); // 3
                }
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 0, (l ? 6 : 5) + (u ? 2 : 1)* 32, spriteBatch, transitionColor, 
                    SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically); // 3
            }
            if (!u && !r) {
                if (!ur) {
                    drawer.Draw(x * 16 + 8, y * 16 + 0, 1, spriteBatch, col);
                } else {
                    drawer.Draw(x * 16 + 8, y * 16 + 0, 8 + 0 * 32, spriteBatch, transitionColor, 
                        SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically);//3
                }
            } else
                drawer.Draw(x * 16 + 8, y * 16 + 0, (r ? 4 : 5) + (u ? 2 : 1) * 32, spriteBatch, transitionColor, 
                    SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically); //3

            if (!d && !l) {
                if (!dl) {
                    drawer.Draw(x * 16 + 0, y * 16 + 8, 2, spriteBatch, col);
                } else {
                    drawer.Draw(x * 16 + 0, y * 16 + 8, 7 + 1 * 32, spriteBatch, transitionColor, 
                        SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically);//3
                }
            } else {
                drawer.Draw(x * 16 + 0, y * 16 + 8, (l ? 6 : 5) + (d ? 0 : 1)  * 32, spriteBatch, transitionColor, 
                    SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically);//3
            }
            if (!d && !r) {
                if (!dr){
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 3, spriteBatch, col);
                }else{
                    drawer.Draw(x * 16 + 8, y * 16 + 8, 8 + 1 * 32, spriteBatch, transitionColor, 
                        SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically);//3
                }
            } else{
                drawer.Draw(x * 16 + 8, y * 16 + 8, (r ? 4 : 5) + (d ? 0 : 1) * 32, spriteBatch, transitionColor, 
                    SpriteEffects.FlipHorizontally|SpriteEffects.FlipVertically);//3
            }

            /*
            drawer.Draw(x * 16 + 0, y * 16 + 0, 37, spriteBatch, col);
            drawer.Draw(x * 16 + 8, y * 16 + 0, 37, spriteBatch, col);
            drawer.Draw(x * 16 + 0, y * 16 + 8, 37, spriteBatch, col);
            drawer.Draw(x * 16 + 8, y * 16 + 8, 37, spriteBatch, col);
             * */
        }

       override public bool mayPass(Level level, int x, int y, Entity e) {
            return false;
        }
    }
}

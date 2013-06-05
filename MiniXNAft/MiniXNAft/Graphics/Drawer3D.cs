using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MiniXNAft.Graphics {
    public class Drawer3D : Drawer {


        BasicEffect basicEffect;
        Vector3 cameraPosition = new Vector3(0, 50, 50);
        Vector3 cameraFront = new Vector3(0, 0, -1);

        public Drawer3D(SharedGraphicsDeviceManager graphics, Texture2D spriteSheet)
            : base(graphics, spriteSheet) {

            basicEffect = new BasicEffect(graphics.GraphicsDevice) {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
        }

        public override void Draw(int x, int y, int spritex, int spritey, int spritew, int spriteh, Color color, SpriteEffects se) {
            if (doScaling) {
                spriteBatch.Draw(spriteSheet, new Rectangle((x - xoffset) * GamePage.ScaleFactor, (y - yoffset) * GamePage.ScaleFactor,
                    spritew * GamePage.ScaleFactor, spriteh * GamePage.ScaleFactor),
                    new Rectangle(spritex * 8, spritey * 8, spritew, spriteh), color, 0, new Vector2(), se, 0);
            } else {
                spriteBatch.Draw(spriteSheet, new Rectangle(x - xoffset, y - yoffset,
                    spritew * GamePage.ScaleFactor, spriteh * GamePage.ScaleFactor),
                    new Rectangle(spritex * 8, spritey * 8, spritew, spriteh), color, 0, new Vector2(), se, 0);
            }
        }

        override internal void StartDrawing() {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            Viewport viewport = graphics.GraphicsDevice.Viewport;


            Matrix view = Matrix.CreateLookAt(cameraPosition,
                                              cameraPosition + cameraFront,
                                              Vector3.Up);

            Matrix offset = Matrix.CreateTranslation(-0.5f, -0.5f, -1f);

            //Matrix projection = offset* Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);

            Matrix projection = offset * Matrix.CreatePerspectiveOffCenter(0, viewport.Width, viewport.Height, 0, 1F, 1000F);


            basicEffect.Projection = projection;
            // basicEffect.World = Matrix.CreateRotationX(MathHelper.ToRadians(60)) * Matrix.CreateScale(1, 1, 0);

            // FIXME use SpriteSortMode.Defered when not debugging 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone, basicEffect);

            // spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, 
            //     RasterizerState.CullClockwise, basicEffect);
        }

        override internal void Init() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        internal override void EndDrawing() {
            spriteBatch.End();
        }

    }
}

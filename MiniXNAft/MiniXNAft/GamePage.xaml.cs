using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiniXNAft.Entities;
using Microsoft.Xna.Framework.Input;
using MiniXNAft.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MiniXNAft.Levels;

namespace MiniXNAft {
    public partial class GamePage : PhoneApplicationPage {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        Texture2D spriteSheet;
        Player player;
        private SharedGraphicsDeviceManager graphics;
        private Drawer drawer;
        Slime slime;
        Level level;

        public const int ScaleFactor = 4;
        private FrameRateCounter fr;

        public GamePage() {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            this.graphics = SharedGraphicsDeviceManager.Current;


            Initialize();


        }

        private void Initialize() {
            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            fr = new FrameRateCounter();


        }

        private void LoadContent() {

            // set landscape
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);


            spriteSheet = this.contentManager.Load<Texture2D>("icons");
            drawer = new Drawer(graphics, spriteSheet);


            drawer.font = this.contentManager.Load<SpriteFont>("UI");

            player = new Player(drawer);

            //
            level = new Levels.Level(128, 128);
            level.add(player);

            slime = new Slime(3);
            slime.level = level;
            slime.findStartPos(slime.level);

            slime.x = player.x - 10;
            slime.y = player.y - 10;

            level.add(slime);
            // Start the timer
            timer.Start();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            graphics.GraphicsDevice.SetSharingMode(true);

            LoadContent();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            graphics.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e) {

            fr.Update(new GameTime(e.TotalTime, e.ElapsedTime));

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            TouchCollection touches = TouchPanel.GetState();

            player.Interact(gamePadState, touches);
            player.Update(new GameTime(e.TotalTime, e.ElapsedTime));
            slime.Update();
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e) {


            int xScrolldp = player.x - drawer.Width / 2;
            int yScrolldp = player.y - (drawer.Height - 8) / 2;
            if (xScrolldp < 16)
                xScrolldp = 16;
            if (yScrolldp < 16)
                yScrolldp = 16;
            if (xScrolldp > level.w * 16 - drawer.Width - 16)
                xScrolldp = level.w * 16 - drawer.Width - 16;
            if (yScrolldp > level.h * 16 - drawer.Height - 16)
                yScrolldp = level.h * 16 - drawer.Height - 16;



            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            // FIXME use SpriteSortMode.Defered when not debugging 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            level.renderBackground(drawer, spriteBatch, xScrolldp, yScrolldp);

            drawer.SetOffset(xScrolldp, yScrolldp);
            slime.Draw(drawer, spriteBatch);
            drawer.ResetOffset();
            
            player.Draw(drawer, spriteBatch);


            //fr.Draw(new GameTime(e.TotalTime, e.ElapsedTime), spriteBatch,drawer.font);
            fr.Print(new GameTime(e.TotalTime, e.ElapsedTime));
            spriteBatch.End();

        }

    }
}
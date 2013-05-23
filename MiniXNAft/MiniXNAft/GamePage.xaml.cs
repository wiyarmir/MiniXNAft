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

namespace MiniXNAft {
    public partial class GamePage : PhoneApplicationPage {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        Texture2D spriteSheet;
        Player player;
        private SharedGraphicsDeviceManager graphics;
        private Drawer drawer;

        public const int ScaleFactor = 3;

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

            // set landscape
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;

        }

        private void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);


            spriteSheet = this.contentManager.Load<Texture2D>("icons");
            drawer = new Drawer(graphics, spriteSheet);


            player = new Player(drawer);
            
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
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            TouchCollection touches = TouchPanel.GetState();

            player.Interact(gamePadState, touches);
            player.Update(new GameTime(e.TotalTime, e.ElapsedTime));
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e) {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            player.Draw(graphics, spriteBatch);

            spriteBatch.End();

        }

    }
}
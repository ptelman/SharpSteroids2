using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpSteroids.Base.Model.Objects;
using SharpSteroids.Base.Model;

namespace SharpSteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D shipTexture;
        private Texture2D asteroidTexture;
        private Texture2D shootTexture;
        private Ship Ship;

        float timer = 0.3f;
        float TIMER = 0.3f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Resources";
            this.Ship = GameObjects.Ship;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.shipTexture = Content.Load<Texture2D>("Textures\\spaceship");
            this.shootTexture = Content.Load<Texture2D>("Textures\\torpedo");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.Right))
            {
                Ship.Angle += 0.1f;
            }
            if (currentState.IsKeyDown(Keys.Left))
            {
                Ship.Angle -= 0.1f;
            }
            if (currentState.IsKeyDown(Keys.Up))
            {
                Ship.MoveForwards();
            }
            else
            {
                Ship.Move();
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);

            spriteBatch.Begin();

            //draw ship
            Rectangle sourceRectangle = new Rectangle(0, 0, shipTexture.Width, shipTexture.Height);
            Vector2 origin = new Vector2(shipTexture.Width / 2, shipTexture.Height / 2);
            spriteBatch.Draw(shipTexture, new Vector2(Ship.Coordinates.x, Ship.Coordinates.y), sourceRectangle, Color.White, Ship.Angle, origin, 0.5f, SpriteEffects.None, 1);

            DrawShoots(gameTime);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawShoots(GameTime gameTime)
        {
            if (IsTimeToFireShoot(gameTime))
            {
                var shoot = new Shoot(new Model.Coordinates(Ship.Coordinates.x, Ship.Coordinates.y));
                shoot.Angle = Ship.Angle;
                GameObjects.Shoots.Add(shoot);
            }

            foreach(var item in GameObjects.Shoots)
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, shootTexture.Width, shootTexture.Height);
                Vector2 origin = new Vector2(shootTexture.Width / 2, shootTexture.Height / 2);
                spriteBatch.Draw(shootTexture, new Vector2(item.Coordinates.x, item.Coordinates.y), sourceRectangle, Color.White, item.Angle, origin, 0.05f, SpriteEffects.None, 1);

                item.MoveInDirectionByOne();
            }
        }

        private bool IsTimeToFireShoot(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                timer = TIMER;
                return true;
            }
            return false;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpSteroids.Base.Model;
using SharpSteroids.Base.Model.Objects;
using SharpSteroids.Controller;
using SharpSteroids.Model;
using SharpSteroids.Model.Enum;
using System;
using System.Collections.Generic;

namespace SharpSteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D shipTexture;
        private Texture2D asteroidTexture;
        private Texture2D shootTexture;
        private SpriteFont Font1;
        private RemoteController remoteController;

        private int score = 0;
        private float shootTimer = 0.3f;
        private float asteroidTimer = 1f;
        private float baseShootTIMER = 0.3f;
        private float baseAsteroidTIMER = 1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            GameSharedItems.windowWidth = graphics.PreferredBackBufferWidth;
            GameSharedItems.windowHeight = graphics.PreferredBackBufferHeight;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Resources";
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
            this.remoteController = new RemoteController();
            this.remoteController.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.shipTexture = Content.Load<Texture2D>("Textures\\ship");
            this.shootTexture = Content.Load<Texture2D>("Textures\\torpedo");
            this.asteroidTexture = Content.Load<Texture2D>("Textures\\asteroid");
            this.Font1 = Content.Load<SpriteFont>("Font");

            GameSharedItems.Ship = new Ship(new Coordinates(300, 300), shipTexture.Width, shipTexture.Height);
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

            if (remoteController.lastDirection == Directions.Right)
            {
                GameSharedItems.Ship.Angle += 0.05f;
            }
            if (remoteController.lastDirection == Directions.Left)
            {
                GameSharedItems.Ship.Angle -= 0.05f;
            }
            if (remoteController.lastDirection == Directions.Up)
            {
                GameSharedItems.Ship.MoveForwards();
            }
            GameSharedItems.Ship.Move();

            DetectShipCollisionWithAsteroid();
            DetectShootsCollisionWithAsteroids();
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
            spriteBatch.Draw(shipTexture, new Vector2(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y), sourceRectangle, Color.White, GameSharedItems.Ship.Angle, origin, GameSharedItems.shipScale, SpriteEffects.None, 1);

            DrawShoots(gameTime);
            DrawAsteroids(gameTime);

            spriteBatch.DrawString(Font1, $"Score: {score}", new Vector2(5, 5), Color.Black);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DetectShootsCollisionWithAsteroids()
        {
            List<Asteroid> asteroids = new List<Asteroid>();
            List<Shoot> shoots = new List<Shoot>();

            foreach (var asteroid in GameSharedItems.Asteroids)
            {
                foreach (var shoot in GameSharedItems.Shoots)
                {
                    var distance = GetDistanceBetweenCoordinates(asteroid.Coordinates, shoot.Coordinates);
                    if (distance < 30)
                    {
                        asteroids.Add(asteroid);
                        shoots.Add(shoot);
                        score++;
                    }
                }
            }
            foreach (var asteroid in asteroids)
            {
                GameSharedItems.Asteroids.Remove(asteroid);
            }
            foreach (var shoot in shoots)
            {
                GameSharedItems.Shoots.Remove(shoot);
            }
        }

        private void DetectShipCollisionWithAsteroid()
        {
            foreach (var asteroid in GameSharedItems.Asteroids)
            {
                var distance = GetDistanceBetweenCoordinates(GameSharedItems.Ship.Coordinates, asteroid.Coordinates);
                if (distance <= 40)
                {
                    //ship collided with asteroid
                }
            }
        }

        private void DrawAsteroids(GameTime gameTime)
        {
            if (IsTimeToAddAsteroid(gameTime))
            {
                var asteroid = new Asteroid(asteroidTexture.Width, asteroidTexture.Height);
                asteroid.Angle = 1f;
                GameSharedItems.Asteroids.Add(asteroid);
            }

            foreach (var item in GameSharedItems.Asteroids)
            {
                item.Move();
                Rectangle sourceRectangle = new Rectangle(0, 0, asteroidTexture.Width, asteroidTexture.Height);
                Vector2 origin = new Vector2(asteroidTexture.Width / 2, asteroidTexture.Height / 2);

                spriteBatch.Draw(asteroidTexture, new Vector2(item.Coordinates.x, item.Coordinates.y), sourceRectangle, Color.White, item.Angle, origin, GameSharedItems.asteroidScale, SpriteEffects.None, 1);
            }
        }

        private void DrawShoots(GameTime gameTime)
        {
            if (IsTimeToFireShoot(gameTime))
            {
                var shoot = new Shoot(new Model.Coordinates(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y), shootTexture.Width, shootTexture.Height);
                shoot.Angle = GameSharedItems.Ship.Angle;
                GameSharedItems.Shoots.Add(shoot);
            }

            foreach (var item in GameSharedItems.Shoots)
            {
                item.Move();
                //HERDASFAWFAWVAWEFAWERAWER HEIGHT/2!!!!!!!!!
                Rectangle sourceRectangle = new Rectangle(0, 0, shootTexture.Width, shootTexture.Height / 2);
                Vector2 origin = new Vector2(shootTexture.Width / 2, shootTexture.Height / 2);

                spriteBatch.Draw(shootTexture, new Vector2(item.Coordinates.x, item.Coordinates.y), sourceRectangle, Color.White, item.Angle, origin, GameSharedItems.shootScale, SpriteEffects.None, 1);
            }
        }

        private bool IsTimeToAddAsteroid(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            asteroidTimer -= elapsed;
            if (asteroidTimer < 0)
            {
                asteroidTimer = baseAsteroidTIMER;
                return true;
            }
            return false;
        }

        private bool IsTimeToFireShoot(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            shootTimer -= elapsed;
            if (shootTimer < 0)
            {
                shootTimer = baseShootTIMER;
                return true;
            }
            return false;
        }

        private float GetDistanceBetweenCoordinates(Coordinates item1, Coordinates item2)
        {
            return (float)Math.Sqrt(Math.Pow((item1.x - item2.x), 2) + Math.Pow((item1.y - item2.y), 2));
        }
    }
}
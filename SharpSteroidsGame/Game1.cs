using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpSteroids.Base.Model;
using SharpSteroids.Base.Model.Objects;
using SharpSteroids.Controller;
using SharpSteroids.Model;
using SharpSteroids.Model.Enum;

namespace SharpSteroids
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private Texture2D asteroidTexture;
        private float asteroidTimer = 0.1f;
        private float baseAsteroidTIMER = 1f;
        private readonly float baseShootTIMER = 0.6f;
        private SpriteFont Font1;
        private readonly GraphicsDeviceManager graphics;
        private int maxScore;
        private RemoteController remoteController;

        private int score;
        private Texture2D shipTexture;
        private Texture2D shootTexture;
        private Texture2D background;
        private float shootTimer = 0.3f;
        private SpriteBatch spriteBatch;
        private ParticleEngine particleEngine;

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
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            remoteController = new RemoteController();
            remoteController.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shipTexture = Content.Load<Texture2D>("Textures\\ship");
            shootTexture = Content.Load<Texture2D>("Textures\\torpedo");
            asteroidTexture = Content.Load<Texture2D>("Textures\\asteroid");
            background = Content.Load<Texture2D>("Textures\\background");
            Font1 = Content.Load<SpriteFont>("Font");

            GameSharedItems.Ship = new Ship(new Coordinates(300, 300), shipTexture.Width, shipTexture.Height);

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("Textures\\circle"));
            textures.Add(Content.Load<Texture2D>("Textures\\star"));
            this.particleEngine = new ParticleEngine(textures, new Vector2(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            if (IsTimeToAddAsteroid(gameTime))
            {
                var asteroid = new Asteroid(asteroidTexture.Width, asteroidTexture.Height);
                asteroid.Angle = 1f;
                GameSharedItems.Asteroids.Add(asteroid);
            }

            if (IsTimeToFireShoot(gameTime))
            {
                var shoot = new Shoot(
                    new Coordinates(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y),
                    shootTexture.Width, shootTexture.Height);
                shoot.Angle = GameSharedItems.Ship.Angle;
                GameSharedItems.Shoots.Add(shoot);
            }

            if (remoteController.lastDirection == Directions.Right)
                GameSharedItems.Ship.Angle += 0.05f;
            if (remoteController.lastDirection == Directions.Left)
                GameSharedItems.Ship.Angle -= 0.05f;
            if (remoteController.lastDirection == Directions.Up)
                GameSharedItems.Ship.MoveForwards();
            GameSharedItems.Ship.Move();

            particleEngine.EmitterLocation = new Vector2(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y);
            particleEngine.Update();

            DetectShipCollisionWithAsteroid();
            DetectShootsCollisionWithAsteroids();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Bisque);

            spriteBatch.Begin();

            var sourceRectangle = new Rectangle(0, 0, GameSharedItems.windowWidth, GameSharedItems.windowHeight);
            spriteBatch.Draw(background, sourceRectangle, Color.White);

            particleEngine.Draw(spriteBatch);

            //draw ship
            sourceRectangle = new Rectangle(0, 0, shipTexture.Width, shipTexture.Height);
            var origin = new Vector2(shipTexture.Width / 2, shipTexture.Height / 2);
            spriteBatch.Draw(shipTexture,
                new Vector2(GameSharedItems.Ship.Coordinates.x, GameSharedItems.Ship.Coordinates.y), sourceRectangle,
                Color.White, GameSharedItems.Ship.Angle, origin, GameSharedItems.shipScale, SpriteEffects.None, 0f);

            DrawShoots(gameTime);
            DrawAsteroids(gameTime);

            spriteBatch.DrawString(Font1, $"Score: {score}", new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(Font1, $"Max score: {maxScore}", new Vector2(150, 5), Color.Black);
            
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            GameSharedItems.Asteroids = new List<Asteroid>();
            GameSharedItems.Shoots = new List<Shoot>();
            if (score > maxScore)
                maxScore = score;

            score = 0;
            GameSharedItems.Ship = new Ship(new Coordinates(300, 300), shipTexture.Width, shipTexture.Height);
            baseAsteroidTIMER = 1f;
        }

        private void DetectShootsCollisionWithAsteroids()
        {
            var asteroids = new List<Asteroid>();
            var shoots = new List<Shoot>();

            foreach (var asteroid in GameSharedItems.Asteroids)
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
            foreach (var asteroid in asteroids)
                GameSharedItems.Asteroids.Remove(asteroid);
            foreach (var shoot in shoots)
                GameSharedItems.Shoots.Remove(shoot);
        }

        private void DetectShipCollisionWithAsteroid()
        {
            foreach (var asteroid in GameSharedItems.Asteroids)
            {
                var distance = GetDistanceBetweenCoordinates(GameSharedItems.Ship.Coordinates, asteroid.Coordinates);
                if (distance <= 40)
                    ResetGame();
            }
        }

        private void DrawAsteroids(GameTime gameTime)
        {
            foreach (var item in GameSharedItems.Asteroids)
            {
                item.Move();
                var sourceRectangle = new Rectangle(0, 0, asteroidTexture.Width, asteroidTexture.Height);
                var origin = new Vector2(asteroidTexture.Width / 2, asteroidTexture.Height / 2);

                spriteBatch.Draw(asteroidTexture, new Vector2(item.Coordinates.x, item.Coordinates.y), sourceRectangle,
                    Color.White, item.Angle, origin, GameSharedItems.asteroidScale, SpriteEffects.None, 1);
            }
        }

        private void DrawShoots(GameTime gameTime)
        {
            foreach (var item in GameSharedItems.Shoots)
            {
                item.Move();
                //HERDASFAWFAWVAWEFAWERAWER HEIGHT/2!!!!!!!!!
                var sourceRectangle = new Rectangle(0, 0, shootTexture.Width, shootTexture.Height / 2);
                var origin = new Vector2(shootTexture.Width / 2, shootTexture.Height / 2);

                spriteBatch.Draw(shootTexture, new Vector2(item.Coordinates.x, item.Coordinates.y), sourceRectangle,
                    Color.White, item.Angle, origin, GameSharedItems.shootScale, SpriteEffects.None, 1);
            }
        }

        private bool IsTimeToAddAsteroid(GameTime gameTime)
        {
            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            asteroidTimer -= elapsed;
            if (asteroidTimer < 0)
            {
                baseAsteroidTIMER *= 0.99f;
                asteroidTimer = baseAsteroidTIMER;
                return true;
            }
            return false;
        }

        private bool IsTimeToFireShoot(GameTime gameTime)
        {
            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
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
            return (float) Math.Sqrt(Math.Pow(item1.x - item2.x, 2) + Math.Pow(item1.y - item2.y, 2));
        }
    }
}
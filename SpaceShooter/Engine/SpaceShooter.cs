using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

// TODO
// sounds
// explosions
// asteroids spawning out of map
// collision detection for CollisionBox class
// removing asteroids and lasers traveling out of map
// damage instead of instant death
// levels

namespace SpaceShooter
{
    /// <summary>
    /// This is the main games class.
    /// </summary>
    public class SpaceShooter : Game
    {
        /// <summary>
        /// Stores the graphics device manager.
        /// </summary>
        GraphicsDeviceManager Graphics;
        /// <summary>
        /// Stores the sprite batch object.
        /// </summary>
        Renderer SpriteBatch;
        /// <summary>
        /// Treats the screen as a rectangle (use to check if objects have left the screen).
        /// </summary>
        Rectangle Screen;
        /// <summary>
        /// Stores the game font.
        /// </summary>
        private SpriteFont GameFont;
        /// <summary>
        /// Stores the background texture.
        /// </summary>
        private Texture2D BackgroundTexture;
        /// <summary>
        /// Stores the idle texture of the main ship.
        /// </summary>
        private Texture2D ShipIdleTexture;
        /// <summary>
        /// Stores the ignition texture of the main ship.
        /// </summary>
        private Texture2D ShipIgnitionTexture;
        /// <summary>
        /// Stores the asteroid texture.
        /// </summary>
        private Texture2D AsteroidTexture;
        /// <summary>
        /// Stores the laser texture.
        /// </summary>
        private Texture2D LaserTexture;
        /// <summary>
        /// Stores the line texture.
        /// </summary>
        private Texture2D LineTexture;
        /// <summary>
        /// Stores the background music.
        /// </summary>
        private SoundEffect Music;
        private SoundEffect ExplosionEffect;
        private SoundEffect ShootEffect;
        private SoundEffect NoShootEffect;
        private SoundEffectInstance CurrentMusic;
        /// <summary>
        /// Is the game getting debugged?
        /// </summary>
        private bool Debug;
        /// <summary>
        /// Stores the game volume.
        /// </summary>
        private float Volume;
        /// <summary>
        /// Stores the level number
        /// </summary>
        // private int Level = 0;
        /// <summary>
        /// Stores the current scene.
        /// </summary>
        // private Scene CurrentScene;
        /// <summary>
        /// Stores the scene player.
        /// </summary>
        private Player Player;
        /// <summary>
        /// A linked list of asteroids within the scene.
        /// </summary>
        private List<Asteroid> Asteroids = new List<Asteroid>();
        /// <summary>
        /// Stores the main Quadtree that is to be used for collision.
        /// </summary>
        // private Quadtree MainQuad;
        /// <summary>
        /// Stores the previous keyboard state to check if buttons are being held down.
        /// </summary>
        private KeyboardState OldState;
        /// <summary>
        /// Creates a new seeded random.
        /// </summary>
        private Random Rand = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Constructs the game instance.
        /// </summary>
        /// <param name="Width">The width of the window.</param>
        /// <param name="Height">The height of the window.</param>
        public SpaceShooter(int Width, int Height, bool FullScreen=false, bool Debug=false, float Volume=1)
        {
            // Creates our graphics device manager.
            Graphics = new GraphicsDeviceManager(this);
            // Sets the game in full screen mode.
            Graphics.IsFullScreen = FullScreen;
            // Sets the debug flag.
            this.Debug = Debug;
            // Sets the volume.
            this.Volume = Volume;
            // Sets the content root directory.
            Content.RootDirectory = "Content";
            // Sets the window size.
            SetWindowSize(Width, Height);
        }

        /// <summary>
        /// Used to initialise any game objects.
        /// </summary>
        protected override void Initialize()
        {
            // Initializes the parent class.
            base.Initialize();
        }

        /// <summary>
        /// Used to load all of the game content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new Renderer(GraphicsDevice);
            // loads the game font.
            GameFont = Content.Load<SpriteFont>("Font");
            // Loads up the textures.
            BackgroundTexture = Content.Load<Texture2D>("background");
            ShipIdleTexture = Content.Load<Texture2D>("ship_idle");
            ShipIgnitionTexture = Content.Load<Texture2D>("ship_ignition");
            AsteroidTexture = Content.Load<Texture2D>("asteroid");
            LaserTexture = Content.Load<Texture2D>("laser_torpedo");
            LineTexture = Content.Load<Texture2D>("line");
            // Loads the sound effects.
            Music = Content.Load<SoundEffect>("background_music");
            ExplosionEffect = Content.Load<SoundEffect>("shoot");
            ShootEffect = Content.Load<SoundEffect>("shoot");
            NoShootEffect = Content.Load<SoundEffect>("no_shoot");
            // Creates an instance of the music for control.
            CurrentMusic = Music.CreateInstance();
            CurrentMusic.Volume = Volume;
            CurrentMusic.Pitch = 0;
            CurrentMusic.Pan = 0;
            // We want to start the game after we have loaded all textures.
            StartGame();
        }

        /// <summary>
        /// Used to unload all of the game content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Used to update the game state on every frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime GameTime)
        {
            // Gets the new keyboard state.
            KeyboardState State = Keyboard.GetState();
            // Restart game.
            if (State.IsKeyDown(Keys.R) && !OldState.IsKeyDown(Keys.R)) StartGame();
            // Enable debug.
            if (State.IsKeyDown(Keys.P) && !OldState.IsKeyDown(Keys.P)) Debug = !Debug;
            // Stores the old keyboard state.
            OldState = State;
            // Updates the scene player.
            Player.Update(GameTime);
            // Clamps the ship position to the scene.
            Player.Position = new Vector2(Helper.Clamp(Player.Position.X, 0, Graphics.PreferredBackBufferWidth), Helper.Clamp(Player.Position.Y, 0, Graphics.PreferredBackBufferHeight));
            // Iterates through the scene asteroids
            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                // Updates the scene asteroid.
                Asteroids[i].Update(GameTime);
                // Checks if the player has collided with an asteroid.
                if (Player.Collision(Asteroids[i]))
                {
                    // New level.
                    StartGame();
                }
                // Checks if the asteroid has left the screen.
                if (!Screen.Intersects(Asteroids[i].BoundingBox))
                {
                    // Removes the entity from the managed entities.
                    Asteroids.RemoveAt(i);
                    // Gets the next asteroid.
                    continue;
                }
                // Iterates through the player lasers.
                for (int y = Player.Lasers.Count - 1; y >= 0; y--)
                {
                    // Checks if the laser has left teh screen.
                    if (!Screen.Intersects(Player.Lasers[y].BoundingBox))
                    {
                        // Removes the entity from the managed entities.
                        Player.Lasers.RemoveAt(y);
                        // Gets the next laser.
                        continue;
                    }
                    // Checks for a collision.
                    if (Player.Lasers[y].Collision(Asteroids[i]))
                    {
                        Asteroids[i].Explosion.Play(Volume, 0, 0);
                        // Removes the entity from the managed entities.
                        Asteroids.RemoveAt(i);
                        // Removes the entity from the managed entities.
                        Player.Lasers.RemoveAt(y);
                        // Breaks from the loop
                        break;
                    }
                }
            }
            // Checks the asteroid count.
            if (Asteroids.Count == 0)
            {
                // New level.
                StartGame();
            }
            // Calls the update method of the parent class.
            base.Update(GameTime);
        }

        /// <summary>
        /// This method is used to draw a frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clears and sets the device background color.
            GraphicsDevice.Clear(Color.Black);
            // Starts drawing a scene
            SpriteBatch.Begin();
            // Draws the background.
            SpriteBatch.Draw(BackgroundTexture, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Color.White);
            // Draws the scene player.
            Player.Draw(SpriteBatch);
            // SpriteBatch.Draw(Player.Texture, Player.Position, null, Color.White, Player.Rotation, Player.Origin, Player.Scale, SpriteEffects.None, 0);
            // Iterates through the scene asteroids
            foreach (Asteroid Asteroid in Asteroids)
            {
                // Draws the asteroid.
                Asteroid.Draw(SpriteBatch);
                // SpriteBatch.Draw(Asteroid.Texture, Asteroid.Position, null, Color.White, Asteroid.Rotation, Asteroid.Origin, Asteroid.Scale, SpriteEffects.None, 0);
            }
            // Prints the level number.
            // SpriteBatch.DrawString(GameFont, "Level: " + Level, new Vector2(10, 10), Color.White);
            // Prints the number of asteroids in the scene.
            SpriteBatch.DrawString(GameFont, "Asteroids: " + Asteroids.Count, new Vector2(10, 10), Color.White);
            // Prints the player speed.
            SpriteBatch.DrawString(GameFont, "Speed: " + Player.SpeedMultiplier, new Vector2(10, 30), Color.White);
            // Prints the player fuel.
            SpriteBatch.DrawString(GameFont, "Fuel: " + Player.Fuel, new Vector2(10, 50), Color.White);
            // Prints the player ammunition.
            SpriteBatch.DrawString(GameFont, "Ammunition: " + Player.Ammunition, new Vector2(10, 70), Color.White);
            // Checks if the game is getting debugged.
            if (Debug)
            {
                SpriteBatch.DrawString(GameFont, "Scene Entities: " + (Player.Lasers.Count + Asteroids.Count + 1), new Vector2(10, Graphics.PreferredBackBufferHeight - 60), Color.White);
                SpriteBatch.DrawString(GameFont, "Player collision box: " + Player.BoundingBox, new Vector2(10, Graphics.PreferredBackBufferHeight - 40), Color.White);
                SpriteBatch.DrawString(GameFont, "Player position: " + Player.Position, new Vector2(10, Graphics.PreferredBackBufferHeight - 20), Color.White);
                // Draws the player's collision box.
                SpriteBatch.DrawBox(LineTexture, Player.BoundingBox, Color.Red, 2);
                // Iterates through the scene asteroids
                foreach (Asteroid Asteroid in Asteroids)
                {
                    // Draws the asteroid collision box.
                    SpriteBatch.DrawBox(LineTexture, Asteroid.BoundingBox, Color.Red, 2);
                }
                // Iterates through the scene lasers
                foreach (Laser Laser in Player.Lasers)
                {
                    // Draws the laser collision box.
                    SpriteBatch.DrawBox(LineTexture, Laser.BoundingBox, Color.Red, 2);
                }
            }
            // Ends the scene drawing.
            SpriteBatch.End();
            // Calls the parent drawing method.
            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the window size.
        /// </summary>
        /// <param name="Width">The new width of the window.</param>
        /// <param name="Height">The new height of the window.</param>
        private void SetWindowSize(int Width, int Height)
        {
            // Sets the back buffer width.
            Graphics.PreferredBackBufferWidth = Width;
            // Sets the back buffer height.
            Graphics.PreferredBackBufferHeight = Height;
            // Applies the changes to the back buffer.
            Graphics.ApplyChanges();
            // Sets the screen rectangle.
            Screen = new Rectangle(0, 0, Width, Height);
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        private void StartGame()
        {
            // Creates a new Quadtree to be used for collision.
            // MainQuad = new Quadtree(0, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight));
            // Stops the current music
            CurrentMusic.Stop();
            // Creates a new instance of the music to avoid an exception
            CurrentMusic = Music.CreateInstance();
            // Clears current asteroids
            Asteroids.Clear();
            // Creates a new player.
            Player = new Player(ShipIdleTexture, ShipIgnitionTexture, LaserTexture, ShootEffect, NoShootEffect, new Vector2((Graphics.PreferredBackBufferWidth / 2), (Graphics.PreferredBackBufferHeight / 2)), 0, 0.2f, 100);
            // Spawns the asteroids.
            SpawnAsteroids(5);
            // Plays the background music.
            CurrentMusic.Play();
            // Next level.
            // Level++;
        }

        /// <summary>
        /// Spawns asteroids in the scene at arbitrary locations.ss
        /// </summary>
        /// <param name="Count">The amount of asteroids to spawn.</param>
        public void SpawnAsteroids(int Count)
        {
            // Iterates through the asteroid count.
            for (int i = 0; i < Count; i++)
            {
                // The position for the asteroid to face.
                Vector2 Direction = new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight / 2);
                // Generates the position of the asteroid.
                Vector2 Position = new Vector2(Rand.Next(0, Graphics.PreferredBackBufferWidth), Rand.Next(0, Graphics.PreferredBackBufferHeight));

                // Calculates the difference vector.
                Vector2 difference = Direction - Position;
                // Calculates the rotation of the line.
                float angle = (float)Math.Atan2(difference.Y, difference.X);
                // Creates a new asteroid.
                Asteroids.Add(new Asteroid(AsteroidTexture, ExplosionEffect, new Vector2(Rand.Next(0, Graphics.PreferredBackBufferWidth), Rand.Next(0, Graphics.PreferredBackBufferHeight)), angle, (float)Rand.NextDouble(), Rand.Next(0, 100)));
            }
        }
    }
}

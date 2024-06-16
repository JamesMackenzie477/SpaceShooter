using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    /// <summary>
    /// A player within the game scene.
    /// </summary>
    class Player : Entity
    {
        /// <summary>
        /// Stores the idle texture of the player.
        /// </summary>
        public Texture2D IdleTexture;
        /// <summary>
        /// Stores the moving texture of the player.
        /// </summary>
        public Texture2D MovingTexture;
        /// <summary>
        /// Stores the laser texture.
        /// </summary>
        public Texture2D LaserTexture;
        /// <summary>
        /// Stores the player speed.
        /// </summary>
        // public float Speed;
        /// <summary>
        /// Stores the player speed multiplier.
        /// </summary>
        public float SpeedMultiplier = 10;
        /// <summary>
        /// Stores the player fuel.
        /// </summary>
        public float Fuel = 1000;
        /// <summary>
        /// The players laser ammunition.
        /// </summary>
        public int Ammunition = 10;
        /// <summary>
        /// Stores the reload cooldown (in seconds).
        /// </summary>
        public float Cooldown = 1;
        /// <summary>
        /// Stores the time left until the player can shoot.
        /// </summary>
        public float Reloading = 0;
        /// <summary>
        /// Stores the lasers fired by the player.
        /// </summary>
        public List<Laser> Lasers = new List<Laser>();

        /// <summary>
        /// Stores the player sound effects.
        /// </summary>
        public SoundEffect Shoot;
        public SoundEffect NoAmmo;

        /// <summary>
        /// Stores the previous input states.
        /// </summary>
        private MouseState OldState;

        /// <summary>
        /// Creates a new player in the scene.
        /// </summary>
        /// <param name="IdleTexture">The idle texture to apply to the player when the player is not moving.</param>
        /// <param name="MovingTexture">The moving texture to apply to the player when the player is moving.</param>
        /// <param name="LaserTexture">The texture to apply to the lasers.</param>
        /// <param name="Position">The position of which the player originates.</param>
        /// <param name="Rotation">The rotation radian of the player.</param>
        /// <param name="Scale">The scale of the player from 0 to 1.</param>
        /// <param name="Speed">The speed of the player.</param>
        public Player(Texture2D IdleTexture, Texture2D MovingTexture, Texture2D LaserTexture, SoundEffect Shoot, SoundEffect NoAmmo, Vector2 Position, float Rotation, float Scale, float Speed) : base(IdleTexture, Position, Rotation, Scale)
        {
            // Sets the idle texture of the player.
            this.IdleTexture = IdleTexture;
            // Sets the moving texture of the player.
            this.MovingTexture = MovingTexture;
            // Sets the laser texture.
            this.LaserTexture = LaserTexture;
            // Sets the player sound effects.
            this.Shoot = Shoot;
            this.NoAmmo = NoAmmo;
        }

        /// <summary>
        /// Updates the player position.
        /// </summary>
        public override void Update(GameTime GameTime)
        {
            // Handles any player input.
            PlayerInput(GameTime);
            // Updates the lasers.
            UpdateLasers(GameTime);
        }

        /// <summary>
        /// The draw method used to draw the entity.
        /// </summary>
        public override void Draw(SpriteBatch SpriteBatch)
        {
            // Draws the lasers.
            DrawLasers(SpriteBatch);
            // Draws with the parent.
            base.Draw(SpriteBatch);
        }

        /// <summary>
        /// Handles the player input.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        private void PlayerInput(GameTime GameTime)
        {
            // Gets the input states.
            KeyboardState KS = Keyboard.GetState();
            MouseState MS = Mouse.GetState();
            // Checks if the player wants to shoot.
            if (MS.LeftButton == ButtonState.Pressed && Ammunition > 0 && Reloading <= 0 && OldState.LeftButton != ButtonState.Pressed)
            {
                // Sets the relead cooldown.
                Reloading = Cooldown;
                // Decrements the ammo count.
                Ammunition--;
                // Spawns the laser.
                Lasers.Add(new Laser(LaserTexture, Position, Rotation, 0.1f));
                // Plays the shooting sound.
                Shoot.Play(0.1f, 0, 0);
            }
            // If the player is reloading then decrement the cooldown.
            else if (Reloading > 0) Reloading -= (float)GameTime.ElapsedGameTime.TotalSeconds;
            // NO SHOOT!
            else if (MS.LeftButton == ButtonState.Pressed && Ammunition == 0 && OldState.LeftButton != ButtonState.Pressed) NoAmmo.Play(1, 0, 0);
            // If the player wishes to rotate left.
            if (KS.IsKeyDown(Keys.A)) Rotation -= MathHelper.ToRadians(SpeedMultiplier * (float)GameTime.ElapsedGameTime.TotalSeconds);
            // If the player wishes to rotate right.
            if (KS.IsKeyDown(Keys.D)) Rotation += MathHelper.ToRadians(SpeedMultiplier * (float)GameTime.ElapsedGameTime.TotalSeconds);
            // If the player wishes to move forward.
            if (KS.IsKeyDown(Keys.W) && Fuel > 0)
            {
                // Decrements the player's fuel
                Fuel -= 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                // Increments the speed.
                SpeedMultiplier += 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                // Player is moving so the moving texture is used.
                Texture = MovingTexture;
            }
            else
            {
                // Decrements the speed.
                SpeedMultiplier -= 100 * (float)GameTime.ElapsedGameTime.TotalSeconds;
                // Player is not moving so the idle texture is used.
                Texture = IdleTexture;
            }
            OldState = MS;
            // Clamps the fuel.
            Fuel = Helper.Clamp(Fuel, 0, 1000);
            // Clamps the speed.
            SpeedMultiplier = Helper.Clamp(SpeedMultiplier, 10, 1000);
            // Sets the player position.
            Position += Direction * (SpeedMultiplier * (float)GameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Updates the player lasers.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        private void UpdateLasers(GameTime GameTime)
        {
            // Iterates through the lasers.
            foreach (Laser Laser in Lasers)
            {
                // Updates the laser.
                Laser.Update(GameTime);
            }
        }

        /// <summary>
        /// Draws the lasers.
        /// </summary>
        /// <param name="SpriteBatch">Used to draw to the window.</param>
        private void DrawLasers(SpriteBatch SpriteBatch)
        {
            // Iterates through the lasers.
            foreach (Laser Laser in Lasers)
            {
                // Draws the lasers.
                Laser.Draw(SpriteBatch);
            }
        }
    }
}

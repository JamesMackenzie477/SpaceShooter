using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    /// <summary>
    /// An asteroid within the game scene.
    /// </summary>
    class Asteroid : Entity
    {
        /// <summary>
        /// Stores the asteroid speed.
        /// </summary>
        public float Speed;
        /// <summary>
        /// Stores the asteroid explosion sound.
        /// </summary>
        public SoundEffect Explosion;

        /// <summary>
        /// Creates a new asteroid in the scene.
        /// </summary>
        /// <param name="Texture">The texture to apply to the asteroid.</param>
        /// <param name="Position">The position of which the asteroid originates.</param>
        /// <param name="Rotation">The rotation radian of the asteroid.</param>
        /// <param name="Scale">The scale of the asteroid from 0 to 1.</param>
        /// <param name="Speed">The speed of the asteroid.</param>
        public Asteroid(Texture2D Texture, SoundEffect Explosion, Vector2 Position, float Rotation, float Scale, float Speed) : base(Texture, Position, Rotation, Scale)
        {
            // Sets the asteroid speed.
            this.Speed = Speed;
            // Sets the asteroid explosion sound.
            this.Explosion = Explosion;
        }

        /// <summary>
        /// Updates the asteroid position.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime GameTime)
        {
            // Rotates the player right by the player speed attribute.
            // Rotation += MathHelper.ToRadians(10 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            // Sets the asteroid position.
            Position += Direction * (Speed * (float)GameTime.ElapsedGameTime.TotalSeconds);
            // Updates the parent class.
            base.Update(GameTime);
        }
    }
}
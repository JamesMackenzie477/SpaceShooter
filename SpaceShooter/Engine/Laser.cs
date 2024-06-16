using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    /// <summary>
    /// Laser torpedo class
    /// </summary>
    class Laser : Entity
    {
        /// <summary>
        /// Stores the laser speed.
        /// </summary>
        public float speed = 1000;

        /// <summary>
        /// Creates a new laser.
        /// </summary>
        /// <param name="Texture">The texture to apply to the laser.</param>
        /// <param name="Position">The position of which the laser originates.</param>
        /// <param name="Rotation">The rotation radian of the laser.</param>
        /// <param name="Scale">The scale of the laser from 0 to 1.</param>
        public Laser(Texture2D Texture, Vector2 Position, float Rotation, float Scale) : base(Texture, Position, Rotation, Scale)
        {
        }

        /// <summary>
        /// Updates the laser position.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime GameTime)
        {
            // Sets the laser position.
            Position += Direction * (speed * (float)GameTime.ElapsedGameTime.TotalSeconds);
            // Updates the parent class.
            base.Update(GameTime);
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    /// <summary>
    /// An entity within the game scene.
    /// </summary>
    class Entity
    {
        /// <summary>
        /// Stores the current texture of the entity.
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// Stores the position of the entity.
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// Store the rotation of the entity.
        /// </summary>
        public float Rotation;
        /// <summary>
        /// Stores the entity scale.
        /// </summary>
        public float Scale;
        /// <summary>
        /// Stores the entity origin (the center of the entity).
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                // Calculates and returns the origin.
                return new Vector2(Texture.Width / 2, Texture.Height / 2);
            }
        }
        /// <summary>
        /// Stores the entity directional vector.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                // Calculates the returns the direction of the entity.
                return new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - Rotation));
            }
        }
        /// <summary>
        /// Stores the entity boundign box (used for collision detection).
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                // Creates a new rectangle representing the collision box.
                return new Rectangle((int)(Position.X - (Origin.X * Scale)), (int)(Position.Y - (Origin.Y * Scale)), (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
                // Creates a new collision box with the rectangle.
                //CollisionBox Box = new CollisionBox(rectangle);
                // Rotates the collision box by the rotation of the entity.
                //Box.RotateAround(Position, Rotation);
                // Returns the collsion box to the caller.
                //return Box;
            }
        }
        /// <summary>
        /// Stores the entity speed.
        /// </summary>
        // public float Speed;

        /// <summary>
        /// Creates a new entity in the scene.
        /// </summary>
        /// <param name="Texture">The texture to apply to the entity.</param>
        /// <param name="Position">The position of which the entity originates.</param>
        /// <param name="Rotation">The rotation radian of the entity.</param>
        /// <param name="Scale">The scale of the entity from 0 to 1.</param>
        public Entity(Texture2D Texture, Vector2 Position, float Rotation, float Scale)
        {
            // Sets the texture of the entity.
            this.Texture = Texture;
            // Sets the position of the entity.
            this.Position = Position;
            // Sets the rotation of the entity
            this.Rotation = Rotation;
            // Sets the scale of the entity.
            this.Scale = Scale;
            // Sets the speed of the entity.
            // this.Speed = Speed;
        }

        /// <summary>
        /// The update method called every frame.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime GameTime)
        {

        }

        /// <summary>
        /// The draw method used to draw the entity.
        /// </summary>
        /// <param name="SpriteBatch">Used to draw to the window.</param>
        public virtual void Draw(SpriteBatch SpriteBatch)
        {
            // Draws the entity.
            SpriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Used for collision detection.
        /// </summary>
        /// <param name="Target">The entity to check for a collision with.</param>
        public virtual bool Collision(Entity Target)
        {
            // Checks if the bounding boxes are intersecting.
            return BoundingBox.Intersects(Target.BoundingBox);
        }
    }
}

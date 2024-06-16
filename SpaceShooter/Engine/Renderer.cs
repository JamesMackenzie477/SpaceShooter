using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    /// <summary>
    /// An extended SpriteBatch class.
    /// </summary>
    class Renderer : SpriteBatch
    {
        /// <summary>
        /// Creates a new renderer.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use for the SpriteBatch.</param>
        public Renderer(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        /// <summary>
        /// Draws a line in 2D space.
        /// </summary>
        /// <param name="texture">The texture to apply to the line.</param>
        /// <param name="start">The starting position of the line.</param>
        /// <param name="end">The ending position of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="thickness">The thickness of the line.</param>
        public void DrawLine(Texture2D texture, Vector2 start, Vector2 end, Color color, int thickness)
        {
            // Calculates the difference vector.
            Vector2 difference = end - start;
            // Calculates the rotation of the line.
            float angle = (float)Math.Atan2(difference.Y, difference.X);
            // Draws the line.
            Draw(texture, new Rectangle((int)start.X, (int)start.Y, (int)difference.Length(), thickness), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a hollow box in 2D space.
        /// </summary>
        /// <param name="texture">The texture to apply to the box.</param>
        /// <param name="rectangle">The rectangle for the box to wrap.</param>
        /// <param name="color">The color of the box.</param>
        /// <param name="thickness">The thickness of the box.</param>
        public void DrawBox(Texture2D texture, Rectangle rectangle, Color color, int thickness)
        {
            // Calculates the angle vectors.
            Vector2 a = new Vector2(rectangle.X, rectangle.Y);
            Vector2 b = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
            Vector2 c = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            Vector2 d = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);
            // Draws the lines between the angles.
            DrawLine(texture, a, b, color, thickness);
            DrawLine(texture, b, c, color, thickness);
            DrawLine(texture, c, d, color, thickness);
            DrawLine(texture, d, a, color, thickness);
        }

        /// <summary>
        /// Draws a hollow box in 2D space.
        /// </summary>
        /// <param name="texture">The texture to apply to the box.</param>
        /// <param name="box">The collision box for the box to wrap.</param>
        /// <param name="color">The color of the box.</param>
        /// <param name="thickness">The thickness of the box.</param>
        public void DrawBox(Texture2D texture, CollisionBox box, Color color, int thickness)
        {
            // Draws the lines between the angles.
            DrawLine(texture, box.a, box.b, color, thickness);
            DrawLine(texture, box.b, box.c, color, thickness);
            DrawLine(texture, box.c, box.d, color, thickness);
            DrawLine(texture, box.d, box.a, color, thickness);
        }
    }
}

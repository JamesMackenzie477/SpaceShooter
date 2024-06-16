using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    /// <summary>
    /// Used to test for collision within the game.
    /// </summary>
    class CollisionBox
    {
        /// <summary>
        /// Stores the four points of the collision box.
        /// </summary>
        public Vector2 a, b, c, d;

        /// <summary>
        /// Creates a new collision box.
        /// </summary>
        /// <param name="a">The top left point of the collision box.</param>
        /// <param name="b">The top right point of the collision box.</param>
        /// <param name="c">The bottom right point of the collision box.</param>
        /// <param name="d">The bottom left point of the collision box.</param>
        public CollisionBox(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        /// <summary>
        /// Creates a new collision box.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert into a collision box.</param>
        public CollisionBox(Rectangle rectangle)
        {
            a = new Vector2(rectangle.X, rectangle.Y);
            b = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
            c = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            d = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);
        }

        /// <summary>
        /// Rotates the collision box about the given origin by the given radian.
        /// </summary>
        /// <param name="origin">The origin of which to rotate the collision box.</param>
        /// <param name="radians">The radian of which to rotate the collision box about the origin.</param>
        public void RotateAround(Vector2 origin, float radians)
        {
            a = Vector2.Transform(a - origin, Matrix.CreateRotationZ(radians)) + origin;
            b = Vector2.Transform(b - origin, Matrix.CreateRotationZ(radians)) + origin;
            c = Vector2.Transform(c - origin, Matrix.CreateRotationZ(radians)) + origin;
            d = Vector2.Transform(d - origin, Matrix.CreateRotationZ(radians)) + origin;
        }

        /// <summary>
        /// Use to check if the collision box is colliding with the target.
        /// </summary>
        /// <param name="target">The target collision box to check for a collision.</param>
        /// <returns>A boolean representing if collision box has collided with the target.</returns>
        public bool HasCollided(CollisionBox target)
        {
            bool e = a.X > target.a.X && a.X < target.b.X && a.Y > target.a.Y && a.Y < target.c.Y;
            bool f = b.X > target.a.X && b.X < target.b.X && b.Y > target.a.Y && b.Y < target.c.Y;
            bool g = c.X > target.a.X && c.X < target.b.X && c.Y > target.a.Y && c.Y < target.c.Y;
            bool h = d.X > target.a.X && d.X < target.b.X && d.Y > target.a.Y && d.Y < target.c.Y;
            bool i = target.a.X > a.X && target.a.X < b.X && target.a.Y > a.Y && target.a.Y < c.Y;
            bool j = target.b.X > a.X && target.b.X < b.X && target.b.Y > a.Y && target.b.Y < c.Y;
            bool k = target.c.X > a.X && target.c.X < b.X && target.c.Y > a.Y && target.c.Y < c.Y;
            bool l = target.d.X > a.X && target.d.X < b.X && target.d.Y > a.Y && target.d.Y < c.Y;
            return e || f || g || h || i || j || k || l;
        }

        /// <summary>
        /// The string method for the class.
        /// </summary>
        /// <returns>A nicely formatted string of attributes.</returns>
        public override string ToString()
        {
            return a.ToString() + b.ToString() + c.ToString() + d.ToString();
        }
    }
}

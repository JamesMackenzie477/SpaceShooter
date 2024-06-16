using System;

namespace SpaceShooter
{
    /// <summary>
    /// A static class with a bunch of helper functions.
    /// This will allow for the functionality of the program to change in the future without breaking things.
    /// </summary>
    static class Helper
    {
        /// <summary>
        /// Clamps the given value by teh min and max values sepcified.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="Min">The minimum value the value can be.</param>
        /// <param name="Max">The maxium value the value can be.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float Value, float Min, float Max)
        {
            // Clamps and returns the value.
            return Math.Min(Math.Max(Value, Min), Max);
        }
    }
}

using System;

namespace SpaceShooter
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Creates a new game instance.
            using (var Game = new SpaceShooter(1280, 720, false, false, 0.1f))
                // Runs the game.
                Game.Run();
        }
    }
}

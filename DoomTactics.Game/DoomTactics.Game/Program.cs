using System;

namespace DoomTactics
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DoomTacticsGame game = new DoomTacticsGame())
            {
                game.Run();
            }
        }
    }
#endif
}


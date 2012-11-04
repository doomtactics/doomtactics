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
            bool enableMusic = true;
            bool enableSound = true;
            bool skipLevelIntros = false;
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (arg == "--nomusic")
                        enableMusic = false;
                    else if (arg == "--nosound")
                        enableSound = false;
                    else if (arg == "--skipintros")
                        skipLevelIntros = true;
                }
            }
            using (DoomTacticsGame game = new DoomTacticsGame(enableMusic, enableSound, skipLevelIntros))
            {
                game.Run();
            }
        }
    }
#endif
}


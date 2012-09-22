using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public struct TileTextures
    {
        public Texture2D Top;
        public Texture2D North;
        public Texture2D South;
        public Texture2D East;
        public Texture2D West;

        /// <summary>
        /// Constructor for tiles that have only ONE texture on all sides
        /// </summary>
        /// <param name="texture"></param>
        public TileTextures(Texture2D texture) : this(texture, texture, texture, texture, texture) { }

        public TileTextures(Texture2D top, Texture2D sides) : this(top, sides, sides, sides, sides) { }

        public TileTextures(Texture2D top, Texture2D north, Texture2D south, Texture2D east, Texture2D west)
        {
            Top = top;
            North = north;
            South = south;
            East = east;
            West = west;
        }
    }
}
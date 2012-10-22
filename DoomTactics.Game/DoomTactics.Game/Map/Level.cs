using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    [Serializable]
    public class LevelData
    {
        public int TilesLong;
        public int TilesWide;
        public TileData[] TileDatas;
        public ActorInfo[] ActorInfos;
        public string BackgroundTextureName;
    }

    [Serializable]
    public class TileData
    {
        public decimal YPosition;
        public float VisualHeight;
        public string TopTextureName;
        public string NorthTextureName;
        public string SouthTextureName;
        public string EastTextureName;
        public string WestTextureName;
    }

    [Serializable]
    public class ActorInfo
    {
        public string Name;
        public string DisplayName;
        public ActorType ActorType;
        public int TileX;
        public int TileY;
        public int Team;
    }

    [Serializable]
    public enum ActorType
    {
        Marine,
        Imp,
        Caco,
        Demon,

        // Projectiles
        ImpFireball,
    }

    public class Level
    {
        public readonly int Length;
        public readonly int Width;
        public Texture2D BackgroundImage;
        private readonly Tile[] _tiles;
        public Tile[] Tiles { get { return _tiles; } }
        public IList<ActorBase> Actors; 

        public Level(int length, int width)
        {
            Length = length;
            Width = width;
            _tiles = new Tile[length * width];
            Actors = new List<ActorBase>();
        }      
  
        public Tile GetTileAt(int x, int y)
        {
            if (x < 0 || y < 0)
                return null;

            int index = (y*Length + x);
            if (index < 0 || index >= _tiles.Length)
                return null;

            return _tiles[y*Length + x];
        }

        public Tile GetTileOfActor(ActorBase actor)
        {
            return _tiles.FirstOrDefault(t => t.ActorInTile == actor);
        }

        public void DrawBackground(GraphicsDevice device, SpriteBatch batch)
        {
            if (BackgroundImage != null)
            {
                Rectangle target = new Rectangle(0, 0, device.Viewport.Width, device.Viewport.Height);
                batch.Begin();
                batch.Draw(BackgroundImage, target, Color.White);
                batch.End();
            }
        }
    }
}

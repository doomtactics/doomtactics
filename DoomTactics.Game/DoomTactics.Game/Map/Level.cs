using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    }

    [Serializable]
    public class TileData
    {
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
    }

    [Serializable]
    public enum ActorType
    {
        Marine,
        Imp,
        Caco,
        Demon
    }

    public static class LevelFactory
    {
        private static Dictionary<ActorType, Func<string, Vector3, ActorBase>> actorCreators;             

        public static Level CreateLevel(ContentManager contentManager, LevelData levelData)
        {
            InitializeDictionary();

            var level = new Level(levelData.TilesLong, levelData.TilesWide);
            for (int i = 0; i < level.Length; i++)
            {
                for (int j = 0; j < level.Width; j++)
                {
                    int index = i*level.Length + j;

                    var tileData = levelData.TileDatas[index];
                    var topTexture = contentManager.Load<Texture2D>(tileData.TopTextureName);
                    var northTexture = contentManager.Load<Texture2D>(tileData.NorthTextureName);
                    var southTexture = contentManager.Load<Texture2D>(tileData.SouthTextureName);
                    var eastTexture = contentManager.Load<Texture2D>(tileData.EastTextureName);
                    var westTexture = contentManager.Load<Texture2D>(tileData.WestTextureName);
                    var tileTextures = new TileTextures(topTexture, northTexture, southTexture, eastTexture, westTexture);
                    Vector3 tilePosition = new Vector3(j * 64.0f, 0.0f, i * 64.0f);
                    var tile = new Tile(tileTextures, tilePosition, tileData.VisualHeight);
                    level.Tiles[index] = tile;
                }
            }

            foreach (var info in levelData.ActorInfos)
            {
                level.Actors.Add(MakeActor(info, levelData.TileDatas, levelData.TilesLong));
            }
            return level;
        }

        private static void InitializeDictionary()
        {
            if (actorCreators == null)
            {
                actorCreators = new Dictionary<ActorType, Func<string, Vector3, ActorBase>>();
                actorCreators.Add(ActorType.Imp, (s, v) => new Imp(s, v));
            }
        }

        private static ActorBase MakeActor(ActorInfo info, TileData[] tiledatas, int levelLength)
        {
            Func<string, Vector3, ActorBase> actorCreator;
            int index = info.TileX*levelLength + info.TileY;
            float height = tiledatas[index].VisualHeight;
            Vector3 position = new Vector3(info.TileX * 64.0f, height, info.TileY * 64.0f);
            return actorCreators[ActorType.Imp].Invoke(info.Name, position);
        }
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

        private void GenerateTiles(ContentManager contentManager)
        {
            string[] textureNames = new[] { "textures\\FLAT1_1", "textures\\FLAT5_4", "textures\\FLAT5_5" };
            var random = new Random();
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int num = random.Next(0, 3);
                    TileTextures texture = new TileTextures(contentManager.Load<Texture2D>(textureNames[num]));
                    Vector3 position = new Vector3(j * 64.0f, 0.0f, i * 64.0f);
                    _tiles[i * Length + j] = new Tile(texture, position, 32.0f);
                }
            }

            // overwrite stuff with some specific tiles
            Texture2D topTex = contentManager.Load<Texture2D>("textures\\GRNROCK");
            Texture2D sideTex = contentManager.Load<Texture2D>("textures\\gothic\\ADEL_S70");
            // Texture2D directionTest = contentManager.Load<Texture2D>("directiontest");
            TileTextures text = new TileTextures(topTex, sideTex);
            _tiles[35] = new Tile(text, new Vector3(5 * 64.0f, 24.0f, 3 * 64.0f), 56.0f);
            _tiles[36] = new Tile(text, new Vector3(6 * 64.0f, 48.0f, 3 * 64.0f), 80.0f);
            _tiles[45] = new Tile(text, new Vector3(5 * 64.0f, 72.0f, 4 * 64.0f), 104.0f);
            _tiles[46] = new Tile(text, new Vector3(6 * 64.0f, 96.0f, 4 * 64.0f), 128.0f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public static class LevelFactory
    {
        private static Dictionary<ActorType, Func<string, Vector3, ActorBase>> _actorCreators;             

        public static Level CreateLevel(ContentManager contentManager, LevelData levelData)
        {
            InitializeDictionary();

            var level = new Level(levelData.TilesLong, levelData.TilesWide);
            for (int i = 0; i < level.Width; i++)
            {
                for (int j = 0; j < level.Length; j++)
                {
                    int index = i*level.Length + j;

                    var tileData = levelData.TileDatas[index];
                    var topTexture = contentManager.Load<Texture2D>(tileData.TopTextureName);
                    var northTexture = contentManager.Load<Texture2D>(tileData.NorthTextureName);
                    var southTexture = contentManager.Load<Texture2D>(tileData.SouthTextureName);
                    var eastTexture = contentManager.Load<Texture2D>(tileData.EastTextureName);
                    var westTexture = contentManager.Load<Texture2D>(tileData.WestTextureName);
                    var tileTextures = new TileTextures(topTexture, northTexture, southTexture, eastTexture, westTexture);
                    Vector3 tilePosition = new Vector3(j * 64.0f, tileData.VisualHeight, i * 64.0f);
                    var tile = new Tile(tileTextures, tilePosition, tileData.VisualHeight, j, i, (int)Math.Floor(tileData.VisualHeight));
                    level.Tiles[index] = tile;                    
                }
            }

            foreach (var info in levelData.ActorInfos)
            {
                var actor = MakeActor(info, levelData.TileDatas, levelData.TilesLong);
                actor.Team = info.Team;
                level.Actors.Add(actor);
                var tile = level.Tiles.Single(t => t.XCoord == info.TileX && t.YCoord == info.TileY);
                tile.SetActor(actor);
            }

            level.BackgroundImage = contentManager.Load<Texture2D>(levelData.BackgroundTextureName);

            return level;
        }

        private static void InitializeDictionary()
        {
            if (_actorCreators == null)
            {
                _actorCreators = new Dictionary<ActorType, Func<string, Vector3, ActorBase>>();
                _actorCreators.Add(ActorType.Imp, (s, v) => new Imp(s, v));
            }
        }

        private static ActorBase MakeActor(ActorInfo info, TileData[] tiledatas, int levelLength)
        {
            Func<string, Vector3, ActorBase> actorCreator;
            int index = info.TileY*levelLength + info.TileX;
            float height = tiledatas[index].VisualHeight;
            Vector3 position = new Vector3(info.TileX * 64.0f + 32.0f, height, info.TileY * 64.0f + 32.0f);
            return _actorCreators[info.ActorType].Invoke(info.Name, position);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics.Data
{
    public static class HardcodedTestLevel
    {
        private static Random _random = new Random();

        public static LevelData CreateLevel()
        {
            const int length = 8;
            const int width = 4;
            LevelData data = new LevelData();
            data.TilesLong = length;
            data.TilesWide = width;
            data.TileDatas = new TileData[length*width];

            for (int i = 0; i < data.TilesWide; i++)
            {
                for (int j = 0; j < data.TilesLong; j++)
                {
                    int index = i*length + j;
                    var tileData = new TileData();
                    
                    tileData.YPosition = (float) (_random.NextDouble()*32.0f);
                    tileData.VisualHeight = tileData.YPosition;
                    tileData.TopTextureName = GetRandomFloor();
                    tileData.NorthTextureName = GetRandomTexture();
                    tileData.SouthTextureName = GetRandomTexture();
                    tileData.EastTextureName = GetRandomTexture();
                    tileData.WestTextureName = GetRandomTexture();

                    data.TileDatas[index] = tileData;
                }
            }

            var impOne = new ActorInfo() {ActorType = ActorType.Imp, DisplayName = "Imp1", Name = "Imp1", TileX = 0, TileY = 0};
            var impTwo = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp2", Name = "Imp2", TileX = 1, TileY = 0 };
            var imp3 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp3", Name = "Imp3", TileX = 5, TileY = 3 };
            var imp4 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp4", Name = "Imp4", TileX = length - 1, TileY = width - 1 };
            var imp5 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp5", Name = "Imp5", TileX = 2, TileY = 3 };
            var imp6 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp6", Name = "Imp6", TileX = 7, TileY = 2 };


            data.ActorInfos = new ActorInfo[6];
            data.ActorInfos[0] = impOne;
            data.ActorInfos[1] = impTwo;
            data.ActorInfos[2] = imp3;
            data.ActorInfos[3] = imp4;
            data.ActorInfos[4] = imp5;
            data.ActorInfos[5] = imp6;

            data.BackgroundTextureName = "textures\\doom\\sky3";

            return data;
        }

        private static string GetRandomFloor()
        {
            string[] possibleFloors = new[] {"textures\\FLAT1_1", "textures\\FLAT5_4", "textures\\FLAT5_5"};
            int index = _random.Next(0, possibleFloors.Length);

            return possibleFloors[index];
        }

        private static string GetRandomTexture()
        {
            string[] possibleTextures = new[] { "textures\\gothic\\ADEL_S70" };
            int index = _random.Next(0, possibleTextures.Length);

            return possibleTextures[index];
        }
    }
}

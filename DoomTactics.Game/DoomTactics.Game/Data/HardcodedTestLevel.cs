using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics.Data
{
    public static class HardcodedTestLevel
    {
        public static LevelData CreateLevel()
        {
            const int length = 8;
            const int width = 4;
            LevelData data = new LevelData();
            data.TilesLong = length;
            data.TilesWide = width;
            data.TileDatas = new TileData[length*width];

            for (int i = 0; i < data.TilesLong; i++)
            {
                for (int j = 0; j < data.TilesWide; j++)
                {
                    int index = i*length + width;
                    var tileData = new TileData();
                    tileData.VisualHeight = (float)(new Random().NextDouble()*32.0f);

                    tileData.TopTextureName = GetRandomFloor();
                    tileData.NorthTextureName = GetRandomTexture();
                    tileData.SouthTextureName = GetRandomTexture();
                    tileData.EastTextureName = GetRandomTexture();
                    tileData.WestTextureName = GetRandomTexture();

                    data.TileDatas[index] = tileData;
                }
            }

            var impOne = new ActorInfo() {ActorType = ActorType.Imp, DisplayName = "Imp1", Name = "Imp1", TileX = 3, TileY = 0};
            var impTwo = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp2", Name = "Imp2", TileX = 5, TileY = 0 };

            data.ActorInfos = new ActorInfo[2];
            data.ActorInfos[0] = impOne;
            data.ActorInfos[1] = impTwo;
            
            return data;
        }

        private static string GetRandomFloor()
        {
            string[] possibleFloors = new[] {"textures\\FLAT1_1", "textures\\FLAT5_4", "textures\\FLAT5_5"};
            int index = new Random().Next(0, possibleFloors.Length);

            return possibleFloors[index];
        }

        private static string GetRandomTexture()
        {
            string[] possibleTextures = new[] { "textures\\gothic\\ADEL_S70" };
            int index = new Random().Next(0, possibleTextures.Length);

            return possibleTextures[index];
        }
    }
}

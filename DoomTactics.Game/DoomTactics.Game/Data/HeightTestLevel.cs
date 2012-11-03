using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics.Data
{
    public class HeightTestLevel
    {
        private const string FloorTexture = "textures/doom/SLIME14";
        private const string WallTexture1 = "textures/doom/RW33_1";
        private const string WallTexture2 = "textures/doom/RW33_2";
        private const string CenterFloorTexture = "textures/doom/STEP1";

        public static LevelData CreateLevel()
        {
            const int length = 12;
            const int width = 16;
            LevelData data = new LevelData();
            data.TilesLong = length;
            data.TilesWide = width;
            data.TileDatas = new TileData[length * width];

            // basic layout
            for (int i = 0; i < data.TilesWide; i++)
            {
                for (int j = 0; j < data.TilesLong; j++)
                {
                    int index = i * length + j;
                    var tileData = new TileData();

                    tileData.YPosition = 1.0m;
                    tileData.VisualHeight = 32.0f;
                    tileData.TopTextureName = FloorTexture;
                    tileData.NorthTextureName = WallTexture1;
                    tileData.SouthTextureName = WallTexture1;
                    tileData.EastTextureName = WallTexture1;
                    tileData.WestTextureName = WallTexture1;

                    data.TileDatas[index] = tileData;
                }
            }

            // top row
            for (int i = 0; i < data.TilesLong; i++)
            {
                // top row
                data.TileDatas[i].VisualHeight = 160f;
                data.TileDatas[i].YPosition = 7.0m;

                // bottom
                data.TileDatas[(length) * (width - 1) + i].VisualHeight = 160f;
                data.TileDatas[(length) * (width - 1) + i].YPosition = 7.0m;
            }

            // center area - raised platform
            for (int i = 7; i <= 8; i++)
            {
                for (int j = 5; j <= 6; j++)
                {
                    data.TileDatas[i * length + j].VisualHeight = 160f;
                    data.TileDatas[i * length + j].YPosition = 7.0m;
                    data.TileDatas[i * length + j].NorthTextureName = WallTexture2;
                    data.TileDatas[i * length + j].SouthTextureName = WallTexture2;
                    data.TileDatas[i * length + j].EastTextureName = WallTexture2;
                    data.TileDatas[i * length + j].WestTextureName = WallTexture2;
                }
            }

            // stairs
            int[] indexes = new int[] { 7 * length + 4, 8 * length + 4, 9 * length + 4, 9 * length + 5, 9 * length + 6 };
            float height = 32.0f + 24.0f;
            decimal gameHeight = 2.0m;
            foreach (int index in indexes)
            {
                data.TileDatas[index].VisualHeight = height;
                data.TileDatas[index].YPosition = gameHeight;
                data.TileDatas[index].TopTextureName = CenterFloorTexture;
                height += 24;
                gameHeight += 1;
            }


            var impOne = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp1", Name = "Imp1", TileX = 6, TileY = 4, Team = 1 };
            //var impTwo = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp2", Name = "Imp2", TileX = 5, TileY = 4, Team = 1 };
            var impThree = new ActorInfo()
            {
                ActorType = ActorType.Imp,
                DisplayName = "Imp3",
                Name = "Team2Imp3",
                TileX = 2,
                TileY = 7,
                Team = 2,
            };
            /*var impFour = new ActorInfo()
            {
                ActorType = ActorType.Imp,
                DisplayName = "Imp4",
                Name = "Team2Imp4",
                TileX = 2,
                TileY = 8,
                Team = 2
            };
            */
            //var imp3 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp3", Name = "Imp3", TileX = 5, TileY = 3 };
            //var imp4 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp4", Name = "Imp4", TileX = length - 1, TileY = width - 1 };
            //var imp5 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp5", Name = "Imp5", TileX = 2, TileY = 3 };
            //var imp6 = new ActorInfo() { ActorType = ActorType.Imp, DisplayName = "Imp6", Name = "Imp6", TileX = 7, TileY = 2 };


            data.ActorInfos = new ActorInfo[2];
            data.ActorInfos[0] = impOne;
            //data.ActorInfos[1] = impTwo;
            data.ActorInfos[1] = impThree;
            //data.ActorInfos[3] = impFour;
            //data.ActorInfos[4] = imp5;
            //data.ActorInfos[5] = imp6;

            data.BackgroundTextureName = "textures\\doom\\sky3";

            return data;
        }
    }
}

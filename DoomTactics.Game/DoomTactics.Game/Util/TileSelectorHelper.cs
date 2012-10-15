﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public static class TileSelectorHelper
    {
        public static TileSelector OccupiedTileSelector(Level level, ActorBase exclude)
        {
            var tileList = level.Tiles.Where(x => x.ActorInTile != null && x.ActorInTile != exclude).ToList();
            return new TileSelector(tileList);
        }

        public static TileSelector UnoccupiedTileSelector(Level level)
        {
            var tileList = level.Tiles.Where(x => x.ActorInTile == null).ToList();
            return new TileSelector(tileList);
        }
    }
}
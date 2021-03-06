﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public static class TileSelectorHelper
    {
        public static TileSelector Empty()
        {
            return new TileSelector();
        }

        public static TileSelector SingleTile(Tile tile)
        {
            return new TileSelector(new List<Tile>() { tile });
        }

        public static TileSelector CircularSelector(Level level, Tile currentTile, int radius)
        {
            var selector = new TileSelector();
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) <= radius)
                    {
                        Tile t = level.GetTileAt(currentTile.XCoord + j, currentTile.YCoord + i);
                        if (t != null)
                            selector.AddValidTile(t);
                    }
                }
            }

            return selector;
        }

        public static TileSelector OccupiedTileSelector(Level level, ActorBase exclude)
        {
            var tileList = level.Tiles.Where(x => x.ActorInTile != null && x.ActorInTile != exclude && x.ActorInTile.IsTargetable()).ToList();
            return new TileSelector(tileList);
        }

        public static TileSelector EnemyTileSelector(Level level, Tile currentTile, int myTeam, int maxRange)
        {
            var candidateTileList = level.Tiles.Where(x => x.ActorInTile != null && x.ActorInTile.Team != myTeam).ToList();
            var tileList = new List<Tile>();
            foreach (var tile in candidateTileList)
            {
                if (Math.Abs(currentTile.XCoord - tile.XCoord) + Math.Abs(currentTile.YCoord - tile.YCoord) <= maxRange)
                {
                    tileList.Add(tile);
                }
            }
            return new TileSelector(tileList);
        }

        public static TileSelector UnoccupiedTileSelector(Level level)
        {
            var tileList = level.Tiles.Where(x => x.ActorInTile == null).ToList();
            return new TileSelector(tileList);
        }        

        public static TileSelector StandardMovementTileSelector(Level level, Tile currentTile, ActorBase actor)
        {
            var selector = new TileSelector();
            int movementRange = actor.CurrentStats.MovementRange;
            for (int i = -movementRange; i <= movementRange; i++)
            {
                for (int j = -movementRange; j <= movementRange; j++)
                {                    
                    if (Math.Abs(i) + Math.Abs(j) <= movementRange)
                    {
                        Tile t = level.GetTileAt(currentTile.XCoord + j, currentTile.YCoord + i);
                        if (t != null && t.ActorInTile == null)
                        {
                            if (AStar.CalculateAStarPath(currentTile, t, level, actor) != null)
                            {
                                selector.AddValidTile(t);
                            }
                        }
                    }
                }
            }

            return selector;
        }        
    }
}

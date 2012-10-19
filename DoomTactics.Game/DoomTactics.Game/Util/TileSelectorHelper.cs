using System;
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

        public static TileSelector StandardMovementTileSelector(Level level, Tile currentTile, ActorBase actor)
        {
            var selector = new TileSelector();
            int movementRange = actor.MovementRange;
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

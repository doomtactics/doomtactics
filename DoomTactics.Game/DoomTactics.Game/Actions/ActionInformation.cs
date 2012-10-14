using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ActionInformation
    {
        public Func<Tile, ActionAnimationScript> Script { get; private set; }
        public TileSelector Selector { get; private set; }

        public ActionInformation(Func<Tile, ActionAnimationScript> script, TileSelector selector)
        {
            Script = script;
            Selector = selector;
        }
    }

    public class TileSelector
    {
        private readonly IList<Tile> _validTiles; 

        public TileSelector()
        {
            _validTiles = new List<Tile>();
        }

        public TileSelector(IList<Tile> validTiles)
        {
            _validTiles = validTiles;
        }

        public void AddValidTile(Tile tile)
        {
            _validTiles.Add(tile);
        }

        public bool IsTileValid(Tile tile)
        {
            return _validTiles.Contains(tile);
        }

    }
}

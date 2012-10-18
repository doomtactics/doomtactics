using System.Collections.Generic;

namespace DoomTactics
{
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

        public IList<Tile> ValidTiles()
        {
            return _validTiles;
        }

    }
}
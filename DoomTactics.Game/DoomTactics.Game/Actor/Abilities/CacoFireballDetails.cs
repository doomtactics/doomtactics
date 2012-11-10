using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class CacoFireballDetails : AbilityDetails
    {
        private const int radius = 1;

        public CacoFireballDetails(GameplayStats ownerStats) : base(ownerStats)
        {
            AbilityName = "Fireball";
            AbilityType = AbilityType.Active;            
        }

        public override IList<DamageResult> CalculateDamages(Level level, Tile targeted)
        {
            if (targeted.ActorInTile == null)
                return DamageResult.Empty;
            var damageResults = new List<DamageResult>();

            var aoe = TileSelectorHelper.CircularSelector(level, targeted, radius).ValidTiles();
            foreach (var tile in aoe)
            {
                if (tile.ActorInTile != null)
                {
                    var damageResult = new DamageResult() {DamageType = DamageType.Physical};
                    damageResult.AffectedActor = targeted.ActorInTile;
                    damageResult.DamageReduced = 0;
                    damageResult.NetDamage = 250;
                }
            }

            return damageResults;
        }
    }
}

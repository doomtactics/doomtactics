using System.Collections.Generic;

namespace DoomTactics
{
    public class ImpFireballDetails : AbilityDetails
    {
        public ImpFireballDetails(GameplayStats ownerStats) : base(ownerStats)
        {
            AbilityName = "Fireball";
            AbilityType = AbilityType.Active;            
        }

        public override IList<DamageResult> CalculateDamages(Level level, Tile targeted)
        {
            if (targeted.ActorInTile == null)
                return DamageResult.Empty;

            var damageResult = new DamageResult() {DamageType = DamageType.Physical};
            damageResult.AffectedActor = targeted.ActorInTile;
            damageResult.DamageReduced = 0;
            damageResult.NetDamage = 250;            

            return new List<DamageResult>() {damageResult};
        }


    }
}
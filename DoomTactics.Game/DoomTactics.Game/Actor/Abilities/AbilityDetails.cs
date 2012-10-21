using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public abstract class AbilityDetails
    {
        protected GameplayStats AbilityOwnerStats;
        public AbilityType AbilityType;
        public string AbilityName;

        protected AbilityDetails (GameplayStats ownerStats)
        {
            AbilityOwnerStats = ownerStats;
        }

        public abstract IList<DamageResult> CalculateDamages(Level level, Tile targeted);
    }
}

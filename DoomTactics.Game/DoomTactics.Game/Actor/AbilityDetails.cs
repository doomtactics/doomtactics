using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public enum DamageType
    {
        Physical,
        Demonic
    }

    public class DamageResult
    {
        public DamageType DamageType;
        public int DamageReduced;
        public int NetDamage;
    }

    public enum AbilityType
    {
        Passive,
        Active
    }

    public abstract class AbilityDetails
    {
        protected GameplayStats AbilityOwnerStats;
        public AbilityType AbilityType;
        public string AbilityName;

        protected AbilityDetails (GameplayStats ownerStats)
        {
            AbilityOwnerStats = ownerStats;
        }

        public abstract DamageResult CalculateDamage(Level level, ActorBase actor);
    }

    public class ImpFireballDetails : AbilityDetails
    {
        public ImpFireballDetails(GameplayStats ownerStats) : base(ownerStats)
        {
            AbilityName = "Fireball";
            AbilityType = AbilityType.Active;            
        }

        public override DamageResult CalculateDamage(Level level, ActorBase actor)
        {
            var damageResult = new DamageResult() {DamageType = DamageType.Physical};
            damageResult.DamageReduced = 0;
            damageResult.NetDamage = 40;

            return damageResult;
        }
    }
}

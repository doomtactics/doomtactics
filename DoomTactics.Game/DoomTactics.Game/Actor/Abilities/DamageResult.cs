using System.Collections.Generic;

namespace DoomTactics
{
    public class DamageResult
    {
        public DamageType DamageType;
        public int DamageReduced;
        public int NetDamage;
        public ActorBase AffectedActor;

        public static IList<DamageResult> Empty
        {
            get { return new List<DamageResult>(); }
        }
    }
}
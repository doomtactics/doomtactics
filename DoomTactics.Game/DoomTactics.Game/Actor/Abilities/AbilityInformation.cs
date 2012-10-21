using System;

namespace DoomTactics
{
    public class AbilityInformation
    {
        public AbilityDetails AbilityDetails { get; private set; }
        public Func<Level, ActionInformation> AbilityMethod { get; private set; }

        public AbilityInformation(AbilityDetails abilityDetails, Func<Level, ActionInformation> abilityMethod)
        {
            AbilityDetails = abilityDetails;
            AbilityMethod = abilityMethod;
        }
    }
}
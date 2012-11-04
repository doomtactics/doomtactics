using System;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ActionInformation
    {
        public Func<Tile, ActionAnimationScript> Script { get; private set; }
        public TileSelector AbilityRange { get; private set; }
        public Func<Tile, TileSelector> AbilityAreaOfEffect { get; private set; }
        public ActionType ActionType { get; private set; }

        public ActionInformation(Func<Tile, ActionAnimationScript> script, TileSelector abilityRange, Func<Tile, TileSelector> abilityAreaOfEffect, ActionType actionType)
        {
            Script = script;
            AbilityRange = abilityRange;
            AbilityAreaOfEffect = abilityAreaOfEffect;
            ActionType = actionType;
        }
    }
}

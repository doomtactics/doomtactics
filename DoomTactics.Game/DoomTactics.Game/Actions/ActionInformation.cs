using System;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class ActionInformation
    {
        public Func<Tile, ActionAnimationScript> Script { get; private set; }
        public TileSelector Selector { get; private set; }
        public ActionType ActionType { get; private set; }

        public ActionInformation(Func<Tile, ActionAnimationScript> script, TileSelector selector, ActionType actionType)
        {
            Script = script;
            Selector = selector;
            ActionType = actionType;
        }
    }
}

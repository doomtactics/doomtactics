using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;

namespace DoomTactics
{
    public class ActionSelection : GameStateBase
    {
        public ActionSelection(DoomDesktop desktop, GameState gameState)
            : base(desktop, gameState)
        {
            HighlightHoveredTile = true;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

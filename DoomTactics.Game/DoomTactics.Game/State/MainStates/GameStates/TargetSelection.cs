using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class TargetSelection : GameStateBase
    {
        public TargetSelection(DoomDesktop desktop, GameState gameState)
            : base(desktop, gameState)
        {
            HighlightHoveredTile = true;
        }

        public override void OnEnter()
        {

        }

        public override IState Update(GameTime gameTime)
        {
            IState nextState = base.Update(gameTime);
            if (nextState != null)
                return nextState;

            return nextState;
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

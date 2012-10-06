using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class ActionSelection : GameStateBase
    {
        private IState _nextState;
        private ActionMenu _actionMenu;

        public ActionSelection(GameState gameState)
            : base(gameState)
        {
            HighlightHoveredTile = true;
            new ActionMenuBuilder()
                    .ActorName("someone")//actor.ActorID)
                    .Action("Action", null)
                    .Action("Wait", null)
                    .Action("Turn", null)
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop)
                    .Build();           
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override IState Update(GameTime gameTime)
        {
            _nextState = base.Update(gameTime);
            if (_nextState != null)
                return _nextState;

            return _nextState;
        }

        public void SwitchToFreeCamera()
        {
            _nextState = new FreeCamera(GameState);
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

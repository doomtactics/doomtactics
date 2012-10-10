using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class SelectWaitDirection : GameStateBase
    {
        public SelectWaitDirection(GameState gameState) : base(gameState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

        }

        public override void Update(GameTime gameTime)
        {
            ActorBase nextActiveUnit = GameState.GetNextActiveUnit();
            if (nextActiveUnit == null)
            {
                NextState = new StateTransition(new FreeCamera(GameState, null));
            }
            else
            {
                NextState = new StateTransition(new ActionSelection(GameState, nextActiveUnit));
            }
        }

        public override void OnExit()
        {
            GameState.ActiveUnit.ChargeTime = 0;            
            GameState.ActiveUnit = GameState.GetNextActiveUnit();
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

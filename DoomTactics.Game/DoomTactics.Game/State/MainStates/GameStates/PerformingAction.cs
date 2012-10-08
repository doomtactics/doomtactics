using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class PerformingAction : GameStateBase
    {
        public PerformingAction(GameState gameState) : base(gameState)
        {
            InputProcessor = new NoInputProcessor(Keyboard.GetState(), Mouse.GetState());
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override StateTransition Update(GameTime gameTime)
        {
            return base.Update(gameTime);
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

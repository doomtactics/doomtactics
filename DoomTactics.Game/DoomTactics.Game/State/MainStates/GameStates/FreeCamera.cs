using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class FreeCamera : GameStateBase
    {
        private readonly IState _previousState;

        public FreeCamera(GameState gameState, IState previousState)
            : base(gameState)
        {
            _previousState = previousState;
            InputProcessor = new FreeCameraInputProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = false;
            GameState.Desktop.ShowCursor = false;
        }

        public override void OnExit()
        {
            
        }

        public override bool IsPaused
        {
            get { return false; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
        }

        public void SwitchToHudMode()
        {
            if (_previousState != null)
            {
                NextState = new StateTransition(_previousState);
            }
            else
            {
                NextState = new StateTransition(new ActionSelection(GameState, GameState.ActiveUnit));
            }         
        }
    }
}

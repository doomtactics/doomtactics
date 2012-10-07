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
        public FreeCamera(GameState gameState)
            : base(gameState)
        {
            HighlightHoveredTile = false;
            InputProcessor = new FreeCameraInputProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
        }

        public override void OnEnter()
        {
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

        public override StateTransition Update(GameTime gameTime)
        {
            base.Update(gameTime);
            return NextState;
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
        }

        public void SwitchToHudMode()
        {
            NextState = new StateTransition(null, true);
        }
    }
}

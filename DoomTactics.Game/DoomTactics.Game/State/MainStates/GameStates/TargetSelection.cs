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
    public class TargetSelection : GameStateBase
    {
        private readonly IState _previousState;
        private readonly Action<Tile> _callback;

        public TargetSelection(GameState gameState, IState previousState, Action<Tile> callback)
            : base(gameState)
        {
            _previousState = previousState;
            _callback = callback;
            HighlightHoveredTile = true;
            InputProcessor = new TargetSelectionProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
        }

        public override StateTransition Update(GameTime gameTime)
        {
            if (NextState != null)
                return NextState;

            base.Update(gameTime);
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();

            return NextState;
        }

        public override void OnExit()
        {
            
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
            GameState.Desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
            GameState.Desktop.Draw();
        }

        public override bool IsPaused
        {
            get { return false; }
        }

        public void ReturnToPrevious()
        {
            NextState = new StateTransition(_previousState);
        }

        public void PerformAction()
        {
            Tile targeted = GameState.FindHighlightedTile();
            if (targeted != null)
            {
                _callback.Invoke(targeted);
            }
        }
    }
}

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
        private static readonly Vector4 MoveSelectionTint = new Vector4(0.75f, 0.75f, 10.0f, 1.0f);
        private static readonly Vector4 AttackSelectionTint = new Vector4(10.0f, 0.75f, 0.75f, 1.0f);
        private static readonly Vector4 DefaultTint = new Vector4(1.0f);
        private readonly IState _previousState;
        private readonly ActionInformation _actionInformation;

        public TargetSelection(GameState gameState, IState previousState, ActionInformation actionInformation)
            : base(gameState)
        {
            _previousState = previousState;
            _actionInformation = actionInformation;
            InputProcessor = new TargetSelectionProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
            foreach (var tile in _actionInformation.Selector.ValidTiles())
            {
                tile.Tint = GetTint(_actionInformation.ActionType);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
        }

        public override void OnExit()
        {
            foreach (var tile in _actionInformation.Selector.ValidTiles())
            {
                tile.Tint = DefaultTint;
            }
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
            if (targeted != null && _actionInformation.Selector.IsTileValid(targeted))
            {
                var animationState = new ActionAnimationPlaying(GameState, new SelectWaitDirection(GameState), 
                                                                _actionInformation.Script.Invoke(targeted));
                NextState = new StateTransition(animationState);
            }
        }

        private static Vector4 GetTint(ActionType actionType)
        {
            if (actionType == ActionType.Attack)
                return AttackSelectionTint;
            else
                return MoveSelectionTint;
        }
    }
}

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
        private static readonly Vector4 MoveSelectionHoveredTint = new Vector4(2.5f, 2.5f, 10.0f, 0.5f);
        private static readonly Vector4 MoveSelectionTint = new Vector4(0.75f, 0.75f, 10.0f, 0.5f);
        private static readonly Vector4 AttackSelectionTint = new Vector4(10.0f, 0.75f, 0.75f, 0.5f);
        private static readonly Vector4 AttackSelectionHoveredTint = new Vector4(10.0f, 2.5f, 2.5f, 0.5f);
        private static readonly Vector4 DefaultTint = new Vector4(1.0f);
        private Tile _hoveredTile;
        private readonly IState _previousState;
        private readonly ActionInformation _actionInformation;

        public TargetSelection(GameState gameState, IState previousState, ActionInformation actionInformation)
            : base(gameState)
        {
            _previousState = previousState;
            _actionInformation = actionInformation;
            InputProcessor = new TargetSelectionProcessor(gameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
            foreach (var tile in _actionInformation.Selector.ValidTiles())
            {
                tile.Tint = GetTint(_actionInformation.ActionType, false);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);           
            Tile hoveredTile = GameState.FindHighlightedTile();
            if (hoveredTile != _hoveredTile)
            {
                if (_hoveredTile != null)
                {
                    _hoveredTile.Tint = _actionInformation.Selector.IsTileValid(_hoveredTile)
                                            ? GetTint(_actionInformation.ActionType, false)
                                            : DefaultTint;
                }
                _hoveredTile = hoveredTile;
                if (_hoveredTile != null)
                {
                    _hoveredTile.Tint = GetTint(_actionInformation.ActionType, true);
                }
            }
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
        }

        public override void OnExit()
        {
            foreach (var tile in _actionInformation.Selector.ValidTiles())
            {
                tile.Tint = DefaultTint;
            }
            if (_hoveredTile != null)
            {
                _hoveredTile.Tint = DefaultTint;
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
            NextState = new StateTransition(() => _previousState);
        }

        public void PerformAction()
        {
            Tile targeted = GameState.FindHighlightedTile();
            if (targeted != null && _actionInformation.Selector.IsTileValid(targeted))
            {
                if (_actionInformation.ActionType == ActionType.Move)
                    GameState.ActiveUnit.SetMoved();
                else if (_actionInformation.ActionType == ActionType.Attack)
                    GameState.ActiveUnit.SetActioned();

                IState nextState;
                if (GameState.ActiveUnit.CanMove() || GameState.ActiveUnit.CanAction())
                {
                    nextState = new ActionSelection(GameState, GameState.ActiveUnit);
                }
                else
                {
                    nextState = new SelectWaitDirection(GameState);
                }

                Func<IState> animationState = () => new ActionAnimationPlaying(GameState, nextState, 
                                                                _actionInformation.Script.Invoke(targeted));
                NextState = new StateTransition(animationState);
            }
        }

        private static Vector4 GetTint(ActionType actionType, bool hovered)
        {
            if (actionType == ActionType.Attack && hovered)
                return AttackSelectionHoveredTint;
            if (actionType == ActionType.Attack && !hovered)
                return AttackSelectionTint;
            if (actionType == ActionType.Move && hovered)
                return MoveSelectionHoveredTint;
            if (actionType == ActionType.Move && !hovered)
                return MoveSelectionTint;

            return DefaultTint;
        }
    }
}

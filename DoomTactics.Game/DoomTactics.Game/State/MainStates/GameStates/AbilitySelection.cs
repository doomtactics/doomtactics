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
    public class AbilitySelection : GameStateBase
    {
        private static readonly Vector4 MoveSelectionHoveredTint = new Vector4(2.5f, 2.5f, 10.0f, 0.5f);
        private static readonly Vector4 MoveSelectionTint = new Vector4(0.75f, 0.75f, 10.0f, 0.5f);
        private static readonly Vector4 AttackSelectionTint = new Vector4(10.0f, 0.75f, 0.75f, 0.5f);
        private static readonly Vector4 AttackSelectionHoveredTint = new Vector4(10.0f, 2.5f, 2.5f, 0.5f);
        private static readonly Vector4 DefaultTint = new Vector4(1.0f);
        private Tile _hoveredTile;
        private readonly IState _previousState;
        private readonly ActionInformation _actionInformation;
        private readonly StateMachine _stateMachine;
        public bool IsConfirming;
        private Tile _targetedTile;
        private TileSelector _targetedAoe;

        public AbilitySelection(GameState gameState, IState previousState, ActionInformation actionInformation)
            : base(gameState)
        {
            _previousState = previousState;
            _actionInformation = actionInformation;
            InputProcessor = new AbilitySelectionProcessor(gameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
            foreach (var tile in _actionInformation.AbilityRange.ValidTiles())
            {
                tile.Tint = GetTint(_actionInformation.ActionType, false);
            }
        }

        public void SwitchToConfirming()
        {
            IsConfirming = true;
            _targetedTile = GameState.FindHighlightedTile();
            _targetedAoe = _actionInformation.AbilityAreaOfEffect(_targetedTile);
            var validTiles = _targetedAoe.ValidTiles();
            foreach (var aoeTile in validTiles)
            {
                aoeTile.Tint = GetTint(_actionInformation.ActionType, true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsConfirming)
            {

            }
            else
            {
                Tile hoveredTile = GameState.FindHighlightedTile();
                if (hoveredTile != _hoveredTile)
                {
                    if (_hoveredTile != null)
                    {
                        _hoveredTile.Tint = _actionInformation.AbilityRange.IsTileValid(_hoveredTile)
                                                ? GetTint(_actionInformation.ActionType, false)
                                                : DefaultTint;
                    }
                    _hoveredTile = hoveredTile;
                    if (_hoveredTile != null)
                    {
                        _hoveredTile.Tint = GetTint(_actionInformation.ActionType, true);
                    }
                }
            }
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
        }

        public override void OnExit()
        {
            foreach (var tile in _actionInformation.AbilityRange.ValidTiles())
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
            if (_targetedTile != null && _actionInformation.AbilityRange.IsTileValid(_targetedTile))
            {
                Func<IState> animationState = () =>
                    new ActionAnimationPlaying(GameState, _actionInformation, _targetedTile);
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

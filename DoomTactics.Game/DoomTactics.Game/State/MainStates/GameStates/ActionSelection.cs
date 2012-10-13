using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoomTactics.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squid;

namespace DoomTactics
{
    public class ActionSelection : GameStateBase
    {
        private ActionMenu _actionMenu;
        private ActionMenu _actionSubMenu;
        private ActorBase _actionActor;

        public ActionSelection(GameState gameState, ActorBase actionActor)
            : base(gameState)
        {
            HighlightHoveredTile = true;
            InputProcessor = new ActionSelectionProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
            _actionActor = actionActor;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;

            if (_actionSubMenu == null)
            {
                _actionSubMenu = new ActionMenuBuilder()
                    .AsSubMenu()
                    .Action("Fireball", (ctl, e) => SwitchToTargetSelection((tile) => (_actionActor as Imp).ShootFireball(tile, null)))
                    .Action("Eviscerate", null)
                    .Size(200, 200)
                    .Build();
            }
            if (_actionMenu == null)
            {
                _actionMenu = new ActionMenuBuilder()
                    .ActorName(_actionActor.ActorID)
                    .Action("Action", (ctl, e) => _actionMenu.ShowSubMenu(_actionSubMenu))
                    .Action("Move", (ctl, e) => SwitchToTargetSelection((tile) => _actionActor.MoveToTile(tile, null)))
                    .Action("Wait", null)
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop)
                    .Build();
            }
            _actionMenu.Visible = true;
            _actionSubMenu.Visible = true;
        }

        public override void OnExit()
        {
            _actionMenu.Visible = false;
            _actionSubMenu.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
        }

        public override void Render(GraphicsDevice device)
        {
            base.Render(device);
            GameState.Desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
            GameState.Desktop.Draw();
        }

        public void SwitchToFreeCamera()
        {
            NextState = new StateTransition(new FreeCamera(GameState, this));
        }

        public void SwitchToTargetSelection(Func<Tile, ActionAnimationScript> scriptGenerator)
        {
            NextState = new StateTransition(new TargetSelection(GameState, this, scriptGenerator));
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

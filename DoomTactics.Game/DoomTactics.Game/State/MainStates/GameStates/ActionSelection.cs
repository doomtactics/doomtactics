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
                var menuBuilder = new ActionMenuBuilder().AsSubMenu();
                foreach (var ability in _actionActor.AbilityList)
                {
                    var selectedAbility = ability;
                    menuBuilder.Action(ability.AbilityDetails.AbilityName, (ctl, e) => SwitchToTargetSelection(selectedAbility.AbilityMethod(GameState.Level, ability.AbilityDetails)));
                }
                menuBuilder.Size(200, 200);
                _actionSubMenu = menuBuilder.Build();
            }
            if (_actionMenu == null)
            {
                var _actionMenuBuilder = new ActionMenuBuilder()
                    .ActorName(_actionActor.ActorId);

                if (!_actionActor.DidAction)
                {
                    _actionMenuBuilder
                        .Action("Action", (ctl, e) => _actionMenu.ShowSubMenu(_actionSubMenu));
                }
                if (!_actionActor.DidMove)
                {
                    _actionMenuBuilder
                        .Action("Move", (ctl, e) => SwitchToTargetSelection(_actionActor.MoveToTile(GameState.Level)));
                }

                _actionMenuBuilder
                    .Action("Wait", null)
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop);

                _actionMenu = _actionMenuBuilder.Build();
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

        public void SwitchToTargetSelection(ActionInformation actionInformation)
        {
            NextState = new StateTransition(new TargetSelection(GameState, this, actionInformation));
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

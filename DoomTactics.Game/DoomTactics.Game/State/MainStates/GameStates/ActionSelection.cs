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
        private readonly ActorBase _actionActor;

        public ActionSelection(GameState gameState, ActorBase actionActor)
            : base(gameState)
        {
            InputProcessor = new ActionSelectionProcessor(gameState, this);
            _actionActor = actionActor;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (GameState.IsAIControlled(_actionActor))
            {
                NextState = new StateTransition(() => new AIDecision(GameState, _actionActor));
                return;
            }

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
                var actionMenuBuilder = new ActionMenuBuilder()
                    .ActorName(_actionActor.ActorId);

                if (_actionActor.CanAction())
                {
                    actionMenuBuilder
                        .Action("Action", (ctl, e) => _actionMenu.ShowSubMenu(_actionSubMenu));
                }
                if (_actionActor.CanMove())
                {
                    actionMenuBuilder
                        .Action("Move", (ctl, e) => SwitchToTargetSelection(_actionActor.MoveToTile(GameState.Level)));
                }

                actionMenuBuilder
                    .Action("Wait", (ctl, e) => SwitchToWaitDirection())
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop);

                _actionMenu = actionMenuBuilder.Build();
            }
            _actionMenu.Visible = true;
            _actionSubMenu.Visible = true;
        }

        public override void OnExit()
        {
            if (_actionMenu != null)
                _actionMenu.Visible = false;
            if (_actionSubMenu != null)
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
            NextState = new StateTransition(() => new FreeCamera(GameState, this));
        }

        public void SwitchToTargetSelection(ActionInformation actionInformation)
        {
            NextState = new StateTransition(() => new AbilitySelection(GameState, this, actionInformation));
        }

        public void SwitchToWaitDirection()
        {
            NextState = new StateTransition(() => new SelectWaitDirection(GameState));
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

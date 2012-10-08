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
            _actionMenu = new ActionMenuBuilder()
                            .ActorName(_actionActor.ActorID)
                            .Action("Action", null)
                            .Action("Wait", null)
                            .Action("Turn", null)
                            .Position(50, 100)
                            .Size(200, 200)
                            .Parent(GameState.Desktop)
                            .Build();
        }

        public override void OnExit()
        {
            GameState.Desktop.Controls.Remove(_actionMenu);
            _actionMenu = null;
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

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

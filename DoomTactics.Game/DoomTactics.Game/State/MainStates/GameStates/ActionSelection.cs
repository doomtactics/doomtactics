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
        private IState _freeCameraSubState;

        public ActionSelection(GameState gameState, ActorBase actionActor)
            : base(gameState)
        {
            HighlightHoveredTile = true;
            InputProcessor = new ActionSelectionProcessor(Keyboard.GetState(), Mouse.GetState(), gameState, this);
            _actionActor = actionActor;
            _actionMenu = new ActionMenuBuilder()
                    .ActorName(actionActor.ActorID)
                    .Action("Action", null)
                    .Action("Wait", null)
                    .Action("Turn", null)
                    .Position(50, 100)
                    .Size(200, 200)
                    .Parent(GameState.Desktop)
                    .Build();           
        }

        public override void OnEnter()
        {
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
        }

        public override void OnExit()
        {
            GameState.Desktop.Controls.Remove(_actionMenu);
            _actionMenu = null;
        }

        public override StateTransition Update(GameTime gameTime)
        {
            if (_freeCameraSubState != null)
            {
                if (_freeCameraSubState.Update(gameTime) != null)
                {
                    _freeCameraSubState = null;
                }
            }
            else
            {
                base.Update(gameTime);
                GameState.SquidInputManager.Update(gameTime);
                GameState.Desktop.Update();
            }

            return NextState;
        }

        public override void Render(GraphicsDevice device)
        {
            if (_freeCameraSubState != null)
            {
                _freeCameraSubState.Render(device);
            }
            else
            {
                base.Render(device);
                GameState.Desktop.Size = new Squid.Point(device.Viewport.Width, device.Viewport.Height);
                GameState.Desktop.Draw();
            }
        }

        public void SwitchToFreeCamera()
        {
            _freeCameraSubState = new FreeCamera(GameState);
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

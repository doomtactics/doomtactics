﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class SelectWaitDirection : GameStateBase
    {
        public SelectWaitDirection(GameState gameState) : base(gameState)
        {
            InputProcessor = new GameStateBaseInputProcessor(Keyboard.GetState(), Mouse.GetState(), GameState);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
            SetWaitDirection(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            /*
            ActorBase nextActiveUnit = GameState.GetNextActiveUnit();
            if (nextActiveUnit == null)
            {
                NextState = new StateTransition(new FreeCamera(GameState, null));
            }
            else
            {
                NextState = new StateTransition(new ActionSelection(GameState, nextActiveUnit));
            }*/
        }

        public void SetWaitDirection(Vector2 mousePosition)
        {
            Ray ray = GameState.CreateRayFromMouseCursorPosition(mousePosition);

            var plane = new Plane(Vector3.Up, GameState.ActiveUnit.Height);
            float? distance = ray.Intersects(plane);
            if (distance != null)
            {
                Vector3 targetPoint = ray.Position + ray.Direction*distance.Value;
                GameState.ActiveUnit.FacePoint(targetPoint);
            }
        }


        public override void OnExit()
        {
            GameState.ActiveUnit.ChargeTime = 0;            
            GameState.ActiveUnit = GameState.GetNextActiveUnit();
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

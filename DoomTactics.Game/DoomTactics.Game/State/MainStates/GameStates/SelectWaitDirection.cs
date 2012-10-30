using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DoomTactics
{
    public class SelectWaitDirection : GameStateBase
    {
        public SelectWaitDirection(GameState gameState)
            : base(gameState)
        {
            InputProcessor = new SelectWaitDirectionProcessor(GameState, this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            GameState.Desktop.Visible = true;
            GameState.Desktop.ShowCursor = true;
        }

        public override void Update(GameTime gameTime)
        {
            GameState.SquidInputManager.Update(gameTime);
            GameState.Desktop.Update();
            SetWaitDirection(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            base.Update(gameTime);          
        }

        public void SetWaitDirection(Vector2 mousePosition)
        {
            SetActorWaitDirection(mousePosition, true);
        }

        public void FinalizeWaitDirection(Vector2 mousePosition)
        {
            SetActorWaitDirection(mousePosition, true);
            ActorBase nextActiveUnit = GameState.GetNextActiveUnit();
            if (nextActiveUnit == null)
            {
                NextState = new StateTransition(() => new FreeCamera(GameState, null));
            }
            else
            {
                NextState = new StateTransition(() => new ActionSelection(GameState, nextActiveUnit));
            }
        }

        private void SetActorWaitDirection(Vector2 mousePosition, bool snapDirection)
        {
            Ray ray = GameState.CreateRayFromMouseCursorPosition(mousePosition);
            Tile tile = GameState.Level.GetTileOfActor(GameState.ActiveUnit);

            var plane = new Plane(Vector3.Down, tile.CreateBoundingBox().Max.Y);
            float? distance = ray.Intersects(plane);
            if (distance != null)
            {
                Vector3 targetPoint = ray.Position + ray.Direction * distance.Value;
                GameState.ActiveUnit.FacePoint(targetPoint, snapDirection);
            }
        }

        public override void OnExit()
        {
            GameState.ActiveUnit.EndTurn();
            GameState.ActiveUnit = GameState.GetNextActiveUnit();
        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

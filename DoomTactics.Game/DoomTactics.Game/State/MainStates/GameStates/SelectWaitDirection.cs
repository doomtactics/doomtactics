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
            Vector3? targetPoint = GetTargetPoint(mousePosition);
            if (targetPoint.HasValue)
                GameState.ActiveUnit.FacePoint(targetPoint.Value, true);
        }

        public void FinalizeWaitDirection(Vector2 mousePosition)
        {
            Vector3? targetPoint = GetTargetPoint(mousePosition);
            if (targetPoint.HasValue)
            {
                ActionInformation ai = GameState.ActiveUnit.Wait(targetPoint.Value);               
                NextState = new StateTransition(() => new ActionAnimationPlaying(GameState, ai, null));
            }
        }

        private Vector3? GetTargetPoint(Vector2 mousePosition)
        {
            Ray ray = GameState.CreateRayFromMouseCursorPosition(mousePosition);
            Tile tile = GameState.Level.GetTileOfActor(GameState.ActiveUnit);

            var plane = new Plane(Vector3.Down, tile.CreateBoundingBox().Max.Y);
            float? distance = ray.Intersects(plane);
            if (distance != null)
            {
                Vector3 targetPoint = ray.Position + ray.Direction * distance.Value;
                return targetPoint;
            }

            return null;
        }

        public override void OnExit()
        {

        }

        public override bool IsPaused
        {
            get { return false; }
        }
    }
}

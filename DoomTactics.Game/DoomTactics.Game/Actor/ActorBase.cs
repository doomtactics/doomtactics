using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;

namespace DoomTactics
{
    public abstract class ActorBase
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string ActorId;
        public int Height;
        public int Width;
        public int Speed;
        public int ChargeTime;
        public bool IncreaseChargeTime;
        public int MovementRange;
        protected ActorAnimation CurrentAnimation;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 FacingDirection;        

        public virtual SpriteSheet SpriteSheet
        {
            get;
            protected set;
        }

        public virtual Rectangle CurrentTextureRectangle
        {
            get;
            protected set;
        }

        protected ActorBase(string id)
        {
            ActorId = id;
            ChargeTime = 0;
            IncreaseChargeTime = true;
            Velocity = Vector3.Zero;
            FacingDirection = Vector3.Forward;
        }

        public virtual void Update(GameTime elapsedTime)
        {
            CurrentAnimation.Update(elapsedTime.ElapsedGameTime);
            Position += Velocity;
        }

        public void IncreaseCT()
        {
            ChargeTime += Speed;
            if (ChargeTime >= 100)
            {
                ChargeTime = 100;
                var turnEvent = new TurnEvent(DoomEventType.ChargeTimeReached, this);
                MessagingSystem.DispatchEvent(turnEvent, ActorId);
            }
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, AlphaTestEffect spriteEffect, Camera camera, int passNumber)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            SpriteRenderDetails details = SpriteSheet.GetRenderDetails(CurrentAnimation.CurrentImageName(), GetAngleToCamera(camera));

            if (passNumber == 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.DepthRead,
                                  RasterizerState.CullNone, spriteEffect);
                device.DepthStencilState = DepthStencilState.Default;
                spriteBatch.Draw(SpriteSheet.Texture, details.TargetRectangle, details.SourceRectangle, Color.White, 0.0f, Vector2.Zero, details.SpriteEffects, 0);
                spriteBatch.End();
            }
            else
            {
                // spritebatch begin ignores depth buffer
                spriteBatch.Begin(0, null, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
                spriteBatch.Draw(SpriteSheet.Texture, details.TargetRectangle, details.SourceRectangle, Color.White, 0.0f, Vector2.Zero, details.SpriteEffects, 0);
                spriteBatch.End();
            }
        }

        private Angle GetAngleToCamera(Camera camera)
        {
            return GetAngle(camera.Position);
        }

        public Angle GetAngle(Vector3 sourcePosition)
        {
            Vector3 vectorBetween = Vector3.Normalize(sourcePosition - Position);
            float angle = MathHelper.ToDegrees(MathUtils.SignedAngleOnXzPlane(vectorBetween, FacingDirection));
            Angle angleEnum;

            if (180 <= angle && angle <= 202.5 || 0 <= angle && angle <= 22.5)
            {
                angleEnum = Angle.Forward;
            }
            else if (angle > 22.5 && angle <= 67.5)
            {
                angleEnum = Angle.ForwardLeft;
            }
            else if (angle > 67.5 && angle <= 112.5)
            {
                angleEnum = Angle.Left;
            }
            else if (angle > 112.5 && angle <= 157.5)
            {
                angleEnum = Angle.BackLeft;
            }
            else if (360 >= angle && angle > 337.5 || angle > 157.5 && angle <= 202.5)
            {
                angleEnum = Angle.Back;
            }
            else if (angle > 202.5 && angle <= 247.5)
            {
                angleEnum = Angle.ForwardRight;
            }
            else if (angle > 247.5 && angle <= 292.5)
            {
                angleEnum = Angle.Right;
            }
            else // if (angle > 292.5 && angle <= 337.5)
            {
                angleEnum = Angle.BackRight;
            }

            if (ActorId == "Imp1")
            {
                //Logger.Trace("Camera: " + camera.Position + ", my pos: " + Position + "Between: " + vectorBetween +
                //             ", facing: " + FacingDirection + ", angle: " + angle);
                //Logger.Trace("Angle: " + angleEnum.ToString());
            }

            return angleEnum;
        }

        public BoundingBox CreateBoundingBox()
        {
            var bb = new BoundingBox(new Vector3(Position.X - Width/2, Position.Y, Position.Z - Width/2),
                                   new Vector3(Position.X + Width/2, Position.Y + Height, Position.Z + Width/2));
            return bb;
        }

        public void FacePoint(Vector3 targetPosition, bool snapToEightDirections)
        {            
            FacingDirection = new Vector3(targetPosition.X, 0, targetPosition.Z) - new Vector3(Position.X, 0, Position.Z);
            if (snapToEightDirections)
            {
                // Get the enumerated render angle as if the camera were at the origin, and turn that into a Vector3.
                Angle angle = GetAngle(Vector3.Zero);
                if (angle == Angle.Forward)
                    FacingDirection = new Vector3(-1, 0, 0);
                if (angle == Angle.ForwardLeft)
                    FacingDirection = new Vector3(-1, 0, -1);
                if (angle == Angle.Left)
                    FacingDirection = new Vector3(0, 0, -1);
                if (angle == Angle.BackLeft)
                    FacingDirection = new Vector3(1, 0, -1);
                if (angle == Angle.Back)
                    FacingDirection = new Vector3(1, 0, 0);
                if (angle == Angle.BackRight)
                    FacingDirection = new Vector3(1, 0, 1);
                if (angle == Angle.Right)
                    FacingDirection = new Vector3(0, 0, 1);
                if (angle == Angle.ForwardRight)
                    FacingDirection = new Vector3(-1, 0, 1);
            }

            FacingDirection.Normalize();
            // Logger.Debug("Target Position: " + targetPosition + ", My position: " + Position + ", facing direction: " + FacingDirection);
        }

        public virtual void Die()
        {
            
        }        

        private void SnapToTile(Tile tile)
        {
            this.Position = new Vector3(tile.XCoord * 64.0f + 32.0f, tile.CreateBoundingBox().Max.Y, tile.YCoord * 64.0f + 32.0f);
        }
        
        public ActionInformation MoveToTile(Level level)
        {
            Func<Tile, ActionAnimationScript> scriptGenerator = MoveToTileAction;
            TileSelector selector = TileSelectorHelper.StandardMovementTileSelector(level, level.GetTileOfActor(this), MovementRange);
            return new ActionInformation(scriptGenerator, selector, ActionType.Move);
        }

        protected virtual ActionAnimationScript MoveToTileAction(Tile tile)
        {
            var tilePosition = new Vector3(tile.XCoord * 64.0f + 32.0f, tile.CreateBoundingBox().Max.Y, tile.YCoord * 64.0f + 32.0f);
            var directionToMove = tilePosition - Position;
            directionToMove.Normalize();

            BoundingBox checkBox = new BoundingBox(tilePosition - new Vector3(5.0f), tilePosition + new Vector3(5.0f));
            var removeFromTileEvent = new ActorEvent(DoomEventType.RemoveFromCurrentTile, this);

            var script = new ActionAnimationScriptBuilder().Name(ActorId + "move")
                .Segment()
                    .OnStart(() =>
                                 {              
                                     MessagingSystem.DispatchEvent(removeFromTileEvent, ActorId);
                                     Velocity = Speed*directionToMove;
                                 })
                    .EndCondition(() => (checkBox.Contains(Position) == ContainmentType.Contains))
                    .OnComplete(() =>
                                    {
                                        Velocity = Vector3.Zero; 
                                        SnapToTile(tile);
                                        tile.SetActor(this);
                                    })
                .Build();

            return script;
        }
    }
}

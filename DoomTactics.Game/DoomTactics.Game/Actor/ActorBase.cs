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
               
        public bool IncreaseChargeTime;
        
        protected ActorAnimation CurrentAnimation;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 FacingDirection;
        public int Team;

        public virtual float MovementVelocityModifier { get; protected set; }

        public GameplayStats BaseStats;
        public GameplayStats CurrentStats;

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

        protected ActorBase(string id) : this(id, Vector3.Zero, Vector3.Zero, 0)
        {
            
        }

        protected ActorBase(string id, Vector3 position) : this (id, position, Vector3.Zero, 0)
        {
            
        }

        protected ActorBase(string id, Vector3 position, Vector3 velocity) : this(id, position, velocity, 0)
        {
            
        }

        protected ActorBase(string id, Vector3 position, Vector3 velocity, int team)
        {
            ActorId = id;            
            IncreaseChargeTime = true;
            Position = position;
            Velocity = velocity;
            FacingDirection = Vector3.Forward;
            Team = team;
            SetupStats();
        }

        public virtual void Update(GameTime elapsedTime)
        {
            CurrentAnimation.Update(elapsedTime.ElapsedGameTime);
            Position += Velocity;
        }

        public abstract void SetupStats();

        public void IncreaseCT()
        {
            CurrentStats.ChargeTime += CurrentStats.Speed;
            if (CurrentStats.ChargeTime >= 100)
            {
                CurrentStats.ChargeTime = 100;
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
            var bb = new BoundingBox(new Vector3(Position.X - Width / 2, Position.Y, Position.Z - Width / 2),
                                   new Vector3(Position.X + Width / 2, Position.Y + Height, Position.Z + Width / 2));
            return bb;
        }

        public void FacePoint(Vector3 targetPosition, bool snapToEightDirections)
        {
            FacingDirection = new Vector3(targetPosition.X, 0, targetPosition.Z) - new Vector3(Position.X, 0, Position.Z);
            FacingDirection.Normalize();
            if (snapToEightDirections)
            {
                if (FacingDirection.X > 0.5f) FacingDirection.X = 1.0f;
                else if (FacingDirection.X < -0.5f) FacingDirection.X = -1.0f;
                else FacingDirection.X = 0;

                if (FacingDirection.Z > 0.5f) FacingDirection.Z = 1.0f;
                else if (FacingDirection.Z < -0.5f) FacingDirection.Z = -1.0f;
                else FacingDirection.Z = 0;
                
                /*
                // Get the enumerated render angle as if the camera were at the origin, and turn that into a Vector3.
                Angle angle = GetAngle(Vector3.Zero);
                if (angle == Angle.Forward)
                    FacingDirection = new Vector3(0, 0, -1);
                if (angle == Angle.ForwardLeft)
                    FacingDirection = new Vector3(1, 0, -1);
                if (angle == Angle.Left)
                    FacingDirection = new Vector3(1, 0, 0);
                if (angle == Angle.BackLeft)
                    FacingDirection = new Vector3(1, 0, 1);
                if (angle == Angle.Back)
                    FacingDirection = new Vector3(0, 0, 1);
                if (angle == Angle.BackRight)
                    FacingDirection = new Vector3(-1, 0, 1);
                if (angle == Angle.Right)
                    FacingDirection = new Vector3(-1, 0, 0);
                if (angle == Angle.ForwardRight)
                    FacingDirection = new Vector3(-1, 0, -1);*/
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
            Func<Tile, ActionAnimationScript> scriptGenerator = (tile) => MoveToTileAction(tile, level);
            TileSelector selector = TileSelectorHelper.StandardMovementTileSelector(level, level.GetTileOfActor(this), this);
            return new ActionInformation(scriptGenerator, selector, ActionType.Move);
        }

        protected virtual ActionAnimationScript MoveToTileAction(Tile goalTile, Level level)
        {
            IList<Tile> path = AStar.CalculateAStarPath(level.GetTileOfActor(this), goalTile, level, this);

            var scriptBuilder = new ActionAnimationScriptBuilder().Name(ActorId + "Move");
            for (int pathIndex = 1; pathIndex < path.Count; pathIndex++)
            {
                var tile = path[pathIndex];
                var tilePosition = new Vector3(tile.XCoord * 64.0f + 32.0f, tile.CreateBoundingBox().Max.Y, tile.YCoord * 64.0f + 32.0f);
                BoundingBox checkBox = new BoundingBox(tilePosition - new Vector3(5.0f), tilePosition + new Vector3(5.0f));

                if (pathIndex == path.Count - 1)
                {
                    var removeFromTileEvent = new ActorEvent(DoomEventType.RemoveFromCurrentTile, this);
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                                     {
                                         if (path.Count == 2) MessagingSystem.DispatchEvent(removeFromTileEvent, ActorId);
                                         Vector3 directionToMove = GetDirectionToPoint(tilePosition);
                                         FacePoint(tilePosition, false);
                                         Velocity = MovementVelocityModifier * directionToMove;
                                     })
                        .EndCondition(() => (checkBox.Contains(Position) == ContainmentType.Contains))
                        .OnComplete(() =>
                                        {
                                            Velocity = Vector3.Zero;
                                            SnapToTile(tile);
                                            tile.SetActor(this);
                                        });
                }
                else if (pathIndex == 1)
                {
                    var removeFromTileEvent = new ActorEvent(DoomEventType.RemoveFromCurrentTile, this);
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                                     {
                                         MessagingSystem.DispatchEvent(removeFromTileEvent, ActorId);
                                         Vector3 directionToMove = GetDirectionToPoint(tilePosition);
                                         FacePoint(tilePosition, false);
                                         Velocity = MovementVelocityModifier * directionToMove;
                                     })
                        .EndCondition(() => (checkBox.Contains(Position) == ContainmentType.Contains));
                }
                else if (pathIndex != path.Count - 1)
                {
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                                     {
                                         Vector3 directionToMove = GetDirectionToPoint(tilePosition);
                                         FacePoint(tilePosition, false);
                                         Velocity = MovementVelocityModifier * directionToMove;
                                     })
                        .EndCondition(() => (checkBox.Contains(Position) == ContainmentType.Contains));
                }
              
            }
            return scriptBuilder.Build();
        }

        private Vector3 GetDirectionToPoint(Vector3 targetPosition)
        {
            return Vector3.Normalize(targetPosition - Position);
        }
    }
}

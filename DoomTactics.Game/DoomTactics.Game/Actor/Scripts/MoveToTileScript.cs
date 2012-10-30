using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public static class MoveToTileScript
    {
        public static ActionAnimationScript MakeScript(IList<Tile> path, ActorBase actor)
        {
            var scriptBuilder = new ActionAnimationScriptBuilder().Name(actor.ActorId + "Move");
            for (int pathIndex = 1; pathIndex < path.Count; pathIndex++)
            {
                var tile = path[pathIndex];
                float tileYPos = tile.CreateBoundingBox().Max.Y;
                var tilePosition = tile.GetTopCenter();
                BoundingBox centerCheckBox = new BoundingBox(tilePosition - new Vector3(5.0f), tilePosition + new Vector3(5.0f));
                scriptBuilder = scriptBuilder
                    .Segment()
                    .OnStart((sv) =>
                    {
                        Vector3 directionToMove = actor.GetDirectionToPoint(tilePosition);
                        FaceAndMoveToPoint(tilePosition, actor);
                        if (directionToMove.Y > 0.0f)
                        {
                            actor.Velocity.Y *= 2;
                            sv.SetVariable("evalFunc", new Func<Vector3, bool>((v) => v.Y >= tileYPos));
                        }
                        else if (directionToMove.Y <= 0)
                        {
                            Vector3 currentPosition = actor.Position;
                            if (directionToMove.X >= 0.9f)
                            {
                                sv.SetVariable("evalFunc", new Func<Vector3, bool>((v) => v.X >= currentPosition.X + 32f));
                            }
                            else if (directionToMove.X <= -0.9f)
                            {
                                sv.SetVariable("evalFunc", new Func<Vector3, bool>((v) => v.X <= currentPosition.X - 32f));
                            }
                            else if (directionToMove.Z <= -0.9f)
                            {
                                sv.SetVariable("evalFunc", new Func<Vector3, bool>((v) => v.Z <= currentPosition.Z - 32f));
                            }
                            else if (directionToMove.Z >= 0.9f)
                            {
                                sv.SetVariable("evalFunc", new Func<Vector3, bool>((v) => v.Z >= currentPosition.Z + 32f));
                            }
                            actor.Velocity.Y = 0;
                        }
                    })
                    .EndCondition((sv) =>
                    {
                        bool result = sv.GetVariable<Func<Vector3, bool>>("evalFunc")(actor.Position);
                        return result;
                    });

                if (pathIndex == path.Count - 1)
                {
                    var removeFromTileEvent = new ActorEvent(DoomEventType.RemoveFromCurrentTile, actor);
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                        {
                            if (path.Count == 2) MessagingSystem.DispatchEvent(removeFromTileEvent, actor.ActorId);
                            FaceAndMoveToPoint(tilePosition, actor);
                        })
                        .EndCondition(() =>
                        {
                            return centerCheckBox.Contains(actor.Position) == ContainmentType.Contains;
                        })
                        .OnComplete(() =>
                        {
                            actor.Velocity = Vector3.Zero;
                            actor.SnapToTile(tile);
                            tile.SetActor(actor);
                        });
                }
                else if (pathIndex == 1)
                {
                    var removeFromTileEvent = new ActorEvent(DoomEventType.RemoveFromCurrentTile, actor);
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                        {
                            MessagingSystem.DispatchEvent(removeFromTileEvent, actor.ActorId);
                            FaceAndMoveToPoint(tilePosition, actor);
                        })
                        .EndCondition(() =>
                        {
                            return centerCheckBox.Contains(actor.Position) == ContainmentType.Contains;
                        });
                }
                else if (pathIndex != path.Count - 1)
                {
                    scriptBuilder = scriptBuilder
                        .Segment()
                        .OnStart(() =>
                        {
                            FaceAndMoveToPoint(tilePosition, actor);
                        })
                        .EndCondition(() =>
                        {
                            return centerCheckBox.Contains(actor.Position) == ContainmentType.Contains;
                        });
                }

            }
            return scriptBuilder.Build();
        }

        private static void FaceAndMoveToPoint(Vector3 tilePosition, ActorBase actor)
        {
            Vector3 directionToMove = actor.GetDirectionToPoint(tilePosition);
            actor.FacePoint(tilePosition, true);
            actor.Velocity = actor.MovementVelocityModifier * directionToMove;
        }
    }
}

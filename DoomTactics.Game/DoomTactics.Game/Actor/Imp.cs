﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class Imp : ActorBase
    {        
        public Imp(string id, Vector3 position)
            : base(id)
        {
            Height = 56;
            Width = 40;
            Speed = 4;
            Position = position;
            CurrentAnimation = ActorAnimationManager.Make("impidle", "testimp");
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.Imp);
        }

        //public void ShootFireball(Tile tile)
        //{
        //    // get distance between target tile and me
        //    var tilebox = tile.CreateBoundingBox();
        //    var average = tilebox.Min + (tilebox.Max - tilebox.Min)/2.0f;
        //    Vector3 target = new Vector3(average.X, tilebox.Max.Y + Height/2.0f, average.Z);
        //    Vector3 source = new Vector3(Position.X, Position.Y + Height/2.0f, Position.Z);
        //    var direction = target - source;
        //    var velocity = Vector3.Normalize(direction)*5.0f;
        //    var evt = new SpawnActorEvent(DoomEventType.SpawnActor, ActorType.ImpFireball, Position, velocity);
        //    MessagingSystem.DispatchEvent(evt);
        //}

        public ActionAnimationScript ShootFireball(Tile tile, Action onScriptFinish)
        {
            var tilebox = tile.CreateBoundingBox();
            var average = tilebox.Min + (tilebox.Max - tilebox.Min)/2.0f;
            Vector3 target = new Vector3(average.X, tilebox.Max.Y + Height/2.0f, average.Z);
            BoundingBox targetBoundingBox = new BoundingBox(target - new Vector3(20, 20, 20), target + new Vector3(20, 20, 20));
            Vector3 source = new Vector3(Position.X, Position.Y + Height/2.0f, Position.Z);
            var direction = target - source;
            var velocity = Vector3.Normalize(direction)*5.0f;
            var impFireball = ActorSpawnMethods.GetSpawnMethod(ActorType.ImpFireball).Invoke(source, velocity);
            var spawnEvent = new SpawnActorEvent(DoomEventType.SpawnActor, impFireball);

            var script = new ActionAnimationScriptBuilder().Name(ActorID + "shootFireball")
                .Segment()
                    .OnStart(() => MessagingSystem.DispatchEvent(spawnEvent, ActorID))
                    .EndCondition(() => targetBoundingBox.Contains(impFireball.Position) == ContainmentType.Contains)
                    .OnComplete(impFireball.Die)
                .Segment()
                    .EndOnEvent(DoomEventType.AnimationEnd, impFireball.ActorID)
                    .OnComplete(() => MessagingSystem.DispatchEvent(new DespawnActorEvent(DoomEventType.DespawnActor, impFireball), ActorID))
                //.Finish(onScriptFinish)
                .Build();

            return script;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class Caco : ActorBase
    {
        private const string FireballShootSound = "fireballshoot";
        private const string PainSound = "demonpain";
        private const string DeathSound = "cacodeath";

        public Caco(string id, Vector3 position)
            : base(id, position)
        {
            Height = 70;
            Width = 64;
            CurrentAnimation = ActorAnimationManager.Make("cacoidle", ActorId);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.Caco);
            MovementVelocityModifier = 4f;
        }

        public ActionInformation ShootFireball(Level level, AbilityDetails abilityDetails)
        {
            Func<Tile, ActionAnimationScript> actionScriptGenerator = (tile) => ShootFireballAction(level, abilityDetails, tile);
            TileSelector abilityRange = TileSelectorHelper.EnemyTileSelector(level, level.GetTileOfActor(this), Team, 5);
            Func<Tile, TileSelector> abilityAreaOfEffect = TileSelectorHelper.SingleTile;

            return new ActionInformation(actionScriptGenerator, abilityRange, abilityAreaOfEffect, ActionType.Attack);
        }

        private ActionAnimationScript ShootFireballAction(Level level, AbilityDetails abilityDetails, Tile selectedTile)
        {
            // calculate damages
            var damageList = abilityDetails.CalculateDamages(level, selectedTile);

            // animation
            CurrentAnimation = ActorAnimationManager.Make("cacoshoot", ActorId);
            CurrentAnimation.OnComplete = Idle;

            var tilebox = selectedTile.CreateBoundingBox();
            var average = tilebox.Min + (tilebox.Max - tilebox.Min) / 2.0f;
            Vector3 target = new Vector3(average.X, tilebox.Max.Y + Height / 2.0f, average.Z);
            BoundingBox targetBoundingBox = new BoundingBox(target - new Vector3(20, 20, 20), target + new Vector3(20, 20, 20));
            Vector3 source = new Vector3(Position.X, Position.Y + Height / 3.0f, Position.Z);
            var direction = target - source;
            var velocity = Vector3.Normalize(direction) * 5.0f;
            var cacoFireball = ActorSpawnMethods.GetSpawnMethod(ActorType.CacoFireball).Invoke(source, velocity);
            var spawnEvent = new ActorEvent(DoomEventType.SpawnActor, cacoFireball);
            var soundEvent = new SoundEvent(DoomEventType.PlaySound, FireballShootSound);

            var script = new ActionAnimationScriptBuilder().Name(ActorId + "shootFireball")
                .Segment()
                    .OnStart(() =>
                    {
                        FacePoint(selectedTile.GetTopCenter(), false);
                        MessagingSystem.DispatchEvent(spawnEvent, ActorId);
                        MessagingSystem.DispatchEvent(soundEvent, ActorId);
                    })
                    .EndCondition(() => targetBoundingBox.Contains(cacoFireball.Position) == ContainmentType.Contains)
                    .OnComplete(() =>
                    {
                        ApplyAndDisplayDamages(damageList);
                        cacoFireball.Die();
                    })
                .Segment()
                    .EndOnEvent(DoomEventType.AnimationEnd, cacoFireball.ActorId)
                    .OnComplete(() =>
                        MessagingSystem.DispatchEvent(new DespawnActorEvent(DoomEventType.DespawnActor, cacoFireball), ActorId)
                        )
                .Build();

            return script;
        }

        public override void Idle()
        {
            CurrentAnimation = ActorAnimationManager.Make("cacoidle", ActorId);
        }

        public override void Pain()
        {
            MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, PainSound), ActorId);
            CurrentAnimation = ActorAnimationManager.Make("cacopain", ActorId);
            CurrentAnimation.OnComplete = Idle;
        }

        public override void Die()
        {
            base.Die();

            MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, DeathSound), ActorId);
            CurrentAnimation = ActorAnimationManager.Make("cacodie", ActorId);
            CurrentAnimation.OnComplete =
                () => MessagingSystem.DispatchEvent(new ActorEvent(DoomEventType.ActorDied, this), ActorId);
        }

        public override void SetupStats()
        {
            BaseStats = GameplayStats.DefaultStats(ActorType.Imp);
            CurrentStats = BaseStats.CopyStats();
        }

        public override void SetupAbilityList()
        {
            var fireball = new AbilityInformation(new ImpFireballDetails(CurrentStats), ShootFireball);
            AbilityList.Add(fireball);
        }
    }
}

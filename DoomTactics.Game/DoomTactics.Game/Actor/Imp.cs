using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class Imp : ActorBase
    {
        private const string FireballShootSound = "sound/fireballshoot";
        private const string PainSound = "sound/imppain1";
        private const string DeathSound1 = "sound/impdeath1";
        private const string DeathSound2 = "sound/impdeath2";

        public Imp(string id, Vector3 position)
            : base(id, position)
        {
            Height = 56;
            Width = 40;
            CurrentAnimation = ActorAnimationManager.Make("impidle", ActorId);
            SpriteSheet = SpriteSheetFactory.CreateSpriteSheet(ActorType.Imp);
            MovementVelocityModifier = 3f;
        }

        public ActionInformation ShootFireball(Level level, AbilityDetails abilityDetails)
        {
            Func<Tile, ActionAnimationScript> actionScriptGenerator = (tile) => ShootFireballAction(level, abilityDetails, tile);
            TileSelector selector = TileSelectorHelper.EnemyTileSelector(level, level.GetTileOfActor(this), Team, 5);

            return new ActionInformation(actionScriptGenerator, selector, ActionType.Attack);
        }

        private ActionAnimationScript ShootFireballAction(Level level, AbilityDetails abilityDetails, Tile selectedTile)
        {
            // calculate damages
            var damageList = abilityDetails.CalculateDamages(level, selectedTile);

            // animation
            CurrentAnimation = ActorAnimationManager.Make("impshoot", ActorId);
            CurrentAnimation.OnComplete = Idle;

            var tilebox = selectedTile.CreateBoundingBox();
            var average = tilebox.Min + (tilebox.Max - tilebox.Min)/2.0f;
            Vector3 target = new Vector3(average.X, tilebox.Max.Y + Height/2.0f, average.Z);
            BoundingBox targetBoundingBox = new BoundingBox(target - new Vector3(20, 20, 20), target + new Vector3(20, 20, 20));
            Vector3 source = new Vector3(Position.X, Position.Y + Height/2.0f, Position.Z);
            var direction = target - source;
            var velocity = Vector3.Normalize(direction)*5.0f;
            var impFireball = ActorSpawnMethods.GetSpawnMethod(ActorType.ImpFireball).Invoke(source, velocity);
            var spawnEvent = new ActorEvent(DoomEventType.SpawnActor, impFireball);
            var soundEvent = new SoundEvent(DoomEventType.PlaySound, FireballShootSound);

            var script = new ActionAnimationScriptBuilder().Name(ActorId + "shootFireball")
                .Segment()
                    .OnStart(() =>
                                 {
                                     FacePoint(selectedTile.GetTopCenter(), false);
                                     MessagingSystem.DispatchEvent(spawnEvent, ActorId);
                                     MessagingSystem.DispatchEvent(soundEvent, ActorId);
                                 })
                    .EndCondition(() => targetBoundingBox.Contains(impFireball.Position) == ContainmentType.Contains)
                    .OnComplete(() =>
                                    {
                                        ApplyAndDisplayDamages(damageList);
                                        impFireball.Die();
                                    })
                .Segment()
                    .EndOnEvent(DoomEventType.AnimationEnd, impFireball.ActorId)
                    .OnComplete(() => 
                        MessagingSystem.DispatchEvent(new DespawnActorEvent(DoomEventType.DespawnActor, impFireball), ActorId)
                        )
                .Build();                        

            return script;
        }

        public override void MakeAIDecision(Level currentLevel, Action<ActionInformation, Tile> onComplete)
        {            
            var currentTile = currentLevel.GetTileOfActor(this);
            onComplete(MoveToTile(currentLevel), currentLevel.GetTileAt(currentTile.XCoord, currentTile.YCoord + 2));
        }

        public override void Idle()
        {
            CurrentAnimation = ActorAnimationManager.Make("impidle", ActorId);
        }

        public override void Pain()
        {
            MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, PainSound), ActorId);
            CurrentAnimation = ActorAnimationManager.Make("imppain", ActorId);
            CurrentAnimation.OnComplete = Idle;
        }

        public override void Die()
        {
            string deathSound = DeathSound1;
            int deathSoundNum = new Random().Next(0, 2);
            if (deathSoundNum == 1) deathSound = DeathSound2;
            MessagingSystem.DispatchEvent(new SoundEvent(DoomEventType.PlaySound, deathSound), ActorId);
            CurrentAnimation = ActorAnimationManager.Make("impdie", ActorId);
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

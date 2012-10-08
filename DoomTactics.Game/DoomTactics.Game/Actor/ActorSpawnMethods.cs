using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public static class ActorSpawnMethods
    {
        private static IDictionary<ActorType, Func<Vector3, Vector3, ActorBase>> _spawnMethods;
        private static bool _initialized = false;

        private static void Initialize()
        {
            _spawnMethods = new Dictionary<ActorType, Func<Vector3, Vector3, ActorBase>>();

            _spawnMethods.Add(ActorType.ImpFireball, (p, v) =>
            {
                var fireball = new ImpFireball("fireball");
                fireball.Position = p;
                fireball.Velocity = v;
                return fireball;
            });

            _initialized = true;
        }

        public static Func<Vector3, Vector3, ActorBase> GetSpawnMethod(ActorType actorType)
        {
            if (!_initialized)
                Initialize();

            return _spawnMethods[actorType];
        }
    }
}

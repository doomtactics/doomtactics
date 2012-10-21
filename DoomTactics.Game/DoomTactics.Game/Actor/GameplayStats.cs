﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomTactics
{
    public class GameplayStats
    {
        public int Health;
        public int Essence;
        public int AD;
        public int DP;
        public int Armor;
        public int DR;
        public int Speed;
        public int ChargeTime;
        public int MovementRange;

        public static GameplayStats Statsless()
        {
            return new GameplayStats();
        }

        public static GameplayStats DefaultStats(ActorType actorType)
        {
            if (actorType == ActorType.Imp)
            {
                return new GameplayStats()
                           {
                               Health = 500,
                               Essence = 100,
                               AD = 40,
                               DP = 15,
                               Armor = 20,
                               DR = 15,
                               Speed = 6,
                               MovementRange = 3
                           };
            }

            return Statsless();
        }

        public GameplayStats CopyStats()
        {
            return new GameplayStats()
                       {
                           Health = this.Health,
                           Essence = this.Essence,
                           AD = this.AD,
                           DP = this.DP,
                           Armor = this.Armor,
                           DR = this.DR,
                           Speed = this.Speed,
                           MovementRange = this.MovementRange
                       };
        }
    }
}
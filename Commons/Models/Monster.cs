using ShorewoodForest.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorewoodForest.Commons.Models
{
    internal class Monster : Creature
    {
        #region CONST

        private const int LOW_MIN_STAT = 3;
        private const int LOW_MAX_STAT = 7;

        private const int MEDIUM_MIN_STAT = 8;
        private const int MEDIUM_MAX_STAT = 13;

        private const int HIGH_MIN_STAT = 14;
        private const int HIGH_MAX_STAT = 19;

        #endregion

        private CreatureRace.MonsterRace _Race;
        private int _Gold;
        private int _Leather;

        public CreatureRace.MonsterRace Race { get => _Race; set => _Race = value; }
        public int Gold { get => _Gold; set => _Gold = value; }
        public int Leather { get => _Leather; set => _Leather = value; }

        public Monster(int health, int stamina, int strength, CreatureRace.MonsterRace race)
        {
            Random dice = new Random();

            Health = health;
            Stamina = stamina;
            Strength = strength;
            Race = race;

            switch(race)
            {
                case CreatureRace.MonsterRace.Wolf:
                    Leather = dice.Next(1,5);
                    Gold = 0;
                    break;
                case CreatureRace.MonsterRace.Orc:
                    Leather = 0;
                    Gold = dice.Next(1, 7);
                    break;
                case CreatureRace.MonsterRace.Whelp:
                    Leather = dice.Next(1, 5);
                    Gold = dice.Next(1, 7);
                    break;
            }
        }

        public Monster(CreatureRace.MonsterRace race)
        {
            Random dice = new Random();

            Race = race;

            switch (race)
            {
                case CreatureRace.MonsterRace.Wolf:
                    Health = dice.Next(LOW_MIN_STAT, LOW_MAX_STAT);
                    Stamina = dice.Next(HIGH_MIN_STAT, HIGH_MAX_STAT);
                    Strength = dice.Next(LOW_MIN_STAT, LOW_MAX_STAT);
                    Leather = dice.Next(1, 5);
                    Gold = 0;
                    break;
                case CreatureRace.MonsterRace.Orc:
                    Health = dice.Next(MEDIUM_MIN_STAT, MEDIUM_MAX_STAT);
                    Stamina = dice.Next(LOW_MIN_STAT, LOW_MAX_STAT);
                    Strength = dice.Next(HIGH_MIN_STAT, HIGH_MAX_STAT);
                    Leather = 0;
                    Gold = dice.Next(1, 7);
                    break;
                case CreatureRace.MonsterRace.Whelp:
                    Health = dice.Next(HIGH_MIN_STAT, HIGH_MAX_STAT);
                    Stamina = dice.Next(MEDIUM_MIN_STAT, MEDIUM_MAX_STAT);
                    Strength = dice.Next(HIGH_MIN_STAT, HIGH_MAX_STAT);
                    Leather = dice.Next(1, 5);
                    Gold = dice.Next(1, 7);
                    break;
            }
        }
    }
}

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
    }
}

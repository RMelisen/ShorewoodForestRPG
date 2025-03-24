using ShorewoodForest.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorewoodForest.Commons.Models
{
    internal class Hero : Creature
    {
        private string _Name;
        private CreatureRace.HeroRace _Race;
        private int _Gold;
        private int _Leather;

        public string Name { get => _Name; set => _Name = value; }
        internal CreatureRace.HeroRace Race { get => _Race; set => _Race = value; }
        public int Gold { get => _Gold; set => _Gold = value; }
        public int Leather { get => _Leather; set => _Leather = value; }

        public Hero(int health, int stamina, int strength, CreatureRace.HeroRace race, string name)
        {
            Name = name;
            Health = health;
            Stamina = stamina;
            Strength = strength;
            Race = race;
            Leather = 0;
            Gold = 0;
        }
    }
}

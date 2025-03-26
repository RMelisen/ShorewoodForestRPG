using ShorewoodForest.Commons.Enums;
using ShorewoodForest.UI;

namespace ShorewoodForest.Commons.Models
{
    internal class Hero : Creature
    {
        private string _Name;
        private CreatureRace.HeroRace _Race;
        private int _Gold;
        private int _Leather;
        private int _Experience = 0;

        public string Name { get => _Name; set => _Name = value; }
        internal CreatureRace.HeroRace Race { get => _Race; set => _Race = value; }
        public int Gold { get => _Gold; set => _Gold = value; }
        public int Leather { get => _Leather; set => _Leather = value; }
        public int Experience { get => _Leather; set => _Leather = value; }

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

        internal void AddExperience(int experience)
        { 
            Experience += experience;

            if (Experience >= 10)
            {
                Experience -= 10;

                Strength += 3;
                Stamina += 3;
                Health += 5;

                MainMenu.ShowLevelUp();
            }
        }
    }
}

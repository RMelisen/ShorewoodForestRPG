using ShorewoodForest.Commons.Enums;
using System;

namespace ShorewoodForest.Commons.Models
{
    internal abstract class Creature
    {
        private int _Health;
        private int _Stamina;
        private int _Strength;

        public int Strength { get => _Strength; set => _Strength = value; }
        public int Stamina { get => _Stamina; set => _Stamina = value; }
        public int Health { get => _Health; set => _Health = value; }

        internal int Hit()
        {
            Random dice = new Random();
            int damages = dice.Next(1,5);

            if (Strength < 6)
                damages--;
            else if (Strength < 16)
                damages++;
            else 
                damages += 2;

            return damages;
        }
    }
}

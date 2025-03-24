using ShorewoodForest.Commons.Enums;
using ShorewoodForest.UI.Commons;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShorewoodForest.Core
{
    internal class ShorewoodCore
    {
        #region Character Creation

        internal static int CalculateStamina(CreatureRace.HeroRace heroRace)
        {
            int stamina = 0;
            List<int> diceValues = new List<int>();

            ThrowDices(diceValues, 4);

            diceValues.Sort((x, y) => y.CompareTo(x)); // Sort in descending order

            foreach (int diceValue in diceValues.Take(3).ToList())
                stamina += diceValue;

            switch (heroRace)
            {
                case CreatureRace.HeroRace.Human:
                    stamina++;
                    break;
                case CreatureRace.HeroRace.Dwarf:
                    stamina += 2;
                    break;
                case CreatureRace.HeroRace.Savage:
                    break;
            }

            if (stamina < 6)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/] [{UIStyle.NEGATIVE_INDICATOR_COLOR}]{stamina}[/]");
            }
            else if (stamina < 11)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/] {stamina}");
            }
            else if (stamina < 16)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/] [{UIStyle.POSITIVE_INDICATOR_COLOR}]{stamina}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/] [{UIStyle.VERYPOSITIVE_INDICATOR_COLOR}]{stamina}[/]");
            }

            return stamina;
        }

        internal static int CalculateStrength(CreatureRace.HeroRace heroRace)
        {
            int strength = 0;
            List<int> diceValues = new List<int>();

            ThrowDices(diceValues, 4);

            diceValues.Sort((x, y) => y.CompareTo(x)); // Sort in descending order

            foreach (int diceValue in diceValues.Take(3).ToList())
                strength += diceValue;

            switch (heroRace)
            {
                case CreatureRace.HeroRace.Human:
                    strength++;
                    break;
                case CreatureRace.HeroRace.Dwarf:
                    break;
                case CreatureRace.HeroRace.Savage:
                    strength++;
                    break;
            }

            if (strength < 6)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/] [{UIStyle.NEGATIVE_INDICATOR_COLOR}]{strength}[/]");
            }
            else if (strength < 11)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/] {strength}");
            }
            else if (strength < 16)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/] [{UIStyle.POSITIVE_INDICATOR_COLOR}]{strength}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/] [{UIStyle.VERYPOSITIVE_INDICATOR_COLOR}]{strength}[/]");
            }

            return strength;
        }

        internal static int CalculateHealth(CreatureRace.HeroRace heroRace, int stamina)
        {
            int health = stamina;

            switch (heroRace)
            {
                case CreatureRace.HeroRace.Human:
                    break;
                case CreatureRace.HeroRace.Dwarf:
                    break;
                case CreatureRace.HeroRace.Savage:
                    health++;
                    break;
            }

            if (stamina < 6)
            {
                health--;
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/] [{UIStyle.NEGATIVE_INDICATOR_COLOR}]{stamina}[/]");
            }
            else if (stamina < 11)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/] {stamina}");
            }
            else if (stamina < 16)
            {
                health++;
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/] [{UIStyle.POSITIVE_INDICATOR_COLOR}]{stamina}[/]");
            }
            else
            {
                health += 2;
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/] [{UIStyle.VERYPOSITIVE_INDICATOR_COLOR}]{stamina}[/]");
            }

            return health;
        }

        #endregion

        #region Dices

        internal static void ThrowDices(List<int> diceValues, int counter)
        {

            for (int i = 0; i < counter; i++)
            {
                Thread.Sleep(500);
                diceValues.Add(ThrowDice());
            }
        }

        internal static int ThrowDice()
        {
            Random dice = new Random();

            int diceValue = dice.Next(1, 7);

            if (diceValue < 3)
                AnsiConsole.Markup($"[{UIStyle.NEGATIVE_INDICATOR_COLOR}]{diceValue}[/]  ");
            else if (diceValue < 5)
                AnsiConsole.Markup($"{diceValue}  ");
            else
                AnsiConsole.Markup($"[{UIStyle.POSITIVE_INDICATOR_COLOR}]{diceValue}[/]  ");

            return diceValue;
        }

        #endregion
    }
}

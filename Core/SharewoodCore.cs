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
            int stamina = CalculateStat(4, 500);

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

            DisplayStat("Stamina", stamina);

            return stamina;
        }

        internal static int CalculateStrength(CreatureRace.HeroRace heroRace)
        {
            int strength = CalculateStat(4, 500);

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

            DisplayStat("Strength", strength);

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

        internal static void DisplayStat(string stat, int statValue)
        {
            if (statValue < 6)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{stat} :[/] [{UIStyle.NEGATIVE_INDICATOR_COLOR}]{statValue}[/]");
            }
            else if (statValue < 11)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{stat} :[/] {statValue}");
            }
            else if (statValue < 16)
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{stat} :[/] [{UIStyle.POSITIVE_INDICATOR_COLOR}]{statValue}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{stat} :[/] [{UIStyle.VERYPOSITIVE_INDICATOR_COLOR}]{statValue}[/]");
            }
        }

        #endregion

        #region Dices

        internal static void ThrowDices(List<int> diceValues, int counter, int sleepTime)
        {

            for (int i = 0; i < counter; i++)
            {
                Thread.Sleep(sleepTime);
                diceValues.Add(ThrowDice(7));
            }
        }

        internal static int ThrowDice(int maxValue)
        {
            Random dice = new Random();

            int diceValue = dice.Next(1, maxValue);

            if (diceValue < 3)
                AnsiConsole.Markup($"[{UIStyle.NEGATIVE_INDICATOR_COLOR}]{diceValue}[/]  ");
            else if (diceValue < 5)
                AnsiConsole.Markup($"{diceValue}  ");
            else
                AnsiConsole.Markup($"[{UIStyle.POSITIVE_INDICATOR_COLOR}]{diceValue}[/]  ");

            return diceValue;
        }

        internal static int CalculateStat(int numberOfDices, int sleepTime)
        {
            List<int> diceValues = new List<int>();

            ThrowDices(diceValues, numberOfDices,  sleepTime);

            diceValues.Sort((x, y) => y.CompareTo(x)); // Sort in descending order

            return diceValues.Take(3).ToList().Sum();
        }

        #endregion
    }
}

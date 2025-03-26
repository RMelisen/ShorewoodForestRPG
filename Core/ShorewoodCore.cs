using ShorewoodForest.Commons.Enums;
using ShorewoodForest.Commons.Models;
using ShorewoodForest.UI.Commons;
using Spectre.Console;
using ShorewoodForest.UI;

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

            AnsiConsole.Markup($"{diceValue}  ");

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

        #region Fight

        internal static bool Fight(Monster monster, Layout layout, LiveDisplayContext liveDisplayContext)
        {
            Panel infoPanel = new Panel(
                Align.Left(
                new Markup($"A wild [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Race}[/] appeared !" +
                $"\n\nPress any key to [{UIStyle.NEUTRAL_INDICATOR_COLOR}]fight[/] !..."),
                VerticalAlignment.Middle));
            infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Fight ![/]");

            layout["RightBottom"].Update(infoPanel.Expand());
            liveDisplayContext.Refresh();

            Thread.Sleep(1200);
            Console.ReadKey();

            int heroDamages = 0;
            int monsterDamages = 0;

            while (monster.Health > 0 && MainMenu.UserHero.Health > 0)
            {
                heroDamages = MainMenu.UserHero.Attack();
                monsterDamages = monster.Attack();

                infoPanel = new Panel(
                    Align.Left(
                    new Markup($"You hit the [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Race}[/] for [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{heroDamages}[/] damages !"),
                    VerticalAlignment.Middle));
                infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Fight ![/]");

                layout["RightBottom"].Update(infoPanel.Expand());
                liveDisplayContext.Refresh();

                monster.Health -= heroDamages;
                MainMenu.DisplayMonsterInfo(monster, layout);
                liveDisplayContext.Refresh();

                Thread.Sleep(2000);

                if (monster.Health > 0)
                {
                    MainMenu.UserHero.Health -= monsterDamages;
                    MainMenu.DisplayHeroInfo(layout);
                    liveDisplayContext.Refresh();

                    infoPanel = new Panel(
                        Align.Left(
                        new Markup($"The [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Race}[/] hit you for [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monsterDamages}[/] damages !"),
                        VerticalAlignment.Middle));
                    infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Fight ![/]");

                    layout["RightBottom"].Update(infoPanel.Expand());
                    liveDisplayContext.Refresh();

                    Thread.Sleep(2000);
                }
            }

            if (MainMenu.UserHero.Health > 0)
            {
                ShowVictory(monster, infoPanel, layout, liveDisplayContext);
                return true;
            }
            else
            {
                ShowDefeat(infoPanel, layout, liveDisplayContext);
                return false;
            }
        }

        #region Victory/Defeat

        internal static void ShowVictory(Monster monster, Panel infoPanel, Layout layout, LiveDisplayContext liveDisplayContext)
        {
            MainMenu.UserHero.Leather += monster.Leather;
            MainMenu.UserHero.Gold += monster.Gold;

            switch (monster.Race)
            {
                case CreatureRace.MonsterRace.Wolf:
                    infoPanel = new Panel(
                        Align.Left(
                        new Markup($"[{UIStyle.POSITIVE_INDICATOR_COLOR}]You won ![/]" +
                        $"\n\nYou carved [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Leather} leather[/] on the wolf"),
                        VerticalAlignment.Middle));
                    infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Victory ![/]");
                    break;
                case CreatureRace.MonsterRace.Orc:
                    infoPanel = new Panel(
                        Align.Left(
                        new Markup($"[{UIStyle.POSITIVE_INDICATOR_COLOR}]You won ![/]" +
                        $"\n\nYou found [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Gold} gold[/] on the orc"),
                        VerticalAlignment.Middle));
                    infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Victory ![/]");
                    break;
                case CreatureRace.MonsterRace.Whelp:
                    infoPanel = new Panel(
                        Align.Left(
                        new Markup($"[{UIStyle.POSITIVE_INDICATOR_COLOR}]You won ![/]" +
                        $"\n\nYou carved [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Leather} leather[/] on the whelp" +
                        $"\nYou found [{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Gold} gold[/] on the whelp"),
                        VerticalAlignment.Middle));
                    infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Victory ![/]");
                    break;
            }

            layout["RightBottom"].Update(infoPanel.Expand());
            liveDisplayContext.Refresh();
        }
        internal static void ShowDefeat(Panel infoPanel, Layout layout, LiveDisplayContext liveDisplayContext)
        {
            infoPanel = new Panel(
                Align.Left(
                new Markup($"[{UIStyle.NEGATIVE_INDICATOR_COLOR} bold]YOU DIED ![/]" +
                $"\n\nPress any key to [{UIStyle.NEUTRAL_INDICATOR_COLOR}]quit[/]..."),
                VerticalAlignment.Middle));
            infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Defeat ![/]");

            layout["RightBottom"].Update(infoPanel.Expand());
            liveDisplayContext.Refresh();

            Console.ReadKey();

            Environment.Exit(0);
        }

        #endregion

        #endregion
    }
}

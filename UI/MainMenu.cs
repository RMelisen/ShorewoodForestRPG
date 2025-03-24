using Spectre.Console;
using ShorewoodForest.UI.Commons;
using ShorewoodForest.Commons.Models;
using ShorewoodForest.Commons.Enums;
using ShorewoodForest.Core;

namespace ShorewoodForest.UI
{
    internal class MainMenu
    {
        #region CONSTS

        const int CANVAS_WIDTH = 20;
        const int CANVAS_HEIGHT = 20;
        private const int NUMBER_OF_ENEMIES= 10;
        private const int MIN_SPACE_BETWEEN_ENEMIES = 2;

        #endregion

        internal static Hero UserHero;
        internal static int pixelX = CANVAS_WIDTH / 2; // Start in the center
        internal static int pixelY = CANVAS_HEIGHT / 2;
        internal static int prevPixelX = pixelX;
        internal static int prevPixelY = pixelY;

        internal static void WelcomeUser()
        {
            AnsiConsole.MarkupLine($"[{UIStyle.NEUTRAL_INDICATOR_COLOR} Bold]Welcome to the Shorewood Forest ![/]\n");

            //CreateHero();

            RunMapExplorer();
        }

        internal static void CreateHero()
        {
            int health = 0;
            int stamina = 0;
            int strength = 0;
            CreatureRace.HeroRace heroRace;

            heroRace = AnsiConsole.Prompt(new SelectionPrompt<CreatureRace.HeroRace>().Title($"Choose a [{UIStyle.NEUTRAL_INDICATOR_COLOR}]race[/] :").AddChoices(Enum.GetValues<CreatureRace.HeroRace>()));
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine($"Let's throw some [{UIStyle.NEUTRAL_INDICATOR_COLOR}]dices[/] to determine your [{UIStyle.NEUTRAL_INDICATOR_COLOR}]stats[/] !");

            AnsiConsole.MarkupLine($"Press any key to throw the [{UIStyle.NEUTRAL_INDICATOR_COLOR}]dices[/]...");
            Console.ReadKey();
            AnsiConsole.MarkupLine("");

            stamina = ShorewoodCore.CalculateStamina(heroRace);
            AnsiConsole.MarkupLine("");
            strength = ShorewoodCore.CalculateStrength(heroRace);
            AnsiConsole.MarkupLine("");
            health = ShorewoodCore.CalculateHealth(heroRace, stamina);
            AnsiConsole.MarkupLine("");

            string name = AnsiConsole.Ask<string>($"\n\nEnter your [{UIStyle.NEUTRAL_INDICATOR_COLOR}]name[/] : ");

            UserHero = new Hero(health, stamina, strength, heroRace, name);
        }

        #region MapUI

        internal static void RunMapExplorer()
        {
            AnsiConsole.Clear();

            Canvas mapCanvas = new Canvas(CANVAS_WIDTH, CANVAS_HEIGHT);
            Panel mapPanel = new Panel(Align.Center(mapCanvas, VerticalAlignment.Middle)).Expand();

            Layout layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right")
                    .SplitRows(
                        new Layout("RightTop"),
                        new Layout("RightBottom")));

            layout["RightTop"].Update(new Panel(
                Align.Center(
                    new Markup($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]TODO[/]"),
                    VerticalAlignment.Middle))
                .Expand());
            layout["RightBottom"].Update(new Panel(
                Align.Center(
                    new Markup($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]TODO[/]"),
                    VerticalAlignment.Middle))
                .Expand());

            AnsiConsole.Live(layout).Start(liveDisplayContext =>
            {
                while (true)
                {
                    MovePlayer(liveDisplayContext, mapPanel, mapCanvas, layout); 
                }
            });
        }

        internal static void MovePlayer(LiveDisplayContext liveDisplayContext, Panel mapPanel, Canvas mapCanvas, Layout layout)
        {
            mapCanvas.SetPixel(prevPixelX, prevPixelY, Color.Black);
            mapCanvas.SetPixel(pixelX, pixelY, UIStyle.NEUTRAL_INDICATOR_COLOR);

            layout["Left"].Update(mapPanel);

            liveDisplayContext.Refresh();

            prevPixelX = pixelX;
            prevPixelY = pixelY;

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (pixelY > 0)
                        pixelY--;
                    break;
                case ConsoleKey.DownArrow:
                    if (pixelY < CANVAS_HEIGHT - 1)
                        pixelY++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (pixelX > 0)
                        pixelX--;
                    break;
                case ConsoleKey.RightArrow:
                    if (pixelX < CANVAS_WIDTH - 1)
                        pixelX++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }

        #endregion
    }
}

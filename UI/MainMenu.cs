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
        private const int NUMBER_OF_ENEMIES = 10;
        private const int MIN_SPACE_BETWEEN_ENEMIES = 2;
        private const int DIFF_MONSTER_COUNT = 3;

        #endregion

        internal static Hero UserHero;
        internal static int _PixelX = CANVAS_WIDTH / 2; // Start in the center
        internal static int _PixelY = CANVAS_HEIGHT / 2;
        internal static int _PrevPixelX = _PixelX;
        internal static int _PrevPixelY = _PixelY;
        internal static char[,] mapData = new char[CANVAS_HEIGHT, CANVAS_WIDTH];
        internal static List<(int x, int y, Monster monster)> _EnemiesPositions = new List<(int x, int y, Monster monster)>();

        internal static void WelcomeUser()
        {
            AnsiConsole.MarkupLine($"[{UIStyle.NEUTRAL_INDICATOR_COLOR} Bold]Welcome to the Shorewood Forest ![/]\n");

            CreateHeroUI();

            RunMapExplorer();
        }

        #region Hero Creation

        internal static void CreateHeroUI()
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

        #endregion

        #region MapUI

        internal static void RunMapExplorer()
        {
            AnsiConsole.Clear();


            Canvas mapCanvas = new Canvas(CANVAS_WIDTH, CANVAS_HEIGHT);
            Panel mapPanel = new Panel(Align.Center(mapCanvas, VerticalAlignment.Middle)).Expand();

            mapData[_PixelX, _PixelY] = 'H';
            _EnemiesPositions = PlaceEnemies(mapCanvas);

            Layout layout = new Layout("Root")
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right")
                        .SplitRows(
                            new Layout("RightTop")
                                .SplitColumns(
                                    new Layout("RightTopLeft"),
                                    new Layout("RightTopRight")),
                            new Layout("RightBottom")));

            DisplayHeroInfo(layout);

            layout["RightTopRight"].Update(new Panel(
                Align.Left(
                    new Markup($""),
                    VerticalAlignment.Middle))
                .Expand());

            Panel infoPanel = new Panel(
                Align.Left(
                new Markup($"You are in the Shorewood Forest ...\n\nUse the [{UIStyle.NEUTRAL_INDICATOR_COLOR}]directional keys[/] to move"),
                VerticalAlignment.Middle));
            infoPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Exploration[/]");

            layout["RightBottom"].Update(infoPanel.Expand());

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
            mapCanvas.SetPixel(_PrevPixelX, _PrevPixelY, Color.Black);
            mapCanvas.SetPixel(_PixelX, _PixelY, UIStyle.NEUTRAL_INDICATOR_COLOR);

            layout["Left"].Update(mapPanel);

            liveDisplayContext.Refresh();

            _PrevPixelX = _PixelX;
            _PrevPixelY = _PixelY;

            CheckIfAnyEnemyAdjacent(_PixelX, _PixelY, layout, liveDisplayContext, mapCanvas);

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (_PixelY > 0)
                        _PixelY--;
                    break;
                case ConsoleKey.DownArrow:
                    if (_PixelY < CANVAS_HEIGHT - 1)
                        _PixelY++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (_PixelX > 0)
                        _PixelX--;
                    break;
                case ConsoleKey.RightArrow:
                    if (_PixelX < CANVAS_WIDTH - 1)
                        _PixelX++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }

        internal static void CheckIfAnyEnemyAdjacent(int x, int y, Layout layout, LiveDisplayContext liveDisplayContext, Canvas mapCanvas)
        {
            foreach ((int x, int y, Monster monster) enemy in _EnemiesPositions)
            {
                if ((Math.Abs(enemy.x - x) <= 1 && Math.Abs(enemy.y - y) == 0) || (Math.Abs(enemy.x - x) == 0 && Math.Abs(enemy.y - y) <= 1))
                {
                    // Adjacent enemy found
                    DisplayMonsterInfo(enemy.monster, layout);
                    liveDisplayContext.Refresh();

                    ShorewoodCore.Fight(enemy.monster, layout, liveDisplayContext);

                    mapCanvas.SetPixel(enemy.x, enemy.y, Color.Black);
                    _EnemiesPositions.Remove((enemy.x, enemy.y, enemy.monster));

                    ClearDisplay(layout, "RightTopRight");
                    liveDisplayContext.Refresh();

                    return;
                }
            }
        }

        internal static List<(int x, int y, Monster monster)> PlaceEnemies(Canvas mapCanvas)
        {
            Random random = new Random();
            List<(int x, int y, Monster monster)> enemiesPositions = new List<(int x, int y, Monster)>();

            for (int i = 0; i < NUMBER_OF_ENEMIES; i++)
            {
                int x, y;
                bool validPosition;

                do
                {
                    x = random.Next(CANVAS_WIDTH);
                    y = random.Next(CANVAS_HEIGHT);
                    validPosition = true;

                    // Check for minimum space between enemies
                    foreach ((int x, int y, Monster monster) position in enemiesPositions)
                    {
                        if (Math.Abs(x - position.x) <= MIN_SPACE_BETWEEN_ENEMIES && Math.Abs(y - position.y) <= MIN_SPACE_BETWEEN_ENEMIES)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    // Should not spawn next to the Hero
                    if (Math.Abs(x - _PixelX) <= 2 && Math.Abs(y - _PixelY) <= 2)  
                    {
                        validPosition = false;
                        break;
                    }

                    // Check if position is already occupied by something
                    if (mapData[x, y] != '\0')
                    {
                        validPosition = false;
                    }
                } while (!validPosition);

                Monster newMonster;
                switch (random.Next(DIFF_MONSTER_COUNT))
                {
                    case 0:
                        newMonster = new Monster(CreatureRace.MonsterRace.Wolf);
                        break;
                    case 1:
                        newMonster = new Monster(CreatureRace.MonsterRace.Orc);
                        break;
                    case 2:
                        newMonster = new Monster(CreatureRace.MonsterRace.Whelp);
                        break;
                    default:
                        newMonster = new Monster(CreatureRace.MonsterRace.Wolf);
                        break;
                }
                mapData[x, y] = 'e';
                enemiesPositions.Add((x, y, newMonster));
            }

            foreach (var enemy in enemiesPositions)
            {
                mapCanvas.SetPixel(enemy.x, enemy.y, UIStyle.NEGATIVE_INDICATOR_COLOR);
            }

            return enemiesPositions;
        }

        #endregion

        #region Character Display

        internal static void DisplayHeroInfo(Layout layout)
        {
            Panel heroPanel = new Panel(
                Align.Left(
                new Markup($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/]     {UserHero.Health}" +
                $"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/]    {UserHero.Stamina}" +
                $"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/]   {UserHero.Strength}" +
                $"\n\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Leather :[/]    {UserHero.Leather}" +
                $"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Gold :[/]       {UserHero.Gold}"),
                VerticalAlignment.Middle));
            heroPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{UserHero.Name} (you)[/]");

            layout["RightTopLeft"].Update(heroPanel.Expand());
        }

        internal static void DisplayMonsterInfo(Monster monster, Layout layout)
        {
            Panel monsterPanel = new Panel(
                Align.Left(
                new Markup($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Health :[/]     {monster.Health}" +
                $"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Stamina :[/]    {monster.Stamina}" +
                $"\n[{UIStyle.NEUTRAL_INDICATOR_COLOR}]Strength :[/]   {monster.Strength}"),
                VerticalAlignment.Middle));
            monsterPanel.Header = new PanelHeader($"[{UIStyle.NEUTRAL_INDICATOR_COLOR}]{monster.Race}[/]");

            layout["RightTopRight"].Update(monsterPanel.Expand());
        }

        internal static void ClearDisplay(Layout layout, string layoutPosition)
        {
            layout[layoutPosition].Update(new Panel(
            Align.Left(
                new Markup($""),
                VerticalAlignment.Middle))
            .Expand());
        }

        #endregion
    }
}

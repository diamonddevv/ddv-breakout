using Breakout.Decor;
using Breakout.Game;
using Breakout.Resource;
using Breakout.Util;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Breakout.Window
{
    internal class GameState : WindowState
    {

        public static bool debug = false;

        public static Random random;

        public static BlockManager blockManager = new BlockManager();
        public static BallManager ballManager = new BallManager();
        private Player player;
        private PrimaryBall pball;

        private static bool paused;

        public static Button BUTTON_INGAME_SETTINGS;
        public static Button BUTTON_TO_MAIN_MENU;

        public GameState(WindowState? parent) : base("In-Game", parent)
        {
        }

        public override void Init()
        {
            paused = false;
            random = new Random();

            this.player = new Player(360, 400);
            this.pball = new PrimaryBall(0, 0, Vector2.Zero, true);
            ballManager.balls.Add(pball);

            blockManager.Generate();

            skyPhaseFrames = 0;

            BUTTON_INGAME_SETTINGS = new Button((int)(Program.width * 0.5) - 20 - 128, (int)(Program.height * 0.5) + 116, 128, 96, "Settings", 25, () =>
            {
                Program.SwitchState((parent) => new SettingsState(parent));
            });


            BUTTON_TO_MAIN_MENU = new Button((int)(Program.width * 0.5) + 20, (int)(Program.height * 0.5) + 116, 128, 96, "Main Menu", 20, () =>
            {
                Program.SwitchState((parent) => new YesNoWindowState("Are you sure you want to go back to the main menu?\nThis game's score will be saved.", () =>
                {
                    OnLose(player);
                }, () =>
                {
                    Program.RevertToLastState();
                }, parent));
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(SKYCOLOR);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {
                paused = !paused;
            }

            if (!paused)
            {
                TickSkyColor();
                Raylib.DrawRectangleGradientV(0, player.y, Program.width, Program.height - player.y, Color.BLUE, Color.DARKBLUE);



                if (random.Next(45) == 0)
                {
                    Cloud cloud = Cloud.CreateCloud(random);
                    DecorManager.Add(cloud);
                }

                if (random.Next(80) == 0)
                {
                    Fish fish = Fish.CreateFish(random, player);
                    DecorManager.Add(fish);
                }

                if (random.Next(500) == 0)
                {
                    Decor.Wave wave = new Decor.Wave(player.y, random);
                    DecorManager.Add(wave);
                }


                

                int digits = player.score.ToString().Length;
                Raylib.DrawText($"{player.score}", (Program.width / 2) - digits * 25, 240, 100, Color.DARKGRAY);
                Raylib.DrawText($"Lives: {player.lives}", 0 + 20, Program.height - 60, 50, Color.LIGHTGRAY);
                ballManager.Tick();

                player.Tick();
                ballManager.ForEachBall(b => b.Tick(player, blockManager, ballManager));

                DecorManager.TickDecor();

                player.Draw(debug);
                ballManager.ForEachBall(b => b.Draw(debug));
                blockManager.DrawAll(debug);

                

                if (debug)
                {
                    Color color = GetVisibleTextColor(SKYCOLOR);
                    int fps = Raylib.GetFPS();
                    int x = player.x;

                    Raylib.DrawText($"FPS: {fps}", 10, 10, 5, color);
                    Raylib.DrawText($"Timer: {Program.getTimeS() - Program.startTime}", 10, 20, 5, color);
                    Raylib.DrawText($"Player X: {x}", 10, 30, 5, color);
                    Raylib.DrawText($"Has Primary Ball?: {pball.inPlay}", 10, 40, 5, color);
                    Raylib.DrawText($"Balls in Play: {ballManager.balls.Count}", 10, 50, 5, color);
                    Raylib.DrawText($"World Decorations Rendered: {DecorManager.decor.Count}", 10, 60, 5, color);
                    Raylib.DrawText($"Fractional Sky Phase Delta: {skyPhaseFrames}/{ticksPerPhase}", 10, 70, 5, color);
                    Raylib.DrawText($"Showing Bounding Boxes", 10, 80, 5, color);
                }

                skyPhaseFrames++;
            } else
            {
                int m = Raylib.MeasureText("Paused", 25);
                Raylib.DrawTexture(ResourceManager.PAUSE_OVERLAY, 0, 0, Color.WHITE);
                Raylib.DrawText("Paused", Program.width / 2 - m / 2, Program.height / 2, 25, Color.WHITE);

                BUTTON_INGAME_SETTINGS.Tick();
                BUTTON_TO_MAIN_MENU.Tick();
            }
        }

        public static Color SKYCOLOR = Color.SKYBLUE;
        public static int skyPhaseFrames;
        private const int skyPhases = 4;
        private int skyPhase = 0;
        private int ticksPerPhase = 60*90;
        
        private static readonly Color[] SKY_COLOR_MAIN_PHASES = new Color[skyPhases]
        {
            Color.SKYBLUE,                  // day
            new Color(253, 96, 81, 255),    // evening
            Color.BLACK,                    // night
            new Color(199, 236, 234, 255)   // morning
        };

        public object HighscoreManager { get; private set; }

        private void TickSkyColor()
        {
            float delta = (float)skyPhaseFrames / (float)ticksPerPhase;

            SKYCOLOR = LerpBetweenColors(SKY_COLOR_MAIN_PHASES[skyPhase], SKY_COLOR_MAIN_PHASES[NextPhase(skyPhase)], delta);

            if (delta == 1) {
                skyPhase = NextPhase(skyPhase);
                skyPhaseFrames = 0;
            }
        }

        private static int NextPhase(int i)
        {
            int j = i += 1;

            if (j >= skyPhases)
            {
                return 0;
            }
            return j;
        }

        public static Color LerpBetweenColors(Color a, Color b, float delta)
        {
            int alpha = Program.clamp(0, 255, Lerp(a.a, b.a, delta));
            int red = Program.clamp(0, 255, Lerp(a.r, b.r, delta));
            int green = Program.clamp(0, 255, Lerp(a.g, b.g, delta));
            int blue = Program.clamp(0, 255, Lerp(a.b, b.b, delta));
            return new Color(red, green, blue, alpha);
        }

        public static int Lerp(int a, int b, float delta)
        {
            return (int) Math.Round(a + (b - a) * delta);
        }


        public static int CalculateUsefulDarknessByteSizedInt(Color color)
        {
            return Program.clamp(0, 255, (color.r + color.g + color.b) / 2);
        }

        public static int InvertColorByteSizedInt(int i)
        {
            return 255 - i;
        }

        public static Color GetVisibleTextColor(Color color)
        {
            int Icolor = InvertColorByteSizedInt(CalculateUsefulDarknessByteSizedInt(color));
            return new Color(Icolor, Icolor, Icolor, 255);
        }

        public static void OnLose(Player player)
        {
            GameState.ballManager.balls.Clear();
            GameState.ballManager.queue.Clear();
            Array.Clear(GameState.blockManager.blocks);

            Program.SwitchState((parent) => new KeyInputScreen("Enter Name for Highscore:", 16, (s) =>
            {
                HighScoreManager.AddNewScore(s, player.score);
                Program.SwitchState((parent) => new TitleMenuState(parent));
            }, parent));
        }
    }
}

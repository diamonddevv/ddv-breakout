using Breakout.Game;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal class GameState : WindowState
    {

        public static bool debug = false;


        public static BlockManager blockManager = new BlockManager();
        public static BallManager ballManager = new BallManager();
        private Player player;
        private PrimaryBall pball;

        public GameState() : base("In-Game")
        {
        }

        public override void Init()
        {
            this.player = new Player(360, 400);
            this.pball = new PrimaryBall(0, 0, Vector2.Zero, true);
            ballManager.balls.Add(pball);

            blockManager.Generate();
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            Raylib.DrawRectangleGradientV(0, player.y, Program.width, Program.height - player.y, Color.BLUE, Color.DARKBLUE);

            int digits = player.score.ToString().Length;
            Raylib.DrawText($"{player.score}", (Program.width / 2) - digits * 25, 240, 100, Color.DARKGRAY);
            Raylib.DrawText($"Lives: {player.lives}", 0 + 20, Program.height - 60, 50, Color.LIGHTGRAY);
            ballManager.Tick();

            player.Tick();
            ballManager.ForEachBall(b => b.Tick(player, blockManager, ballManager));

            player.Draw(debug);
            ballManager.ForEachBall(b => b.Draw(debug));
            blockManager.DrawAll(debug);


            if (debug)
            {
                int fps = Raylib.GetFPS();
                int x = player.x;

                Raylib.DrawText($"FPS: {fps}", 10, 10, 5, Color.BLACK);
                Raylib.DrawText($"Timer: {Program.getTimeS() - Program.startTime}", 10, 20, 5, Color.BLACK);
                Raylib.DrawText($"Player X: {x}", 10, 30, 5, Color.BLACK);
                Raylib.DrawText($"Has Primary Ball?: {pball.inPlay}", 10, 40, 5, Color.BLACK);
                Raylib.DrawText($"Balls in Play: {ballManager.balls.Count}", 10, 50, 5, Color.BLACK);
                Raylib.DrawText($"Showing Bounding Boxes", 10, 60, 5, Color.BLACK);
            }
        }
    }
}

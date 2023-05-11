using Breakout.Window;
using Breakout.Physics;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breakout.Util;
using Breakout.Resource;

namespace Breakout.Game
{
    internal class Player
    {
        public int x;
        public int y;
        public int score;
        public int lives;

        public int width = 80;
        public int height = 20;

        public Physics.BoundingBox boundingBox;

        public Player(int x, int y) {
            this.x = x;
            this.y = y;
            this.score = 0;
            this.lives = 5;

            this.boundingBox = new Physics.BoundingBox(x, y, width, height);
        }

        public void Tick()
        {
            PollControls();
            boundingBox.TrackPlayer(this);
            TestLose();
        }

        private void TestLose()
        {
            if (lives <= 0)
            {
                HighScoreManager.addNewScore("<Insert Name Here>", score);
                Raylib.PlaySound(ResourceManager.LOSE);
                GameState.ballManager.balls.Clear();
                GameState.ballManager.queue.Clear();
                Array.Clear(GameState.blockManager.blocks);
                Program.RevertToLastState();
            }
        }

        public void PollControls()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_GRAVE))
            {
                GameState.debug = !GameState.debug;
            }

            if (!(bool)Settings.settingsSSKVPF.GetObject(Settings.KEY_USEMOUSE))
            {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                {
                    x -= 5;
                }

                if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    x += 5;
                }
            } else
            {
                x = Raylib.GetMouseX() - width / 2;
            }

            (int x, int y) clamped = Program.ClampPositionInsideWindow(x, y, width, height);

            x = clamped.x;
            y = clamped.y;
        }

        public void Draw(bool drawBoundingBox)
        {
            Raylib.DrawRectangleRounded(new Rectangle(x, y, width, height), 0.25f, 1, Color.BLACK);
            if (drawBoundingBox) boundingBox.DrawBoundingBox();
        }


        public void OnEmpty()
        {
            this.lives++;
        }
    }
}

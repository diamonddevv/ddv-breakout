using Breakout.Util;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal class HighscoresScreenState : WindowState
    {
        public HighscoresScreenState() : base("Viewing Highscores")
        {
        }

        public static Button BUTTON_BACK;

        public override void Init()
        {
            HighScoreManager.Fetch();
            //HighScoreManager.sort();

            BUTTON_BACK = new Button(20, 20, 128, 96, "Back", 25, () =>
            {
                Program.RevertToLastState();
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);

            int starty = 40 + 96;
            int x = 20;
            int i = 1;

            foreach (var score in HighScoreManager.cachedHighscores)
            {
                Raylib.DrawText($"{i}) Score: {score}", x, starty + (25 * i), 25, Color.BLACK);
                i++;
            }

            BUTTON_BACK.Tick();
        }
    }
}

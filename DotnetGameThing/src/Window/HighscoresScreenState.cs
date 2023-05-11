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
            HighScoreManager.Sort();

            BUTTON_BACK = new Button(20, 20, 128, 96, "Back", 25, () =>
            {
                Program.RevertToLastState();
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            DrawMenuGradient();

            int starty = 40 + 96;
            int x = 20;
            int i = 1;

            
            if (HighScoreManager.cachedHighscores.Count <= 0)
            {
                Raylib.DrawText($"No scores yet! Play a bit first.", x, starty, 25, Color.BLACK);
                if (HighScoreManager.highscoresSave.ReadWasCorrupted())
                {
                    Raylib.DrawText($"The scores previously stored were corrupted, \nor the file format was changed. They are irretrievable.", x, starty + 30, 25, Color.BLACK);
                }
            } else
            {
                foreach (var scoring in HighScoreManager.cachedHighscores)
                {
                    int y = starty + (25 * i);
                    int m = $"{i})".Length;
                    Raylib.DrawText($"{i})", x, y, 25, Color.BLACK);
                    Raylib.DrawText($"{scoring.user}: {scoring.score} Points", x + m + 30, y, 25, Color.BLACK);
                    i++;
                }
            }

            BUTTON_BACK.Tick();
        }
    }
}

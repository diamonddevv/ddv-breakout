using Breakout.Game.Achievements;
using Breakout.Util;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Breakout.Util.HighScoreManager;

namespace Breakout.Window
{
    internal class AchievementsScreenState : WindowState
    {
        public AchievementsScreenState(WindowState? parent) : base("Viewing Achievements", parent)
        {
        }

        public static Button BUTTON_BACK;
        public static Button BUTTON_RESET;

        public override void Init()
        {
            AchievementManager.Fetch();

            BUTTON_BACK = new Button(20, 20, 128, 96, "Back", 25, () =>
            {
                Program.RevertToLastState();
            });

            BUTTON_RESET = new Button(Program.width - 148, 20, 128, 96, "Reset", 25, () =>
            {
                Program.SwitchState((parent) => new YesNoWindowState("Are you sure that you want to reset your achievement progress?", () =>
                {
                    AchievementManager.ACHIEVEMENT_SAVE.Reset(false);
                    AchievementManager.ACHIEVEMENT_CACHE.Clear();
                    Program.RevertToLastState();
                }, () => Program.RevertToLastState(), parent));
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            DrawMenuGradient();

            int starty = 40 + 96;
            int x = 20;
            int i = 1;

            foreach (var achv in AchievementManager.ACHIEVEMENT_CACHE)
            {
                if (AchievementManager.achievementDict.TryGetValue(achv.Key, out var data)) {
                    int y = starty + (25 * i);
                    Raylib.DrawText($"{data.Name} - {data.Description} : {achv.Value}", x, y, 25, Color.BLACK);
                    i++;
                }
            }

            BUTTON_BACK.Tick(); BUTTON_RESET.Tick();
        }
    }
}

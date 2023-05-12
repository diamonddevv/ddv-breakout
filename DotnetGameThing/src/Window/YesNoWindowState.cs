using Breakout.Resource;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal class YesNoWindowState : WindowState
    {
        private string question;

        private static Button BUTTON_Y, BUTTON_N;

        public YesNoWindowState(string question, Action onYes, Action onNo, WindowState? parent) : base(Program.GetPreviousWindowStateTitleConcat(), parent)
        {
            this.question = question;


            int y = (Program.height / 4) * 3;
            int cX = Program.width / 2;

            BUTTON_Y = new Button(cX - 148, y, 128, 96, "Yes", 25, onYes);
            BUTTON_N = new Button(cX + 20, y, 128, 96, "No", 25, onNo);
        }

        public override void Init()
        {
        }

        public override void UpdateWindow()
        {
            DrawMenuGradient();
            Raylib.DrawTexture(ResourceManager.PAUSE_OVERLAY, 0, 0, Color.WHITE);

            int i = Raylib.MeasureText(question, 20) / 2;
            Raylib.DrawText(question, Program.width / 2 - i, Program.height / 3, 20, Color.WHITE);

            BUTTON_Y.Tick(); BUTTON_N.Tick();
        }
    }
}

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
    internal class KeyInputScreen : WindowState
    {
        private string s;
        private int characterLimit;
        private string input;

        private static Button BUTTON_ACCEPT;

        public KeyInputScreen(string s, int characterLimit, Action<string> onAccept, WindowState? parent) : base(Program.GetPreviousWindowStateTitleConcat(), parent)
        {
            this.s = s;
            this.characterLimit = characterLimit;
            this.input = "";

            int y = (Program.height / 4) * 3;
            int cX = (Program.width / 3) * 2;

            BUTTON_ACCEPT = new Button(cX, y, 128, 96, "Accept", 25, () =>
            {
                onAccept.Invoke(input);
            });
        }

        public override void Init()
        {
            input = "";
        }

        public override void UpdateWindow()
        {   
            DrawMenuGradient();
            Raylib.DrawTexture(ResourceManager.PAUSE_OVERLAY, 0, 0, Color.WHITE);

            int i = Raylib.MeasureText(s, 20) / 2;
            Raylib.DrawText(s, Program.width / 2 - i, Program.height / 3, 20, Color.WHITE);

            YieldInput();

            int m = Raylib.MeasureText(input, 20) / 2;
            Raylib.DrawText(input, Program.width / 2 - m, Program.height / 3 + 20, 20, Color.WHITE);

            string cr = $"({characterLimit - input.Length} characters remaining)";
            int n = Raylib.MeasureText(cr, 20) / 2;
            Raylib.DrawText(cr, Program.width / 2 - n, Program.height / 3 + 40, 20, Color.WHITE);

            BUTTON_ACCEPT.Tick();
        }

        private string YieldInput()
        {
            if (!(input.Length >= characterLimit))
            {
                int key = Raylib.GetCharPressed();

                while (key > 0)
                {
                    if ((key >= 32) && (key <= 125))
                    {
                        input += (char)key;
                    }
                    key = Raylib.GetCharPressed();
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
            {
                if (input.Length > 0)
                {
                    char[] chars = input.ToCharArray();
                    chars[chars.Length - 1] = '\0';
                    string str = "";
                    foreach (char c in chars)
                    {
                        if (c != '\0') str += c;
                    }
                    input = str;    
                }
            }

            return s;
        }
    }
}

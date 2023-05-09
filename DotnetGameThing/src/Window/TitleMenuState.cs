using Breakout.Resource;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal class TitleMenuState : WindowState
    {
        private static (int w, int h) standardLogoDims = (48, 32);
        private static int scale = 8;

        private static Button BUTTON_PLAY;
        private static Button BUTTON_SETTINGS;
        private static Button BUTTON_HIGHSCORES;
        private static Button BUTTON_QUIT;

        public TitleMenuState() : base("Main Menu")
        {
        }

        public override void Init()
        {
            BUTTON_PLAY = new Button((int)(Program.width * 0.5) + 20, (int)(Program.height * 0.5), 128, 96, "Play", 25, () =>
            {
                Program.SwitchState(new GameState());
            });

            BUTTON_SETTINGS = new Button((int)(Program.width * 0.5) - 20 - 128, (int)(Program.height * 0.5), 128, 96, "Settings", 25, () =>
            {
                Program.SwitchState(new SettingsState());
            });

            BUTTON_HIGHSCORES = new Button((int)(Program.width * 0.5) - 20 - 128, (int)(Program.height * 0.5) + 20 + 96, 128, 96, "Highscores", 19, () =>
            {
                Program.SwitchState(new HighscoresScreenState());
            });


            BUTTON_QUIT = new Button((int)(Program.width * 0.5) + 20, (int)(Program.height * 0.5) + 20 + 96, 128, 96, "Quit", 25, () =>
            {
                Program.Quit(1);
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);

            int logoX = Program.width / 2;
            int logoY = Program.height / 32;

            Raylib.DrawTextureEx(ResourceManager.LOGO, new Vector2(logoX - ((standardLogoDims.w / 2) * scale), logoY), 0, scale, Color.WHITE);

            BUTTON_PLAY.Tick();
            BUTTON_SETTINGS.Tick();
            BUTTON_HIGHSCORES.Tick();
            BUTTON_QUIT.Tick();
        }
    }
}

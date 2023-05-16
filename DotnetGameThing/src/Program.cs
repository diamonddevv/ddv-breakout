using Breakout.Game;
using Breakout.Window;
using Breakout.Resource;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using Breakout.Util;
using Breakout.Game.Achievements;

namespace BreakoutGame
{
    internal class Program
    {
        public static int width = 800;
        public static int height = 480;

        public static (int w, int h) win = (0, 0);

        public static string title = "DDV Breakout";
        public static int targetFps = 60;
        public static string iconLoc = "assets/icon.png";

        public static long startTime;

        private static WindowState currentState = new TitleMenuState(null);

        static void Main(string[] args)
        {
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE); // resizable window

            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(targetFps);
            Raylib.SetWindowIcon(Raylib.LoadImage(iconLoc));
            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            Raylib.InitAudioDevice();

            ResourceManager.LoadResources();

            startTime = getTimeS();

            AchievementManager.Prepare();
            HighScoreManager.highscoresSave.read();


            Settings.Default();
            Settings.ForEachSetting(s =>
            {
                Settings.settingsSSKVPF.dict.Add(s.key, s.obj);
            });
            Settings.settingsSSKVPF.read();
            Settings.settingsSSKVPF.write();


            // framebuffer
            RenderTexture2D FRAMEBUFFER = Raylib.LoadRenderTexture(width, height);
            Raylib.SetTextureFilter(FRAMEBUFFER.texture, TextureFilter.TEXTURE_FILTER_TRILINEAR);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.SetWindowTitle(title + " - " + currentState.titleConcat);


                Raylib.SetMasterVolume((float)Settings.GetValue(Settings.KEY_MASTERVOL));


                win = (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

                // Draw Game to Framebuffer
                Raylib.BeginTextureMode(FRAMEBUFFER);
                currentState.UpdateWindow();
                Raylib.EndTextureMode();


                // Draw Framebuffer to Windows
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                DrawFramebuffer(FRAMEBUFFER);
                Raylib.EndDrawing();
            }

            ResourceManager.UnloadResources();
            Raylib.CloseWindow();
        }

        public static void DrawFramebuffer(RenderTexture2D framebuffer)
        {
            ResourceManager.DrawSpreadTexture(framebuffer.texture, win.w, win.h, Color.WHITE);
        }

        public static (int x, int y) GetFramebufferMousePos()
        {
            (float x, float y) mouse = (Raylib.GetMouseX(), Raylib.GetMouseY());

            float tX = mouse.x / win.w;
            float tY = mouse.y / win.h;
            mouse.x = tX * width;
            mouse.y = tY * height;

            return ((int)Math.Round(mouse.x), (int)Math.Round(mouse.y));
        }

        public static (int, int) ClampPositionInsideWindow(int x, int y, int w, int h)
        {
            int nX = x;
            int nY = y;

            if (x < 0) nX = 0;
            if (x + w > win.w) nX = win.w - w;
            if (y < 0) nY = 0;
            if (y + h > win.h) nY = win.h - h;

            return (nX, nY);
        }


        public static int clamp(int min, int max, int v)
        {
            return v > min ? v < max ? v : max : min;
        }

        public static int getTimeS()
        {
            return (int)DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public static void SwitchState(Func<WindowState, WindowState> switcher)
        {
            currentState = switcher.Invoke(currentState);
        }

        public static void RevertToLastState()
        {
            SwitchState((parent) => parent.parent);
        }

        public static string GetPreviousWindowStateTitleConcat()
        {
            return currentState.parent.titleConcat;
        }

        public static void Quit(int code)
        {
            Console.WriteLine($"Exiting.. (Exit Code: {code})");
            Environment.Exit(code);
        }
    }
}
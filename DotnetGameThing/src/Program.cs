﻿using Breakout.Game;
using Breakout.Window;
using Breakout.Resource;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using Breakout.Util;

namespace BreakoutGame
{
    internal class Program
    {
        public static int width = 800;
        public static int height = 480;
        public static string title = "DDV Breakout";
        public static int targetFps = 60;
        public static string iconLoc = "assets/icon.png";

        public static long startTime;

        private static WindowState? lastState = null;
        private static WindowState currentState = new TitleMenuState();

        static void Main(string[] args)
        {
            Raylib.InitWindow(width, height, title);
            Raylib.SetTargetFPS(targetFps);
            Raylib.SetWindowIcon(Raylib.LoadImage(iconLoc));
            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            Raylib.InitAudioDevice();

            ResourceManager.LoadResources();

            startTime = getTimeS();

            HighScoreManager.highscoresSave.read();

            Settings.Default();
            Settings.settingsSSKVPF.read();
            Settings.settingsSSKVPF.write();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.SetWindowTitle(title + " - " + currentState.titleConcat);

                width = Raylib.GetScreenWidth();
                height = Raylib.GetScreenHeight();

                Raylib.BeginDrawing();
                currentState.UpdateWindow();
                Raylib.EndDrawing();
            }

            ResourceManager.UnloadResources();
            Raylib.CloseWindow();
        }


        public static (int, int) ClampPositionInsideWindow(int x, int y, int w, int h)
        {
            int nX = x;
            int nY = y;

            if (x < 0) nX = 0;
            if (x + w > width) nX = width - w;
            if (y < 0) nY = 0;
            if (y + h > height) nY = height - h;

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

        public static void SwitchState(WindowState newState)
        {
            lastState = currentState;
            currentState = newState;
        }

        public static void RevertToLastState()
        {
            if (lastState != null)
            {
                WindowState tS = currentState;
                currentState = lastState;
                lastState = tS;
            }
        }

        public static void Quit(int code)
        {
            Console.WriteLine($"Exiting.. (Exit Code: {code})");
            Environment.Exit(code);
        }
    }
}
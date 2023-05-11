using Breakout.Resource;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal abstract class WindowState
    {
        public string titleConcat;

        public WindowState(string titleConcat) {
            this.titleConcat = titleConcat;
            
            Init();
        }

        public abstract void Init();

        public abstract void UpdateWindow();

        public static Color DrawMenuGradient()
        {
            Color color = GetSinColor();
            Raylib.DrawTexture(ResourceManager.MENU_GRADIENT_MAP, 0, 0, color);
            return color;
        }

        private const int colors = 7;
        private static Color[] SPECTRUM = new Color[colors]
        {
            Color.RED, Color.ORANGE, Color.YELLOW, Color.GREEN, Color.BLUE, Color.PURPLE, Color.PINK
        };
        private static int pos = 0;
        private static float index = 0;

        private static Color GetSinColor()
        {
            index += 0.01f;
            if (index >= 1)
            {
                index = 0;
                pos = GetNextPos(pos);
            }
            return GameState.LerpBetweenColors(SPECTRUM[pos], SPECTRUM[GetNextPos(pos)], index);
        }

        private static int GetNextPos(int i)
        {
            int j = i + 1;
            if (j >= colors)
            {
                return 0;
            } return j;
        }
    }
}

using Breakout.Resource;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal class Button
    {
        private (int x, int y) pos;
        private (int w, int h) dim;
        private string text;
        private int fontSize;
        private Action onPress;
        private bool highlight;

        public Button(int x, int y, int w, int h, string text, int fontSize, Action onPress)
        {
            this.pos = (x, y);
            this.dim = (w, h);
            this.text = text;
            this.fontSize = fontSize;
            this.onPress = onPress;

            this.highlight = false;
        }

        public void Tick()
        {
            Poll();
            Draw();
        }

        private void Draw()
        {
            Color p = highlight ? Color.BLUE : Color.RAYWHITE;
            Color s = highlight ? Color.WHITE : Color.BLACK;

            Raylib.DrawRectangle(pos.x, pos.y, dim.w, dim.h, p);
            Raylib.DrawRectangleLinesEx(new Rectangle(pos.x, pos.y, dim.w, dim.h), 7.5f, s);
            int m = Raylib.MeasureText(text, fontSize);
            Raylib.DrawText(text, pos.x + (dim.w / 2) - (m/2), pos.y + (dim.h / 2) - (fontSize / 2), fontSize, s);
        }

        private void Poll()
        {
            (int x, int y) mouse = (Raylib.GetMouseX(), Raylib.GetMouseY());

            if (CalculatePointIntersection(mouse, pos, dim))
            {
                highlight = true;
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    Raylib.PlaySound(ResourceManager.SELECT_MENU);
                    this.onPress.Invoke();
                }
            } else
            {
                highlight = false;
            }
        }



        public static bool CalculatePointIntersection((int x, int y) point, (int x, int y) rectPos, (int w, int h) rectDims)
        {
            return point.x >= rectPos.x && point.x <= rectPos.x + rectDims.w && point.y >= rectPos.y && point.y <= rectPos.y + rectDims.h;
        }
    }
}

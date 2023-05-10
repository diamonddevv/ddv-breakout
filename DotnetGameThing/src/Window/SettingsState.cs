using Breakout.Resource;
using Breakout.Util;
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
    internal class SettingsState : WindowState
    {
        public SettingsState() : base("Editing Settings")
        {
        }

        private static Button BUTTON_BACK;

        public override void Init()
        {
            Settings.settingsSSKVPF.read();

            BUTTON_BACK = new Button(20, 20, 128, 96, "Save & Exit", 18, () =>
            {
                Settings.ForEachSetting(s => s.Write());
                Settings.settingsSSKVPF.write();
                Program.RevertToLastState();
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            BUTTON_BACK.Tick();

            Settings.ForEachSetting(s => s.Tick());
        }

        public abstract class SettingType
        {
            public (int x, int y) pos;
            public string text;
            public object setting;

            public SettingType()
            {
            }

            public abstract void Poll();
            public abstract void Draw();
            public void Tick()
            {
                Poll(); Draw();
            }
        }

        public class BooleanSetting : SettingType
        {
            private static Rectangle frame = Scale(new Rectangle(0f, 0f, 16f, 16f), 2);
            private static Rectangle overlay = Scale(new Rectangle(16f, 0f, 16f, 16f), 2);

            public BooleanSetting() : base()
            {
            }

            public override void Poll()
            {
                int m = Raylib.MeasureText(text, 20);
                (int x, int y) mouse = (Raylib.GetMouseX(), Raylib.GetMouseY());

                if (Button.CalculatePointIntersection(mouse, (pos.x + m + 5, pos.y - 10), (32, 32)))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        setting = !((bool) setting);
                    }
                }
            }

            public override void Draw()
            {
                int m = Raylib.MeasureText(text, 20);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);
                var drawPos = new Vector2(pos.x + m + 5, pos.y - 10);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, frame, drawPos, Color.WHITE);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, overlay, drawPos, (bool)setting ? Color.GREEN : Color.RED);
            }
        }

        public class IntSetting : SettingType
        {
            private static Rectangle pull = Index(0, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle line = Index(2, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endl = Index(1, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endr = Index(3, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));

            public IntSetting() : base()
            {
            }

            public override void Poll()
            {

            }

            public override void Draw()
            {
                int m = Raylib.MeasureText(text, 20);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);
                int drawStartX = pos.x + m + 5;
                int drawY = pos.y - 24;
                //Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endl, new Vector2(drawStartX + (48 * 0), drawY), Color.WHITE);
                //for (int i = 0; i < 10; i++) { Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, line, new Vector2(drawStartX + (48 * i), drawY), Color.WHITE); }
                //Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endr, new Vector2(drawStartX + (48 * 10), drawY), Color.WHITE);
            }

            
        }

        private static Rectangle Scale(Rectangle rect, int scale)
        {
            return new Rectangle(rect.x * scale, rect.y * scale, rect.width * scale, rect.height * scale);
        }

        private static Rectangle Index(int i, Rectangle rect)
        {
            return new Rectangle(rect.x * i, rect.y, rect.width, rect.height);
        }
    }
}

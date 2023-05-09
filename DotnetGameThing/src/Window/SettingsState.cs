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
using static System.Formats.Asn1.AsnWriter;

namespace Breakout.Window
{
    internal class SettingsState : WindowState
    {
        public SettingsState() : base("Editing Settings")
        {
        }

        private static Button BUTTON_BACK;

        private static BooleanSetting BS_USEMOUSE;
        private static BooleanSetting BS_USEEMPTY;
        private static IntSetting IS_VOL;

        public override void Init()
        {
            Settings.settingsSSKVPF.read();

            BUTTON_BACK = new Button(20, 20, 128, 96, "Save & Exit", 18, () =>
            {
                Settings.settingsSSKVPF.SetObject( Settings.KEY_USEMOUSE,   BS_USEMOUSE.setting  );
                Settings.settingsSSKVPF.SetObject( Settings.KEY_USEEMPTY,   BS_USEEMPTY.setting  );
                Settings.settingsSSKVPF.SetObject( Settings.KEY_MASTERVOL,  IS_VOL.setting       );


                Settings.settingsSSKVPF.write();
                Program.RevertToLastState();
            });

            BS_USEMOUSE = new BooleanSetting(   (20, 120), "Use Mouse to Control Paddle:",  (bool)  Settings.settingsSSKVPF.GetObject(Settings.KEY_USEMOUSE));
            BS_USEEMPTY = new BooleanSetting(   (20, 150), "Use Empty Blocks:",             (bool)  Settings.settingsSSKVPF.GetObject(Settings.KEY_USEEMPTY));
            IS_VOL = new IntSetting(            (20, 180), "Master Volume:",                (int)   Settings.settingsSSKVPF.GetObject(Settings.KEY_MASTERVOL));
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            BUTTON_BACK.Tick();

            BS_USEMOUSE.Tick();
            BS_USEEMPTY.Tick();
            IS_VOL.Tick();
        }

        public class BooleanSetting
        {
            private static Rectangle frame = Scale(new Rectangle(0f, 0f, 16f, 16f), 2);
            private static Rectangle overlay = Scale(new Rectangle(16f, 0f, 16f, 16f), 2);

            private (int x, int y) pos;
            public string text;
            public bool setting;
            public BooleanSetting((int x, int y) pos, string text, bool setting)
            {
                this.pos = pos;
                this.text = text;
                this.setting = setting;
            }

            private void Poll()
            {
                int m = Raylib.MeasureText(text, 20);
                (int x, int y) mouse = (Raylib.GetMouseX(), Raylib.GetMouseY());

                if (Button.CalculatePointIntersection(mouse, (pos.x + m + 5, pos.y - 10), (32, 32)))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        setting = !setting;
                    }
                }
            }

            private void Draw()
            {
                int m = Raylib.MeasureText(text, 20);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);
                var drawPos = new Vector2(pos.x + m + 5, pos.y - 10);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, frame, drawPos, Color.WHITE);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, overlay, drawPos, setting ? Color.GREEN : Color.RED);
            }

            public void Tick()
            {
                Poll(); Draw();
            }
        }

        public class IntSetting
        {
            private static Rectangle pull = Index(0, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle line = Index(2, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endl = Index(1, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endr = Index(3, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));

            private (int x, int y) pos;
            public string text;
            public int setting;
            public IntSetting((int x, int y) pos, string text, int setting)
            {
                this.pos = pos;
                this.text = text;
                this.setting = setting;
            }

            private void Poll()
            {

            }

            private void Draw()
            {
                int m = Raylib.MeasureText(text, 20);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);
                int drawStartX = pos.x + m + 5;
                int drawY = pos.y - 24;
                //Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endl, new Vector2(drawStartX + (48 * 0), drawY), Color.WHITE);
                //for (int i = 0; i < 10; i++) { Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, line, new Vector2(drawStartX + (48 * i), drawY), Color.WHITE); }
                //Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endr, new Vector2(drawStartX + (48 * 10), drawY), Color.WHITE);
            }

            public void Tick()
            {
                Poll(); Draw();
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

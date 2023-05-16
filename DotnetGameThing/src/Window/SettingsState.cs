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
        public SettingsState(WindowState? parent) : base("Editing Settings", parent)
        {
        }

        private static Button BUTTON_BACK;

        private static int longestSettingText = 0;
        private static int furthestDrawDistance = 0;
        private static int longestSettingTool = 0;

        public static void CheckLength(int m)
        {
            if (m > longestSettingText)
            {
                longestSettingText = m;
            }
        }
        public static void CheckDrawDistance(int m)
        {
            if (m > furthestDrawDistance)
            {
                furthestDrawDistance = m;
            }
        }
        public static void CheckToolLength(int m)
        {
            if (m > longestSettingTool)
            {
                longestSettingTool = m;
            }
        }

        public override void Init()
        {
            Settings.settingsSSKVPF.read();
            Settings.ForEachSetting(x => { x.Init(); });

            BUTTON_BACK = new Button(20, 10, 128, 96, "Save & Exit", 18, () =>
            {
                Settings.ForEachSetting(s => s.Write());
                Settings.settingsSSKVPF.write();
                Program.RevertToLastState();
            });
        }

        public override void UpdateWindow()
        {
            Raylib.ClearBackground(Color.SKYBLUE);
            DrawMenuGradient();

            BUTTON_BACK.Tick();

            Raylib.DrawRectangleRounded(new Rectangle(8, 111, longestSettingText + longestSettingTool + 84, (Settings.wrapperMap.Count * 30) + 10), 0.25f, 10, Color.WHITE);
            Settings.ForEachSetting(s => s.Tick());
        }

        public abstract class SettingType
        {
            public (int x, int y) pos;
            public string text;
            public string settingKey;
            public object setting;

            public SettingType()
            {
            }

            public abstract void Poll();
            public abstract void Draw();

            public virtual void Initialize()
            {
                this.setting = Settings.GetValue(settingKey);
            }

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
                (int x, int y) mouse = Program.GetFramebufferMousePos();

                var drawPos = UpdateDrawPoint(m);

                if (Button.CalculatePointIntersection(mouse, ((int)drawPos.X, (int)drawPos.Y), (32, 32)))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        Raylib.PlaySound(ResourceManager.SELECT_MENU);
                        setting = !(bool) setting;
                    }
                }
            }


            public override void Draw()
            {
                int m = Raylib.MeasureText(text, 20);
                CheckLength(m);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);
                var drawPos = UpdateDrawPoint(m);
                CheckToolLength((int)frame.width);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, frame, drawPos, Color.WHITE);
                Raylib.DrawTextureRec(ResourceManager.ATLAS_BOOLEAN, overlay, drawPos, (bool)setting ? Color.GREEN : Color.RED);
            }

            private Vector2 UpdateDrawPoint(int m)
            {
                var drawPos = new Vector2(pos.x + m + 5, pos.y - 8);
                CheckDrawDistance((int)drawPos.X);
                drawPos.X = furthestDrawDistance;
                return drawPos;
            }
        }

        public class FloatSetting : SettingType
        {
            private static Rectangle pull = Index(0, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle line = Index(2, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endl = Index(1, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));
            private static Rectangle endr = Index(3, Scale(new Rectangle(16f, 0f, 16f, 16f), 3));


            private Func<double, string> deltaHandle;
            private int m;
            private float pullDelta;
            private (int x, int y) pullDrawPos;
            private ((int x, int y) start, (int x, int y) end) barEdges;

            private bool initialized;
            private bool hasCalculatedEdgePositions;

            private const int SECTIONS = 5;

            public FloatSetting(Func<double, string> deltaHandle) : base()
            {
                this.deltaHandle = deltaHandle;

                this.m = Raylib.MeasureText(text, 20);

                this.pullDelta = 0;

                this.pullDrawPos = (0,0);
                this.barEdges = ((0, 0),(0, 0));
            }

            public override void Initialize()
            {
                base.Initialize();
                this.initialized = false;
            }

            public override void Poll()
            {
                (int x, int y) mouse = Program.GetFramebufferMousePos();

                if (Button.CalculatePointIntersection(mouse, pullDrawPos, ((int)Math.Round(pull.width), (int)Math.Round(pull.height))))
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        pullDrawPos.x = mouse.x - (int)pull.width / 2;
                    }
                }

                if (!initialized && hasCalculatedEdgePositions)
                {
                    pullDelta = (float)setting;
                    pullDrawPos.x = (int)Math.Round(GetDrawPos(barEdges.start.x, barEdges.end.x, pullDelta));
                    initialized = true;
                }
                if (initialized)
                {
                    pullDrawPos.x = Math.Clamp(pullDrawPos.x, barEdges.start.x, barEdges.end.x); // Clamp Pull Pos

                    pullDelta = GetPercentage(barEdges.start.x, barEdges.end.x, pullDrawPos.x);
                    setting = pullDelta;
                }
            }

            public static float GetPercentage(float a, float b, float delta)
            {
                return (delta-a)/(b-a);
            }

            public static float GetDrawPos(float a, float b, float p)
            {
                return (p * b) - (p * a) + a;
            }

            public override void Draw()
            {
                CheckLength(m);
                Raylib.DrawText(text, pos.x, pos.y, 20, Color.BLACK);

                var drawPos = UpdateDrawPoint(m);

                var startv = MultiplyIndex(0, drawPos);
                var endv = MultiplyIndex(SECTIONS, drawPos);
                barEdges.start = ((int)startv.X, (int)startv.Y);
                barEdges.end = ((int)endv.X, (int)endv.Y);
                if (!hasCalculatedEdgePositions)
                {
                    hasCalculatedEdgePositions = true;
                }

                Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endl, startv, Color.WHITE);
                for (int i = 0; i < SECTIONS; i++) { Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, line, MultiplyIndex(i, drawPos), Color.WHITE); }
                Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, endr, endv, Color.WHITE);


                CheckToolLength(((int)line.width) * SECTIONS + 2);

                // Draw Pull
                pullDrawPos.y = (int)startv.Y;
                Raylib.DrawTextureRec(ResourceManager.ATLAS_INT, pull, new Vector2(pullDrawPos.x, pullDrawPos.y), Color.WHITE);

                // Draw Delta Text
                string s = deltaHandle.Invoke(pullDelta);
                int ma = Raylib.MeasureText(s, 20);
                int x = (int)startv.X - 20 - ma;
                int y = (int)startv.Y + 20;
                Raylib.DrawText(s, x, y, 20, Color.BLACK);
            }

            private Vector2 UpdateDrawPoint(int m)
            {
                var drawPos = new Vector2(pos.x + m + 5, pos.y - 16);
                CheckDrawDistance((int)drawPos.X);
                drawPos.X = furthestDrawDistance;
                return drawPos;
            }

            private Vector2 MultiplyIndex(int i, Vector2 vec)
            {
                vec.X += (48 * i);
                return vec;
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

using Breakout.Resource;
using Breakout.Window;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Decor
{
    internal class Cloud : Decor
    {
        public class CloudUVs
        {
            public static readonly Rectangle BIG = new Rectangle(0, 0, 32, 16);
            public static readonly Rectangle SMALL1 = new Rectangle(32, 0, 16, 16);
            public static readonly Rectangle SMALL2 = new Rectangle(48, 0, 16, 16);

            public static Rectangle GetRandomUV(Random random)
            {
                int i = random.Next(3);
                switch (i) {
                    case 0: return BIG;
                    case 1: return SMALL1;
                    case 2: return SMALL2;

                    default: return BIG;
                }
            }
        }

        private (int x, int y) pos;
        private bool left;
        private Rectangle uv;
        private (int unitPerMove, int tickPerMove) speed;

        private int ticksSinceLastMove = 0;

        public Cloud(int y, bool left, Rectangle uv, int tickPerMove, int unitPerMove) : base() {
            this.pos = (left ? (int)(-10 - uv.width) : Program.width + 10, y);
            this.left = left;
            this.uv = uv;
            this.speed = (unitPerMove, tickPerMove);
        }

        public override void Tick()
        {
            if (!redundant)
            {
                if (ticksSinceLastMove == speed.tickPerMove)
                {
                    if (left)
                    {
                        pos.x += speed.unitPerMove;
                    }
                    else
                    {
                        pos.x -= speed.unitPerMove;
                    }

                    ticksSinceLastMove = 0;
                } else {
                    ticksSinceLastMove++;
                }
                if (pos.x < 0 - 10 - uv.width || pos.x > Program.width + 10)
                {
                    redundant = true;
                }
                Draw();
            }
        }

        public override void Draw()
        {
            

            Raylib.DrawTextureRec(ResourceManager.ATLAS_DECOR, uv, new Vector2(pos.x, pos.y), new Color(255, 255, 255, GameState.CalculateUsefulDarknessByteSizedInt(GameState.SKYCOLOR)));
        }


        public static Cloud CreateCloud(Random r)
        {
            int tpm = r.Next(-1, 5);
            return new Cloud(r.Next(10, Program.height / 3), r.Next(2) == 1 ? true : false, CloudUVs.GetRandomUV(r), tpm, tpm == 0 ? r.Next(2) : 1);
        }
    }
}

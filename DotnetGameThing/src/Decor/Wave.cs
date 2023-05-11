using Breakout.Game;
using Breakout.Resource;
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
    internal class Wave : Decor
    {
        public static readonly Rectangle uv = new Rectangle(0, 16, 16, 16);
        private int speed;
        public (int x, int y) pos;

        public Wave(int playery, Random r) : base()
        {
            this.speed = r.Next(2);
            this.pos = (Program.width, (int)(playery - uv.height));
        }

        public override void Draw()
        {
            Raylib.DrawTextureRec(ResourceManager.ATLAS_DECOR, uv, new Vector2(pos.x, pos.y), Color.WHITE);
        }

        public override void Tick()
        {
            pos.x -= speed;
            if (pos.x <= -16)
            {
                this.redundant = true;
            }

            Draw();
        }
    }
}

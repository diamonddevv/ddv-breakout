using Breakout.Game;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Physics
{
    internal class BoundingBox
    {
        public (int x, int y) xy1;
        public (int w, int h) dims;
        public (int x, int y) xy2;

        public BoundingBox(int x, int y, int width, int height)
        {
            this.xy1 = (x, y);
            this.xy2 = (x + width, y + height);
            this.dims = (width, height);
        }


        public void TrackPlayer(Player player)
        {
            this.xy1 = (player.x, player.y);
            this.dims = (player.width, player.height);
            refreshXY2();
        }

        public void TrackBall(Ball ball)
        {
            int hyp = (int) Math.Sqrt(ball.radius + ball.radius);

            this.xy1 = (ball.x - hyp, ball.y - hyp);
            this.dims = ((int) ball.radius * 2, (int) ball.radius * 2);
            refreshXY2();
        }

        public void refreshXY2()
        {
            this.xy2 = (xy1.x + dims.w, xy1.y + dims.h);
        }

        public bool IntersectsBox(BoundingBox other)
        {
            return
                   this.xy1.x < other.xy1.x + other.dims.w &&
                   this.xy1.x + other.dims.w > other.xy1.x &&
                   this.xy1.y < other.xy1.y + other.dims.h &&
                   this.dims.h + other.xy1.y > other.xy1.y;
        }

        public void DrawBoundingBox()
        {
            Raylib.DrawRectangleLinesEx(new Rectangle(xy1.x, xy1.y, dims.w, dims.h), 1.5f, Color.LIME);
        }

    }
}

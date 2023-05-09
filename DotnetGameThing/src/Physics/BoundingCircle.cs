using Breakout.Game;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Physics
{
    internal class BoundingCircle
    {
        private (int x, int y) c;
        private float r;

        public BoundingCircle(int cX, int cY, float radius)
        {
            this.c = (cX, cY);
            this.r = radius;
        }

        public void TrackBall(Ball ball)
        {
            this.c = (ball.x, ball.y);
            this.r = ball.radius;
        }

        public (bool test, bool side) IntersectsBox(BoundingBox box)
        { // adapted from: http://jeffreythompson.org/collision-detection/circle-rect.php - thank you for publishing this!

            // temporary variables to set edges for testing
            float testX = c.x;
            float testY = c.y;

            bool side = false;

            // which edge is closest?
            if (c.x < box.xy1.x){                   testX = box.xy1.x;              side = true;  }  // left edge
            else if (c.x > box.xy1.x + box.dims.w){ testX = box.xy1.x + box.dims.w; side = true;  }  // right edge
            if (c.y < box.xy1.y){                   testY = box.xy1.y;              side = false; }  // top edge
            else if (c.y > box.xy1.y + box.dims.h){ testY = box.xy1.y + box.dims.h; side = false; }  // bottom edge

            // get distance from closest edges
            float distX = c.x - testX;
            float distY = c.y - testY;
            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            // if the distance is less than the radius, collision!
            if (distance <= r)
            {
                return (true, side);
            }
            return (false, side);
        }

        public void DrawBoundingBox()
        {
            Raylib.DrawCircleLines(c.x, c.y, r, Color.LIME);
        }
    }
}

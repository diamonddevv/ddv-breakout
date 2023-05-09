using Breakout.Resource;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Game
{
    internal class BallBlock : Block
    {
        public BallBlock(BlockState state, int row, int column, int x, int y, int height, int width) : base(state, row, column, x, y, height, width, Color.GREEN)
        {
        }

        public override void onBreak(Player player, BallManager ballManager, BlockManager manager)
        {
            base.onBreak(player, ballManager, manager);
            Random r = new Random();

            (int x, int y) sp = (pos.x + (BlockManager.blockWidth / 2), pos.y + (BlockManager.blockHeight / 2));
            Ball ball = new Ball(sp.x, sp.y, new Vector2(0, 3), false);
            ballManager.queue.Add(ball);
        }

        public override void DrawOverlayedTexture()
        {
            Raylib.DrawTexture(ResourceManager.SPECIAL_BLOCK_OVERLAY, pos.x, pos.y, Color.WHITE);
        }
    }
}

using Breakout.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreakoutGame;
using Raylib_cs;

namespace Breakout.Game
{
    internal class Block
    {
        public static Color DEFAULT_COLOR = Color.RED;

        public BlockState state;
        public int row;
        public int column;

        public (int x, int y) pos;
        public (int width, int height) dims;
        public Color color;
        public Physics.BoundingBox? boundingBox;

        public Block(BlockState state, int row, int column, int x, int y, int height, int width, Color color)
        {
            this.state = state;
            this.row = row;
            this.column = column;

            this.pos = (x, y);
            this.dims = (width, height);

            this.color = color;

            this.boundingBox = new Physics.BoundingBox(x, y, width, height);
        }

        public void Break(Player player, BallManager ballManager, BlockManager blockManager)
        {
            this.state = BlockState.Empty;
            this.boundingBox = null;
            player.score += 1;
            onBreak(player, ballManager, blockManager);
        }

        public virtual void onBreak(Player player, BallManager ballManager, BlockManager manager)
        {

        }

        public virtual void DrawOverlayedTexture()
        {

        }

        public enum BlockState
        {
            Empty, Present
        }
    }
}

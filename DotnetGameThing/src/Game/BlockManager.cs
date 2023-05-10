using Breakout.Resource;
using Breakout.Util;
using BreakoutGame;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Breakout.Game.Block;

namespace Breakout.Game
{
    internal class BlockManager
    {
        public static int originX = 35;
        public static int originY = 20;
        public static int padding = 5;
        public static int columns = 7;
        public static int rows = 4;

        public static int blockWidth = 100;
        public static int blockHeight = 50;

        public Block[,] blocks;

        public BlockManager() 
        {
            this.blocks = new Block[columns, rows];
        }

        public void Generate()
        {
            Random r = new Random();

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {

                    int posX = originX + (x * blockWidth) + (x * padding);
                    int posY = originY + (y * blockHeight) + (y * padding);
                    blocks[x,y] = GetRandomBlock(r.Next(0, 101), Block.BlockState.Present, y, x, posX, posY, blockHeight, blockWidth, new Color(getRandomByteInt(r), getRandomByteInt(r), getRandomByteInt(r), 255));
                }
            }
        }

        public void BreakBlock(int column, int row, Player player, BallManager ballManager, BlockManager blockManager)
        {
            blocks[column, row].Break(player, ballManager, blockManager);
            TestEmpty(player, ballManager);
        }

        public void TestEmpty(Player player, BallManager balls)
        {
            bool empty = true;
            foreach (var block in blocks)
            {
                if (block.state.Equals(Block.BlockState.Present))
                {
                    empty = false;
                    break;
                }
            }

            if (empty)
            {
                OnEmpty(player, balls);
            }
        }

        public void OnEmpty(Player player, BallManager balls)
        {
            Raylib.PlaySound(ResourceManager.CLEARED);
            balls.ForEachBall(ball => { if (ball != null) { ball.OnEmpty(); } });
            player.OnEmpty();
            Generate();
        }

        public void DrawAll(bool debug)
        {
            foreach (var block in blocks)
            {
                if (block.state == Block.BlockState.Present)
                {
                    Raylib.DrawRectangle(block.pos.x, block.pos.y, block.dims.width, block.dims.height, block.color);
                    block.DrawOverlayedTexture();
                    if (debug) if (block.boundingBox != null) block.boundingBox.DrawBoundingBox();
                }
            }
        }


        public Block GetRandomBlock(int i, BlockState state, int row, int column, int x, int y, int height, int width, Color color)
        {
            if (RangeRoll(i, 20, 0) && (bool)Settings.GetValue(Settings.KEY_USEBALLBLOCK)) return new BallBlock(state, row, column, x, y, height, width);
            else if (RangeRoll(i, 10, 20) && (bool)Settings.GetValue(Settings.KEY_USEEMPTY)) return new Block(Block.BlockState.Empty, row, column, x, y, height, width, color);
            else return new Block(state, row, column, x, y, height, width, color);
        }

        public static bool RangeRoll(int roll, int range, int prevTotal)
        {
            return roll < range + prevTotal;
        }


        public static int getRandomByteInt(Random r)
        {
            return r.Next(0, 256);
        }
    }
}

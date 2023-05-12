using Breakout.Window;
using Breakout.Physics;
using Breakout.Resource;
using BreakoutGame;
using Raylib_cs;
using System.Numerics;
using Breakout.Util;

namespace Breakout.Game
{
    internal class Ball
    {
        public float radius = 8;

        public bool bound;
        public bool pause;
        public bool stopUsing;
        private int hyp;
        private BoundingCircle boundingCircle;
        protected Color color;
        public Player? player;
        public int x;
        public int y;
        public Vector2 speed;

        public static int speedV;

        public Ball(int x, int y, Vector2 speed, bool bound)
        {
            this.x = x;
            this.y = y;

            this.speed = speed;
            this.bound = bound;

            this.pause = false;
            this.stopUsing = false;

            this.hyp = (int)Math.Sqrt(radius) * 2;
            this.boundingCircle = new BoundingCircle(x, y, radius);

            this.color = Color.RAYWHITE;
        }

        public virtual void Tick(Player player, BlockManager manager, BallManager ballManager)
        {
            Move(player, manager, ballManager);
            boundingCircle.TrackBall(this);

            speedV = (int)((float)Settings.GetValue(Settings.KEY_BALLSPEED) * 10);
        }

        public void Move(Player player, BlockManager manager, BallManager ballManager)
        {
            if (!pause)
            {
                if (GameState.debug)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
                    {
                        this.x = Raylib.GetMouseX();
                        this.y = Raylib.GetMouseY();
                    }
                }


                if (this.bound)
                {
                    this.x = player.x + (player.width / 2);
                    this.y = player.y - 15;

                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                    {
                        this.bound = false;
                        this.speed = new Vector2(0, -speedV);
                    }

                }
                else
                {
                    x += (int)speed.X;
                    y += (int)speed.Y;
                }

                if (y - radius >= player.y) OnLost(player);

                if (!bound) Collide(player, manager, ballManager);
            }
        }

        public void Collide(Player player, BlockManager blockManager, BallManager ballManager)
        {

            // Ball v Bounds
            if (x + radius >= Program.width || x - radius <= 0)
            {
                speed.X *= -1;

                // sfx
                Raylib.PlaySound(ResourceManager.BOUNCE);
            }
            if (y - radius <= 0)
            {
                speed.Y *= -1;

                // sfx
                Raylib.PlaySound(ResourceManager.BOUNCE);
            }

            // Ball v Brick
            foreach (var block in blockManager.blocks)
            {
                if (block.boundingBox != null && boundingCircle.IntersectsBox(block.boundingBox).test)
                {
                    bool side = boundingCircle.IntersectsBox(block.boundingBox).side;
                    blockManager.BreakBlock(block.column, block.row, player, ballManager, blockManager);
                    speed.Y *= -1;
                    if (side) speed.X *= -1;

                    // sfx
                    Raylib.PlaySound(ResourceManager.BOUNCE);
                }
            }
        
            // Ball v Paddle
            if (boundingCircle.IntersectsBox(player.boundingBox).test)
            {
                bool side = boundingCircle.IntersectsBox(player.boundingBox).side;
                y -= 10 + speedV * (7/4);
                speed.Y *= -1;
                speed.X = (x - (player.x + player.width / 2)) / 8;
                if (side) speed.X *= -1;

                // sfx
                Raylib.PlaySound(ResourceManager.BOUNCE);
            }
        }

        public void Draw(bool drawBoundingCircle)
        {
            if (!stopUsing) {
                Raylib.DrawCircle(x, y, radius, color);
                if (drawBoundingCircle) boundingCircle.DrawBoundingBox();
            }
        }

        public void OnEmpty()
        {
            this.pause = true;
            Raylib.PlaySound(ResourceManager.CLEARED);
            if (this is PrimaryBall) Thread.Sleep(1000);
            this.bound = true;
            this.pause = false;
        }

        public virtual void OnLost(Player player)
        {
            Raylib.PlaySound(ResourceManager.DIE);
            stopUsing = true;
        }
    }
}

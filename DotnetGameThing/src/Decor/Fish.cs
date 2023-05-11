using Breakout.Game;
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
    internal class Fish : Decor
    {

        public class FishFrameUVs
        {
            public const int animationFrames = 4;

            public static readonly Rectangle[] RIGHT_TRAVEL_FISH = new Rectangle[animationFrames]       { new Rectangle(16, 16, 16, 8), new Rectangle(32, 16, 16, 8), new Rectangle(16, 16, 16, 8), new Rectangle(48, 16, 16, 8) };
            public static readonly Rectangle[] LEFT_TRAVEL_FISH  = new Rectangle[animationFrames]       { new Rectangle(16, 24, 16, 8), new Rectangle(32, 24, 16, 8), new Rectangle(16, 24, 16, 8), new Rectangle(48, 24, 16, 8) };
            public static readonly Rectangle[] SHINY_RIGHT_TRAVEL_FISH = new Rectangle[animationFrames] { new Rectangle(16, 32, 16, 8), new Rectangle(32, 32, 16, 8), new Rectangle(16, 32, 16, 8), new Rectangle(48, 32, 16, 8) };
            public static readonly Rectangle[] SHINY_LEFT_TRAVEL_FISH = new Rectangle[animationFrames]  { new Rectangle(16, 40, 16, 8), new Rectangle(32, 40, 16, 8), new Rectangle(16, 40, 16, 8), new Rectangle(48, 40, 16, 8) };


            public static Rectangle GetUVFrame(bool leftTravel, int frame, bool shiny)
            {
                if (leftTravel)
                {
                    if (shiny) return SHINY_LEFT_TRAVEL_FISH[frame];
                    return LEFT_TRAVEL_FISH[frame];
                } else
                {
                    if (shiny) return SHINY_RIGHT_TRAVEL_FISH[frame];
                    return RIGHT_TRAVEL_FISH[frame];
                }
            }
            public static int GetRandomFrame(bool leftTravel, Random r)
            {
               return r.Next(0, 3);
            }
        }

        private (int x, int y) pos;
        private bool left;
        private int frameIndex;
        private (int unitPerMove, int tickPerMove) speed;
        private int ticksPerFrame;
        private bool shiny;
        private bool leftTravel;
        private Rectangle frame;

        private int ticksSinceLastMove = 0;
        private int ticksSinceLastFrame = 0;

        public Fish(int y, bool left, int frameIndex, int tickPerMove, int unitPerMove, int ticksPerFrame, bool shiny) : base()
        {
            this.leftTravel = !left;

            this.frame = FishFrameUVs.GetUVFrame(leftTravel, frameIndex, shiny);

            this.pos = (left ? (int)(-10 - frame.width) : Program.width + 10, y);
            this.left = left;
            this.frameIndex = frameIndex;
            this.speed = (unitPerMove, tickPerMove);
            this.ticksPerFrame = ticksPerFrame;

            this.shiny = shiny;
        }

        public override void Draw()
        {
            Raylib.DrawTextureRec(ResourceManager.ATLAS_DECOR, frame, new Vector2(pos.x, pos.y), Color.WHITE);
        }

        public override void Tick()
        {
            if (!redundant)
            {
                if (ticksSinceLastMove >= speed.tickPerMove) // pos handle
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
                }
                else
                {
                    ticksSinceLastMove++;
                }

                if (ticksSinceLastFrame >= ticksPerFrame) // animation frame handle
                {

                    int nextFrame = frameIndex + 1;
                    if (nextFrame >= FishFrameUVs.animationFrames)
                    {
                        nextFrame = 0;
                    }
                    frameIndex = nextFrame;

                    frame = FishFrameUVs.GetUVFrame(leftTravel, frameIndex, shiny);
                    ticksSinceLastFrame = 0;
                }
                else ticksSinceLastFrame++;

                if (pos.x < 0 - 10 - frame.width || pos.x > Program.width + 10) // redundancy handle
                {
                    redundant = true;
                }
                Draw();
            }
        }

        public static Fish CreateFish(Random r, Player player)
        {
            int tpm = r.Next(-1, 5);
            bool left = r.Next(2) == 1 ? true : false;
            bool shiny = r.Next(2048) == 1 ? true : false;
            return new Fish(r.Next(400, Program.height), left, FishFrameUVs.GetRandomFrame(!left, r), tpm, tpm == 0 ? r.Next(5) : 1, r.Next(60), shiny);
        }
    }
}

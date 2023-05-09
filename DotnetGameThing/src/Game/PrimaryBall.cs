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
    internal class PrimaryBall : Ball
    {
        public bool inPlay;

        public PrimaryBall(int x, int y, Vector2 speed, bool bound) : base(x, y, speed, bound)
        {
            this.color = Color.GOLD;
            this.inPlay = true;
        }

        public override void Tick(Player player, BlockManager manager, BallManager ballManager)
        {
            if (player.lives <= 0)
            {
                this.pause = true;
                this.stopUsing = true;
            } else
            {
                this.pause = false;
                this.stopUsing = false;
            }

            this.inPlay = !pause && !stopUsing;

            base.Tick(player, manager, ballManager);
        }

        public override void OnLost(Player player)
        {
            this.bound = true;
            player.lives--;
            Raylib.PlaySound(ResourceManager.DIE);
        }
    }
}

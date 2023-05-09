using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Game
{
    internal class BallManager
    {
        public List<Ball> queue;
        private List<Ball> markedForRemoval;
        public List<Ball?> balls;

        public BallManager() {
            this.queue = new List<Ball>();
            this.balls = new List<Ball?>();
            this.markedForRemoval = new List<Ball>();
        }

        public void ForEachBall(Action<Ball?> consumer)
        {
            this.balls.ForEach(consumer);
        }

        public void Tick()
        {
            // Cycle Balls
            queue.ForEach(ball => 
            {
                balls.Add(ball);
            });
            queue.Clear();


            ForEachBall(ball =>
            {
                if (ball != null && ball.stopUsing)
                {
                    markedForRemoval.Add(ball);
                }
            });

            markedForRemoval.ForEach(ball => 
            {
                this.balls.Remove(ball);
            });
            markedForRemoval.Clear();
        }
    }
}

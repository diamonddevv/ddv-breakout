using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Decor
{
    internal class DecorManager
    {
        public static List<Decor> decor = new List<Decor>();

        public static void TickDecor()
        {
            List<Decor> toRemove = new List<Decor>();

            decor.ForEach(d => 
            {
                d.Tick();
                if (d.redundant)
                {
                    toRemove.Add(d);
                }
            });

            toRemove.ForEach(d =>
            {
                decor.Remove(d);
            });

            toRemove.Clear();
        }


        public static void Add(Decor d)
        {
            decor.Add(d);
        }

        public static void Clear()
        {
            decor.Clear();
        }
    }
}

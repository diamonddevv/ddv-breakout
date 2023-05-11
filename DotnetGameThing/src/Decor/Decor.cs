using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Decor
{
    internal abstract class Decor
    {
        public bool redundant;

        public Decor()
        {
            this.redundant = false;
        }

        public abstract void Draw();
        public abstract void Tick();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Window
{
    internal abstract class WindowState
    {
        public string titleConcat;

        public WindowState(string titleConcat) {
            this.titleConcat = titleConcat;
            
            Init();
        }

        public abstract void Init();

        public abstract void UpdateWindow();
    }
}

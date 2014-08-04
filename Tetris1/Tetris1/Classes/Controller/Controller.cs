using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Tetris1
{
    class Controller
    {
        public System.Timers.Timer timer;

        public Controller()
        {
            this.timer = new System.Timers.Timer(500);
        }

        public void Edit(int newTime)
        {
            this.timer = new System.Timers.Timer(newTime);
        }
    }
}

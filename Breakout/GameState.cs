using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    class GameState
    {
        public int[] Bricks { get; set; }
        public Ball Ball { get; set; }
        public Bat Bat { get; set; }

        public string Name { get; set; }

        public int Score { get; set; }
    }
}

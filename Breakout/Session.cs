using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    internal class Session
    {
        public EndPoint Address { get; set; }
        public DateTime LastCommTime { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public Ball Ball { get; set; }
        public Bat Bat { get; set; }
        public SolidBrush Brush { get; set; }
    }
}

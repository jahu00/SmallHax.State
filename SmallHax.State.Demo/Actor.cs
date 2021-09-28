using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.State.Demo
{
    public class Actor
    {
        public string Team { get; set; }
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public bool IsDead => Hp == 0;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class Coord {
        public int X { get; set; }
        public int Y { get; set; }
        public Coord() { }
        public Coord(int x, int y ) {
            X = x;
            Y = y;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class Process {
        public decimal Pid { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Icon { get; set; }
    }
}
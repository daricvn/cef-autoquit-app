﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class ScriptItem {
        public int Index { get; set; }
        public string EventType { get; set; }
        public string KeyName { get; set; }
        public long? X { get; set; }
        public long? Y { get; set; }
        public long TimeOffset { get; set; }
        public bool Active { get; set; }
        public bool SendInput { get; set; }
    }
}

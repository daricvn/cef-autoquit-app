using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class AppSettings {
        public bool DarkTheme { get; set; }
        public KeyCombination PlayHotkey { get; set; }
        public KeyCombination RecordHotkey { get; set; }
        public KeyCombination TopHotkey { get; set; }
        public int TypeSpeed { get; set; }
        public string Language { get; set; }
    }
}

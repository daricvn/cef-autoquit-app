using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    public class AppSettings {
        public bool DarkTheme { get; set; } = false;
        public KeyCombination PlayHotkey { get; set; } = null;
        public KeyCombination RecordHotkey { get; set; } = null;
        public KeyCombination TopHotkey { get; set; } = null;
        public int TypeSpeed { get; set; } = 10;
        public string Language { get; set; } = "en-US";
    }
}

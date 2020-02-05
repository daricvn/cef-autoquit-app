using Autoquit2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2.Models {
    public class KeyCombination {
        public bool CtrlKey { get; set; } = false;
        public bool AltKey { get; set; } = false;
        public bool ShiftKey { get; set; } = false;
        public string Key { get; set; }

        public bool IsInput(bool alt, bool ctrl, bool shift, Keys keys ) {
            return (alt == AltKey) && ctrl == CtrlKey && shift == ShiftKey &&
                ChromiumKeyConverter.GetKey(Key) == keys;
        }
    }
}

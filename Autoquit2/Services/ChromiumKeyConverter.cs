using Autoquit2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2.Services {
    public class ChromiumKeyConverter {
        public static Keys GetKey(string keyName) {
            if (Enum.TryParse<KeyCode>(keyName, out KeyCode keyCode) ) {
                var value = (int)keyCode;
                return (Keys)Enum.ToObject(typeof(Keys), value);
            }
            if (Enum.TryParse<Keys>(keyName, out Keys key) ) {
                return key;
            }
            return Keys.None;
        }
        public static KeyCode GetKeyCode(Keys which ) {
            var value = (int)which;
            return (KeyCode)Enum.ToObject(typeof(KeyCode), value);
        }
    }
}

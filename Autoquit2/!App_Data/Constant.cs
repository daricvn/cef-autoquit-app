using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2._App_Data {
    internal class Constant {
        public const string APP_SETTINGS_PATH = "settings.ini";
        public const string LOCALIZATION_FOLDER = "localization";

        public static string AppPath {
            get {
                return Path.GetDirectoryName(Application.ExecutablePath);
            }
        }

        public const string ENV_MOUSE_CLICK = "MOUSE_CLICK";
        public const string ENV_MOUSE_DOWN = "MOUSE_DOWN";
        public const string ENV_MOUSE_UP = "MOUSE_UP";
        public const string ENV_KEY_PRESS = "KEY_PRESS";
        public const string ENV_KEY_DOWN = "KEY_DOWN";
        public const string ENV_KEY_UP = "KEY_UP";
        public const string ENV_ENTER_TEXT = "ENTER_TEXT";
        public const string ENV_ENTER_SECRET = "ENTER_SECRET";
        public const string ENV_RANDOM_TEXT = "RANDOM_TEXT";
        public const string ENV_FROM_FILE = "FROM_FILE";
        public const string ENV_DO_NOTHING = "DO_NOTHING";
    }
}

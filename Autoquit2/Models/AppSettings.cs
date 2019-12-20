using System.Windows.Forms;

namespace Autoquit2.Models {
    public class AppSettings {
        public bool DarkTheme { get; set; } = false;
        public KeyCombination PlayHotkey { get; set; } = null;
        public KeyCombination RecordHotkey { get; set; } = null;
        public KeyCombination TopHotkey { get; set; } = null;
        public bool ShowMouseCoordinate { get; set; } = false;
        public int TypeSpeed { get; set; } = 10;
        public string Language { get; set; } = "en-US";
        public int Port { get; set; } = 7709;
        public string Version { get {
                return Application.ProductVersion;
            } 
        }
    }
}

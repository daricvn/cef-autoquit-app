using Autoquit2._App_Data;
using System.Windows.Forms;

namespace Autoquit2.Models {
    public class AppSettings {
        public bool DarkTheme { get; set; } = false;
        public KeyCombination PlayHotkey { get; set; } = new KeyCombination() {
            Key =""
        };
        public KeyCombination RecordHotkey { get; set; } = new KeyCombination() {
            Key = ""
        };
        public KeyCombination TopHotkey { get; set; } = new KeyCombination() {
            Key = ""
        };
        public bool ShowMouseCoordinate { get; set; } = false;
        public bool AutoMerge { get; set; } = true;
        public int TypeSpeed { get; set; } = 10;
        public string Language { get; set; } = "en-US";
        public int Port { get; set; } = 7709;
        public string Version { get {
                return Constant.AppVersion;
            } 
        }
    }
}

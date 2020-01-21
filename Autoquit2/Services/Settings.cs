using Autoquit2._App_Data;
using Autoquit2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Services {
    internal class Settings {
        public static AppSettings GetConfig() {
            var path = Path.Combine(Constant.AppPath, Constant.APP_SETTINGS_PATH);
            AppSettings settings = null;
            if ( File.Exists(path) ) {
                settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
                if ( settings == null ) {
                    settings = new AppSettings();
                    File.WriteAllText(path, JsonConvert.SerializeObject(settings));
                }
            }
            else {
                settings = new AppSettings();
                File.WriteAllText(path, JsonConvert.SerializeObject(settings));
            }
            return settings;
        }
    }
}

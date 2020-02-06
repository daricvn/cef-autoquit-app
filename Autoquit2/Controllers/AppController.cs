using Autoquit2._App_Data;
using Autoquit2.Models;
using Autoquit2.Services;
using CefCore;
using HttpService;
using HttpService.Core;
using HttpService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2.Controllers {
    [Route("config")]
    public class AppController : Controller
    {
        private static string LocalizationFolder {
            get {
                return Path.Combine(Constant.AppPath, Constant.LOCALIZATION_FOLDER);
            }
        }
        [Get]
        public IResponse GetConfig() {
            Program.Settings = Settings.GetConfig();
            return Response.WithSuccess(Program.Settings);
        }

        [Post]
        public IResponse SaveConfig(AppSettings model) {
            if (model != null ) {
                var path = Path.Combine(Constant.AppPath, Constant.APP_SETTINGS_PATH);
                try {
                    File.WriteAllText(path, JsonConvert.SerializeObject(model));
                }
                catch ( Exception ) { }
                Program.Settings = model;
                return Response.Success;
            }
            return Response.BadRequest;
        }

        [Post("bindhotkeys")]
        public IResponse RegisterHotkey() {
            if ( Program.Settings !=null ) {
                Master.Form.Invoke((MethodInvoker)(() => {
                    var master = Master.Form as Master;
                    master.UnregisterHotkeys();
                    master.RegisterHotkeys(Program.Settings);
                }));
                return Response.Success;
            }
            return Response.InternalError;
        }

        [Get("language")]
        public IResponse GetLanguage(string name) {
            var fileName = Path.ChangeExtension(name, "json");
            try {
                var path = Path.Combine(LocalizationFolder, fileName);
                if ( !File.Exists(path) ) {
                    path = Path.Combine(LocalizationFolder, "en-US.json");
                }
                var content = File.ReadAllText(path);
                Program.Established = true;
                Program.Language = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                return Response.WithSuccess(content);
            }
            catch(Exception ex ) {

            }
            return Response.InternalError;
        }

        [Post("open_link")]
        public IResponse OpenLink(string link ) {
            Process.Start(link);
            return Response.Success;
        }

        [Put("force_exit")]
        public void ForceExit() {
            Program.ForceClose = true;
            try {
                Recorder.Dispose();
            }
            catch (Exception) { }
            try {
                Master.Form.Invoke((MethodInvoker)(() => {
                    Master.Form.Close();
                }));
            }
            catch ( Exception ) { }
            Application.Exit();
        }
        [Post("event")]
        public IResponse GetEvent(string name ) {
            switch ( name ) {
                case "bar-mouse-down":
                    Master.Form.Invoke((MethodInvoker)(() => {
                        ((Master)Master.Form).StartMove();
                    }));
                    break;
                case "bar-mouse-up":
                    Master.Form.Invoke((MethodInvoker)(() => {
                        ((Master)Master.Form).StopMove();
                    }));
                    break;
            }
            return Response.Success;
        }
    }
}

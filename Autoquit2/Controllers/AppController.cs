using Autoquit2.Models;
using CefCore;
using HttpService;
using HttpService.Core;
using HttpService.Interfaces;
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
                return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "localization");
            }
        }
        [Get]
        public IResponse GetConfig() {
            return Response.WithSuccess(new AppSettings());
        }

        [Post]
        public IResponse SaveConfig(AppSettings model) {
            return Response.BadRequest;
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
        public IResponse ForceExit() {
            Program.ForceClose = true;
            Application.Exit();
            return Response.Success;
        }
    }
}

using CefCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2
{
    static class Program
    {
        private const string DEBUG_CONTENT = "../../../../VueInterface/app-ui/dist";
        //===============================================
        private static string AppScheme = "daricapp";
        private static string AppName = "autoquit";
        private static string AppPort = "7709";

        public static volatile bool ForceClose = false;
        public static volatile bool Established = false;
        private static string Url
        {
            get
            {
                return AppScheme + "://" + AppName;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if ( System.Diagnostics.Debugger.IsAttached ) {
                Chromium.UseLocalContent(DEBUG_CONTENT);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var assemblyPath = string.Join(".", typeof(Program).Namespace, "resources");
            var url = Url + "/index.html";
            Chromium.SetAssemblyResource(typeof(Program));
            Chromium.InitializeCore(AppScheme, AppName, url, AppPort, "Autoquit2", assemblyPath);
            Application.Run(new Master());
            Chromium.Shutdown();
        }
    }
}

using CefCore.Core;
using CefCore.Handler;
using CefSharp;
using CefSharp.WinForms;
using HttpService;
using LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CefCore
{
    public static class Chromium
    {
        /// <summary>
        /// Get or set experimental mode. While turning this mode on, controller will expose all web API requests for any browser.
        /// </summary> 
        public static bool ExperimentalMode { get; set; } = false;
        public static bool PureMode { get; set; } = false;

        private static ChromiumWebBrowser _browser=null;
        /// <summary>
        /// Initialize Chromium Core
        /// </summary>
        /// <param name="defaultUrl"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ChromiumWebBrowser InitializeCore(string scheme, string host, string defaultUrl, string port, string controllerDomain, string assemblyPath=null)
        {
            if (_browser == null)
            {
                if (!PureMode)
                {
                    SchemeRequestHandler.SchemeName = scheme;
                    SchemeRequestHandler.AssemblyPrefix = assemblyPath;
                    HttpService.HttpService.Host = "localhost";
                    HttpService.HttpService.Port = port;
                    HttpService.HttpService.Origin = scheme+"://"+host;
                    if (!ExperimentalMode)
                    {
                        HttpService.HttpService.SetOriginAllowance(HttpService.HttpService.Origin);
                        HttpService.HttpService.SetOriginAllowance(HttpService.HttpService.Origin + "/");
                    }
                    var settings = CreateSettings();
                    Cef.Initialize(settings);

                    var controllers = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x=>x.FullName.StartsWith(controllerDomain))
                        ?.GetTypes().Where(x => x.IsClass && x.IsSubclassOf(typeof(Controller)));
                    if ( controllers != null && controllers.Count() > 0 ) {
                        foreach ( var ctl in controllers )
                            if ( !ctl.IsAbstract ) {
                                try {
                                    Activator.CreateInstance(ctl);
                                }
                                catch ( Exception ex ) {
                                    Logger.Error("Error during bind controllers", ex);
                                }
                            }
                        HttpService.HttpService.Listen();
                    }
                }
                _browser = CreateBrowserNode(defaultUrl);
            }
            return _browser;
        }

        public static ChromiumWebBrowser MainBrowser
        {
            get { return _browser; }
        }

        public static void SetAssemblyResource<T>()
        {
            AssemblyReader.TargetAssembly = typeof(T).Assembly;
        }
        public static void SetAssemblyResource(Type t)
        {
            AssemblyReader.TargetAssembly = t.Assembly;
        }

        /// <summary>
        /// Use local file content instead of assembly files.
        /// </summary>
        /// <param name="contentPath"></param>
        public static void UseLocalContent(string contentPath ) {
            SchemeRequestHandler.LocalPath = contentPath;
        }

        public static ChromiumWebBrowser CreateBrowserNode(string defaultUrl = "chrome://version")
        {
            var browser = new ChromiumWebBrowser(defaultUrl);
            browser.BrowserSettings.ApplicationCache = CefState.Disabled;
            browser.BrowserSettings.WindowlessFrameRate = 20;
            // Disable menu
            browser.MenuHandler = new ContextMenuHandler();
            // Disable drag & drop
            browser.DragHandler = new DragHandler();
            return browser;
        }
        public static void Invoke(ChromiumWebBrowser browser, string scriptName, params object[] parameters)
        {
            browser.ExecuteScriptAsync(scriptName, parameters);
        }

        public static void Shutdown()
        {
            Cef.Shutdown();
            HttpService.HttpService.Close();
        }

        private static CefSettings CreateSettings()
        {
            var setting = new CefSettings();
            setting.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = SchemeRequestHandler.SchemeName,
                SchemeHandlerFactory = new SchemeRequestHandler()
            });
            CefSharpSettings.WcfEnabled = false;
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            setting.CefCommandLineArgs.Remove("process-per-tab");
            setting.CefCommandLineArgs.Add("enable-media-stream", "0");
            setting.CefCommandLineArgs.Add("disable-gpu", "1");
            setting.CefCommandLineArgs.Add("disable-plugins-discovery", "1");
            setting.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            setting.CefCommandLineArgs.Add("disable-extensions", "1");
            setting.CefCommandLineArgs.Add("disable-flash-3d", "1");
            setting.CefCommandLineArgs.Add("disable-speech-api", "1");

            return setting;
        }
        /// <summary>
        /// Set browser zoom scale by percent
        /// </summary>
        /// <param name="percent">Range from 0.25 to 5.0</param>
        public static void SetZoomScale(this ChromiumWebBrowser browser, double percent ) {
            if ( percent > 5 )
                percent = 5;
            if ( percent < 0.25 )
                percent = 0.25;
            var level = Math.Log(percent, 1.2);
            if ( Double.IsNaN(level) )
                level = 0;
            browser.SetZoomLevel(level);
        }
    }
}

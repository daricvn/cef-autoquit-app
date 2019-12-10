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
        public static ChromiumWebBrowser InitializeCore(string scheme, string host, string defaultUrl, string port, string assemblyPath=null)
        {
            if (_browser == null)
            {
                if (!PureMode)
                {
                    SchemeRequestHandler.SchemeName = scheme;
                    SchemeRequestHandler.AssemblyPrefix = assemblyPath;
                    HttpService.HttpService.Host = host;
                    HttpService.HttpService.Port = port;
                    HttpService.HttpService.Scheme = scheme;
                    HttpService.HttpService.Origin = HttpService.HttpService.URL;
                    if (!ExperimentalMode)
                    {
                        HttpService.HttpService.SetOriginAllowance(HttpService.HttpService.URL);
                        HttpService.HttpService.SetOriginAllowance(HttpService.HttpService.URL + "/");
                    }
                    var settings = CreateSettings();
                    Cef.Initialize(settings);

                    var controllers = Assembly.GetAssembly(typeof(Controller)).GetTypes().Where(x => x.IsClass && x.IsSubclassOf(typeof(Controller)));
                    if (controllers != null && controllers.Count() > 0)
                    {
                        foreach (var ctl in controllers)
                            if (!ctl.IsAbstract)
                            {
                                try
                                {
                                    Activator.CreateInstance(ctl);
                                }
                                catch (Exception ex)
                                {
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

        public static ChromiumWebBrowser CreateBrowserNode(string defaultUrl = "chrome://version")
        {
            var browser = new ChromiumWebBrowser(defaultUrl);
            //browser.MenuHandler = new ContextMenuHandler();
            //browser.DragHandler = new DragHandler();
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
            return setting;
        }

    }
}

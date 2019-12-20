using Autoquit2.Services;
using CefCore;
using CefSharp;
using CefSharp.WinForms;
using HttpService.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoquit2
{
    public partial class Master : Form
    {
        public static Form Form => _master;
        private static Form _master;
        public Master()
        {
            InitializeComponent();
            this.Controls.Add(Chromium.MainBrowser);
            Chromium.MainBrowser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            Chromium.MainBrowser.FrameLoadEnd += MainBrowser_FrameLoadEnd;
            _master = this;
        }

        private void MainBrowser_FrameLoadEnd( object sender, FrameLoadEndEventArgs e ) {
            Chromium.MainBrowser.SetZoomScale(0.8);
            var settings = Settings.GetConfig();
            Chromium.MainBrowser.ExecuteScriptAsync($"window.loadApp({Serializer.Json(settings)})");
            Chromium.MainBrowser.ShowDevTools();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {

        }

        private void Master_Load(object sender, EventArgs e)
        {

        }

        private void Master_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( !Program.ForceClose ) {
                e.Cancel = true;
                Chromium.MainBrowser.ExecuteScriptAsync("window.closeApp()");
            }
        }
    }
}

using Autoquit2.Models;
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
using WinAPI.Hook;

namespace Autoquit2
{
    public partial class Master : Form
    {
        public static Form Form => _master;
        private static Form _master;
        private Coord Offset;
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
            Chromium.MainBrowser.ExecuteScriptAsync($"window.loadApp({settings.ToJson()})");
            Chromium.MainBrowser.ShowDevTools();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {

        }

        private void Master_Load(object sender, EventArgs e)
        {
            Recorder.Initialize();
        }

        private void Master_FormClosing( object sender, FormClosingEventArgs e ) {
            if ( !Program.ForceClose ) {
                e.Cancel = true;
                Chromium.MainBrowser.ExecuteScriptAsync("window.closeApp()");
            }
        }

        public void StartMove() {
            var coord = MouseHook.GetCursorPosition();
            Offset = new Coord(coord.x - this.Location.X,  coord.y - this.Location.Y);
            this.moveTimer.Enabled = true;
        }
        public void StopMove() {
            Offset = null;
            this.moveTimer.Enabled = false;
        }

        private void moveTimer_Tick( object sender, EventArgs e ) {
            if ( this.Offset != null ) {
                var coord = MouseHook.GetCursorPosition();
                this.Location = new Point(coord.x - Offset.X, coord.y - Offset.Y);
                if (Form.ActiveForm != this ) { 
                    this.StopMove();
                }
            }
        }
    }
}

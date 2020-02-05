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
        private HashSet<int> registeredHotkeys = new HashSet<int>();
        public Master()
        {
            InitializeComponent();
            this.Controls.Add(Chromium.MainBrowser);
            Chromium.MainBrowser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            Chromium.MainBrowser.FrameLoadEnd += MainBrowser_FrameLoadEnd;
            _master = this;
            Program.Settings = Settings.GetConfig();
        }

        private void MainBrowser_FrameLoadEnd( object sender, FrameLoadEndEventArgs e ) {
            Chromium.MainBrowser.SetZoomScale(0.75);
            Chromium.MainBrowser.ExecuteScriptAsync($"window.loadApp({Program.Settings.ToJson()})");
            Chromium.MainBrowser.ShowDevTools();
            if ( this.InvokeRequired )
                this.Invoke((MethodInvoker)(() => {
                    this.RegisterHotkeys(Program.Settings);
                }));
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

        public void UnregisterHotkeys() {
            UnregisterHotkey(0);
            UnregisterHotkey(1);
            UnregisterHotkey(2);
        }

        public void RegisterHotkeys(AppSettings settings) {
            if ( settings.PlayHotkey != null && !string.IsNullOrWhiteSpace(settings.PlayHotkey.Key) ) {
                RegisterHotkey(0, settings.PlayHotkey);
            }
            if ( settings.RecordHotkey != null && !string.IsNullOrWhiteSpace(settings.RecordHotkey.Key) ) {
                RegisterHotkey(1, settings.RecordHotkey);
            }
            if ( settings.TopHotkey != null && !string.IsNullOrWhiteSpace(settings.TopHotkey.Key) ) {
                RegisterHotkey(2, settings.TopHotkey);
            }
        }

        public void RegisterHotkey(int id, KeyCombination keyComb ) {
            List<KeyModifier> keys = new List<KeyModifier>(4);
            if ( keyComb.AltKey )
                keys.Add(KeyModifier.Alt);
            if ( keyComb.CtrlKey )
                keys.Add(KeyModifier.Control);
            if ( keyComb.ShiftKey )
                keys.Add(KeyModifier.Shift);
            var key = ChromiumKeyConverter.GetKey(keyComb.Key);
            RegisterHotkey(id, key, keys.ToArray());
        }

        public void RegisterHotkey(int id, Keys key, params KeyModifier[] mods) {
            if ( registeredHotkeys.Contains(id) ) return;
            int? mod = null;
            if ( mods != null && mods.Length > 0 ) {
                foreach ( var m in mods )
                    if ( m != KeyModifier.None ) {
                        if ( mod == null )
                            mod = (int)m;
                        else
                            mod |= (int)m;
                    }
            }
            else mod = 0;
            registeredHotkeys.Add(id);
            KeyboardHook.RegisterHotKey(this.Handle, id, mod.Value, key.GetHashCode());
        }
        public void UnregisterHotkey(int id ) {
            if ( registeredHotkeys.Contains(id) ) {
                KeyboardHook.UnregisterHotKey(this.Handle, id);
                registeredHotkeys.Remove(id);
            }
        }


        protected override void WndProc( ref Message m ) {
            base.WndProc(ref m);

            if ( m.Msg == 0x0312 ) {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                var modifier = ((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                bool ctrl = false;
                bool alt = false;
                bool shift = false;
                if (modifier == 3) {
                    alt = ctrl = true;
                }
                else if (modifier == 5 ) {
                    alt = shift = true;
                }
                else if (modifier == 6 ) {
                    ctrl = shift = true;
                }
                else if (modifier == 7 ) {
                    alt = shift = ctrl = true;
                }
                else {
                    ctrl = (KeyModifier)modifier == KeyModifier.Control;
                    alt = (KeyModifier)modifier == KeyModifier.Alt;
                    shift = (KeyModifier)modifier == KeyModifier.Shift;
                }
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.
                //if ( SharedProperty.IsHotkey(id, key, modifier) && !isSettingOpen && !IsErrorDisplayed ) {
                //    var hotkey = SharedProperty.FindHotkey(id);
                //    if ( hotkey.event_name == "PLAY" )
                //        BtnPlay_Click(null, null);
                //    if ( hotkey.event_name == "RECORD" )
                //        ToggleRecord(true);
                //}
                if (id >=0 && !string.IsNullOrWhiteSpace(Program.Settings.PlayHotkey.Key) && Program.Settings.PlayHotkey.IsInput(alt, ctrl, shift, key) ) {
                    Chromium.MainBrowser.ExecuteScriptAsync($"window.play()");
                }
                if ( id >= 0 && !string.IsNullOrWhiteSpace(Program.Settings.RecordHotkey.Key) && Program.Settings.RecordHotkey.IsInput(alt, ctrl, shift, key) ) {
                    Chromium.MainBrowser.ExecuteScriptAsync($"window.record()");
                }
                if ( id >= 0 && !string.IsNullOrWhiteSpace(Program.Settings.TopHotkey.Key) && Program.Settings.TopHotkey.IsInput(alt, ctrl, shift, key) ) {
                    Chromium.MainBrowser.ExecuteScriptAsync($"window.bringTop()");
                }
                // do something
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPI;
using WinAPI.Handler;
using WinAPI.Hook;

namespace Autoquit2.Views {
    public partial class MouseCoord : Form {
        private MouseHook.POINT currentPoint;
        private IntPtr targetWindows = IntPtr.Zero;
        private bool Retrieving = false;

        public int LastX {
            get {
                return currentPoint.x;
            }
        }
        public int LastY {
            get {
                return currentPoint.y;
            }
        }
        public MouseCoord() {
            InitializeComponent();
            this.BackColor = Color.DarkMagenta;
            this.TransparencyKey = Color.DarkMagenta;
        }

        private void MouseCoord_Load( object sender, EventArgs e ) {
            int initialStyle = WinProcess.GetWindowLong(this.Handle, -20);
            WinProcess.SetWindowLong(this.Handle, -20, (uint)(initialStyle | 0x80000 | 0x20 | 0x00000080));
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Location = new Point(0, 0);
            xLine.Size = new Size(Screen.PrimaryScreen.Bounds.Width, 1);
            yLine.Size = new Size(1, Screen.PrimaryScreen.Bounds.Height);

            MouseHook.MouseAction += MouseHook_MouseAction;
        }

        private void MouseHook_MouseAction( MouseHookEventArgs arg ) {
            if ( this.Retrieving ) {
                if (arg.Key == MouseKey.LEFT_CLICK || arg.Key == MouseKey.RIGHT_CLICK )
                    this.Close();
            }
        }

        public void Show( IntPtr target ) {
            currentPoint = MouseHook.GetCursorPosition();
            base.ShowDialog();
            timeTicker.Enabled = true;
            targetWindows = target;
            this.Retrieving = true;
        }
        public new void Close() {
            timeTicker.Enabled = false;
            this.Retrieving = false;
            base.Close();
        }

        private void timeTicker_Tick( object sender, EventArgs e ) {
            currentPoint = MouseHook.GetCursorPosition();
            xLine.Location = new Point(0, currentPoint.y);
            yLine.Location = new Point(currentPoint.x, 0);
            try {
                if ( targetWindows != IntPtr.Zero &&
                    MouseHook.ScreenToClient(targetWindows, ref currentPoint) ) {
                }
            }
            catch ( Exception ) {
                this.Close();
            }
            lbMousecoord.Text = string.Format("{0}:{1}", currentPoint.x, currentPoint.y);
        }
    }
}

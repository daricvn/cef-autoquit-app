using CefCore;
using CefSharp;
using CefSharp.WinForms;
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
        public Master()
        {
            InitializeComponent();
            this.Controls.Add(Chromium.MainBrowser);
            Chromium.MainBrowser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
        }

        private void Master_Load(object sender, EventArgs e)
        {
        }
    }
}

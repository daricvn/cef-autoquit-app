namespace Autoquit2
{
    partial class Master
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Master));
            this.moveTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // moveTimer
            // 
            this.moveTimer.Interval = 10;
            this.moveTimer.Tick += new System.EventHandler(this.moveTimer_Tick);
            // 
            // Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 555);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Master";
            this.Text = "Autoquit2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Master_FormClosing);
            this.Load += new System.EventHandler(this.Master_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer moveTimer;
    }
}


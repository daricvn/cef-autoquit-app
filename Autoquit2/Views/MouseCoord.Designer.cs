namespace Autoquit2.Views {
    partial class MouseCoord {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.yLine = new System.Windows.Forms.PictureBox();
            this.xLine = new System.Windows.Forms.PictureBox();
            this.timeTicker = new System.Windows.Forms.Timer(this.components);
            this.lbMousecoord = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.yLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xLine)).BeginInit();
            this.SuspendLayout();
            // 
            // yLine
            // 
            this.yLine.BackColor = System.Drawing.Color.Gold;
            this.yLine.Location = new System.Drawing.Point(179, 35);
            this.yLine.Name = "yLine";
            this.yLine.Size = new System.Drawing.Size(2, 100);
            this.yLine.TabIndex = 3;
            this.yLine.TabStop = false;
            this.yLine.Click += new System.EventHandler(this.yLine_Click);
            // 
            // xLine
            // 
            this.xLine.BackColor = System.Drawing.Color.Gold;
            this.xLine.Location = new System.Drawing.Point(126, 84);
            this.xLine.Name = "xLine";
            this.xLine.Size = new System.Drawing.Size(100, 2);
            this.xLine.TabIndex = 2;
            this.xLine.TabStop = false;
            this.xLine.Click += new System.EventHandler(this.xLine_Click);
            // 
            // timeTicker
            // 
            this.timeTicker.Interval = 30;
            this.timeTicker.Tick += new System.EventHandler(this.timeTicker_Tick);
            // 
            // lbMousecoord
            // 
            this.lbMousecoord.AutoSize = true;
            this.lbMousecoord.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMousecoord.ForeColor = System.Drawing.Color.Gold;
            this.lbMousecoord.Location = new System.Drawing.Point(3, 3);
            this.lbMousecoord.Name = "lbMousecoord";
            this.lbMousecoord.Size = new System.Drawing.Size(100, 18);
            this.lbMousecoord.TabIndex = 4;
            this.lbMousecoord.Text = "mouse coord";
            // 
            // MouseCoord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 170);
            this.Controls.Add(this.lbMousecoord);
            this.Controls.Add(this.yLine);
            this.Controls.Add(this.xLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MouseCoord";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MouseCoord";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MouseCoord_Load);
            this.Click += new System.EventHandler(this.MouseCoord_Click);
            ((System.ComponentModel.ISupportInitialize)(this.yLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox yLine;
        private System.Windows.Forms.PictureBox xLine;
        private System.Windows.Forms.Timer timeTicker;
        private System.Windows.Forms.Label lbMousecoord;
    }
}
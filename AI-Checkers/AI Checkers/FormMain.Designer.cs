namespace AICheckers
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.boardPanel2 = new AICheckers.BoardPanel();
            this.SuspendLayout();

            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 33;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);

            this.boardPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boardPanel2.Location = new System.Drawing.Point(12, 12);
            this.boardPanel2.Name = "boardPanel2";
            this.boardPanel2.Size = new System.Drawing.Size(504, 504);
            this.boardPanel2.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 524);
            this.Controls.Add(this.boardPanel2);
            this.Name = "FormMain";
            this.Text = "Dammen";
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Timer timerRefresh;
        private BoardPanel boardPanel2;
    }
}


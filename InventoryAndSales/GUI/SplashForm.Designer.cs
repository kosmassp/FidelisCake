namespace InventoryAndSales.GUI
{
    partial class SplashForm
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
      this.progressBarLoading = new System.Windows.Forms.ProgressBar();
      this.panelStatus = new System.Windows.Forms.Panel();
      this.labelStatus = new System.Windows.Forms.Label();
      this.panelStatus.SuspendLayout();
      this.SuspendLayout();
      // 
      // progressBarLoading
      // 
      this.progressBarLoading.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBarLoading.ForeColor = System.Drawing.Color.SandyBrown;
      this.progressBarLoading.Location = new System.Drawing.Point(0, 281);
      this.progressBarLoading.Name = "progressBarLoading";
      this.progressBarLoading.Size = new System.Drawing.Size(400, 17);
      this.progressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.progressBarLoading.TabIndex = 0;
      this.progressBarLoading.Value = 20;
      // 
      // panelStatus
      // 
      this.panelStatus.Controls.Add(this.labelStatus);
      this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelStatus.Location = new System.Drawing.Point(0, 269);
      this.panelStatus.Margin = new System.Windows.Forms.Padding(0);
      this.panelStatus.Name = "panelStatus";
      this.panelStatus.Size = new System.Drawing.Size(400, 12);
      this.panelStatus.TabIndex = 1;
      // 
      // labelStatus
      // 
      this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelStatus.Location = new System.Drawing.Point(0, 0);
      this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(400, 12);
      this.labelStatus.TabIndex = 0;
      this.labelStatus.Text = "label1";
      this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // SplashForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::InventoryAndSales.Properties.Resources.splash;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(400, 298);
      this.ControlBox = false;
      this.Controls.Add(this.panelStatus);
      this.Controls.Add(this.progressBarLoading);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SplashForm";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SplashForm";
      this.Load += new System.EventHandler(this.SplashForm_Load);
      this.panelStatus.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoading;
    private System.Windows.Forms.Panel panelStatus;
    private System.Windows.Forms.Label labelStatus;
  }
}
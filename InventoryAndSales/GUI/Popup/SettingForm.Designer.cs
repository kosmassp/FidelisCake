namespace InventoryAndSales.GUI.Popup
{
    partial class SettingForm
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
      this.listBoxSettingSelection = new System.Windows.Forms.ListBox();
      this.panelSettingPage = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // listBoxSettingSelection
      // 
      this.listBoxSettingSelection.Dock = System.Windows.Forms.DockStyle.Left;
      this.listBoxSettingSelection.FormattingEnabled = true;
      this.listBoxSettingSelection.Location = new System.Drawing.Point(0, 0);
      this.listBoxSettingSelection.Name = "listBoxSettingSelection";
      this.listBoxSettingSelection.Size = new System.Drawing.Size(141, 450);
      this.listBoxSettingSelection.TabIndex = 0;
      this.listBoxSettingSelection.SelectedIndexChanged += new System.EventHandler(this.listBoxSettingSelection_SelectedIndexChanged);
      // 
      // panelSettingPage
      // 
      this.panelSettingPage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelSettingPage.Location = new System.Drawing.Point(141, 0);
      this.panelSettingPage.Name = "panelSettingPage";
      this.panelSettingPage.Size = new System.Drawing.Size(659, 450);
      this.panelSettingPage.TabIndex = 1;
      // 
      // SettingForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.panelSettingPage);
      this.Controls.Add(this.listBoxSettingSelection);
      this.Name = "SettingForm";
      this.Text = "SettingForm";
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSettingSelection;
    private System.Windows.Forms.Panel panelSettingPage;
  }
}
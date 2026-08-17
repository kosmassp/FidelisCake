namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class SecuritySettingForm
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

    #region Component Designer generated code

    private void InitializeComponent()
    {
      this.labelTitle = new System.Windows.Forms.Label();
      this.checkBoxAllowBuiltInAdmin = new System.Windows.Forms.CheckBox();
      this.labelDescription = new System.Windows.Forms.Label();
      this.labelWarning = new System.Windows.Forms.Label();
      this.buttonSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // labelTitle
      //
      this.labelTitle.AutoSize = true;
      this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
      this.labelTitle.Location = new System.Drawing.Point(6, 14);
      this.labelTitle.Name = "labelTitle";
      this.labelTitle.Size = new System.Drawing.Size(120, 13);
      this.labelTitle.TabIndex = 0;
      this.labelTitle.Text = "Akun Pemulihan";
      //
      // checkBoxAllowBuiltInAdmin
      //
      this.checkBoxAllowBuiltInAdmin.AutoSize = true;
      this.checkBoxAllowBuiltInAdmin.Location = new System.Drawing.Point(9, 40);
      this.checkBoxAllowBuiltInAdmin.Name = "checkBoxAllowBuiltInAdmin";
      this.checkBoxAllowBuiltInAdmin.Size = new System.Drawing.Size(250, 17);
      this.checkBoxAllowBuiltInAdmin.TabIndex = 1;
      this.checkBoxAllowBuiltInAdmin.Text = "Izinkan login dengan akun pemulihan";
      this.checkBoxAllowBuiltInAdmin.UseVisualStyleBackColor = true;
      this.checkBoxAllowBuiltInAdmin.CheckedChanged += new System.EventHandler(this.checkBoxAllowBuiltInAdmin_CheckedChanged);
      //
      // labelDescription
      //
      this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDescription.Location = new System.Drawing.Point(6, 70);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new System.Drawing.Size(519, 95);
      this.labelDescription.TabIndex = 2;
      this.labelDescription.Text = "-";
      //
      // labelWarning
      //
      this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelWarning.ForeColor = System.Drawing.Color.Firebrick;
      this.labelWarning.Location = new System.Drawing.Point(6, 175);
      this.labelWarning.Name = "labelWarning";
      this.labelWarning.Size = new System.Drawing.Size(519, 60);
      this.labelWarning.TabIndex = 3;
      this.labelWarning.Text = "";
      //
      // buttonSave
      //
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(450, 340);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(75, 23);
      this.buttonSave.TabIndex = 4;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      //
      // SecuritySettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelWarning);
      this.Controls.Add(this.labelDescription);
      this.Controls.Add(this.checkBoxAllowBuiltInAdmin);
      this.Controls.Add(this.labelTitle);
      this.Name = "SecuritySettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Keamanan";
      this.Load += new System.EventHandler(this.SecuritySettingForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.CheckBox checkBoxAllowBuiltInAdmin;
    private System.Windows.Forms.Label labelDescription;
    private System.Windows.Forms.Label labelWarning;
    private System.Windows.Forms.Button buttonSave;
  }
}

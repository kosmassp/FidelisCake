namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class ReportSettingForm
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
      this.labelFolder = new System.Windows.Forms.Label();
      this.textBoxFolder = new System.Windows.Forms.TextBox();
      this.buttonBrowse = new System.Windows.Forms.Button();
      this.buttonOpenFolder = new System.Windows.Forms.Button();
      this.buttonDefault = new System.Windows.Forms.Button();
      this.labelDescription = new System.Windows.Forms.Label();
      this.labelAssetStatus = new System.Windows.Forms.Label();
      this.buttonSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // labelFolder
      //
      this.labelFolder.AutoSize = true;
      this.labelFolder.Location = new System.Drawing.Point(6, 14);
      this.labelFolder.Name = "labelFolder";
      this.labelFolder.Size = new System.Drawing.Size(84, 13);
      this.labelFolder.TabIndex = 0;
      this.labelFolder.Text = "Folder Laporan:";
      //
      // textBoxFolder
      //
      this.textBoxFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxFolder.Location = new System.Drawing.Point(9, 32);
      this.textBoxFolder.Name = "textBoxFolder";
      this.textBoxFolder.Size = new System.Drawing.Size(430, 20);
      this.textBoxFolder.TabIndex = 1;
      this.textBoxFolder.TextChanged += new System.EventHandler(this.textBoxFolder_TextChanged);
      //
      // buttonBrowse
      //
      this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonBrowse.Location = new System.Drawing.Point(445, 30);
      this.buttonBrowse.Name = "buttonBrowse";
      this.buttonBrowse.Size = new System.Drawing.Size(80, 23);
      this.buttonBrowse.TabIndex = 2;
      this.buttonBrowse.Text = "Pilih Folder";
      this.buttonBrowse.UseVisualStyleBackColor = true;
      this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
      //
      // buttonOpenFolder
      //
      this.buttonOpenFolder.Location = new System.Drawing.Point(9, 58);
      this.buttonOpenFolder.Name = "buttonOpenFolder";
      this.buttonOpenFolder.Size = new System.Drawing.Size(110, 23);
      this.buttonOpenFolder.TabIndex = 3;
      this.buttonOpenFolder.Text = "Buka Folder";
      this.buttonOpenFolder.UseVisualStyleBackColor = true;
      this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
      //
      // buttonDefault
      //
      this.buttonDefault.Location = new System.Drawing.Point(125, 58);
      this.buttonDefault.Name = "buttonDefault";
      this.buttonDefault.Size = new System.Drawing.Size(130, 23);
      this.buttonDefault.TabIndex = 4;
      this.buttonDefault.Text = "Kembali ke Bawaan";
      this.buttonDefault.UseVisualStyleBackColor = true;
      this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
      //
      // labelDescription
      //
      this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDescription.Location = new System.Drawing.Point(6, 95);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new System.Drawing.Size(519, 60);
      this.labelDescription.TabIndex = 5;
      this.labelDescription.Text = "Laporan HTML akan disimpan di folder ini. File pendukung (untuk fitur urut, cari d" +
    "an export Excel/PDF) disiapkan otomatis di dalam sub-folder \"assets\".";
      //
      // labelAssetStatus
      //
      this.labelAssetStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelAssetStatus.Location = new System.Drawing.Point(6, 160);
      this.labelAssetStatus.Name = "labelAssetStatus";
      this.labelAssetStatus.Size = new System.Drawing.Size(519, 45);
      this.labelAssetStatus.TabIndex = 6;
      this.labelAssetStatus.Text = "-";
      //
      // buttonSave
      //
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(450, 340);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(75, 23);
      this.buttonSave.TabIndex = 7;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      //
      // ReportSettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelAssetStatus);
      this.Controls.Add(this.labelDescription);
      this.Controls.Add(this.buttonDefault);
      this.Controls.Add(this.buttonOpenFolder);
      this.Controls.Add(this.buttonBrowse);
      this.Controls.Add(this.textBoxFolder);
      this.Controls.Add(this.labelFolder);
      this.Name = "ReportSettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Laporan";
      this.Load += new System.EventHandler(this.ReportSettingForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelFolder;
    private System.Windows.Forms.TextBox textBoxFolder;
    private System.Windows.Forms.Button buttonBrowse;
    private System.Windows.Forms.Button buttonOpenFolder;
    private System.Windows.Forms.Button buttonDefault;
    private System.Windows.Forms.Label labelDescription;
    private System.Windows.Forms.Label labelAssetStatus;
    private System.Windows.Forms.Button buttonSave;
  }
}

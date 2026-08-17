namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class PaymentOptionSettingForm
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
      this.labelEdc = new System.Windows.Forms.Label();
      this.listBoxEdc = new System.Windows.Forms.ListBox();
      this.textBoxNewEdc = new System.Windows.Forms.TextBox();
      this.buttonAddEdc = new System.Windows.Forms.Button();
      this.buttonRemoveEdc = new System.Windows.Forms.Button();
      this.labelQris = new System.Windows.Forms.Label();
      this.listBoxQris = new System.Windows.Forms.ListBox();
      this.textBoxNewQris = new System.Windows.Forms.TextBox();
      this.comboBoxQrisMode = new System.Windows.Forms.ComboBox();
      this.buttonAddQris = new System.Windows.Forms.Button();
      this.buttonRemoveQris = new System.Windows.Forms.Button();
      this.labelDescription = new System.Windows.Forms.Label();
      this.buttonSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // labelEdc
      //
      this.labelEdc.AutoSize = true;
      this.labelEdc.Location = new System.Drawing.Point(9, 12);
      this.labelEdc.Name = "labelEdc";
      this.labelEdc.Size = new System.Drawing.Size(85, 13);
      this.labelEdc.TabIndex = 0;
      this.labelEdc.Text = "Terminal EDC:";
      //
      // listBoxEdc
      //
      this.listBoxEdc.FormattingEnabled = true;
      this.listBoxEdc.Location = new System.Drawing.Point(9, 30);
      this.listBoxEdc.Name = "listBoxEdc";
      this.listBoxEdc.Size = new System.Drawing.Size(175, 160);
      this.listBoxEdc.TabIndex = 1;
      //
      // textBoxNewEdc
      //
      this.textBoxNewEdc.Location = new System.Drawing.Point(9, 197);
      this.textBoxNewEdc.Name = "textBoxNewEdc";
      this.textBoxNewEdc.Size = new System.Drawing.Size(175, 20);
      this.textBoxNewEdc.TabIndex = 2;
      this.textBoxNewEdc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNewEdc_KeyDown);
      //
      // buttonAddEdc
      //
      this.buttonAddEdc.Location = new System.Drawing.Point(9, 223);
      this.buttonAddEdc.Name = "buttonAddEdc";
      this.buttonAddEdc.Size = new System.Drawing.Size(85, 23);
      this.buttonAddEdc.TabIndex = 3;
      this.buttonAddEdc.Text = "Tambah";
      this.buttonAddEdc.UseVisualStyleBackColor = true;
      this.buttonAddEdc.Click += new System.EventHandler(this.buttonAddEdc_Click);
      //
      // buttonRemoveEdc
      //
      this.buttonRemoveEdc.Location = new System.Drawing.Point(99, 223);
      this.buttonRemoveEdc.Name = "buttonRemoveEdc";
      this.buttonRemoveEdc.Size = new System.Drawing.Size(85, 23);
      this.buttonRemoveEdc.TabIndex = 4;
      this.buttonRemoveEdc.Text = "Hapus";
      this.buttonRemoveEdc.UseVisualStyleBackColor = true;
      this.buttonRemoveEdc.Click += new System.EventHandler(this.buttonRemoveEdc_Click);
      //
      // labelQris
      //
      this.labelQris.AutoSize = true;
      this.labelQris.Location = new System.Drawing.Point(280, 12);
      this.labelQris.Name = "labelQris";
      this.labelQris.Size = new System.Drawing.Size(85, 13);
      this.labelQris.TabIndex = 5;
      this.labelQris.Text = "Provider QRIS:";
      //
      // listBoxQris
      //
      this.listBoxQris.FormattingEnabled = true;
      this.listBoxQris.Location = new System.Drawing.Point(280, 30);
      this.listBoxQris.Name = "listBoxQris";
      this.listBoxQris.Size = new System.Drawing.Size(175, 160);
      this.listBoxQris.TabIndex = 6;
      this.listBoxQris.SelectedIndexChanged += new System.EventHandler(this.listBoxQris_SelectedIndexChanged);
      //
      // textBoxNewQris
      //
      this.textBoxNewQris.Location = new System.Drawing.Point(280, 197);
      this.textBoxNewQris.Name = "textBoxNewQris";
      this.textBoxNewQris.Size = new System.Drawing.Size(175, 20);
      this.textBoxNewQris.TabIndex = 7;
      this.textBoxNewQris.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNewQris_KeyDown);
      //
      // comboBoxQrisMode
      //
      this.comboBoxQrisMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxQrisMode.Location = new System.Drawing.Point(280, 223);
      this.comboBoxQrisMode.Name = "comboBoxQrisMode";
      this.comboBoxQrisMode.Size = new System.Drawing.Size(90, 21);
      this.comboBoxQrisMode.TabIndex = 8;
      this.comboBoxQrisMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxQrisMode_SelectedIndexChanged);
      //
      // buttonAddQris
      //
      this.buttonAddQris.Location = new System.Drawing.Point(376, 223);
      this.buttonAddQris.Name = "buttonAddQris";
      this.buttonAddQris.Size = new System.Drawing.Size(79, 23);
      this.buttonAddQris.TabIndex = 8;
      this.buttonAddQris.Text = "Tambah";
      this.buttonAddQris.UseVisualStyleBackColor = true;
      this.buttonAddQris.Click += new System.EventHandler(this.buttonAddQris_Click);
      //
      // buttonRemoveQris
      //
      this.buttonRemoveQris.Location = new System.Drawing.Point(280, 250);
      this.buttonRemoveQris.Name = "buttonRemoveQris";
      this.buttonRemoveQris.Size = new System.Drawing.Size(85, 23);
      this.buttonRemoveQris.TabIndex = 9;
      this.buttonRemoveQris.Text = "Hapus";
      this.buttonRemoveQris.UseVisualStyleBackColor = true;
      this.buttonRemoveQris.Click += new System.EventHandler(this.buttonRemoveQris_Click);
      //
      // labelDescription
      //
      this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDescription.Location = new System.Drawing.Point(9, 285);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new System.Drawing.Size(516, 45);
      this.labelDescription.TabIndex = 10;
      this.labelDescription.Text = "Yang terdaftar di sini dapat dipilih kasir saat pembayaran. Bila daftar kosong, me" +
    "tode tersebut tidak ditawarkan di layar kasir. Tipe kode QRIS mengikuti providernya.";
      //
      // buttonSave
      //
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(450, 340);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(75, 23);
      this.buttonSave.TabIndex = 11;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      //
      // PaymentOptionSettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelDescription);
      this.Controls.Add(this.buttonRemoveQris);
      this.Controls.Add(this.comboBoxQrisMode);
      this.Controls.Add(this.buttonAddQris);
      this.Controls.Add(this.textBoxNewQris);
      this.Controls.Add(this.listBoxQris);
      this.Controls.Add(this.labelQris);
      this.Controls.Add(this.buttonRemoveEdc);
      this.Controls.Add(this.buttonAddEdc);
      this.Controls.Add(this.textBoxNewEdc);
      this.Controls.Add(this.listBoxEdc);
      this.Controls.Add(this.labelEdc);
      this.Name = "PaymentOptionSettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Pembayaran";
      this.Load += new System.EventHandler(this.PaymentOptionSettingForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelEdc;
    private System.Windows.Forms.ListBox listBoxEdc;
    private System.Windows.Forms.TextBox textBoxNewEdc;
    private System.Windows.Forms.Button buttonAddEdc;
    private System.Windows.Forms.Button buttonRemoveEdc;
    private System.Windows.Forms.Label labelQris;
    private System.Windows.Forms.ListBox listBoxQris;
    private System.Windows.Forms.TextBox textBoxNewQris;
    private System.Windows.Forms.ComboBox comboBoxQrisMode;
    private System.Windows.Forms.Button buttonAddQris;
    private System.Windows.Forms.Button buttonRemoveQris;
    private System.Windows.Forms.Label labelDescription;
    private System.Windows.Forms.Button buttonSave;
  }
}

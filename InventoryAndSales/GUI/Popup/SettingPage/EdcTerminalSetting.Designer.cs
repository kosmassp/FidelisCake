namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class EdcTerminalSettingForm
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
      this.labelCaption = new System.Windows.Forms.Label();
      this.listBoxTerminals = new System.Windows.Forms.ListBox();
      this.textBoxNewTerminal = new System.Windows.Forms.TextBox();
      this.buttonAdd = new System.Windows.Forms.Button();
      this.buttonRemove = new System.Windows.Forms.Button();
      this.labelDescription = new System.Windows.Forms.Label();
      this.buttonSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      //
      // labelCaption
      //
      this.labelCaption.AutoSize = true;
      this.labelCaption.Location = new System.Drawing.Point(6, 14);
      this.labelCaption.Name = "labelCaption";
      this.labelCaption.Size = new System.Drawing.Size(85, 13);
      this.labelCaption.TabIndex = 0;
      this.labelCaption.Text = "Daftar Terminal:";
      //
      // listBoxTerminals
      //
      this.listBoxTerminals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxTerminals.FormattingEnabled = true;
      this.listBoxTerminals.Location = new System.Drawing.Point(9, 32);
      this.listBoxTerminals.Name = "listBoxTerminals";
      this.listBoxTerminals.Size = new System.Drawing.Size(430, 173);
      this.listBoxTerminals.TabIndex = 1;
      this.listBoxTerminals.SelectedIndexChanged += new System.EventHandler(this.listBoxTerminals_SelectedIndexChanged);
      //
      // buttonRemove
      //
      this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRemove.Location = new System.Drawing.Point(445, 32);
      this.buttonRemove.Name = "buttonRemove";
      this.buttonRemove.Size = new System.Drawing.Size(80, 23);
      this.buttonRemove.TabIndex = 2;
      this.buttonRemove.Text = "Hapus";
      this.buttonRemove.UseVisualStyleBackColor = true;
      this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
      //
      // textBoxNewTerminal
      //
      this.textBoxNewTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxNewTerminal.Location = new System.Drawing.Point(9, 213);
      this.textBoxNewTerminal.Name = "textBoxNewTerminal";
      this.textBoxNewTerminal.Size = new System.Drawing.Size(430, 20);
      this.textBoxNewTerminal.TabIndex = 3;
      this.textBoxNewTerminal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNewTerminal_KeyDown);
      //
      // buttonAdd
      //
      this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAdd.Location = new System.Drawing.Point(445, 211);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new System.Drawing.Size(80, 23);
      this.buttonAdd.TabIndex = 4;
      this.buttonAdd.Text = "Tambah";
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
      //
      // labelDescription
      //
      this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDescription.Location = new System.Drawing.Point(6, 245);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new System.Drawing.Size(519, 75);
      this.labelDescription.TabIndex = 5;
      this.labelDescription.Text = "Terminal yang terdaftar di sini dapat dipilih kasir saat pembayaran EDC.";
      //
      // buttonSave
      //
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(450, 340);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(75, 23);
      this.buttonSave.TabIndex = 6;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      //
      // EdcTerminalSettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelDescription);
      this.Controls.Add(this.buttonAdd);
      this.Controls.Add(this.textBoxNewTerminal);
      this.Controls.Add(this.buttonRemove);
      this.Controls.Add(this.listBoxTerminals);
      this.Controls.Add(this.labelCaption);
      this.Name = "EdcTerminalSettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Terminal EDC";
      this.Load += new System.EventHandler(this.EdcTerminalSettingForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelCaption;
    private System.Windows.Forms.ListBox listBoxTerminals;
    private System.Windows.Forms.TextBox textBoxNewTerminal;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.Button buttonRemove;
    private System.Windows.Forms.Label labelDescription;
    private System.Windows.Forms.Button buttonSave;
  }
}

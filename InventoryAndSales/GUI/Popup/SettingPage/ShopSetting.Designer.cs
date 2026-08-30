namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class ShopSettingForm
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
      this.labelName = new System.Windows.Forms.Label();
      this.textBoxShopName = new System.Windows.Forms.TextBox();
      this.labelDescription = new System.Windows.Forms.Label();
      this.labelInherited = new System.Windows.Forms.Label();
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
      this.labelTitle.Text = "Identitas Toko";
      //
      // labelName
      //
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(6, 46);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(64, 13);
      this.labelName.TabIndex = 1;
      this.labelName.Text = "Nama Toko:";
      //
      // textBoxShopName
      //
      this.textBoxShopName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxShopName.Location = new System.Drawing.Point(88, 43);
      this.textBoxShopName.MaxLength = 60;
      this.textBoxShopName.Name = "textBoxShopName";
      this.textBoxShopName.Size = new System.Drawing.Size(437, 20);
      this.textBoxShopName.TabIndex = 2;
      this.textBoxShopName.TextChanged += new System.EventHandler(this.textBoxShopName_TextChanged);
      //
      // labelInherited
      //
      this.labelInherited.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelInherited.ForeColor = System.Drawing.Color.DimGray;
      this.labelInherited.Location = new System.Drawing.Point(6, 70);
      this.labelInherited.Name = "labelInherited";
      this.labelInherited.Size = new System.Drawing.Size(519, 32);
      this.labelInherited.TabIndex = 3;
      this.labelInherited.Text = "";
      //
      // labelDescription
      //
      this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDescription.Location = new System.Drawing.Point(6, 112);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new System.Drawing.Size(519, 120);
      this.labelDescription.TabIndex = 4;
      this.labelDescription.Text = "-";
      //
      // buttonSave
      //
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(450, 340);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(75, 23);
      this.buttonSave.TabIndex = 5;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      //
      // ShopSettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelDescription);
      this.Controls.Add(this.labelInherited);
      this.Controls.Add(this.textBoxShopName);
      this.Controls.Add(this.labelName);
      this.Controls.Add(this.labelTitle);
      this.Name = "ShopSettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Toko";
      this.Load += new System.EventHandler(this.ShopSettingForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxShopName;
    private System.Windows.Forms.Label labelInherited;
    private System.Windows.Forms.Label labelDescription;
    private System.Windows.Forms.Button buttonSave;
  }
}

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class HeaderAndFooterForm
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.textBoxHeader = new System.Windows.Forms.TextBox();
      this.textBoxFooter = new System.Windows.Forms.TextBox();
      this.textBoxExample = new System.Windows.Forms.TextBox();
      this.labelHeader = new System.Windows.Forms.Label();
      this.labelFooter = new System.Windows.Forms.Label();
      this.labelPreview = new System.Windows.Forms.Label();
      this.buttonSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // textBoxHeader
      // 
      this.textBoxHeader.Location = new System.Drawing.Point(262, 26);
      this.textBoxHeader.Multiline = true;
      this.textBoxHeader.Name = "textBoxHeader";
      this.textBoxHeader.Size = new System.Drawing.Size(229, 79);
      this.textBoxHeader.TabIndex = 0;
      this.textBoxHeader.TextChanged += new System.EventHandler(this.textBoxHeader_TextChanged);
      // 
      // textBoxFooter
      // 
      this.textBoxFooter.Location = new System.Drawing.Point(262, 149);
      this.textBoxFooter.Multiline = true;
      this.textBoxFooter.Name = "textBoxFooter";
      this.textBoxFooter.Size = new System.Drawing.Size(229, 84);
      this.textBoxFooter.TabIndex = 1;
      this.textBoxFooter.TextChanged += new System.EventHandler(this.textBoxFooter_TextChanged);
      // 
      // textBoxExample
      // 
      this.textBoxExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.textBoxExample.Location = new System.Drawing.Point(6, 26);
      this.textBoxExample.Multiline = true;
      this.textBoxExample.Name = "textBoxExample";
      this.textBoxExample.ReadOnly = true;
      this.textBoxExample.Size = new System.Drawing.Size(247, 337);
      this.textBoxExample.TabIndex = 2;
      // 
      // labelHeader
      // 
      this.labelHeader.AutoSize = true;
      this.labelHeader.Location = new System.Drawing.Point(259, 10);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(52, 13);
      this.labelHeader.TabIndex = 3;
      this.labelHeader.Text = "Pembuka";
      // 
      // labelFooter
      // 
      this.labelFooter.AutoSize = true;
      this.labelFooter.Location = new System.Drawing.Point(259, 133);
      this.labelFooter.Name = "labelFooter";
      this.labelFooter.Size = new System.Drawing.Size(47, 13);
      this.labelFooter.TabIndex = 4;
      this.labelFooter.Text = "Penutup";
      // 
      // labelPreview
      // 
      this.labelPreview.AutoSize = true;
      this.labelPreview.Location = new System.Drawing.Point(3, 10);
      this.labelPreview.Name = "labelPreview";
      this.labelPreview.Size = new System.Drawing.Size(90, 13);
      this.labelPreview.TabIndex = 5;
      this.labelPreview.Text = "Contoh Tampilan:";
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
      // HeaderAndFooterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.labelPreview);
      this.Controls.Add(this.labelFooter);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.textBoxExample);
      this.Controls.Add(this.textBoxFooter);
      this.Controls.Add(this.textBoxHeader);
      this.Name = "HeaderAndFooterForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Nota";
      this.Load += new System.EventHandler(this.HeaderAndFooterForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxHeader;
    private System.Windows.Forms.TextBox textBoxFooter;
    private System.Windows.Forms.TextBox textBoxExample;
    private System.Windows.Forms.Label labelHeader;
    private System.Windows.Forms.Label labelFooter;
    private System.Windows.Forms.Label labelPreview;
    private System.Windows.Forms.Button buttonSave;
  }
}

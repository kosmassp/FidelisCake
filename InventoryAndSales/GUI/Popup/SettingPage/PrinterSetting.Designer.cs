namespace InventoryAndSales.GUI.Popup.SettingPage
{
  partial class PrinterSettingForm
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
      this.labelPrinter = new System.Windows.Forms.Label();
      this.comboBoxPrinter = new System.Windows.Forms.ComboBox();
      this.labelPrinterStatus = new System.Windows.Forms.Label();
      this.labelPaperWidth = new System.Windows.Forms.Label();
      this.numericPaperWidth = new System.Windows.Forms.NumericUpDown();
      this.labelPaperWidthUnit = new System.Windows.Forms.Label();
      this.buttonWidth58 = new System.Windows.Forms.Button();
      this.buttonWidth80 = new System.Windows.Forms.Button();
      this.labelPreviewCaption = new System.Windows.Forms.Label();
      this.textBoxPreview = new System.Windows.Forms.TextBox();
      this.buttonTestPrint = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.numericPaperWidth)).BeginInit();
      this.SuspendLayout();
      //
      // labelPrinter
      //
      this.labelPrinter.AutoSize = true;
      this.labelPrinter.Location = new System.Drawing.Point(6, 14);
      this.labelPrinter.Name = "labelPrinter";
      this.labelPrinter.Size = new System.Drawing.Size(44, 13);
      this.labelPrinter.TabIndex = 0;
      this.labelPrinter.Text = "Printer:";
      //
      // comboBoxPrinter
      //
      this.comboBoxPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPrinter.Location = new System.Drawing.Point(9, 32);
      this.comboBoxPrinter.Name = "comboBoxPrinter";
      this.comboBoxPrinter.Size = new System.Drawing.Size(516, 21);
      this.comboBoxPrinter.TabIndex = 1;
      this.comboBoxPrinter.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrinter_SelectedIndexChanged);
      //
      // labelPrinterStatus
      //
      this.labelPrinterStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.labelPrinterStatus.Location = new System.Drawing.Point(6, 58);
      this.labelPrinterStatus.Name = "labelPrinterStatus";
      this.labelPrinterStatus.Size = new System.Drawing.Size(519, 30);
      this.labelPrinterStatus.TabIndex = 2;
      this.labelPrinterStatus.Text = "-";
      //
      // labelPaperWidth
      //
      this.labelPaperWidth.AutoSize = true;
      this.labelPaperWidth.Location = new System.Drawing.Point(6, 100);
      this.labelPaperWidth.Name = "labelPaperWidth";
      this.labelPaperWidth.Size = new System.Drawing.Size(70, 13);
      this.labelPaperWidth.TabIndex = 3;
      this.labelPaperWidth.Text = "Lebar Kertas:";
      //
      // numericPaperWidth
      //
      this.numericPaperWidth.Location = new System.Drawing.Point(9, 118);
      this.numericPaperWidth.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
      this.numericPaperWidth.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
      this.numericPaperWidth.Name = "numericPaperWidth";
      this.numericPaperWidth.Size = new System.Drawing.Size(70, 20);
      this.numericPaperWidth.TabIndex = 4;
      this.numericPaperWidth.Value = new decimal(new int[] { 67, 0, 0, 0 });
      this.numericPaperWidth.ValueChanged += new System.EventHandler(this.numericPaperWidth_ValueChanged);
      //
      // labelPaperWidthUnit
      //
      this.labelPaperWidthUnit.AutoSize = true;
      this.labelPaperWidthUnit.Location = new System.Drawing.Point(85, 120);
      this.labelPaperWidthUnit.Name = "labelPaperWidthUnit";
      this.labelPaperWidthUnit.Size = new System.Drawing.Size(23, 13);
      this.labelPaperWidthUnit.TabIndex = 5;
      this.labelPaperWidthUnit.Text = "mm";
      //
      // buttonWidth58
      //
      this.buttonWidth58.Location = new System.Drawing.Point(120, 116);
      this.buttonWidth58.Name = "buttonWidth58";
      this.buttonWidth58.Size = new System.Drawing.Size(75, 23);
      this.buttonWidth58.TabIndex = 6;
      this.buttonWidth58.Text = "58 mm";
      this.buttonWidth58.UseVisualStyleBackColor = true;
      this.buttonWidth58.Click += new System.EventHandler(this.buttonWidth58_Click);
      //
      // buttonWidth80
      //
      this.buttonWidth80.Location = new System.Drawing.Point(201, 116);
      this.buttonWidth80.Name = "buttonWidth80";
      this.buttonWidth80.Size = new System.Drawing.Size(75, 23);
      this.buttonWidth80.TabIndex = 7;
      this.buttonWidth80.Text = "80 mm";
      this.buttonWidth80.UseVisualStyleBackColor = true;
      this.buttonWidth80.Click += new System.EventHandler(this.buttonWidth80_Click);
      //
      // labelPreviewCaption
      //
      this.labelPreviewCaption.AutoSize = true;
      this.labelPreviewCaption.Location = new System.Drawing.Point(6, 152);
      this.labelPreviewCaption.Name = "labelPreviewCaption";
      this.labelPreviewCaption.Size = new System.Drawing.Size(90, 13);
      this.labelPreviewCaption.TabIndex = 8;
      this.labelPreviewCaption.Text = "Contoh Tampilan:";
      //
      // textBoxPreview
      //
      this.textBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPreview.Location = new System.Drawing.Point(9, 168);
      this.textBoxPreview.Multiline = true;
      this.textBoxPreview.Name = "textBoxPreview";
      this.textBoxPreview.ReadOnly = true;
      this.textBoxPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxPreview.Size = new System.Drawing.Size(516, 165);
      this.textBoxPreview.TabIndex = 9;
      //
      // buttonTestPrint
      //
      this.buttonTestPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTestPrint.Location = new System.Drawing.Point(9, 340);
      this.buttonTestPrint.Name = "buttonTestPrint";
      this.buttonTestPrint.Size = new System.Drawing.Size(100, 23);
      this.buttonTestPrint.TabIndex = 10;
      this.buttonTestPrint.Text = "Tes Cetak";
      this.buttonTestPrint.UseVisualStyleBackColor = true;
      this.buttonTestPrint.Click += new System.EventHandler(this.buttonTestPrint_Click);
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
      // PrinterSettingForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.buttonTestPrint);
      this.Controls.Add(this.textBoxPreview);
      this.Controls.Add(this.labelPreviewCaption);
      this.Controls.Add(this.buttonWidth80);
      this.Controls.Add(this.buttonWidth58);
      this.Controls.Add(this.labelPaperWidthUnit);
      this.Controls.Add(this.numericPaperWidth);
      this.Controls.Add(this.labelPaperWidth);
      this.Controls.Add(this.labelPrinterStatus);
      this.Controls.Add(this.comboBoxPrinter);
      this.Controls.Add(this.labelPrinter);
      this.Name = "PrinterSettingForm";
      this.Size = new System.Drawing.Size(537, 378);
      this.Tag = "Printer";
      this.Load += new System.EventHandler(this.PrinterSettingForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.numericPaperWidth)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelPrinter;
    private System.Windows.Forms.ComboBox comboBoxPrinter;
    private System.Windows.Forms.Label labelPrinterStatus;
    private System.Windows.Forms.Label labelPaperWidth;
    private System.Windows.Forms.NumericUpDown numericPaperWidth;
    private System.Windows.Forms.Label labelPaperWidthUnit;
    private System.Windows.Forms.Button buttonWidth58;
    private System.Windows.Forms.Button buttonWidth80;
    private System.Windows.Forms.Label labelPreviewCaption;
    private System.Windows.Forms.TextBox textBoxPreview;
    private System.Windows.Forms.Button buttonTestPrint;
    private System.Windows.Forms.Button buttonSave;
  }
}

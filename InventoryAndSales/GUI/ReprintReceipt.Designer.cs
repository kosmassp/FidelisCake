namespace InventoryAndSales.GUI
{
  partial class ReprintReceipt
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
      this.buttonPrint = new System.Windows.Forms.Button();
      this.labelFacturNumber = new System.Windows.Forms.Label();
      this.textBoxFacturNumber = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // buttonPrint
      // 
      this.buttonPrint.Location = new System.Drawing.Point(212, 85);
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Size = new System.Drawing.Size(60, 26);
      this.buttonPrint.TabIndex = 0;
      this.buttonPrint.Text = "Print";
      this.buttonPrint.UseVisualStyleBackColor = true;
      this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
      // 
      // labelFacturNumber
      // 
      this.labelFacturNumber.AutoSize = true;
      this.labelFacturNumber.Location = new System.Drawing.Point(12, 37);
      this.labelFacturNumber.Name = "labelFacturNumber";
      this.labelFacturNumber.Size = new System.Drawing.Size(57, 13);
      this.labelFacturNumber.TabIndex = 1;
      this.labelFacturNumber.Text = "No Faktur:";
      // 
      // textBoxFacturNumber
      // 
      this.textBoxFacturNumber.Location = new System.Drawing.Point(84, 32);
      this.textBoxFacturNumber.Name = "textBoxFacturNumber";
      this.textBoxFacturNumber.Size = new System.Drawing.Size(188, 20);
      this.textBoxFacturNumber.TabIndex = 2;
      // 
      // ReprintReceipt
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(288, 136);
      this.Controls.Add(this.textBoxFacturNumber);
      this.Controls.Add(this.labelFacturNumber);
      this.Controls.Add(this.buttonPrint);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ReprintReceipt";
      this.Text = "Print ulang nota";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonPrint;
    private System.Windows.Forms.Label labelFacturNumber;
    private System.Windows.Forms.TextBox textBoxFacturNumber;
  }
}
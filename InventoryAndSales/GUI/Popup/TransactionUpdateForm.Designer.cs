namespace InventoryAndSales.GUI.Popup
{
  partial class TransactionUpdateForm
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
      this.transactionUpdatePage1 = new InventoryAndSales.GUI.Page.TransactionUpdatePage();
      this.SuspendLayout();
      // 
      // transactionUpdatePage1
      // 
      this.transactionUpdatePage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.transactionUpdatePage1.Location = new System.Drawing.Point(0, 0);
      this.transactionUpdatePage1.Name = "transactionUpdatePage1";
      this.transactionUpdatePage1.Size = new System.Drawing.Size(1026, 544);
      this.transactionUpdatePage1.TabIndex = 0;
      this.transactionUpdatePage1.CheckoutSucceed += new System.EventHandler(this.transactionUpdatePage1_CheckoutSucceed);
      this.transactionUpdatePage1.BackClick += new System.EventHandler(this.transactionUpdatePage1_BackClick);
      // 
      // TransactionUpdateForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1026, 544);
      this.ControlBox = false;
      this.Controls.Add(this.transactionUpdatePage1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "TransactionUpdateForm";
      this.Text = "TransactionUpdateForm";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransactionUpdateForm_FormClosing);
      this.ResumeLayout(false);

    }

    #endregion

    private InventoryAndSales.GUI.Page.TransactionUpdatePage transactionUpdatePage1;
  }
}
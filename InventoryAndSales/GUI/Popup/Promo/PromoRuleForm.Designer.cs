
namespace InventoryAndSales.GUI.Popup.Promo {
  partial class PromoRuleForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.comboBoxRuleType = new System.Windows.Forms.ComboBox();
      this.comboBoxRuleComparator = new System.Windows.Forms.ComboBox();
      this.promoRuleControl1 = new InventoryAndSales.GUI.Popup.Promo.PromoRuleControl();
      this.SuspendLayout();
      // 
      // comboBoxRuleType
      // 
      this.comboBoxRuleType.FormattingEnabled = true;
      this.comboBoxRuleType.Location = new System.Drawing.Point(25, 27);
      this.comboBoxRuleType.Name = "comboBoxRuleType";
      this.comboBoxRuleType.Size = new System.Drawing.Size(152, 21);
      this.comboBoxRuleType.TabIndex = 0;
      // 
      // comboBoxRuleComparator
      // 
      this.comboBoxRuleComparator.FormattingEnabled = true;
      this.comboBoxRuleComparator.Location = new System.Drawing.Point(183, 27);
      this.comboBoxRuleComparator.Name = "comboBoxRuleComparator";
      this.comboBoxRuleComparator.Size = new System.Drawing.Size(78, 21);
      this.comboBoxRuleComparator.TabIndex = 1;
      // 
      // promoRuleControl1
      // 
      this.promoRuleControl1.Location = new System.Drawing.Point(283, 27);
      this.promoRuleControl1.Name = "promoRuleControl1";
      this.promoRuleControl1.Size = new System.Drawing.Size(215, 66);
      this.promoRuleControl1.TabIndex = 2;
      // 
      // PromoRuleForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 244);
      this.Controls.Add(this.promoRuleControl1);
      this.Controls.Add(this.comboBoxRuleComparator);
      this.Controls.Add(this.comboBoxRuleType);
      this.Name = "PromoRuleForm";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxRuleType;
    private System.Windows.Forms.ComboBox comboBoxRuleComparator;
    private PromoRuleControl promoRuleControl1;
  }
}
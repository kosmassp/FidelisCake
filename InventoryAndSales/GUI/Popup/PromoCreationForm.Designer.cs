
namespace InventoryAndSales.GUI.Popup {
  partial class PromoCreationForm {
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
      this.buttonAddPromoRule = new System.Windows.Forms.Button();
      this.textBoxPromoName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.listBox2 = new System.Windows.Forms.ListBox();
      this.buttonDeletePromoApplication = new System.Windows.Forms.Button();
      this.buttonDeletePromoRule = new System.Windows.Forms.Button();
      this.buttonAddPromoApplication = new System.Windows.Forms.Button();
      this.groupBoxPromoRule = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.buttonPromoOk = new System.Windows.Forms.Button();
      this.buttonPromoCancel = new System.Windows.Forms.Button();
      this.groupBoxPromoRule.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonAddPromoRule
      // 
      this.buttonAddPromoRule.BackgroundImage = global::InventoryAndSales.Properties.Resources.Add;
      this.buttonAddPromoRule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.buttonAddPromoRule.Location = new System.Drawing.Point(13, 19);
      this.buttonAddPromoRule.Name = "buttonAddPromoRule";
      this.buttonAddPromoRule.Size = new System.Drawing.Size(40, 40);
      this.buttonAddPromoRule.TabIndex = 1;
      this.buttonAddPromoRule.UseVisualStyleBackColor = true;
      // 
      // textBoxPromoName
      // 
      this.textBoxPromoName.Location = new System.Drawing.Point(86, 13);
      this.textBoxPromoName.Name = "textBoxPromoName";
      this.textBoxPromoName.Size = new System.Drawing.Size(313, 20);
      this.textBoxPromoName.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(68, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Nama Promo";
      // 
      // listBox1
      // 
      this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new System.Drawing.Point(59, 19);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(708, 121);
      this.listBox1.TabIndex = 3;
      this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
      // 
      // listBox2
      // 
      this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox2.FormattingEnabled = true;
      this.listBox2.Location = new System.Drawing.Point(59, 18);
      this.listBox2.Name = "listBox2";
      this.listBox2.Size = new System.Drawing.Size(708, 147);
      this.listBox2.TabIndex = 6;
      // 
      // buttonDeletePromoApplication
      // 
      this.buttonDeletePromoApplication.BackgroundImage = global::InventoryAndSales.Properties.Resources.Remove;
      this.buttonDeletePromoApplication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.buttonDeletePromoApplication.Location = new System.Drawing.Point(13, 65);
      this.buttonDeletePromoApplication.Name = "buttonDeletePromoApplication";
      this.buttonDeletePromoApplication.Size = new System.Drawing.Size(40, 40);
      this.buttonDeletePromoApplication.TabIndex = 5;
      this.buttonDeletePromoApplication.UseVisualStyleBackColor = true;
      // 
      // buttonDeletePromoRule
      // 
      this.buttonDeletePromoRule.BackgroundImage = global::InventoryAndSales.Properties.Resources.Remove;
      this.buttonDeletePromoRule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.buttonDeletePromoRule.Location = new System.Drawing.Point(13, 65);
      this.buttonDeletePromoRule.Name = "buttonDeletePromoRule";
      this.buttonDeletePromoRule.Size = new System.Drawing.Size(40, 40);
      this.buttonDeletePromoRule.TabIndex = 2;
      this.buttonDeletePromoRule.UseVisualStyleBackColor = true;
      // 
      // buttonAddPromoApplication
      // 
      this.buttonAddPromoApplication.BackgroundImage = global::InventoryAndSales.Properties.Resources.Add;
      this.buttonAddPromoApplication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.buttonAddPromoApplication.Location = new System.Drawing.Point(13, 19);
      this.buttonAddPromoApplication.Name = "buttonAddPromoApplication";
      this.buttonAddPromoApplication.Size = new System.Drawing.Size(40, 40);
      this.buttonAddPromoApplication.TabIndex = 4;
      this.buttonAddPromoApplication.UseVisualStyleBackColor = true;
      // 
      // groupBoxPromoRule
      // 
      this.groupBoxPromoRule.Controls.Add(this.listBox1);
      this.groupBoxPromoRule.Controls.Add(this.buttonDeletePromoRule);
      this.groupBoxPromoRule.Controls.Add(this.buttonAddPromoRule);
      this.groupBoxPromoRule.Location = new System.Drawing.Point(15, 56);
      this.groupBoxPromoRule.Name = "groupBoxPromoRule";
      this.groupBoxPromoRule.Size = new System.Drawing.Size(773, 151);
      this.groupBoxPromoRule.TabIndex = 7;
      this.groupBoxPromoRule.TabStop = false;
      this.groupBoxPromoRule.Text = "Syarat dan Ketentuan";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.listBox2);
      this.groupBox2.Controls.Add(this.buttonAddPromoApplication);
      this.groupBox2.Controls.Add(this.buttonDeletePromoApplication);
      this.groupBox2.Location = new System.Drawing.Point(15, 213);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(773, 183);
      this.groupBox2.TabIndex = 8;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Promo";
      // 
      // buttonPromoOk
      // 
      this.buttonPromoOk.Location = new System.Drawing.Point(15, 415);
      this.buttonPromoOk.Name = "buttonPromoOk";
      this.buttonPromoOk.Size = new System.Drawing.Size(75, 23);
      this.buttonPromoOk.TabIndex = 9;
      this.buttonPromoOk.Text = "OK";
      this.buttonPromoOk.UseVisualStyleBackColor = true;
      // 
      // buttonPromoCancel
      // 
      this.buttonPromoCancel.Location = new System.Drawing.Point(96, 415);
      this.buttonPromoCancel.Name = "buttonPromoCancel";
      this.buttonPromoCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonPromoCancel.TabIndex = 10;
      this.buttonPromoCancel.Text = "Cancel";
      this.buttonPromoCancel.UseVisualStyleBackColor = true;
      // 
      // PromoCreationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.buttonPromoCancel);
      this.Controls.Add(this.buttonPromoOk);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBoxPromoRule);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBoxPromoName);
      this.Name = "PromoCreationForm";
      this.Text = "Promo Creation";
      this.groupBoxPromoRule.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonAddPromoRule;
    private System.Windows.Forms.TextBox textBoxPromoName;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonAddPromoApplication;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.ListBox listBox2;
    private System.Windows.Forms.Button buttonDeletePromoApplication;
    private System.Windows.Forms.Button buttonDeletePromoRule;
    private System.Windows.Forms.GroupBox groupBoxPromoRule;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button buttonPromoOk;
    private System.Windows.Forms.Button buttonPromoCancel;
  }
}

namespace InventoryAndSales.GUI.Page
{
  partial class PromoPage
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
      this.components = new System.ComponentModel.Container();
      this.panel1 = new System.Windows.Forms.Panel();
      this.groupBoxPromoType = new System.Windows.Forms.GroupBox();
      this.flowLayoutPanelPromoType = new System.Windows.Forms.FlowLayoutPanel();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPageDiscount = new System.Windows.Forms.TabPage();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.radioButton3 = new System.Windows.Forms.RadioButton();
      this.radioButton4 = new System.Windows.Forms.RadioButton();
      this.tabPageFreeItem = new System.Windows.Forms.TabPage();
      this.label3 = new System.Windows.Forms.Label();
      this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
      this.tabPageSpecialPrice = new System.Windows.Forms.TabPage();
      this.label4 = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.tabPageVoucher = new System.Windows.Forms.TabPage();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.button3 = new System.Windows.Forms.Button();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.radioButton8 = new System.Windows.Forms.RadioButton();
      this.button2 = new System.Windows.Forms.Button();
      this.radioButton7 = new System.Windows.Forms.RadioButton();
      this.groupBoxPromoFilter = new System.Windows.Forms.GroupBox();
      this.buttonFilterPromoDateSelection = new System.Windows.Forms.Button();
      this.checkBoxDateFilter = new System.Windows.Forms.CheckBox();
      this.checkBoxItemFilter = new System.Windows.Forms.CheckBox();
      this.checkBoxItemCount = new System.Windows.Forms.CheckBox();
      this.checkBoxFilterPromoAmountInclusive = new System.Windows.Forms.CheckBox();
      this.checkBoxTotalAmount = new System.Windows.Forms.CheckBox();
      this.textBoxFilterPromoAmount = new System.Windows.Forms.TextBox();
      this.checkBoxFilterPromoMultipleApply = new System.Windows.Forms.CheckBox();
      this.numericUpDownFilterPromoItemCount = new System.Windows.Forms.NumericUpDown();
      this.buttonFilterPromoItemSelection = new System.Windows.Forms.Button();
      this.labelPromoName = new System.Windows.Forms.Label();
      this.textBoxPromoName = new System.Windows.Forms.TextBox();
      this.toolTipCheckboxRule = new System.Windows.Forms.ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.groupBoxPromoType.SuspendLayout();
      this.flowLayoutPanelPromoType.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPageDiscount.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      this.tabPageFreeItem.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
      this.tabPageSpecialPrice.SuspendLayout();
      this.tabPageVoucher.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBoxPromoFilter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterPromoItemCount)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.groupBoxPromoType);
      this.panel1.Controls.Add(this.groupBoxPromoFilter);
      this.panel1.Controls.Add(this.labelPromoName);
      this.panel1.Controls.Add(this.textBoxPromoName);
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(432, 497);
      this.panel1.TabIndex = 2;
      this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
      // 
      // groupBoxPromoType
      // 
      this.groupBoxPromoType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPromoType.Controls.Add(this.flowLayoutPanelPromoType);
      this.groupBoxPromoType.Location = new System.Drawing.Point(8, 208);
      this.groupBoxPromoType.Name = "groupBoxPromoType";
      this.groupBoxPromoType.Size = new System.Drawing.Size(411, 277);
      this.groupBoxPromoType.TabIndex = 34;
      this.groupBoxPromoType.TabStop = false;
      this.groupBoxPromoType.Text = "Promo";
      // 
      // flowLayoutPanelPromoType
      // 
      this.flowLayoutPanelPromoType.Controls.Add(this.tabControl1);
      this.flowLayoutPanelPromoType.Controls.Add(this.groupBox3);
      this.flowLayoutPanelPromoType.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanelPromoType.Location = new System.Drawing.Point(3, 16);
      this.flowLayoutPanelPromoType.Name = "flowLayoutPanelPromoType";
      this.flowLayoutPanelPromoType.Size = new System.Drawing.Size(405, 258);
      this.flowLayoutPanelPromoType.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
      this.tabControl1.Controls.Add(this.tabPageDiscount);
      this.tabControl1.Controls.Add(this.tabPageFreeItem);
      this.tabControl1.Controls.Add(this.tabPageSpecialPrice);
      this.tabControl1.Controls.Add(this.tabPageVoucher);
      this.tabControl1.Location = new System.Drawing.Point(3, 3);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(392, 155);
      this.tabControl1.TabIndex = 17;
      // 
      // tabPageDiscount
      // 
      this.tabPageDiscount.Controls.Add(this.textBox2);
      this.tabPageDiscount.Controls.Add(this.numericUpDown1);
      this.tabPageDiscount.Controls.Add(this.textBox1);
      this.tabPageDiscount.Controls.Add(this.checkBox1);
      this.tabPageDiscount.Controls.Add(this.radioButton3);
      this.tabPageDiscount.Controls.Add(this.radioButton4);
      this.tabPageDiscount.Location = new System.Drawing.Point(4, 25);
      this.tabPageDiscount.Name = "tabPageDiscount";
      this.tabPageDiscount.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageDiscount.Size = new System.Drawing.Size(384, 126);
      this.tabPageDiscount.TabIndex = 0;
      this.tabPageDiscount.Text = "Discount";
      this.tabPageDiscount.UseVisualStyleBackColor = true;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(152, 27);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(109, 20);
      this.textBox2.TabIndex = 26;
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(152, 6);
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(53, 20);
      this.numericUpDown1.TabIndex = 19;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(152, 73);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(109, 20);
      this.textBox1.TabIndex = 19;
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(25, 29);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(119, 17);
      this.checkBox1.TabIndex = 25;
      this.checkBox1.Text = "Maksimal Potongan";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // radioButton3
      // 
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new System.Drawing.Point(6, 74);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new System.Drawing.Size(59, 17);
      this.radioButton3.TabIndex = 24;
      this.radioButton3.TabStop = true;
      this.radioButton3.Text = "Rupiah";
      this.radioButton3.UseVisualStyleBackColor = true;
      // 
      // radioButton4
      // 
      this.radioButton4.AutoSize = true;
      this.radioButton4.Location = new System.Drawing.Point(6, 6);
      this.radioButton4.Name = "radioButton4";
      this.radioButton4.Size = new System.Drawing.Size(58, 17);
      this.radioButton4.TabIndex = 23;
      this.radioButton4.TabStop = true;
      this.radioButton4.Text = "Persen";
      this.radioButton4.UseVisualStyleBackColor = true;
      // 
      // tabPageFreeItem
      // 
      this.tabPageFreeItem.Controls.Add(this.label3);
      this.tabPageFreeItem.Controls.Add(this.numericUpDown2);
      this.tabPageFreeItem.Location = new System.Drawing.Point(4, 25);
      this.tabPageFreeItem.Name = "tabPageFreeItem";
      this.tabPageFreeItem.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageFreeItem.Size = new System.Drawing.Size(384, 126);
      this.tabPageFreeItem.TabIndex = 1;
      this.tabPageFreeItem.Text = "Gratis";
      this.tabPageFreeItem.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 7);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(107, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "Jumlah Barang Free: ";
      // 
      // numericUpDown2
      // 
      this.numericUpDown2.Location = new System.Drawing.Point(119, 5);
      this.numericUpDown2.Name = "numericUpDown2";
      this.numericUpDown2.Size = new System.Drawing.Size(50, 20);
      this.numericUpDown2.TabIndex = 20;
      // 
      // tabPageSpecialPrice
      // 
      this.tabPageSpecialPrice.Controls.Add(this.label4);
      this.tabPageSpecialPrice.Controls.Add(this.textBox3);
      this.tabPageSpecialPrice.Location = new System.Drawing.Point(4, 25);
      this.tabPageSpecialPrice.Name = "tabPageSpecialPrice";
      this.tabPageSpecialPrice.Size = new System.Drawing.Size(384, 126);
      this.tabPageSpecialPrice.TabIndex = 3;
      this.tabPageSpecialPrice.Text = "Harga Khusus";
      this.tabPageSpecialPrice.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 13);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(42, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "Harga: ";
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(56, 10);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(143, 20);
      this.textBox3.TabIndex = 19;
      // 
      // tabPageVoucher
      // 
      this.tabPageVoucher.Controls.Add(this.label2);
      this.tabPageVoucher.Controls.Add(this.label1);
      this.tabPageVoucher.Controls.Add(this.comboBox1);
      this.tabPageVoucher.Controls.Add(this.button3);
      this.tabPageVoucher.Location = new System.Drawing.Point(4, 25);
      this.tabPageVoucher.Name = "tabPageVoucher";
      this.tabPageVoucher.Size = new System.Drawing.Size(384, 126);
      this.tabPageVoucher.TabIndex = 2;
      this.tabPageVoucher.Text = "Voucher";
      this.tabPageVoucher.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.ForeColor = System.Drawing.Color.Red;
      this.label2.Location = new System.Drawing.Point(3, 69);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(176, 13);
      this.label2.TabIndex = 23;
      this.label2.Text = "Promo yang dipilih berlaku kelipatan";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 29);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(62, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Pilih Promo:";
      // 
      // comboBox1
      // 
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(6, 45);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(197, 21);
      this.comboBox1.TabIndex = 21;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(5, 3);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(109, 23);
      this.button3.TabIndex = 20;
      this.button3.Text = "Pilih Tanggal ...";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.Controls.Add(this.radioButton8);
      this.groupBox3.Controls.Add(this.button2);
      this.groupBox3.Controls.Add(this.radioButton7);
      this.groupBox3.Location = new System.Drawing.Point(3, 164);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(383, 73);
      this.groupBox3.TabIndex = 33;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Promo terhadap barang ";
      // 
      // radioButton8
      // 
      this.radioButton8.AutoSize = true;
      this.radioButton8.Checked = true;
      this.radioButton8.Location = new System.Drawing.Point(16, 19);
      this.radioButton8.Name = "radioButton8";
      this.radioButton8.Size = new System.Drawing.Size(95, 17);
      this.radioButton8.TabIndex = 31;
      this.radioButton8.TabStop = true;
      this.radioButton8.Text = "Barang Terkait";
      this.radioButton8.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(124, 39);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(109, 23);
      this.button2.TabIndex = 30;
      this.button2.Text = "Pilih Barang ...";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // radioButton7
      // 
      this.radioButton7.AutoSize = true;
      this.radioButton7.Location = new System.Drawing.Point(16, 42);
      this.radioButton7.Name = "radioButton7";
      this.radioButton7.Size = new System.Drawing.Size(102, 17);
      this.radioButton7.TabIndex = 32;
      this.radioButton7.Text = "Barang Tertentu";
      this.radioButton7.UseVisualStyleBackColor = true;
      // 
      // groupBoxPromoFilter
      // 
      this.groupBoxPromoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPromoFilter.Controls.Add(this.buttonFilterPromoDateSelection);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxDateFilter);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxItemFilter);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxItemCount);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxFilterPromoAmountInclusive);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxTotalAmount);
      this.groupBoxPromoFilter.Controls.Add(this.textBoxFilterPromoAmount);
      this.groupBoxPromoFilter.Controls.Add(this.checkBoxFilterPromoMultipleApply);
      this.groupBoxPromoFilter.Controls.Add(this.numericUpDownFilterPromoItemCount);
      this.groupBoxPromoFilter.Controls.Add(this.buttonFilterPromoItemSelection);
      this.groupBoxPromoFilter.Location = new System.Drawing.Point(8, 38);
      this.groupBoxPromoFilter.Name = "groupBoxPromoFilter";
      this.groupBoxPromoFilter.Size = new System.Drawing.Size(411, 164);
      this.groupBoxPromoFilter.TabIndex = 34;
      this.groupBoxPromoFilter.TabStop = false;
      this.groupBoxPromoFilter.Text = "Syarat && Ketentuan";
      // 
      // buttonFilterPromoDateSelection
      // 
      this.buttonFilterPromoDateSelection.Location = new System.Drawing.Point(77, 21);
      this.buttonFilterPromoDateSelection.Name = "buttonFilterPromoDateSelection";
      this.buttonFilterPromoDateSelection.Size = new System.Drawing.Size(32, 23);
      this.buttonFilterPromoDateSelection.TabIndex = 18;
      this.buttonFilterPromoDateSelection.Text = "...";
      this.buttonFilterPromoDateSelection.UseVisualStyleBackColor = true;
      // 
      // checkBoxDateFilter
      // 
      this.checkBoxDateFilter.AutoSize = true;
      this.checkBoxDateFilter.Location = new System.Drawing.Point(13, 25);
      this.checkBoxDateFilter.Name = "checkBoxDateFilter";
      this.checkBoxDateFilter.Size = new System.Drawing.Size(58, 17);
      this.checkBoxDateFilter.TabIndex = 0;
      this.checkBoxDateFilter.Text = "Waktu";
      this.checkBoxDateFilter.UseVisualStyleBackColor = true;
      // 
      // checkBoxItemFilter
      // 
      this.checkBoxItemFilter.AutoSize = true;
      this.checkBoxItemFilter.Location = new System.Drawing.Point(13, 48);
      this.checkBoxItemFilter.Name = "checkBoxItemFilter";
      this.checkBoxItemFilter.Size = new System.Drawing.Size(60, 17);
      this.checkBoxItemFilter.TabIndex = 1;
      this.checkBoxItemFilter.Text = "Barang";
      this.checkBoxItemFilter.UseVisualStyleBackColor = true;
      // 
      // checkBoxItemCount
      // 
      this.checkBoxItemCount.AutoSize = true;
      this.checkBoxItemCount.Location = new System.Drawing.Point(13, 71);
      this.checkBoxItemCount.Name = "checkBoxItemCount";
      this.checkBoxItemCount.Size = new System.Drawing.Size(96, 17);
      this.checkBoxItemCount.TabIndex = 2;
      this.checkBoxItemCount.Text = "Jumlah Barang";
      this.checkBoxItemCount.UseVisualStyleBackColor = true;
      // 
      // checkBoxFilterPromoAmountInclusive
      // 
      this.checkBoxFilterPromoAmountInclusive.AutoSize = true;
      this.checkBoxFilterPromoAmountInclusive.Location = new System.Drawing.Point(290, 94);
      this.checkBoxFilterPromoAmountInclusive.Name = "checkBoxFilterPromoAmountInclusive";
      this.checkBoxFilterPromoAmountInclusive.Size = new System.Drawing.Size(88, 17);
      this.checkBoxFilterPromoAmountInclusive.TabIndex = 15;
      this.checkBoxFilterPromoAmountInclusive.Text = "Item terdaftar";
      this.toolTipCheckboxRule.SetToolTip(this.checkBoxFilterPromoAmountInclusive, "Jumlah yang dihitung hanya untuk item yang ada di list barang.");
      this.checkBoxFilterPromoAmountInclusive.UseVisualStyleBackColor = true;
      // 
      // checkBoxTotalAmount
      // 
      this.checkBoxTotalAmount.AutoSize = true;
      this.checkBoxTotalAmount.Location = new System.Drawing.Point(13, 94);
      this.checkBoxTotalAmount.Name = "checkBoxTotalAmount";
      this.checkBoxTotalAmount.Size = new System.Drawing.Size(111, 17);
      this.checkBoxTotalAmount.TabIndex = 3;
      this.checkBoxTotalAmount.Text = "Jumlah Pembelian";
      this.checkBoxTotalAmount.UseVisualStyleBackColor = true;
      // 
      // textBoxFilterPromoAmount
      // 
      this.textBoxFilterPromoAmount.Location = new System.Drawing.Point(141, 92);
      this.textBoxFilterPromoAmount.Name = "textBoxFilterPromoAmount";
      this.textBoxFilterPromoAmount.Size = new System.Drawing.Size(143, 20);
      this.textBoxFilterPromoAmount.TabIndex = 13;
      // 
      // checkBoxFilterPromoMultipleApply
      // 
      this.checkBoxFilterPromoMultipleApply.AutoSize = true;
      this.checkBoxFilterPromoMultipleApply.Checked = true;
      this.checkBoxFilterPromoMultipleApply.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxFilterPromoMultipleApply.Location = new System.Drawing.Point(13, 117);
      this.checkBoxFilterPromoMultipleApply.Name = "checkBoxFilterPromoMultipleApply";
      this.checkBoxFilterPromoMultipleApply.Size = new System.Drawing.Size(109, 17);
      this.checkBoxFilterPromoMultipleApply.TabIndex = 6;
      this.checkBoxFilterPromoMultipleApply.Text = "Berlaku Kelipatan";
      this.toolTipCheckboxRule.SetToolTip(this.checkBoxFilterPromoMultipleApply, "Kalau promo untuk voucher, seharusnya tidak berlaku kelipatan");
      this.checkBoxFilterPromoMultipleApply.UseVisualStyleBackColor = true;
      // 
      // numericUpDownFilterPromoItemCount
      // 
      this.numericUpDownFilterPromoItemCount.Location = new System.Drawing.Point(141, 70);
      this.numericUpDownFilterPromoItemCount.Name = "numericUpDownFilterPromoItemCount";
      this.numericUpDownFilterPromoItemCount.Size = new System.Drawing.Size(59, 20);
      this.numericUpDownFilterPromoItemCount.TabIndex = 12;
      // 
      // buttonFilterPromoItemSelection
      // 
      this.buttonFilterPromoItemSelection.Location = new System.Drawing.Point(77, 44);
      this.buttonFilterPromoItemSelection.Name = "buttonFilterPromoItemSelection";
      this.buttonFilterPromoItemSelection.Size = new System.Drawing.Size(32, 23);
      this.buttonFilterPromoItemSelection.TabIndex = 11;
      this.buttonFilterPromoItemSelection.Text = "...";
      this.buttonFilterPromoItemSelection.UseVisualStyleBackColor = true;
      // 
      // labelPromoName
      // 
      this.labelPromoName.AutoSize = true;
      this.labelPromoName.Location = new System.Drawing.Point(13, 15);
      this.labelPromoName.Name = "labelPromoName";
      this.labelPromoName.Size = new System.Drawing.Size(41, 13);
      this.labelPromoName.TabIndex = 5;
      this.labelPromoName.Text = "Nama: ";
      // 
      // textBoxPromoName
      // 
      this.textBoxPromoName.Location = new System.Drawing.Point(60, 12);
      this.textBoxPromoName.Name = "textBoxPromoName";
      this.textBoxPromoName.Size = new System.Drawing.Size(198, 20);
      this.textBoxPromoName.TabIndex = 4;
      // 
      // PromoPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.Name = "PromoPage";
      this.Size = new System.Drawing.Size(861, 520);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBoxPromoType.ResumeLayout(false);
      this.flowLayoutPanelPromoType.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPageDiscount.ResumeLayout(false);
      this.tabPageDiscount.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      this.tabPageFreeItem.ResumeLayout(false);
      this.tabPageFreeItem.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
      this.tabPageSpecialPrice.ResumeLayout(false);
      this.tabPageSpecialPrice.PerformLayout();
      this.tabPageVoucher.ResumeLayout(false);
      this.tabPageVoucher.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBoxPromoFilter.ResumeLayout(false);
      this.groupBoxPromoFilter.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterPromoItemCount)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckBox checkBoxFilterPromoMultipleApply;
    private System.Windows.Forms.Label labelPromoName;
    private System.Windows.Forms.TextBox textBoxPromoName;
    private System.Windows.Forms.CheckBox checkBoxTotalAmount;
    private System.Windows.Forms.CheckBox checkBoxItemCount;
    private System.Windows.Forms.CheckBox checkBoxItemFilter;
    private System.Windows.Forms.CheckBox checkBoxDateFilter;
    private System.Windows.Forms.ToolTip toolTipCheckboxRule;
    private System.Windows.Forms.TextBox textBoxFilterPromoAmount;
    private System.Windows.Forms.NumericUpDown numericUpDownFilterPromoItemCount;
    private System.Windows.Forms.Button buttonFilterPromoItemSelection;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPageDiscount;
    private System.Windows.Forms.TabPage tabPageFreeItem;
    private System.Windows.Forms.TabPage tabPageVoucher;
    private System.Windows.Forms.TabPage tabPageSpecialPrice;
    private System.Windows.Forms.CheckBox checkBoxFilterPromoAmountInclusive;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.RadioButton radioButton3;
    private System.Windows.Forms.RadioButton radioButton4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown numericUpDown2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.RadioButton radioButton7;
    private System.Windows.Forms.RadioButton radioButton8;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBoxPromoFilter;
    private System.Windows.Forms.GroupBox groupBoxPromoType;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPromoType;
    private System.Windows.Forms.Button buttonFilterPromoDateSelection;
  }
}

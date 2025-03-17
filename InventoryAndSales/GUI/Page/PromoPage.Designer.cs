
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
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPageDiscount = new System.Windows.Forms.TabPage();
      this.textBoxDiscountMax = new System.Windows.Forms.TextBox();
      this.numericUpDownDiscountPercent = new System.Windows.Forms.NumericUpDown();
      this.textBoxDiscountAmount = new System.Windows.Forms.TextBox();
      this.checkBoxDiscountMaxAmount = new System.Windows.Forms.CheckBox();
      this.radioButtonDiscountRupiah = new System.Windows.Forms.RadioButton();
      this.radioButtonDiscountPercent = new System.Windows.Forms.RadioButton();
      this.tabPageFreeItem = new System.Windows.Forms.TabPage();
      this.label3 = new System.Windows.Forms.Label();
      this.numericUpDownFreeCount = new System.Windows.Forms.NumericUpDown();
      this.tabPageSpecialPrice = new System.Windows.Forms.TabPage();
      this.label4 = new System.Windows.Forms.Label();
      this.textBoxSpecialPrice = new System.Windows.Forms.TextBox();
      this.tabPageVoucher = new System.Windows.Forms.TabPage();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBoxVoucherPromoSelection = new System.Windows.Forms.ComboBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.radioButtonApplySelectedItem = new System.Windows.Forms.RadioButton();
      this.button2 = new System.Windows.Forms.Button();
      this.radioButtonSelectedItem2 = new System.Windows.Forms.RadioButton();
      this.groupBoxPromoFilter = new System.Windows.Forms.GroupBox();
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.checkBoxDateFilter = new System.Windows.Forms.CheckBox();
      this.buttonFilterPromoTimeSelection = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.radioButton1 = new System.Windows.Forms.RadioButton();
      this.panel1.SuspendLayout();
      this.groupBoxPromoType.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPageDiscount.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiscountPercent)).BeginInit();
      this.tabPageFreeItem.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreeCount)).BeginInit();
      this.tabPageSpecialPrice.SuspendLayout();
      this.tabPageVoucher.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBoxPromoFilter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilterPromoItemCount)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Controls.Add(this.buttonCancel);
      this.panel1.Controls.Add(this.groupBoxPromoType);
      this.panel1.Controls.Add(this.groupBoxPromoFilter);
      this.panel1.Controls.Add(this.labelPromoName);
      this.panel1.Controls.Add(this.textBoxPromoName);
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(547, 529);
      this.panel1.TabIndex = 2;
      this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
      // 
      // groupBoxPromoType
      // 
      this.groupBoxPromoType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPromoType.Controls.Add(this.tabControl1);
      this.groupBoxPromoType.Controls.Add(this.groupBox3);
      this.groupBoxPromoType.Location = new System.Drawing.Point(8, 208);
      this.groupBoxPromoType.Name = "groupBoxPromoType";
      this.groupBoxPromoType.Size = new System.Drawing.Size(526, 277);
      this.groupBoxPromoType.TabIndex = 34;
      this.groupBoxPromoType.TabStop = false;
      this.groupBoxPromoType.Text = "Promo";
      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
      this.tabControl1.Controls.Add(this.tabPageDiscount);
      this.tabControl1.Controls.Add(this.tabPageFreeItem);
      this.tabControl1.Controls.Add(this.tabPageSpecialPrice);
      this.tabControl1.Controls.Add(this.tabPageVoucher);
      this.tabControl1.Location = new System.Drawing.Point(8, 19);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(512, 155);
      this.tabControl1.TabIndex = 17;
      // 
      // tabPageDiscount
      // 
      this.tabPageDiscount.Controls.Add(this.label5);
      this.tabPageDiscount.Controls.Add(this.textBoxDiscountMax);
      this.tabPageDiscount.Controls.Add(this.numericUpDownDiscountPercent);
      this.tabPageDiscount.Controls.Add(this.textBoxDiscountAmount);
      this.tabPageDiscount.Controls.Add(this.checkBoxDiscountMaxAmount);
      this.tabPageDiscount.Controls.Add(this.radioButtonDiscountRupiah);
      this.tabPageDiscount.Controls.Add(this.radioButtonDiscountPercent);
      this.tabPageDiscount.Location = new System.Drawing.Point(4, 25);
      this.tabPageDiscount.Name = "tabPageDiscount";
      this.tabPageDiscount.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageDiscount.Size = new System.Drawing.Size(504, 126);
      this.tabPageDiscount.TabIndex = 0;
      this.tabPageDiscount.Text = "Discount";
      this.tabPageDiscount.UseVisualStyleBackColor = true;
      // 
      // textBoxDiscountMax
      // 
      this.textBoxDiscountMax.Location = new System.Drawing.Point(152, 27);
      this.textBoxDiscountMax.Name = "textBoxDiscountMax";
      this.textBoxDiscountMax.Size = new System.Drawing.Size(109, 20);
      this.textBoxDiscountMax.TabIndex = 26;
      // 
      // numericUpDownDiscountPercent
      // 
      this.numericUpDownDiscountPercent.Location = new System.Drawing.Point(152, 6);
      this.numericUpDownDiscountPercent.Name = "numericUpDownDiscountPercent";
      this.numericUpDownDiscountPercent.Size = new System.Drawing.Size(53, 20);
      this.numericUpDownDiscountPercent.TabIndex = 19;
      // 
      // textBoxDiscountAmount
      // 
      this.textBoxDiscountAmount.Location = new System.Drawing.Point(152, 73);
      this.textBoxDiscountAmount.Name = "textBoxDiscountAmount";
      this.textBoxDiscountAmount.Size = new System.Drawing.Size(109, 20);
      this.textBoxDiscountAmount.TabIndex = 19;
      // 
      // checkBoxDiscountMaxAmount
      // 
      this.checkBoxDiscountMaxAmount.AutoSize = true;
      this.checkBoxDiscountMaxAmount.Location = new System.Drawing.Point(25, 29);
      this.checkBoxDiscountMaxAmount.Name = "checkBoxDiscountMaxAmount";
      this.checkBoxDiscountMaxAmount.Size = new System.Drawing.Size(119, 17);
      this.checkBoxDiscountMaxAmount.TabIndex = 25;
      this.checkBoxDiscountMaxAmount.Text = "Maksimal Potongan";
      this.checkBoxDiscountMaxAmount.UseVisualStyleBackColor = true;
      // 
      // radioButtonDiscountRupiah
      // 
      this.radioButtonDiscountRupiah.AutoSize = true;
      this.radioButtonDiscountRupiah.Location = new System.Drawing.Point(6, 74);
      this.radioButtonDiscountRupiah.Name = "radioButtonDiscountRupiah";
      this.radioButtonDiscountRupiah.Size = new System.Drawing.Size(59, 17);
      this.radioButtonDiscountRupiah.TabIndex = 24;
      this.radioButtonDiscountRupiah.TabStop = true;
      this.radioButtonDiscountRupiah.Text = "Rupiah";
      this.radioButtonDiscountRupiah.UseVisualStyleBackColor = true;
      // 
      // radioButtonDiscountPercent
      // 
      this.radioButtonDiscountPercent.AutoSize = true;
      this.radioButtonDiscountPercent.Location = new System.Drawing.Point(6, 6);
      this.radioButtonDiscountPercent.Name = "radioButtonDiscountPercent";
      this.radioButtonDiscountPercent.Size = new System.Drawing.Size(58, 17);
      this.radioButtonDiscountPercent.TabIndex = 23;
      this.radioButtonDiscountPercent.TabStop = true;
      this.radioButtonDiscountPercent.Text = "Persen";
      this.radioButtonDiscountPercent.UseVisualStyleBackColor = true;
      // 
      // tabPageFreeItem
      // 
      this.tabPageFreeItem.Controls.Add(this.label3);
      this.tabPageFreeItem.Controls.Add(this.numericUpDownFreeCount);
      this.tabPageFreeItem.Location = new System.Drawing.Point(4, 25);
      this.tabPageFreeItem.Name = "tabPageFreeItem";
      this.tabPageFreeItem.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageFreeItem.Size = new System.Drawing.Size(630, 126);
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
      // numericUpDownFreeCount
      // 
      this.numericUpDownFreeCount.Location = new System.Drawing.Point(119, 5);
      this.numericUpDownFreeCount.Name = "numericUpDownFreeCount";
      this.numericUpDownFreeCount.Size = new System.Drawing.Size(50, 20);
      this.numericUpDownFreeCount.TabIndex = 20;
      // 
      // tabPageSpecialPrice
      // 
      this.tabPageSpecialPrice.Controls.Add(this.label4);
      this.tabPageSpecialPrice.Controls.Add(this.textBoxSpecialPrice);
      this.tabPageSpecialPrice.Location = new System.Drawing.Point(4, 25);
      this.tabPageSpecialPrice.Name = "tabPageSpecialPrice";
      this.tabPageSpecialPrice.Size = new System.Drawing.Size(504, 126);
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
      // textBoxSpecialPrice
      // 
      this.textBoxSpecialPrice.Location = new System.Drawing.Point(56, 10);
      this.textBoxSpecialPrice.Name = "textBoxSpecialPrice";
      this.textBoxSpecialPrice.Size = new System.Drawing.Size(143, 20);
      this.textBoxSpecialPrice.TabIndex = 19;
      // 
      // tabPageVoucher
      // 
      this.tabPageVoucher.Controls.Add(this.label2);
      this.tabPageVoucher.Controls.Add(this.label1);
      this.tabPageVoucher.Controls.Add(this.comboBoxVoucherPromoSelection);
      this.tabPageVoucher.Location = new System.Drawing.Point(4, 25);
      this.tabPageVoucher.Name = "tabPageVoucher";
      this.tabPageVoucher.Size = new System.Drawing.Size(630, 126);
      this.tabPageVoucher.TabIndex = 2;
      this.tabPageVoucher.Text = "Voucher";
      this.tabPageVoucher.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.ForeColor = System.Drawing.Color.Red;
      this.label2.Location = new System.Drawing.Point(9, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(176, 13);
      this.label2.TabIndex = 23;
      this.label2.Text = "Promo yang dipilih berlaku kelipatan";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(62, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Pilih Promo:";
      // 
      // comboBoxVoucherPromoSelection
      // 
      this.comboBoxVoucherPromoSelection.FormattingEnabled = true;
      this.comboBoxVoucherPromoSelection.Location = new System.Drawing.Point(12, 27);
      this.comboBoxVoucherPromoSelection.Name = "comboBoxVoucherPromoSelection";
      this.comboBoxVoucherPromoSelection.Size = new System.Drawing.Size(197, 21);
      this.comboBoxVoucherPromoSelection.TabIndex = 21;
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.Controls.Add(this.radioButton1);
      this.groupBox3.Controls.Add(this.radioButtonApplySelectedItem);
      this.groupBox3.Controls.Add(this.button2);
      this.groupBox3.Controls.Add(this.radioButtonSelectedItem2);
      this.groupBox3.Location = new System.Drawing.Point(8, 180);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(508, 91);
      this.groupBox3.TabIndex = 33;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Promo terhadap barang ";
      // 
      // radioButtonApplySelectedItem
      // 
      this.radioButtonApplySelectedItem.AutoSize = true;
      this.radioButtonApplySelectedItem.Location = new System.Drawing.Point(10, 43);
      this.radioButtonApplySelectedItem.Name = "radioButtonApplySelectedItem";
      this.radioButtonApplySelectedItem.Size = new System.Drawing.Size(95, 17);
      this.radioButtonApplySelectedItem.TabIndex = 31;
      this.radioButtonApplySelectedItem.Text = "Barang Terkait";
      this.radioButtonApplySelectedItem.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(118, 63);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(109, 23);
      this.button2.TabIndex = 30;
      this.button2.Text = "Pilih Barang ...";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // radioButtonSelectedItem2
      // 
      this.radioButtonSelectedItem2.AutoSize = true;
      this.radioButtonSelectedItem2.Location = new System.Drawing.Point(10, 66);
      this.radioButtonSelectedItem2.Name = "radioButtonSelectedItem2";
      this.radioButtonSelectedItem2.Size = new System.Drawing.Size(102, 17);
      this.radioButtonSelectedItem2.TabIndex = 32;
      this.radioButtonSelectedItem2.Text = "Barang Tertentu";
      this.radioButtonSelectedItem2.UseVisualStyleBackColor = true;
      // 
      // groupBoxPromoFilter
      // 
      this.groupBoxPromoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxPromoFilter.Controls.Add(this.buttonFilterPromoTimeSelection);
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
      this.groupBoxPromoFilter.Size = new System.Drawing.Size(526, 164);
      this.groupBoxPromoFilter.TabIndex = 34;
      this.groupBoxPromoFilter.TabStop = false;
      this.groupBoxPromoFilter.Text = "Syarat && Ketentuan";
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
      this.checkBoxItemCount.Size = new System.Drawing.Size(116, 17);
      this.checkBoxItemCount.TabIndex = 2;
      this.checkBoxItemCount.Text = "Min Jumlah Barang";
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
      this.checkBoxTotalAmount.Size = new System.Drawing.Size(95, 17);
      this.checkBoxTotalAmount.TabIndex = 3;
      this.checkBoxTotalAmount.Text = "Min Pembelian";
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
      this.buttonFilterPromoItemSelection.Location = new System.Drawing.Point(141, 44);
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
      this.textBoxPromoName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPromoName.Location = new System.Drawing.Point(60, 12);
      this.textBoxPromoName.MaximumSize = new System.Drawing.Size(350, 20);
      this.textBoxPromoName.MinimumSize = new System.Drawing.Size(121, 20);
      this.textBoxPromoName.Name = "textBoxPromoName";
      this.textBoxPromoName.Size = new System.Drawing.Size(255, 20);
      this.textBoxPromoName.TabIndex = 4;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Location = new System.Drawing.Point(106, 491);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 35;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(212, 7);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(15, 13);
      this.label5.TabIndex = 27;
      this.label5.Text = "%";
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
      // buttonFilterPromoTimeSelection
      // 
      this.buttonFilterPromoTimeSelection.Location = new System.Drawing.Point(141, 21);
      this.buttonFilterPromoTimeSelection.Name = "buttonFilterPromoTimeSelection";
      this.buttonFilterPromoTimeSelection.Size = new System.Drawing.Size(32, 23);
      this.buttonFilterPromoTimeSelection.TabIndex = 18;
      this.buttonFilterPromoTimeSelection.Text = "...";
      this.buttonFilterPromoTimeSelection.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(8, 491);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 36;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.Checked = true;
      this.radioButton1.Location = new System.Drawing.Point(10, 20);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(95, 17);
      this.radioButton1.TabIndex = 33;
      this.radioButton1.Text = "Semua Barang";
      this.radioButton1.UseVisualStyleBackColor = true;
      // 
      // PromoPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.Name = "PromoPage";
      this.Size = new System.Drawing.Size(561, 535);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBoxPromoType.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPageDiscount.ResumeLayout(false);
      this.tabPageDiscount.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiscountPercent)).EndInit();
      this.tabPageFreeItem.ResumeLayout(false);
      this.tabPageFreeItem.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFreeCount)).EndInit();
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
    private System.Windows.Forms.TextBox textBoxDiscountMax;
    private System.Windows.Forms.NumericUpDown numericUpDownDiscountPercent;
    private System.Windows.Forms.CheckBox checkBoxDiscountMaxAmount;
    private System.Windows.Forms.RadioButton radioButtonDiscountRupiah;
    private System.Windows.Forms.RadioButton radioButtonDiscountPercent;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxVoucherPromoSelection;
    private System.Windows.Forms.TextBox textBoxDiscountAmount;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown numericUpDownFreeCount;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxSpecialPrice;
    private System.Windows.Forms.RadioButton radioButtonSelectedItem2;
    private System.Windows.Forms.RadioButton radioButtonApplySelectedItem;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBoxPromoFilter;
    private System.Windows.Forms.GroupBox groupBoxPromoType;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button buttonFilterPromoTimeSelection;
    private System.Windows.Forms.CheckBox checkBoxDateFilter;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.RadioButton radioButton1;
  }
}

namespace InventoryAndSales.GUI.Page
{
  partial class MasterProductPage
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
      this.buttonCancelSave = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      this.groupBoxExportImport = new System.Windows.Forms.GroupBox();
      this.buttonImport = new System.Windows.Forms.Button();
      this.buttonExportItems = new System.Windows.Forms.Button();
      this.groupBoxMasterItemDetail = new System.Windows.Forms.GroupBox();
      this.buttonGenerateCode = new System.Windows.Forms.Button();
      this.textBoxDetailItemBarcode = new System.Windows.Forms.TextBox();
      this.labelBarcode = new System.Windows.Forms.Label();
      this.buttonOkEdit = new System.Windows.Forms.Button();
      this.buttonCancelEdit = new System.Windows.Forms.Button();
      this.labelDiscountRupiah = new System.Windows.Forms.Label();
      this.labelPriceRupiah = new System.Windows.Forms.Label();
      this.labelDiscountPercent = new System.Windows.Forms.Label();
      this.radioButtonDiscountPercent = new System.Windows.Forms.RadioButton();
      this.radioButtonDiscountAmount = new System.Windows.Forms.RadioButton();
      this.textBoxDetailItemDiscountPercent = new System.Windows.Forms.TextBox();
      this.labelDiscount = new System.Windows.Forms.Label();
      this.textBoxDetailItemCode = new System.Windows.Forms.TextBox();
      this.labelPrice = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.labelCode = new System.Windows.Forms.Label();
      this.textBoxDetailItemDiscountAmount = new System.Windows.Forms.TextBox();
      this.textBoxDetailItemName = new System.Windows.Forms.TextBox();
      this.textBoxDetailItemPrice = new System.Windows.Forms.TextBox();
      this.dataGridViewMasterItemList = new System.Windows.Forms.DataGridView();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.buttonUpdate = new System.Windows.Forms.Button();
      this.buttonAdd = new System.Windows.Forms.Button();
      this.comboBoxSort = new System.Windows.Forms.ComboBox();
      this.textBoxSearch = new System.Windows.Forms.TextBox();
      this.labelSort = new System.Windows.Forms.Label();
      this.buttonSearch = new System.Windows.Forms.Button();
      this.groupBoxExportImport.SuspendLayout();
      this.groupBoxMasterItemDetail.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonCancelSave
      // 
      this.buttonCancelSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancelSave.Location = new System.Drawing.Point(805, 576);
      this.buttonCancelSave.Name = "buttonCancelSave";
      this.buttonCancelSave.Size = new System.Drawing.Size(164, 28);
      this.buttonCancelSave.TabIndex = 12;
      this.buttonCancelSave.Text = "Batal";
      this.buttonCancelSave.UseVisualStyleBackColor = true;
      this.buttonCancelSave.Visible = false;
      // 
      // buttonSave
      // 
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(630, 576);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(164, 28);
      this.buttonSave.TabIndex = 11;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Visible = false;
      // 
      // groupBoxExportImport
      // 
      this.groupBoxExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxExportImport.Controls.Add(this.buttonImport);
      this.groupBoxExportImport.Controls.Add(this.buttonExportItems);
      this.groupBoxExportImport.Location = new System.Drawing.Point(608, 278);
      this.groupBoxExportImport.Name = "groupBoxExportImport";
      this.groupBoxExportImport.Size = new System.Drawing.Size(385, 84);
      this.groupBoxExportImport.TabIndex = 14;
      this.groupBoxExportImport.TabStop = false;
      this.groupBoxExportImport.Text = "Export / Import";
      this.groupBoxExportImport.Visible = false;
      // 
      // buttonImport
      // 
      this.buttonImport.Location = new System.Drawing.Point(13, 48);
      this.buttonImport.Name = "buttonImport";
      this.buttonImport.Size = new System.Drawing.Size(222, 23);
      this.buttonImport.TabIndex = 1;
      this.buttonImport.Text = "Update barang dari file";
      this.buttonImport.UseVisualStyleBackColor = true;
      // 
      // buttonExportItems
      // 
      this.buttonExportItems.Location = new System.Drawing.Point(13, 19);
      this.buttonExportItems.Name = "buttonExportItems";
      this.buttonExportItems.Size = new System.Drawing.Size(222, 23);
      this.buttonExportItems.TabIndex = 0;
      this.buttonExportItems.Text = "Simpan barang ke file";
      this.buttonExportItems.UseVisualStyleBackColor = true;
      // 
      // groupBoxMasterItemDetail
      // 
      this.groupBoxMasterItemDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonGenerateCode);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemBarcode);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelBarcode);
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonOkEdit);
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonCancelEdit);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscountRupiah);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelPriceRupiah);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.radioButtonDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.radioButtonDiscountAmount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemCode);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelPrice);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelName);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelCode);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemDiscountAmount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemName);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemPrice);
      this.groupBoxMasterItemDetail.Location = new System.Drawing.Point(608, 15);
      this.groupBoxMasterItemDetail.Name = "groupBoxMasterItemDetail";
      this.groupBoxMasterItemDetail.Size = new System.Drawing.Size(394, 257);
      this.groupBoxMasterItemDetail.TabIndex = 13;
      this.groupBoxMasterItemDetail.TabStop = false;
      this.groupBoxMasterItemDetail.Text = "Detail Barang";
      // 
      // buttonGenerateCode
      // 
      this.buttonGenerateCode.Location = new System.Drawing.Point(186, 89);
      this.buttonGenerateCode.Name = "buttonGenerateCode";
      this.buttonGenerateCode.Size = new System.Drawing.Size(92, 23);
      this.buttonGenerateCode.TabIndex = 20;
      this.buttonGenerateCode.Text = "Generate Code";
      this.buttonGenerateCode.UseVisualStyleBackColor = true;
      this.buttonGenerateCode.Click += new System.EventHandler(this.buttonGenerateCode_Click);
      // 
      // textBoxDetailItemBarcode
      // 
      this.textBoxDetailItemBarcode.Enabled = false;
      this.textBoxDetailItemBarcode.Location = new System.Drawing.Point(76, 58);
      this.textBoxDetailItemBarcode.Name = "textBoxDetailItemBarcode";
      this.textBoxDetailItemBarcode.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemBarcode.TabIndex = 2;
      // 
      // labelBarcode
      // 
      this.labelBarcode.AutoSize = true;
      this.labelBarcode.Location = new System.Drawing.Point(19, 61);
      this.labelBarcode.Name = "labelBarcode";
      this.labelBarcode.Size = new System.Drawing.Size(47, 13);
      this.labelBarcode.TabIndex = 19;
      this.labelBarcode.Text = "Barcode";
      // 
      // buttonOkEdit
      // 
      this.buttonOkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOkEdit.Location = new System.Drawing.Point(227, 222);
      this.buttonOkEdit.Name = "buttonOkEdit";
      this.buttonOkEdit.Size = new System.Drawing.Size(75, 23);
      this.buttonOkEdit.TabIndex = 6;
      this.buttonOkEdit.Text = "OK";
      this.buttonOkEdit.UseVisualStyleBackColor = true;
      this.buttonOkEdit.Click += new System.EventHandler(this.buttonOkEdit_Click);
      // 
      // buttonCancelEdit
      // 
      this.buttonCancelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancelEdit.Location = new System.Drawing.Point(310, 222);
      this.buttonCancelEdit.Name = "buttonCancelEdit";
      this.buttonCancelEdit.Size = new System.Drawing.Size(75, 23);
      this.buttonCancelEdit.TabIndex = 7;
      this.buttonCancelEdit.Text = "Batal";
      this.buttonCancelEdit.UseVisualStyleBackColor = true;
      this.buttonCancelEdit.Click += new System.EventHandler(this.buttonCancelEdit_Click);
      // 
      // labelDiscountRupiah
      // 
      this.labelDiscountRupiah.AutoSize = true;
      this.labelDiscountRupiah.Location = new System.Drawing.Point(182, 160);
      this.labelDiscountRupiah.Name = "labelDiscountRupiah";
      this.labelDiscountRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelDiscountRupiah.TabIndex = 15;
      this.labelDiscountRupiah.Text = "Rupiah";
      // 
      // labelPriceRupiah
      // 
      this.labelPriceRupiah.AutoSize = true;
      this.labelPriceRupiah.Location = new System.Drawing.Point(182, 127);
      this.labelPriceRupiah.Name = "labelPriceRupiah";
      this.labelPriceRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelPriceRupiah.TabIndex = 14;
      this.labelPriceRupiah.Text = "Rupiah";
      // 
      // labelDiscountPercent
      // 
      this.labelDiscountPercent.AutoSize = true;
      this.labelDiscountPercent.Location = new System.Drawing.Point(182, 194);
      this.labelDiscountPercent.Name = "labelDiscountPercent";
      this.labelDiscountPercent.Size = new System.Drawing.Size(15, 13);
      this.labelDiscountPercent.TabIndex = 13;
      this.labelDiscountPercent.Text = "%";
      // 
      // radioButtonDiscountPercent
      // 
      this.radioButtonDiscountPercent.AutoSize = true;
      this.radioButtonDiscountPercent.Enabled = false;
      this.radioButtonDiscountPercent.Location = new System.Drawing.Point(76, 193);
      this.radioButtonDiscountPercent.Name = "radioButtonDiscountPercent";
      this.radioButtonDiscountPercent.Size = new System.Drawing.Size(14, 13);
      this.radioButtonDiscountPercent.TabIndex = 12;
      this.radioButtonDiscountPercent.UseVisualStyleBackColor = true;
      this.radioButtonDiscountPercent.CheckedChanged += new System.EventHandler(this.radioButtonDiscount_CheckedChanged);
      // 
      // radioButtonDiscountAmount
      // 
      this.radioButtonDiscountAmount.AutoSize = true;
      this.radioButtonDiscountAmount.Enabled = false;
      this.radioButtonDiscountAmount.Location = new System.Drawing.Point(76, 160);
      this.radioButtonDiscountAmount.Name = "radioButtonDiscountAmount";
      this.radioButtonDiscountAmount.Size = new System.Drawing.Size(14, 13);
      this.radioButtonDiscountAmount.TabIndex = 11;
      this.radioButtonDiscountAmount.UseVisualStyleBackColor = true;
      this.radioButtonDiscountAmount.CheckedChanged += new System.EventHandler(this.radioButtonDiscount_CheckedChanged);
      // 
      // textBoxDetailItemDiscountPercent
      // 
      this.textBoxDetailItemDiscountPercent.Enabled = false;
      this.textBoxDetailItemDiscountPercent.Location = new System.Drawing.Point(97, 190);
      this.textBoxDetailItemDiscountPercent.Name = "textBoxDetailItemDiscountPercent";
      this.textBoxDetailItemDiscountPercent.Size = new System.Drawing.Size(79, 20);
      this.textBoxDetailItemDiscountPercent.TabIndex = 5;
      this.textBoxDetailItemDiscountPercent.TextChanged += new System.EventHandler(this.textBoxDiscountPercent_TextChanged);
      // 
      // labelDiscount
      // 
      this.labelDiscount.AutoSize = true;
      this.labelDiscount.Location = new System.Drawing.Point(19, 160);
      this.labelDiscount.Name = "labelDiscount";
      this.labelDiscount.Size = new System.Drawing.Size(40, 13);
      this.labelDiscount.TabIndex = 9;
      this.labelDiscount.Text = "Diskon";
      // 
      // textBoxDetailItemCode
      // 
      this.textBoxDetailItemCode.Enabled = false;
      this.textBoxDetailItemCode.Location = new System.Drawing.Point(76, 91);
      this.textBoxDetailItemCode.Name = "textBoxDetailItemCode";
      this.textBoxDetailItemCode.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemCode.TabIndex = 0;
      // 
      // labelPrice
      // 
      this.labelPrice.AutoSize = true;
      this.labelPrice.Location = new System.Drawing.Point(19, 127);
      this.labelPrice.Name = "labelPrice";
      this.labelPrice.Size = new System.Drawing.Size(36, 13);
      this.labelPrice.TabIndex = 8;
      this.labelPrice.Text = "Harga";
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(19, 28);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(35, 13);
      this.labelName.TabIndex = 7;
      this.labelName.Text = "Nama";
      // 
      // labelCode
      // 
      this.labelCode.AutoSize = true;
      this.labelCode.Location = new System.Drawing.Point(19, 94);
      this.labelCode.Name = "labelCode";
      this.labelCode.Size = new System.Drawing.Size(32, 13);
      this.labelCode.TabIndex = 6;
      this.labelCode.Text = "Kode";
      // 
      // textBoxDetailItemDiscountAmount
      // 
      this.textBoxDetailItemDiscountAmount.Enabled = false;
      this.textBoxDetailItemDiscountAmount.Location = new System.Drawing.Point(97, 157);
      this.textBoxDetailItemDiscountAmount.Name = "textBoxDetailItemDiscountAmount";
      this.textBoxDetailItemDiscountAmount.Size = new System.Drawing.Size(79, 20);
      this.textBoxDetailItemDiscountAmount.TabIndex = 4;
      this.textBoxDetailItemDiscountAmount.TextChanged += new System.EventHandler(this.textBoxDiscountAmount_TextChanged);
      // 
      // textBoxDetailItemName
      // 
      this.textBoxDetailItemName.Enabled = false;
      this.textBoxDetailItemName.Location = new System.Drawing.Point(76, 25);
      this.textBoxDetailItemName.Name = "textBoxDetailItemName";
      this.textBoxDetailItemName.Size = new System.Drawing.Size(285, 20);
      this.textBoxDetailItemName.TabIndex = 1;
      // 
      // textBoxDetailItemPrice
      // 
      this.textBoxDetailItemPrice.Enabled = false;
      this.textBoxDetailItemPrice.Location = new System.Drawing.Point(76, 124);
      this.textBoxDetailItemPrice.Name = "textBoxDetailItemPrice";
      this.textBoxDetailItemPrice.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemPrice.TabIndex = 3;
      // 
      // dataGridViewMasterItemList
      // 
      this.dataGridViewMasterItemList.AllowUserToAddRows = false;
      this.dataGridViewMasterItemList.AllowUserToDeleteRows = false;
      this.dataGridViewMasterItemList.AllowUserToOrderColumns = true;
      this.dataGridViewMasterItemList.AllowUserToResizeRows = false;
      this.dataGridViewMasterItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewMasterItemList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewMasterItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMasterItemList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
      this.dataGridViewMasterItemList.Enabled = false;
      this.dataGridViewMasterItemList.Location = new System.Drawing.Point(18, 43);
      this.dataGridViewMasterItemList.MultiSelect = false;
      this.dataGridViewMasterItemList.Name = "dataGridViewMasterItemList";
      this.dataGridViewMasterItemList.RowHeadersVisible = false;
      this.dataGridViewMasterItemList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      this.dataGridViewMasterItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewMasterItemList.Size = new System.Drawing.Size(505, 579);
      this.dataGridViewMasterItemList.TabIndex = 9;
      this.dataGridViewMasterItemList.Click += new System.EventHandler(this.dataGridViewMasterItemList_Click);
      // 
      // buttonDelete
      // 
      this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDelete.BackgroundImage = global::InventoryAndSales.Properties.Resources.Remove;
      this.buttonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonDelete.Location = new System.Drawing.Point(546, 121);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(40, 40);
      this.buttonDelete.TabIndex = 10;
      this.buttonDelete.UseVisualStyleBackColor = true;
      this.buttonDelete.Click += new System.EventHandler(this.buttonDeleteProduct_Click);
      // 
      // buttonUpdate
      // 
      this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonUpdate.BackgroundImage = global::InventoryAndSales.Properties.Resources.Edit;
      this.buttonUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonUpdate.Location = new System.Drawing.Point(546, 69);
      this.buttonUpdate.Name = "buttonUpdate";
      this.buttonUpdate.Size = new System.Drawing.Size(40, 40);
      this.buttonUpdate.TabIndex = 16;
      this.buttonUpdate.UseVisualStyleBackColor = true;
      this.buttonUpdate.Click += new System.EventHandler(this.buttonEditProduct_Click);
      // 
      // buttonAdd
      // 
      this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAdd.BackgroundImage = global::InventoryAndSales.Properties.Resources.Add;
      this.buttonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonAdd.Location = new System.Drawing.Point(546, 16);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new System.Drawing.Size(40, 40);
      this.buttonAdd.TabIndex = 15;
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new System.EventHandler(this.buttonAddProduct_Click);
      // 
      // comboBoxSort
      // 
      this.comboBoxSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSort.FormattingEnabled = true;
      this.comboBoxSort.Location = new System.Drawing.Point(402, 16);
      this.comboBoxSort.Name = "comboBoxSort";
      this.comboBoxSort.Size = new System.Drawing.Size(121, 21);
      this.comboBoxSort.TabIndex = 17;
      this.comboBoxSort.SelectedIndexChanged += new System.EventHandler(this.comboBoxSort_SelectedIndexChanged);
      // 
      // textBoxSearch
      // 
      this.textBoxSearch.Location = new System.Drawing.Point(18, 15);
      this.textBoxSearch.Name = "textBoxSearch";
      this.textBoxSearch.Size = new System.Drawing.Size(238, 20);
      this.textBoxSearch.TabIndex = 18;
      this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
      // 
      // labelSort
      // 
      this.labelSort.AutoSize = true;
      this.labelSort.Location = new System.Drawing.Point(351, 19);
      this.labelSort.Name = "labelSort";
      this.labelSort.Size = new System.Drawing.Size(45, 13);
      this.labelSort.TabIndex = 20;
      this.labelSort.Text = "Urutkan";
      // 
      // buttonSearch
      // 
      this.buttonSearch.Location = new System.Drawing.Point(262, 14);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(75, 23);
      this.buttonSearch.TabIndex = 21;
      this.buttonSearch.Text = "Cari";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
      // 
      // MasterProductPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonSearch);
      this.Controls.Add(this.labelSort);
      this.Controls.Add(this.textBoxSearch);
      this.Controls.Add(this.comboBoxSort);
      this.Controls.Add(this.buttonUpdate);
      this.Controls.Add(this.buttonAdd);
      this.Controls.Add(this.buttonCancelSave);
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.groupBoxExportImport);
      this.Controls.Add(this.groupBoxMasterItemDetail);
      this.Controls.Add(this.dataGridViewMasterItemList);
      this.Controls.Add(this.buttonDelete);
      this.Name = "MasterProductPage";
      this.Size = new System.Drawing.Size(1005, 637);
      this.groupBoxExportImport.ResumeLayout(false);
      this.groupBoxMasterItemDetail.ResumeLayout(false);
      this.groupBoxMasterItemDetail.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancelSave;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.GroupBox groupBoxExportImport;
    private System.Windows.Forms.Button buttonImport;
    private System.Windows.Forms.Button buttonExportItems;
    private System.Windows.Forms.GroupBox groupBoxMasterItemDetail;
    private System.Windows.Forms.Button buttonGenerateCode;
    private System.Windows.Forms.TextBox textBoxDetailItemBarcode;
    private System.Windows.Forms.Label labelBarcode;
    private System.Windows.Forms.Button buttonOkEdit;
    private System.Windows.Forms.Button buttonCancelEdit;
    private System.Windows.Forms.Label labelDiscountRupiah;
    private System.Windows.Forms.Label labelPriceRupiah;
    private System.Windows.Forms.Label labelDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountAmount;
    private System.Windows.Forms.TextBox textBoxDetailItemDiscountPercent;
    private System.Windows.Forms.Label labelDiscount;
    private System.Windows.Forms.TextBox textBoxDetailItemCode;
    private System.Windows.Forms.Label labelPrice;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.Label labelCode;
    private System.Windows.Forms.TextBox textBoxDetailItemDiscountAmount;
    private System.Windows.Forms.TextBox textBoxDetailItemName;
    private System.Windows.Forms.TextBox textBoxDetailItemPrice;
    private System.Windows.Forms.DataGridView dataGridViewMasterItemList;
    private System.Windows.Forms.Button buttonDelete;
    private System.Windows.Forms.Button buttonUpdate;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.ComboBox comboBoxSort;
    private System.Windows.Forms.TextBox textBoxSearch;
    private System.Windows.Forms.Label labelSort;
    private System.Windows.Forms.Button buttonSearch;
  }
}

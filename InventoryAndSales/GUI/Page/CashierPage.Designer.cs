namespace InventoryAndSales.GUI.Page
{
  partial class CashierPage
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.groupBoxCart = new System.Windows.Forms.GroupBox();
      this.dataGridViewCart = new System.Windows.Forms.DataGridView();
      this.CartItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBoxSummary = new System.Windows.Forms.GroupBox();
      this.buttonClearCart = new System.Windows.Forms.Button();
      this.labelNotes = new System.Windows.Forms.Label();
      this.textBoxNotes = new System.Windows.Forms.TextBox();
      this.textBoxChanges = new System.Windows.Forms.TextBox();
      this.textBoxPayment = new System.Windows.Forms.TextBox();
      this.textBoxTotal = new System.Windows.Forms.TextBox();
      this.labelChanges = new System.Windows.Forms.Label();
      this.labelPayment = new System.Windows.Forms.Label();
      this.buttonCheckout = new System.Windows.Forms.Button();
      this.labelTotal = new System.Windows.Forms.Label();
      this.groupBoxItemList = new System.Windows.Forms.GroupBox();
      this.dataGridViewItemList = new System.Windows.Forms.DataGridView();
      this.ItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemAdd = new System.Windows.Forms.DataGridViewButtonColumn();
      this.panelFilter = new System.Windows.Forms.Panel();
      this.labelFilter = new System.Windows.Forms.Label();
      this.textBoxFilter = new System.Windows.Forms.TextBox();
      this.groupBoxCart.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCart)).BeginInit();
      this.groupBoxSummary.SuspendLayout();
      this.groupBoxItemList.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemList)).BeginInit();
      this.panelFilter.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxCart
      // 
      this.groupBoxCart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxCart.Controls.Add(this.dataGridViewCart);
      this.groupBoxCart.Location = new System.Drawing.Point(432, 0);
      this.groupBoxCart.Name = "groupBoxCart";
      this.groupBoxCart.Size = new System.Drawing.Size(633, 365);
      this.groupBoxCart.TabIndex = 5;
      this.groupBoxCart.TabStop = false;
      this.groupBoxCart.Text = "Keranjang Belanja";
      // 
      // dataGridViewCart
      // 
      this.dataGridViewCart.AllowUserToAddRows = false;
      this.dataGridViewCart.AllowUserToResizeRows = false;
      this.dataGridViewCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewCart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CartItemCode,
            this.CartItemName,
            this.CartItemQuantity,
            this.CartItemPrice,
            this.CartItemDiscount,
            this.CartItemSubtotal});
      this.dataGridViewCart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewCart.Location = new System.Drawing.Point(3, 16);
      this.dataGridViewCart.Name = "dataGridViewCart";
      this.dataGridViewCart.RowHeadersVisible = false;
      this.dataGridViewCart.Size = new System.Drawing.Size(627, 346);
      this.dataGridViewCart.TabIndex = 0;
      this.dataGridViewCart.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCart_CellValueChanged);
      this.dataGridViewCart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCart_CellContentClick);
      // 
      // CartItemCode
      // 
      this.CartItemCode.HeaderText = "Kode";
      this.CartItemCode.Name = "CartItemCode";
      this.CartItemCode.ReadOnly = true;
      this.CartItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemCode.Width = 45;
      // 
      // CartItemName
      // 
      this.CartItemName.FillWeight = 200F;
      this.CartItemName.HeaderText = "Nama Barang";
      this.CartItemName.Name = "CartItemName";
      this.CartItemName.ReadOnly = true;
      this.CartItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemName.Width = 200;
      // 
      // CartItemQuantity
      // 
      this.CartItemQuantity.FillWeight = 45F;
      this.CartItemQuantity.HeaderText = "Bnyk";
      this.CartItemQuantity.Name = "CartItemQuantity";
      this.CartItemQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemQuantity.Width = 45;
      // 
      // CartItemPrice
      // 
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemPrice.DefaultCellStyle = dataGridViewCellStyle1;
      this.CartItemPrice.FillWeight = 120F;
      this.CartItemPrice.HeaderText = "Harga Barang";
      this.CartItemPrice.Name = "CartItemPrice";
      this.CartItemPrice.ReadOnly = true;
      this.CartItemPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemPrice.Width = 120;
      // 
      // CartItemDiscount
      // 
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemDiscount.DefaultCellStyle = dataGridViewCellStyle2;
      this.CartItemDiscount.FillWeight = 120F;
      this.CartItemDiscount.HeaderText = "Diskon";
      this.CartItemDiscount.Name = "CartItemDiscount";
      this.CartItemDiscount.ReadOnly = true;
      this.CartItemDiscount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemDiscount.Width = 120;
      // 
      // CartItemSubtotal
      // 
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemSubtotal.DefaultCellStyle = dataGridViewCellStyle3;
      this.CartItemSubtotal.FillWeight = 120F;
      this.CartItemSubtotal.HeaderText = "Total";
      this.CartItemSubtotal.Name = "CartItemSubtotal";
      this.CartItemSubtotal.ReadOnly = true;
      this.CartItemSubtotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemSubtotal.Width = 120;
      // 
      // groupBoxSummary
      // 
      this.groupBoxSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSummary.Controls.Add(this.buttonClearCart);
      this.groupBoxSummary.Controls.Add(this.labelNotes);
      this.groupBoxSummary.Controls.Add(this.textBoxNotes);
      this.groupBoxSummary.Controls.Add(this.textBoxChanges);
      this.groupBoxSummary.Controls.Add(this.textBoxPayment);
      this.groupBoxSummary.Controls.Add(this.textBoxTotal);
      this.groupBoxSummary.Controls.Add(this.labelChanges);
      this.groupBoxSummary.Controls.Add(this.labelPayment);
      this.groupBoxSummary.Controls.Add(this.buttonCheckout);
      this.groupBoxSummary.Controls.Add(this.labelTotal);
      this.groupBoxSummary.Location = new System.Drawing.Point(432, 371);
      this.groupBoxSummary.Name = "groupBoxSummary";
      this.groupBoxSummary.Size = new System.Drawing.Size(630, 135);
      this.groupBoxSummary.TabIndex = 4;
      this.groupBoxSummary.TabStop = false;
      this.groupBoxSummary.Text = "Total";
      // 
      // buttonClearCart
      // 
      this.buttonClearCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClearCart.Location = new System.Drawing.Point(481, 31);
      this.buttonClearCart.Name = "buttonClearCart";
      this.buttonClearCart.Size = new System.Drawing.Size(130, 23);
      this.buttonClearCart.TabIndex = 2;
      this.buttonClearCart.Text = "Bersihkan";
      this.buttonClearCart.UseVisualStyleBackColor = true;
      this.buttonClearCart.Click += new System.EventHandler(this.buttonClearCart_Click);
      // 
      // labelNotes
      // 
      this.labelNotes.AutoSize = true;
      this.labelNotes.Location = new System.Drawing.Point(12, 31);
      this.labelNotes.Name = "labelNotes";
      this.labelNotes.Size = new System.Drawing.Size(47, 13);
      this.labelNotes.TabIndex = 8;
      this.labelNotes.Text = "Catatan:";
      // 
      // textBoxNotes
      // 
      this.textBoxNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxNotes.Location = new System.Drawing.Point(15, 54);
      this.textBoxNotes.Multiline = true;
      this.textBoxNotes.Name = "textBoxNotes";
      this.textBoxNotes.Size = new System.Drawing.Size(174, 63);
      this.textBoxNotes.TabIndex = 0;
      // 
      // textBoxChanges
      // 
      this.textBoxChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxChanges.Location = new System.Drawing.Point(272, 93);
      this.textBoxChanges.Name = "textBoxChanges";
      this.textBoxChanges.ReadOnly = true;
      this.textBoxChanges.Size = new System.Drawing.Size(183, 20);
      this.textBoxChanges.TabIndex = 6;
      // 
      // textBoxPayment
      // 
      this.textBoxPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPayment.Location = new System.Drawing.Point(272, 64);
      this.textBoxPayment.Name = "textBoxPayment";
      this.textBoxPayment.Size = new System.Drawing.Size(183, 20);
      this.textBoxPayment.TabIndex = 1;
      this.textBoxPayment.TextChanged += new System.EventHandler(this.textBoxPayment_TextChanged);
      this.textBoxPayment.Click += new System.EventHandler(this.textBoxPayment_Click);
      this.textBoxPayment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPayment_KeyUp);
      // 
      // textBoxTotal
      // 
      this.textBoxTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTotal.Location = new System.Drawing.Point(272, 31);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new System.Drawing.Size(183, 20);
      this.textBoxTotal.TabIndex = 4;
      // 
      // labelChanges
      // 
      this.labelChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelChanges.AutoSize = true;
      this.labelChanges.Location = new System.Drawing.Point(210, 96);
      this.labelChanges.Name = "labelChanges";
      this.labelChanges.Size = new System.Drawing.Size(44, 13);
      this.labelChanges.TabIndex = 3;
      this.labelChanges.Text = "Kembali";
      // 
      // labelPayment
      // 
      this.labelPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelPayment.AutoSize = true;
      this.labelPayment.Location = new System.Drawing.Point(210, 67);
      this.labelPayment.Name = "labelPayment";
      this.labelPayment.Size = new System.Drawing.Size(55, 13);
      this.labelPayment.TabIndex = 2;
      this.labelPayment.Text = "Bayar (F6)";
      // 
      // buttonCheckout
      // 
      this.buttonCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCheckout.Location = new System.Drawing.Point(481, 67);
      this.buttonCheckout.Name = "buttonCheckout";
      this.buttonCheckout.Size = new System.Drawing.Size(130, 50);
      this.buttonCheckout.TabIndex = 3;
      this.buttonCheckout.Text = "Bayar (F7)";
      this.buttonCheckout.UseVisualStyleBackColor = true;
      this.buttonCheckout.Click += new System.EventHandler(this.buttonCheckout_Click);
      // 
      // labelTotal
      // 
      this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelTotal.AutoSize = true;
      this.labelTotal.Location = new System.Drawing.Point(210, 38);
      this.labelTotal.Name = "labelTotal";
      this.labelTotal.Size = new System.Drawing.Size(31, 13);
      this.labelTotal.TabIndex = 0;
      this.labelTotal.Text = "Total";
      // 
      // groupBoxItemList
      // 
      this.groupBoxItemList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBoxItemList.Controls.Add(this.dataGridViewItemList);
      this.groupBoxItemList.Controls.Add(this.panelFilter);
      this.groupBoxItemList.Location = new System.Drawing.Point(3, 0);
      this.groupBoxItemList.Name = "groupBoxItemList";
      this.groupBoxItemList.Size = new System.Drawing.Size(423, 506);
      this.groupBoxItemList.TabIndex = 3;
      this.groupBoxItemList.TabStop = false;
      this.groupBoxItemList.Text = "Daftar Barang";
      // 
      // dataGridViewItemList
      // 
      this.dataGridViewItemList.AllowUserToAddRows = false;
      this.dataGridViewItemList.AllowUserToResizeRows = false;
      this.dataGridViewItemList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridViewItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemCode,
            this.ItemName,
            this.ItemPrice,
            this.ItemAdd});
      this.dataGridViewItemList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewItemList.Location = new System.Drawing.Point(3, 59);
      this.dataGridViewItemList.MultiSelect = false;
      this.dataGridViewItemList.Name = "dataGridViewItemList";
      this.dataGridViewItemList.RowHeadersVisible = false;
      this.dataGridViewItemList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
      this.dataGridViewItemList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dataGridViewItemList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewItemList.Size = new System.Drawing.Size(417, 444);
      this.dataGridViewItemList.TabIndex = 1;
      this.dataGridViewItemList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewItemList_KeyPress);
      this.dataGridViewItemList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewItemList_CellContentClick);
      // 
      // ItemCode
      // 
      this.ItemCode.FillWeight = 20F;
      this.ItemCode.HeaderText = "Kode";
      this.ItemCode.Name = "ItemCode";
      this.ItemCode.ReadOnly = true;
      this.ItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // ItemName
      // 
      this.ItemName.FillWeight = 50F;
      this.ItemName.HeaderText = "Nama Barang";
      this.ItemName.Name = "ItemName";
      this.ItemName.ReadOnly = true;
      this.ItemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // ItemPrice
      // 
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.ItemPrice.DefaultCellStyle = dataGridViewCellStyle4;
      this.ItemPrice.FillWeight = 20F;
      this.ItemPrice.HeaderText = "Harga";
      this.ItemPrice.Name = "ItemPrice";
      this.ItemPrice.ReadOnly = true;
      this.ItemPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // ItemAdd
      // 
      this.ItemAdd.FillWeight = 10F;
      this.ItemAdd.HeaderText = "Pilih";
      this.ItemAdd.Name = "ItemAdd";
      this.ItemAdd.ReadOnly = true;
      // 
      // panelFilter
      // 
      this.panelFilter.Controls.Add(this.labelFilter);
      this.panelFilter.Controls.Add(this.textBoxFilter);
      this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelFilter.Location = new System.Drawing.Point(3, 16);
      this.panelFilter.Name = "panelFilter";
      this.panelFilter.Size = new System.Drawing.Size(417, 43);
      this.panelFilter.TabIndex = 0;
      // 
      // labelFilter
      // 
      this.labelFilter.AutoSize = true;
      this.labelFilter.Location = new System.Drawing.Point(3, 17);
      this.labelFilter.Name = "labelFilter";
      this.labelFilter.Size = new System.Drawing.Size(79, 13);
      this.labelFilter.TabIndex = 2;
      this.labelFilter.Text = "Pencarian (F5):";
      // 
      // textBoxFilter
      // 
      this.textBoxFilter.Location = new System.Drawing.Point(88, 14);
      this.textBoxFilter.Name = "textBoxFilter";
      this.textBoxFilter.Size = new System.Drawing.Size(259, 20);
      this.textBoxFilter.TabIndex = 0;
      this.textBoxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyUp);
      this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
      // 
      // CashierPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBoxCart);
      this.Controls.Add(this.groupBoxSummary);
      this.Controls.Add(this.groupBoxItemList);
      this.Name = "CashierPage";
      this.Size = new System.Drawing.Size(1068, 509);
      this.groupBoxCart.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCart)).EndInit();
      this.groupBoxSummary.ResumeLayout(false);
      this.groupBoxSummary.PerformLayout();
      this.groupBoxItemList.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemList)).EndInit();
      this.panelFilter.ResumeLayout(false);
      this.panelFilter.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxCart;
    private System.Windows.Forms.DataGridView dataGridViewCart;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemQuantity;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemDiscount;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemSubtotal;
    private System.Windows.Forms.GroupBox groupBoxSummary;
    private System.Windows.Forms.Button buttonClearCart;
    private System.Windows.Forms.Label labelNotes;
    private System.Windows.Forms.TextBox textBoxNotes;
    private System.Windows.Forms.TextBox textBoxChanges;
    private System.Windows.Forms.TextBox textBoxPayment;
    private System.Windows.Forms.TextBox textBoxTotal;
    private System.Windows.Forms.Label labelChanges;
    private System.Windows.Forms.Label labelPayment;
    private System.Windows.Forms.Button buttonCheckout;
    private System.Windows.Forms.Label labelTotal;
    private System.Windows.Forms.GroupBox groupBoxItemList;
    private System.Windows.Forms.DataGridView dataGridViewItemList;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemPrice;
    private System.Windows.Forms.DataGridViewButtonColumn ItemAdd;
    private System.Windows.Forms.Panel panelFilter;
    private System.Windows.Forms.Label labelFilter;
    private System.Windows.Forms.TextBox textBoxFilter;

  }
}

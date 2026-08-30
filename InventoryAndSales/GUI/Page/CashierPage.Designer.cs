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
      this.tableLayoutSummary = new System.Windows.Forms.TableLayoutPanel();
      this.panelNotes = new System.Windows.Forms.Panel();
      this.tableLayoutFields = new System.Windows.Forms.TableLayoutPanel();
      this.panelActions = new System.Windows.Forms.Panel();
      this.comboBoxHeldCart = new System.Windows.Forms.ComboBox();
      this.buttonHoldCart = new System.Windows.Forms.Button();
      this.buttonRecallCart = new System.Windows.Forms.Button();
      this.buttonDiscardHeldCart = new System.Windows.Forms.Button();
      this.buttonClearCart = new System.Windows.Forms.Button();
      this.labelNotes = new System.Windows.Forms.Label();
      this.textBoxNotes = new System.Windows.Forms.TextBox();
      this.textBoxChanges = new System.Windows.Forms.TextBox();
      this.textBoxPayment = new System.Windows.Forms.TextBox();
      this.textBoxTotal = new System.Windows.Forms.TextBox();
      this.labelChanges = new System.Windows.Forms.Label();
      this.labelPayment = new System.Windows.Forms.Label();
      this.labelPaymentMethod = new System.Windows.Forms.Label();
      this.comboBoxPaymentMethod = new System.Windows.Forms.ComboBox();
      this.labelReference = new System.Windows.Forms.Label();
      this.comboBoxReference = new System.Windows.Forms.ComboBox();
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
      this.tableLayoutSummary.SuspendLayout();
      this.panelNotes.SuspendLayout();
      this.tableLayoutFields.SuspendLayout();
      this.panelActions.SuspendLayout();
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
      this.groupBoxCart.Size = new System.Drawing.Size(633, 300);
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
      this.dataGridViewCart.Size = new System.Drawing.Size(627, 281);
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
      this.groupBoxSummary.Controls.Add(this.tableLayoutSummary);
      this.groupBoxSummary.Location = new System.Drawing.Point(432, 306);
      this.groupBoxSummary.Name = "groupBoxSummary";
      this.groupBoxSummary.Size = new System.Drawing.Size(633, 200);
      this.groupBoxSummary.TabIndex = 4;
      this.groupBoxSummary.TabStop = false;
      this.groupBoxSummary.Text = "Total";
      //
      // tableLayoutSummary
      //
      // Three columns: the notes take whatever width is left over, the payment fields and the
      // action buttons keep their own width and stay together against the right edge.
      this.tableLayoutSummary.ColumnCount = 3;
      this.tableLayoutSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
      this.tableLayoutSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
      this.tableLayoutSummary.Controls.Add(this.panelNotes, 0, 0);
      this.tableLayoutSummary.Controls.Add(this.tableLayoutFields, 1, 0);
      this.tableLayoutSummary.Controls.Add(this.panelActions, 2, 0);
      this.tableLayoutSummary.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutSummary.Location = new System.Drawing.Point(3, 16);
      this.tableLayoutSummary.Name = "tableLayoutSummary";
      this.tableLayoutSummary.Padding = new System.Windows.Forms.Padding(9, 3, 9, 6);
      this.tableLayoutSummary.RowCount = 1;
      this.tableLayoutSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutSummary.Size = new System.Drawing.Size(627, 181);
      this.tableLayoutSummary.TabIndex = 0;
      //
      // panelNotes
      //
      this.panelNotes.Controls.Add(this.textBoxNotes);
      this.panelNotes.Controls.Add(this.labelNotes);
      this.panelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelNotes.Location = new System.Drawing.Point(9, 3);
      this.panelNotes.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
      this.panelNotes.Name = "panelNotes";
      this.panelNotes.Size = new System.Drawing.Size(197, 172);
      this.panelNotes.TabIndex = 0;
      //
      // labelNotes
      //
      this.labelNotes.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelNotes.Location = new System.Drawing.Point(0, 0);
      this.labelNotes.Name = "labelNotes";
      this.labelNotes.Size = new System.Drawing.Size(197, 18);
      this.labelNotes.TabIndex = 0;
      this.labelNotes.Text = "Catatan:";
      this.labelNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      //
      // textBoxNotes
      //
      this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxNotes.Location = new System.Drawing.Point(0, 18);
      this.textBoxNotes.Multiline = true;
      this.textBoxNotes.Name = "textBoxNotes";
      this.textBoxNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxNotes.Size = new System.Drawing.Size(197, 154);
      this.textBoxNotes.TabIndex = 1;
      //
      // tableLayoutFields
      //
      // One row per field, so the labels and the boxes line up whatever the window width. The
      // terminal row is collapsed to nothing by ApplyPaymentMethod when it does not apply.
      this.tableLayoutFields.AutoSize = true;
      this.tableLayoutFields.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.tableLayoutFields.ColumnCount = 2;
      this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
      this.tableLayoutFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
      this.tableLayoutFields.Controls.Add(this.labelPaymentMethod, 0, 0);
      this.tableLayoutFields.Controls.Add(this.comboBoxPaymentMethod, 1, 0);
      this.tableLayoutFields.Controls.Add(this.labelReference, 0, 1);
      this.tableLayoutFields.Controls.Add(this.comboBoxReference, 1, 1);
      this.tableLayoutFields.Controls.Add(this.labelTotal, 0, 2);
      this.tableLayoutFields.Controls.Add(this.textBoxTotal, 1, 2);
      this.tableLayoutFields.Controls.Add(this.labelPayment, 0, 3);
      this.tableLayoutFields.Controls.Add(this.textBoxPayment, 1, 3);
      this.tableLayoutFields.Controls.Add(this.labelChanges, 0, 4);
      this.tableLayoutFields.Controls.Add(this.textBoxChanges, 1, 4);
      this.tableLayoutFields.Location = new System.Drawing.Point(218, 3);
      this.tableLayoutFields.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
      this.tableLayoutFields.Name = "tableLayoutFields";
      this.tableLayoutFields.RowCount = 5;
      this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
      this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
      this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
      this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
      this.tableLayoutFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
      this.tableLayoutFields.Size = new System.Drawing.Size(255, 135);
      this.tableLayoutFields.TabIndex = 1;
      //
      // labelPaymentMethod
      //
      this.labelPaymentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelPaymentMethod.Location = new System.Drawing.Point(0, 0);
      this.labelPaymentMethod.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
      this.labelPaymentMethod.Name = "labelPaymentMethod";
      this.labelPaymentMethod.Size = new System.Drawing.Size(66, 27);
      this.labelPaymentMethod.TabIndex = 0;
      this.labelPaymentMethod.Text = "Metode";
      this.labelPaymentMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      //
      // comboBoxPaymentMethod
      //
      this.comboBoxPaymentMethod.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.comboBoxPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPaymentMethod.Location = new System.Drawing.Point(72, 3);
      this.comboBoxPaymentMethod.Margin = new System.Windows.Forms.Padding(0);
      this.comboBoxPaymentMethod.Name = "comboBoxPaymentMethod";
      this.comboBoxPaymentMethod.Size = new System.Drawing.Size(183, 21);
      this.comboBoxPaymentMethod.TabIndex = 1;
      this.comboBoxPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxPaymentMethod_SelectedIndexChanged);
      //
      // labelReference
      //
      this.labelReference.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelReference.Location = new System.Drawing.Point(0, 27);
      this.labelReference.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
      this.labelReference.Name = "labelReference";
      this.labelReference.Size = new System.Drawing.Size(66, 27);
      this.labelReference.TabIndex = 2;
      this.labelReference.Text = "Terminal";
      this.labelReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      //
      // comboBoxReference
      //
      this.comboBoxReference.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.comboBoxReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxReference.Location = new System.Drawing.Point(72, 30);
      this.comboBoxReference.Margin = new System.Windows.Forms.Padding(0);
      this.comboBoxReference.Name = "comboBoxReference";
      this.comboBoxReference.Size = new System.Drawing.Size(183, 21);
      this.comboBoxReference.TabIndex = 3;
      //
      // labelTotal
      //
      this.labelTotal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelTotal.Location = new System.Drawing.Point(0, 54);
      this.labelTotal.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
      this.labelTotal.Name = "labelTotal";
      this.labelTotal.Size = new System.Drawing.Size(66, 27);
      this.labelTotal.TabIndex = 4;
      this.labelTotal.Text = "Total";
      this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      //
      // textBoxTotal
      //
      this.textBoxTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.textBoxTotal.Location = new System.Drawing.Point(72, 57);
      this.textBoxTotal.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new System.Drawing.Size(183, 20);
      this.textBoxTotal.TabIndex = 5;
      this.textBoxTotal.TabStop = false;
      this.textBoxTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      //
      // labelPayment
      //
      this.labelPayment.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelPayment.Location = new System.Drawing.Point(0, 81);
      this.labelPayment.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
      this.labelPayment.Name = "labelPayment";
      this.labelPayment.Size = new System.Drawing.Size(66, 27);
      this.labelPayment.TabIndex = 6;
      this.labelPayment.Text = "Bayar (F6)";
      this.labelPayment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      //
      // textBoxPayment
      //
      this.textBoxPayment.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.textBoxPayment.Location = new System.Drawing.Point(72, 84);
      this.textBoxPayment.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxPayment.Name = "textBoxPayment";
      this.textBoxPayment.Size = new System.Drawing.Size(183, 20);
      this.textBoxPayment.TabIndex = 7;
      this.textBoxPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxPayment.TextChanged += new System.EventHandler(this.textBoxPayment_TextChanged);
      this.textBoxPayment.Click += new System.EventHandler(this.textBoxPayment_Click);
      this.textBoxPayment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPayment_KeyUp);
      //
      // labelChanges
      //
      this.labelChanges.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelChanges.Location = new System.Drawing.Point(0, 108);
      this.labelChanges.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
      this.labelChanges.Name = "labelChanges";
      this.labelChanges.Size = new System.Drawing.Size(66, 27);
      this.labelChanges.TabIndex = 8;
      this.labelChanges.Text = "Kembali";
      this.labelChanges.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      //
      // textBoxChanges
      //
      this.textBoxChanges.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.textBoxChanges.Location = new System.Drawing.Point(72, 111);
      this.textBoxChanges.Margin = new System.Windows.Forms.Padding(0);
      this.textBoxChanges.Name = "textBoxChanges";
      this.textBoxChanges.ReadOnly = true;
      this.textBoxChanges.Size = new System.Drawing.Size(183, 20);
      this.textBoxChanges.TabIndex = 9;
      this.textBoxChanges.TabStop = false;
      this.textBoxChanges.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      //
      // panelActions
      //
      this.panelActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
      this.panelActions.Controls.Add(this.comboBoxHeldCart);
      this.panelActions.Controls.Add(this.buttonHoldCart);
      this.panelActions.Controls.Add(this.buttonRecallCart);
      this.panelActions.Controls.Add(this.buttonDiscardHeldCart);
      this.panelActions.Controls.Add(this.buttonClearCart);
      this.panelActions.Controls.Add(this.buttonCheckout);
      this.panelActions.Location = new System.Drawing.Point(485, 3);
      this.panelActions.Margin = new System.Windows.Forms.Padding(0);
      this.panelActions.Name = "panelActions";
      this.panelActions.Size = new System.Drawing.Size(133, 172);
      this.panelActions.TabIndex = 2;
      //
      // comboBoxHeldCart
      //
      this.comboBoxHeldCart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxHeldCart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxHeldCart.Location = new System.Drawing.Point(0, 0);
      this.comboBoxHeldCart.Name = "comboBoxHeldCart";
      this.comboBoxHeldCart.Size = new System.Drawing.Size(133, 21);
      this.comboBoxHeldCart.TabIndex = 0;
      //
      // buttonHoldCart
      //
      this.buttonHoldCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHoldCart.Location = new System.Drawing.Point(0, 27);
      this.buttonHoldCart.Name = "buttonHoldCart";
      this.buttonHoldCart.Size = new System.Drawing.Size(63, 23);
      this.buttonHoldCart.TabIndex = 1;
      this.buttonHoldCart.Text = "Simpan";
      this.buttonHoldCart.UseVisualStyleBackColor = true;
      this.buttonHoldCart.Click += new System.EventHandler(this.buttonHoldCart_Click);
      //
      // buttonRecallCart
      //
      this.buttonRecallCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRecallCart.Location = new System.Drawing.Point(70, 27);
      this.buttonRecallCart.Name = "buttonRecallCart";
      this.buttonRecallCart.Size = new System.Drawing.Size(63, 23);
      this.buttonRecallCart.TabIndex = 2;
      this.buttonRecallCart.Text = "Ambil";
      this.buttonRecallCart.UseVisualStyleBackColor = true;
      this.buttonRecallCart.Click += new System.EventHandler(this.buttonRecallCart_Click);
      //
      // buttonDiscardHeldCart
      //
      this.buttonDiscardHeldCart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDiscardHeldCart.Location = new System.Drawing.Point(0, 56);
      this.buttonDiscardHeldCart.Name = "buttonDiscardHeldCart";
      this.buttonDiscardHeldCart.Size = new System.Drawing.Size(133, 23);
      this.buttonDiscardHeldCart.TabIndex = 3;
      this.buttonDiscardHeldCart.Text = "Hapus Simpanan";
      this.buttonDiscardHeldCart.UseVisualStyleBackColor = true;
      this.buttonDiscardHeldCart.Click += new System.EventHandler(this.buttonDiscardHeldCart_Click);
      //
      // buttonClearCart
      //
      this.buttonClearCart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClearCart.Location = new System.Drawing.Point(0, 85);
      this.buttonClearCart.Name = "buttonClearCart";
      this.buttonClearCart.Size = new System.Drawing.Size(133, 23);
      this.buttonClearCart.TabIndex = 4;
      this.buttonClearCart.Text = "Bersihkan";
      this.buttonClearCart.UseVisualStyleBackColor = true;
      this.buttonClearCart.Click += new System.EventHandler(this.buttonClearCart_Click);
      //
      // buttonCheckout
      //
      this.buttonCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCheckout.Location = new System.Drawing.Point(0, 122);
      this.buttonCheckout.Name = "buttonCheckout";
      this.buttonCheckout.Size = new System.Drawing.Size(133, 50);
      this.buttonCheckout.TabIndex = 5;
      this.buttonCheckout.Text = "Bayar (F7)";
      this.buttonCheckout.UseVisualStyleBackColor = true;
      this.buttonCheckout.Click += new System.EventHandler(this.buttonCheckout_Click);
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
      this.tableLayoutSummary.ResumeLayout(false);
      this.tableLayoutSummary.PerformLayout();
      this.panelNotes.ResumeLayout(false);
      this.panelNotes.PerformLayout();
      this.tableLayoutFields.ResumeLayout(false);
      this.tableLayoutFields.PerformLayout();
      this.panelActions.ResumeLayout(false);
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
    private System.Windows.Forms.TableLayoutPanel tableLayoutSummary;
    private System.Windows.Forms.Panel panelNotes;
    private System.Windows.Forms.TableLayoutPanel tableLayoutFields;
    private System.Windows.Forms.Panel panelActions;
    private System.Windows.Forms.ComboBox comboBoxHeldCart;
    private System.Windows.Forms.Button buttonHoldCart;
    private System.Windows.Forms.Button buttonRecallCart;
    private System.Windows.Forms.Button buttonDiscardHeldCart;
    private System.Windows.Forms.Button buttonClearCart;
    private System.Windows.Forms.Label labelNotes;
    private System.Windows.Forms.TextBox textBoxNotes;
    private System.Windows.Forms.TextBox textBoxChanges;
    private System.Windows.Forms.TextBox textBoxPayment;
    private System.Windows.Forms.TextBox textBoxTotal;
    private System.Windows.Forms.Label labelChanges;
    private System.Windows.Forms.Label labelPayment;
    private System.Windows.Forms.Label labelPaymentMethod;
    private System.Windows.Forms.ComboBox comboBoxPaymentMethod;
    private System.Windows.Forms.Label labelReference;
    private System.Windows.Forms.ComboBox comboBoxReference;
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

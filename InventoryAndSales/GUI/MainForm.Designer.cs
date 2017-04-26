namespace InventoryAndSales.GUI
{
  partial class MainForm
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
      this.menuStripMain = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.daftarBarangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.transaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.penjualanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControlPage = new System.Windows.Forms.TabControl();
      this.tabPageLogin = new System.Windows.Forms.TabPage();
      this.groupBoxLogin = new System.Windows.Forms.GroupBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelUsername = new System.Windows.Forms.Label();
      this.buttonLogin = new System.Windows.Forms.Button();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.textBoxUsername = new System.Windows.Forms.TextBox();
      this.tabPageCashier = new System.Windows.Forms.TabPage();
      this.groupBoxCart = new System.Windows.Forms.GroupBox();
      this.dataGridViewCart = new System.Windows.Forms.DataGridView();
      this.CartItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CartItemSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBoxSummary = new System.Windows.Forms.GroupBox();
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
      this.tabPageInventoryList = new System.Windows.Forms.TabPage();
      this.buttonCancelSave = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      this.groupBoxImport = new System.Windows.Forms.GroupBox();
      this.dataGridViewUploadConfirm = new System.Windows.Forms.DataGridView();
      this.ItemUploadCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemUploadName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemUploadPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemUploadDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ItemUploadStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.buttonBrowseImportFile = new System.Windows.Forms.Button();
      this.buttonImportItems = new System.Windows.Forms.Button();
      this.textBoxFilePathImport = new System.Windows.Forms.TextBox();
      this.groupBoxExport = new System.Windows.Forms.GroupBox();
      this.buttonExportItems = new System.Windows.Forms.Button();
      this.groupBoxMasterItemDetail = new System.Windows.Forms.GroupBox();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.labelDiscountRupiah = new System.Windows.Forms.Label();
      this.labelPriceRupiah = new System.Windows.Forms.Label();
      this.labelDiscountPercent = new System.Windows.Forms.Label();
      this.radioButtonDiscountPercent = new System.Windows.Forms.RadioButton();
      this.radioButtonDiscountAmount = new System.Windows.Forms.RadioButton();
      this.textBoxDetailItemDiscountPercent = new System.Windows.Forms.TextBox();
      this.labelDiscount = new System.Windows.Forms.Label();
      this.textBoxDetailItemCode = new System.Windows.Forms.TextBox();
      this.labelPrice = new System.Windows.Forms.Label();
      this.buttonAdd = new System.Windows.Forms.Button();
      this.labelName = new System.Windows.Forms.Label();
      this.labelCode = new System.Windows.Forms.Label();
      this.buttonUpdate = new System.Windows.Forms.Button();
      this.textBoxDetailItemDiscountAmount = new System.Windows.Forms.TextBox();
      this.textBoxDetailItemName = new System.Windows.Forms.TextBox();
      this.textBoxDetailItemPrice = new System.Windows.Forms.TextBox();
      this.dataGridViewMasterItemList = new System.Windows.Forms.DataGridView();
      this.MasterItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.MasterItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.MasterItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.MasterItemDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.statusStripInformation = new System.Windows.Forms.StatusStrip();
      this.buttonClearCart = new System.Windows.Forms.Button();
      this.menuStripMain.SuspendLayout();
      this.tabControlPage.SuspendLayout();
      this.tabPageLogin.SuspendLayout();
      this.groupBoxLogin.SuspendLayout();
      this.tabPageCashier.SuspendLayout();
      this.groupBoxCart.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCart)).BeginInit();
      this.groupBoxSummary.SuspendLayout();
      this.groupBoxItemList.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemList)).BeginInit();
      this.panelFilter.SuspendLayout();
      this.tabPageInventoryList.SuspendLayout();
      this.groupBoxImport.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUploadConfirm)).BeginInit();
      this.groupBoxExport.SuspendLayout();
      this.groupBoxMasterItemDetail.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).BeginInit();
      this.SuspendLayout();
      // 
      // menuStripMain
      // 
      this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.transaksiToolStripMenuItem});
      this.menuStripMain.Location = new System.Drawing.Point(0, 0);
      this.menuStripMain.Name = "menuStripMain";
      this.menuStripMain.Size = new System.Drawing.Size(1108, 24);
      this.menuStripMain.TabIndex = 0;
      this.menuStripMain.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // loginToolStripMenuItem
      // 
      this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
      this.loginToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
      this.loginToolStripMenuItem.Text = "Logout";
      this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.daftarBarangToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
      this.editToolStripMenuItem.Text = "Edit";
      // 
      // daftarBarangToolStripMenuItem
      // 
      this.daftarBarangToolStripMenuItem.Name = "daftarBarangToolStripMenuItem";
      this.daftarBarangToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
      this.daftarBarangToolStripMenuItem.Text = "Daftar Barang";
      this.daftarBarangToolStripMenuItem.Click += new System.EventHandler(this.daftarBarangToolStripMenuItem_Click);
      // 
      // transaksiToolStripMenuItem
      // 
      this.transaksiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.penjualanToolStripMenuItem});
      this.transaksiToolStripMenuItem.Name = "transaksiToolStripMenuItem";
      this.transaksiToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
      this.transaksiToolStripMenuItem.Text = "Transaksi";
      // 
      // penjualanToolStripMenuItem
      // 
      this.penjualanToolStripMenuItem.Name = "penjualanToolStripMenuItem";
      this.penjualanToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
      this.penjualanToolStripMenuItem.Text = "Penjualan";
      this.penjualanToolStripMenuItem.Click += new System.EventHandler(this.penjualanToolStripMenuItem_Click);
      // 
      // tabControlPage
      // 
      this.tabControlPage.Controls.Add(this.tabPageLogin);
      this.tabControlPage.Controls.Add(this.tabPageCashier);
      this.tabControlPage.Controls.Add(this.tabPageInventoryList);
      this.tabControlPage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlPage.Location = new System.Drawing.Point(0, 24);
      this.tabControlPage.Name = "tabControlPage";
      this.tabControlPage.SelectedIndex = 0;
      this.tabControlPage.Size = new System.Drawing.Size(1108, 683);
      this.tabControlPage.TabIndex = 1;
      // 
      // tabPageLogin
      // 
      this.tabPageLogin.Controls.Add(this.groupBoxLogin);
      this.tabPageLogin.Location = new System.Drawing.Point(4, 22);
      this.tabPageLogin.Name = "tabPageLogin";
      this.tabPageLogin.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageLogin.Size = new System.Drawing.Size(1100, 657);
      this.tabPageLogin.TabIndex = 2;
      this.tabPageLogin.Text = "Login";
      this.tabPageLogin.UseVisualStyleBackColor = true;
      // 
      // groupBoxLogin
      // 
      this.groupBoxLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.groupBoxLogin.Controls.Add(this.labelPassword);
      this.groupBoxLogin.Controls.Add(this.labelUsername);
      this.groupBoxLogin.Controls.Add(this.buttonLogin);
      this.groupBoxLogin.Controls.Add(this.textBoxPassword);
      this.groupBoxLogin.Controls.Add(this.textBoxUsername);
      this.groupBoxLogin.Location = new System.Drawing.Point(372, 235);
      this.groupBoxLogin.Name = "groupBoxLogin";
      this.groupBoxLogin.Size = new System.Drawing.Size(356, 187);
      this.groupBoxLogin.TabIndex = 0;
      this.groupBoxLogin.TabStop = false;
      this.groupBoxLogin.Text = "Please Login";
      // 
      // labelPassword
      // 
      this.labelPassword.AutoSize = true;
      this.labelPassword.Location = new System.Drawing.Point(23, 82);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(53, 13);
      this.labelPassword.TabIndex = 4;
      this.labelPassword.Text = "Password";
      // 
      // labelUsername
      // 
      this.labelUsername.AutoSize = true;
      this.labelUsername.Location = new System.Drawing.Point(23, 50);
      this.labelUsername.Name = "labelUsername";
      this.labelUsername.Size = new System.Drawing.Size(55, 13);
      this.labelUsername.TabIndex = 3;
      this.labelUsername.Text = "Username";
      // 
      // buttonLogin
      // 
      this.buttonLogin.Location = new System.Drawing.Point(207, 139);
      this.buttonLogin.Name = "buttonLogin";
      this.buttonLogin.Size = new System.Drawing.Size(112, 28);
      this.buttonLogin.TabIndex = 2;
      this.buttonLogin.Text = "Login";
      this.buttonLogin.UseVisualStyleBackColor = true;
      this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Location = new System.Drawing.Point(96, 79);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.PasswordChar = '*';
      this.textBoxPassword.Size = new System.Drawing.Size(223, 20);
      this.textBoxPassword.TabIndex = 1;
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Location = new System.Drawing.Point(96, 43);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(223, 20);
      this.textBoxUsername.TabIndex = 0;
      // 
      // tabPageCashier
      // 
      this.tabPageCashier.Controls.Add(this.groupBoxCart);
      this.tabPageCashier.Controls.Add(this.groupBoxSummary);
      this.tabPageCashier.Controls.Add(this.groupBoxItemList);
      this.tabPageCashier.Location = new System.Drawing.Point(4, 22);
      this.tabPageCashier.Name = "tabPageCashier";
      this.tabPageCashier.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageCashier.Size = new System.Drawing.Size(1100, 657);
      this.tabPageCashier.TabIndex = 0;
      this.tabPageCashier.Text = "Kasir";
      this.tabPageCashier.UseVisualStyleBackColor = true;
      // 
      // groupBoxCart
      // 
      this.groupBoxCart.Controls.Add(this.dataGridViewCart);
      this.groupBoxCart.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxCart.Location = new System.Drawing.Point(423, 3);
      this.groupBoxCart.Name = "groupBoxCart";
      this.groupBoxCart.Size = new System.Drawing.Size(674, 516);
      this.groupBoxCart.TabIndex = 2;
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
      this.dataGridViewCart.Size = new System.Drawing.Size(668, 497);
      this.dataGridViewCart.TabIndex = 0;
      // 
      // CartItemCode
      // 
      this.CartItemCode.HeaderText = "Kode";
      this.CartItemCode.Name = "CartItemCode";
      this.CartItemCode.ReadOnly = true;
      this.CartItemCode.Width = 45;
      // 
      // CartItemName
      // 
      this.CartItemName.FillWeight = 200F;
      this.CartItemName.HeaderText = "Nama Barang";
      this.CartItemName.Name = "CartItemName";
      this.CartItemName.ReadOnly = true;
      this.CartItemName.Width = 200;
      // 
      // CartItemQuantity
      // 
      this.CartItemQuantity.FillWeight = 45F;
      this.CartItemQuantity.HeaderText = "Bnyk";
      this.CartItemQuantity.Name = "CartItemQuantity";
      this.CartItemQuantity.Width = 45;
      // 
      // CartItemPrice
      // 
      this.CartItemPrice.FillWeight = 120F;
      this.CartItemPrice.HeaderText = "Harga Barang";
      this.CartItemPrice.Name = "CartItemPrice";
      this.CartItemPrice.ReadOnly = true;
      this.CartItemPrice.Width = 120;
      // 
      // CartItemDiscount
      // 
      this.CartItemDiscount.FillWeight = 120F;
      this.CartItemDiscount.HeaderText = "Diskon";
      this.CartItemDiscount.Name = "CartItemDiscount";
      this.CartItemDiscount.Width = 120;
      // 
      // CartItemSubtotal
      // 
      this.CartItemSubtotal.FillWeight = 120F;
      this.CartItemSubtotal.HeaderText = "Total";
      this.CartItemSubtotal.Name = "CartItemSubtotal";
      this.CartItemSubtotal.ReadOnly = true;
      this.CartItemSubtotal.Width = 120;
      // 
      // groupBoxSummary
      // 
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
      this.groupBoxSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBoxSummary.Location = new System.Drawing.Point(423, 519);
      this.groupBoxSummary.Name = "groupBoxSummary";
      this.groupBoxSummary.Size = new System.Drawing.Size(674, 135);
      this.groupBoxSummary.TabIndex = 1;
      this.groupBoxSummary.TabStop = false;
      this.groupBoxSummary.Text = "Total";
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
      this.textBoxNotes.Size = new System.Drawing.Size(218, 63);
      this.textBoxNotes.TabIndex = 7;
      // 
      // textBoxChanges
      // 
      this.textBoxChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxChanges.Location = new System.Drawing.Point(316, 93);
      this.textBoxChanges.Name = "textBoxChanges";
      this.textBoxChanges.ReadOnly = true;
      this.textBoxChanges.Size = new System.Drawing.Size(183, 20);
      this.textBoxChanges.TabIndex = 6;
      // 
      // textBoxPayment
      // 
      this.textBoxPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPayment.Location = new System.Drawing.Point(316, 64);
      this.textBoxPayment.Name = "textBoxPayment";
      this.textBoxPayment.Size = new System.Drawing.Size(183, 20);
      this.textBoxPayment.TabIndex = 5;
      // 
      // textBoxTotal
      // 
      this.textBoxTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTotal.Location = new System.Drawing.Point(316, 31);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new System.Drawing.Size(183, 20);
      this.textBoxTotal.TabIndex = 4;
      // 
      // labelChanges
      // 
      this.labelChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelChanges.AutoSize = true;
      this.labelChanges.Location = new System.Drawing.Point(254, 96);
      this.labelChanges.Name = "labelChanges";
      this.labelChanges.Size = new System.Drawing.Size(44, 13);
      this.labelChanges.TabIndex = 3;
      this.labelChanges.Text = "Kembali";
      // 
      // labelPayment
      // 
      this.labelPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelPayment.AutoSize = true;
      this.labelPayment.Location = new System.Drawing.Point(254, 67);
      this.labelPayment.Name = "labelPayment";
      this.labelPayment.Size = new System.Drawing.Size(34, 13);
      this.labelPayment.TabIndex = 2;
      this.labelPayment.Text = "Bayar";
      // 
      // buttonCheckout
      // 
      this.buttonCheckout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCheckout.Location = new System.Drawing.Point(525, 67);
      this.buttonCheckout.Name = "buttonCheckout";
      this.buttonCheckout.Size = new System.Drawing.Size(130, 50);
      this.buttonCheckout.TabIndex = 1;
      this.buttonCheckout.Text = "Bayar";
      this.buttonCheckout.UseVisualStyleBackColor = true;
      this.buttonCheckout.Click += new System.EventHandler(this.buttonCheckout_Click);
      // 
      // labelTotal
      // 
      this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelTotal.AutoSize = true;
      this.labelTotal.Location = new System.Drawing.Point(254, 38);
      this.labelTotal.Name = "labelTotal";
      this.labelTotal.Size = new System.Drawing.Size(31, 13);
      this.labelTotal.TabIndex = 0;
      this.labelTotal.Text = "Total";
      // 
      // groupBoxItemList
      // 
      this.groupBoxItemList.Controls.Add(this.dataGridViewItemList);
      this.groupBoxItemList.Controls.Add(this.panelFilter);
      this.groupBoxItemList.Dock = System.Windows.Forms.DockStyle.Left;
      this.groupBoxItemList.Location = new System.Drawing.Point(3, 3);
      this.groupBoxItemList.Name = "groupBoxItemList";
      this.groupBoxItemList.Size = new System.Drawing.Size(420, 651);
      this.groupBoxItemList.TabIndex = 0;
      this.groupBoxItemList.TabStop = false;
      this.groupBoxItemList.Text = "Daftar Barang";
      // 
      // dataGridViewItemList
      // 
      this.dataGridViewItemList.AllowUserToAddRows = false;
      this.dataGridViewItemList.AllowUserToResizeRows = false;
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
      this.dataGridViewItemList.Size = new System.Drawing.Size(414, 589);
      this.dataGridViewItemList.TabIndex = 4;
      this.dataGridViewItemList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewItemList_CellContentClick);
      // 
      // ItemCode
      // 
      this.ItemCode.HeaderText = "Kode";
      this.ItemCode.Name = "ItemCode";
      this.ItemCode.ReadOnly = true;
      this.ItemCode.Width = 45;
      // 
      // ItemName
      // 
      this.ItemName.HeaderText = "Nama Barang";
      this.ItemName.Name = "ItemName";
      this.ItemName.ReadOnly = true;
      this.ItemName.Width = 200;
      // 
      // ItemPrice
      // 
      this.ItemPrice.HeaderText = "Harga";
      this.ItemPrice.Name = "ItemPrice";
      this.ItemPrice.ReadOnly = true;
      // 
      // ItemAdd
      // 
      this.ItemAdd.HeaderText = "Pilih";
      this.ItemAdd.Name = "ItemAdd";
      this.ItemAdd.ReadOnly = true;
      this.ItemAdd.Width = 30;
      // 
      // panelFilter
      // 
      this.panelFilter.Controls.Add(this.labelFilter);
      this.panelFilter.Controls.Add(this.textBoxFilter);
      this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelFilter.Location = new System.Drawing.Point(3, 16);
      this.panelFilter.Name = "panelFilter";
      this.panelFilter.Size = new System.Drawing.Size(414, 43);
      this.panelFilter.TabIndex = 5;
      // 
      // labelFilter
      // 
      this.labelFilter.AutoSize = true;
      this.labelFilter.Location = new System.Drawing.Point(3, 18);
      this.labelFilter.Name = "labelFilter";
      this.labelFilter.Size = new System.Drawing.Size(58, 13);
      this.labelFilter.TabIndex = 2;
      this.labelFilter.Text = "Pencarian:";
      // 
      // textBoxFilter
      // 
      this.textBoxFilter.Location = new System.Drawing.Point(67, 14);
      this.textBoxFilter.Name = "textBoxFilter";
      this.textBoxFilter.Size = new System.Drawing.Size(211, 20);
      this.textBoxFilter.TabIndex = 3;
      this.textBoxFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFilter_KeyPress);
      // 
      // tabPageInventoryList
      // 
      this.tabPageInventoryList.Controls.Add(this.buttonCancelSave);
      this.tabPageInventoryList.Controls.Add(this.buttonSave);
      this.tabPageInventoryList.Controls.Add(this.groupBoxImport);
      this.tabPageInventoryList.Controls.Add(this.groupBoxExport);
      this.tabPageInventoryList.Controls.Add(this.groupBoxMasterItemDetail);
      this.tabPageInventoryList.Controls.Add(this.dataGridViewMasterItemList);
      this.tabPageInventoryList.Location = new System.Drawing.Point(4, 22);
      this.tabPageInventoryList.Name = "tabPageInventoryList";
      this.tabPageInventoryList.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageInventoryList.Size = new System.Drawing.Size(1100, 657);
      this.tabPageInventoryList.TabIndex = 1;
      this.tabPageInventoryList.Text = "Daftar Barang";
      this.tabPageInventoryList.UseVisualStyleBackColor = true;
      // 
      // buttonCancelSave
      // 
      this.buttonCancelSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancelSave.Location = new System.Drawing.Point(928, 621);
      this.buttonCancelSave.Name = "buttonCancelSave";
      this.buttonCancelSave.Size = new System.Drawing.Size(164, 28);
      this.buttonCancelSave.TabIndex = 10;
      this.buttonCancelSave.Text = "Batal";
      this.buttonCancelSave.UseVisualStyleBackColor = true;
      // 
      // buttonSave
      // 
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(758, 621);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(164, 28);
      this.buttonSave.TabIndex = 0;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      // 
      // groupBoxImport
      // 
      this.groupBoxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxImport.Controls.Add(this.dataGridViewUploadConfirm);
      this.groupBoxImport.Controls.Add(this.buttonBrowseImportFile);
      this.groupBoxImport.Controls.Add(this.buttonImportItems);
      this.groupBoxImport.Controls.Add(this.textBoxFilePathImport);
      this.groupBoxImport.Location = new System.Drawing.Point(511, 273);
      this.groupBoxImport.Name = "groupBoxImport";
      this.groupBoxImport.Size = new System.Drawing.Size(581, 342);
      this.groupBoxImport.TabIndex = 9;
      this.groupBoxImport.TabStop = false;
      this.groupBoxImport.Text = "Upload";
      // 
      // dataGridViewUploadConfirm
      // 
      this.dataGridViewUploadConfirm.AllowUserToAddRows = false;
      this.dataGridViewUploadConfirm.AllowUserToDeleteRows = false;
      this.dataGridViewUploadConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewUploadConfirm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewUploadConfirm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemUploadCode,
            this.ItemUploadName,
            this.ItemUploadPrice,
            this.ItemUploadDiscount,
            this.ItemUploadStatus});
      this.dataGridViewUploadConfirm.Location = new System.Drawing.Point(13, 45);
      this.dataGridViewUploadConfirm.Name = "dataGridViewUploadConfirm";
      this.dataGridViewUploadConfirm.RowHeadersVisible = false;
      this.dataGridViewUploadConfirm.Size = new System.Drawing.Size(562, 247);
      this.dataGridViewUploadConfirm.TabIndex = 10;
      // 
      // ItemUploadCode
      // 
      this.ItemUploadCode.HeaderText = "Kode";
      this.ItemUploadCode.Name = "ItemUploadCode";
      this.ItemUploadCode.ReadOnly = true;
      // 
      // ItemUploadName
      // 
      this.ItemUploadName.HeaderText = "Nama";
      this.ItemUploadName.Name = "ItemUploadName";
      this.ItemUploadName.ReadOnly = true;
      // 
      // ItemUploadPrice
      // 
      this.ItemUploadPrice.HeaderText = "Harga";
      this.ItemUploadPrice.Name = "ItemUploadPrice";
      this.ItemUploadPrice.ReadOnly = true;
      // 
      // ItemUploadDiscount
      // 
      this.ItemUploadDiscount.HeaderText = "Diskon";
      this.ItemUploadDiscount.Name = "ItemUploadDiscount";
      this.ItemUploadDiscount.ReadOnly = true;
      // 
      // ItemUploadStatus
      // 
      this.ItemUploadStatus.HeaderText = "Status";
      this.ItemUploadStatus.Name = "ItemUploadStatus";
      this.ItemUploadStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      // 
      // buttonBrowseImportFile
      // 
      this.buttonBrowseImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonBrowseImportFile.Location = new System.Drawing.Point(546, 18);
      this.buttonBrowseImportFile.Name = "buttonBrowseImportFile";
      this.buttonBrowseImportFile.Size = new System.Drawing.Size(29, 23);
      this.buttonBrowseImportFile.TabIndex = 0;
      this.buttonBrowseImportFile.Text = "...";
      this.buttonBrowseImportFile.UseVisualStyleBackColor = true;
      // 
      // buttonImportItems
      // 
      this.buttonImportItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonImportItems.Location = new System.Drawing.Point(466, 304);
      this.buttonImportItems.Name = "buttonImportItems";
      this.buttonImportItems.Size = new System.Drawing.Size(109, 23);
      this.buttonImportItems.TabIndex = 1;
      this.buttonImportItems.Text = "Konfirmasi";
      this.buttonImportItems.UseVisualStyleBackColor = true;
      // 
      // textBoxFilePathImport
      // 
      this.textBoxFilePathImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxFilePathImport.Location = new System.Drawing.Point(13, 19);
      this.textBoxFilePathImport.Name = "textBoxFilePathImport";
      this.textBoxFilePathImport.ReadOnly = true;
      this.textBoxFilePathImport.Size = new System.Drawing.Size(527, 20);
      this.textBoxFilePathImport.TabIndex = 8;
      this.textBoxFilePathImport.Text = "[Pilih lokasi file barang yang baru]";
      // 
      // groupBoxExport
      // 
      this.groupBoxExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxExport.Controls.Add(this.buttonExportItems);
      this.groupBoxExport.Location = new System.Drawing.Point(511, 211);
      this.groupBoxExport.Name = "groupBoxExport";
      this.groupBoxExport.Size = new System.Drawing.Size(581, 56);
      this.groupBoxExport.TabIndex = 8;
      this.groupBoxExport.TabStop = false;
      this.groupBoxExport.Text = "Simpan Daftar Barang";
      // 
      // buttonExportItems
      // 
      this.buttonExportItems.Location = new System.Drawing.Point(13, 19);
      this.buttonExportItems.Name = "buttonExportItems";
      this.buttonExportItems.Size = new System.Drawing.Size(119, 23);
      this.buttonExportItems.TabIndex = 0;
      this.buttonExportItems.Text = "Export to File";
      this.buttonExportItems.UseVisualStyleBackColor = true;
      // 
      // groupBoxMasterItemDetail
      // 
      this.groupBoxMasterItemDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonDelete);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscountRupiah);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelPriceRupiah);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.radioButtonDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.radioButtonDiscountAmount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemDiscountPercent);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelDiscount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemCode);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelPrice);
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonAdd);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelName);
      this.groupBoxMasterItemDetail.Controls.Add(this.labelCode);
      this.groupBoxMasterItemDetail.Controls.Add(this.buttonUpdate);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemDiscountAmount);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemName);
      this.groupBoxMasterItemDetail.Controls.Add(this.textBoxDetailItemPrice);
      this.groupBoxMasterItemDetail.Location = new System.Drawing.Point(511, 6);
      this.groupBoxMasterItemDetail.Name = "groupBoxMasterItemDetail";
      this.groupBoxMasterItemDetail.Size = new System.Drawing.Size(581, 199);
      this.groupBoxMasterItemDetail.TabIndex = 7;
      this.groupBoxMasterItemDetail.TabStop = false;
      this.groupBoxMasterItemDetail.Text = "Detail Barang";
      // 
      // buttonDelete
      // 
      this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDelete.Location = new System.Drawing.Point(316, 164);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(75, 23);
      this.buttonDelete.TabIndex = 5;
      this.buttonDelete.Text = "Hapus";
      this.buttonDelete.UseVisualStyleBackColor = true;
      // 
      // labelDiscountRupiah
      // 
      this.labelDiscountRupiah.AutoSize = true;
      this.labelDiscountRupiah.Location = new System.Drawing.Point(194, 106);
      this.labelDiscountRupiah.Name = "labelDiscountRupiah";
      this.labelDiscountRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelDiscountRupiah.TabIndex = 15;
      this.labelDiscountRupiah.Text = "Rupiah";
      // 
      // labelPriceRupiah
      // 
      this.labelPriceRupiah.AutoSize = true;
      this.labelPriceRupiah.Location = new System.Drawing.Point(173, 78);
      this.labelPriceRupiah.Name = "labelPriceRupiah";
      this.labelPriceRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelPriceRupiah.TabIndex = 14;
      this.labelPriceRupiah.Text = "Rupiah";
      // 
      // labelDiscountPercent
      // 
      this.labelDiscountPercent.AutoSize = true;
      this.labelDiscountPercent.Location = new System.Drawing.Point(194, 134);
      this.labelDiscountPercent.Name = "labelDiscountPercent";
      this.labelDiscountPercent.Size = new System.Drawing.Size(15, 13);
      this.labelDiscountPercent.TabIndex = 13;
      this.labelDiscountPercent.Text = "%";
      // 
      // radioButtonDiscountPercent
      // 
      this.radioButtonDiscountPercent.AutoSize = true;
      this.radioButtonDiscountPercent.Location = new System.Drawing.Point(67, 131);
      this.radioButtonDiscountPercent.Name = "radioButtonDiscountPercent";
      this.radioButtonDiscountPercent.Size = new System.Drawing.Size(14, 13);
      this.radioButtonDiscountPercent.TabIndex = 12;
      this.radioButtonDiscountPercent.UseVisualStyleBackColor = true;
      // 
      // radioButtonDiscountAmount
      // 
      this.radioButtonDiscountAmount.AutoSize = true;
      this.radioButtonDiscountAmount.Location = new System.Drawing.Point(67, 106);
      this.radioButtonDiscountAmount.Name = "radioButtonDiscountAmount";
      this.radioButtonDiscountAmount.Size = new System.Drawing.Size(14, 13);
      this.radioButtonDiscountAmount.TabIndex = 11;
      this.radioButtonDiscountAmount.UseVisualStyleBackColor = true;
      // 
      // textBoxDetailItemDiscountPercent
      // 
      this.textBoxDetailItemDiscountPercent.Location = new System.Drawing.Point(88, 131);
      this.textBoxDetailItemDiscountPercent.Name = "textBoxDetailItemDiscountPercent";
      this.textBoxDetailItemDiscountPercent.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemDiscountPercent.TabIndex = 4;
      this.textBoxDetailItemDiscountPercent.TextChanged += new System.EventHandler(this.textBoxDiscountPercent_TextChanged);
      // 
      // labelDiscount
      // 
      this.labelDiscount.AutoSize = true;
      this.labelDiscount.Location = new System.Drawing.Point(10, 106);
      this.labelDiscount.Name = "labelDiscount";
      this.labelDiscount.Size = new System.Drawing.Size(40, 13);
      this.labelDiscount.TabIndex = 9;
      this.labelDiscount.Text = "Diskon";
      // 
      // textBoxDetailItemCode
      // 
      this.textBoxDetailItemCode.Location = new System.Drawing.Point(67, 19);
      this.textBoxDetailItemCode.Name = "textBoxDetailItemCode";
      this.textBoxDetailItemCode.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemCode.TabIndex = 0;
      // 
      // labelPrice
      // 
      this.labelPrice.AutoSize = true;
      this.labelPrice.Location = new System.Drawing.Point(10, 78);
      this.labelPrice.Name = "labelPrice";
      this.labelPrice.Size = new System.Drawing.Size(36, 13);
      this.labelPrice.TabIndex = 8;
      this.labelPrice.Text = "Harga";
      // 
      // buttonAdd
      // 
      this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAdd.Location = new System.Drawing.Point(411, 164);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new System.Drawing.Size(75, 23);
      this.buttonAdd.TabIndex = 6;
      this.buttonAdd.Text = "Tambah";
      this.buttonAdd.UseVisualStyleBackColor = true;
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(10, 50);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(35, 13);
      this.labelName.TabIndex = 7;
      this.labelName.Text = "Nama";
      // 
      // labelCode
      // 
      this.labelCode.AutoSize = true;
      this.labelCode.Location = new System.Drawing.Point(10, 22);
      this.labelCode.Name = "labelCode";
      this.labelCode.Size = new System.Drawing.Size(32, 13);
      this.labelCode.TabIndex = 6;
      this.labelCode.Text = "Kode";
      // 
      // buttonUpdate
      // 
      this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonUpdate.Location = new System.Drawing.Point(500, 164);
      this.buttonUpdate.Name = "buttonUpdate";
      this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
      this.buttonUpdate.TabIndex = 7;
      this.buttonUpdate.Text = "Ubah";
      this.buttonUpdate.UseVisualStyleBackColor = true;
      // 
      // textBoxDetailItemDiscountAmount
      // 
      this.textBoxDetailItemDiscountAmount.Location = new System.Drawing.Point(88, 103);
      this.textBoxDetailItemDiscountAmount.Name = "textBoxDetailItemDiscountAmount";
      this.textBoxDetailItemDiscountAmount.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemDiscountAmount.TabIndex = 3;
      this.textBoxDetailItemDiscountAmount.TextChanged += new System.EventHandler(this.textBoxDiscountAmount_TextChanged);
      // 
      // textBoxDetailItemName
      // 
      this.textBoxDetailItemName.Location = new System.Drawing.Point(67, 47);
      this.textBoxDetailItemName.Name = "textBoxDetailItemName";
      this.textBoxDetailItemName.Size = new System.Drawing.Size(365, 20);
      this.textBoxDetailItemName.TabIndex = 1;
      // 
      // textBoxDetailItemPrice
      // 
      this.textBoxDetailItemPrice.Location = new System.Drawing.Point(67, 75);
      this.textBoxDetailItemPrice.Name = "textBoxDetailItemPrice";
      this.textBoxDetailItemPrice.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemPrice.TabIndex = 2;
      // 
      // dataGridViewMasterItemList
      // 
      this.dataGridViewMasterItemList.AllowUserToAddRows = false;
      this.dataGridViewMasterItemList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)));
      this.dataGridViewMasterItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMasterItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MasterItemCode,
            this.MasterItemName,
            this.MasterItemPrice,
            this.MasterItemDiscount});
      this.dataGridViewMasterItemList.Location = new System.Drawing.Point(8, 6);
      this.dataGridViewMasterItemList.Name = "dataGridViewMasterItemList";
      this.dataGridViewMasterItemList.RowHeadersVisible = false;
      this.dataGridViewMasterItemList.Size = new System.Drawing.Size(497, 643);
      this.dataGridViewMasterItemList.TabIndex = 0;
      // 
      // MasterItemCode
      // 
      this.MasterItemCode.FillWeight = 50F;
      this.MasterItemCode.HeaderText = "Kode";
      this.MasterItemCode.Name = "MasterItemCode";
      this.MasterItemCode.Width = 50;
      // 
      // MasterItemName
      // 
      this.MasterItemName.HeaderText = "Nama Barang";
      this.MasterItemName.Name = "MasterItemName";
      this.MasterItemName.Width = 200;
      // 
      // MasterItemPrice
      // 
      this.MasterItemPrice.HeaderText = "Harga";
      this.MasterItemPrice.Name = "MasterItemPrice";
      // 
      // MasterItemDiscount
      // 
      this.MasterItemDiscount.HeaderText = "Diskon";
      this.MasterItemDiscount.Name = "MasterItemDiscount";
      // 
      // statusStripInformation
      // 
      this.statusStripInformation.Location = new System.Drawing.Point(0, 707);
      this.statusStripInformation.Name = "statusStripInformation";
      this.statusStripInformation.Size = new System.Drawing.Size(1108, 22);
      this.statusStripInformation.TabIndex = 2;
      this.statusStripInformation.Text = "statusStrip1";
      // 
      // buttonClearCart
      // 
      this.buttonClearCart.Location = new System.Drawing.Point(525, 31);
      this.buttonClearCart.Name = "buttonClearCart";
      this.buttonClearCart.Size = new System.Drawing.Size(130, 23);
      this.buttonClearCart.TabIndex = 9;
      this.buttonClearCart.Text = "Bersihkan";
      this.buttonClearCart.UseVisualStyleBackColor = true;
      this.buttonClearCart.Click += new System.EventHandler(this.buttonClearCart_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1108, 729);
      this.Controls.Add(this.tabControlPage);
      this.Controls.Add(this.statusStripInformation);
      this.Controls.Add(this.menuStripMain);
      this.MainMenuStrip = this.menuStripMain;
      this.MinimumSize = new System.Drawing.Size(1024, 768);
      this.Name = "MainForm";
      this.Text = "Fidelis Cake and Bakery";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.menuStripMain.ResumeLayout(false);
      this.menuStripMain.PerformLayout();
      this.tabControlPage.ResumeLayout(false);
      this.tabPageLogin.ResumeLayout(false);
      this.groupBoxLogin.ResumeLayout(false);
      this.groupBoxLogin.PerformLayout();
      this.tabPageCashier.ResumeLayout(false);
      this.groupBoxCart.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCart)).EndInit();
      this.groupBoxSummary.ResumeLayout(false);
      this.groupBoxSummary.PerformLayout();
      this.groupBoxItemList.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItemList)).EndInit();
      this.panelFilter.ResumeLayout(false);
      this.panelFilter.PerformLayout();
      this.tabPageInventoryList.ResumeLayout(false);
      this.groupBoxImport.ResumeLayout(false);
      this.groupBoxImport.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUploadConfirm)).EndInit();
      this.groupBoxExport.ResumeLayout(false);
      this.groupBoxMasterItemDetail.ResumeLayout(false);
      this.groupBoxMasterItemDetail.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStripMain;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem daftarBarangToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem transaksiToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem penjualanToolStripMenuItem;
    private System.Windows.Forms.TabControl tabControlPage;
    private System.Windows.Forms.TabPage tabPageCashier;
    private System.Windows.Forms.TabPage tabPageInventoryList;
    private System.Windows.Forms.GroupBox groupBoxItemList;
    private System.Windows.Forms.TextBox textBoxFilter;
    private System.Windows.Forms.Label labelFilter;
    private System.Windows.Forms.DataGridView dataGridViewItemList;
    private System.Windows.Forms.GroupBox groupBoxCart;
    private System.Windows.Forms.DataGridView dataGridViewCart;
    private System.Windows.Forms.GroupBox groupBoxSummary;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemPrice;
    private System.Windows.Forms.DataGridViewButtonColumn ItemAdd;
    private System.Windows.Forms.TextBox textBoxChanges;
    private System.Windows.Forms.TextBox textBoxPayment;
    private System.Windows.Forms.TextBox textBoxTotal;
    private System.Windows.Forms.Label labelChanges;
    private System.Windows.Forms.Label labelPayment;
    private System.Windows.Forms.Button buttonCheckout;
    private System.Windows.Forms.Label labelTotal;
    private System.Windows.Forms.Label labelNotes;
    private System.Windows.Forms.TextBox textBoxNotes;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemQuantity;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemDiscount;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemSubtotal;
    private System.Windows.Forms.Panel panelFilter;
    private System.Windows.Forms.DataGridView dataGridViewMasterItemList;
    private System.Windows.Forms.Button buttonUpdate;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.Button buttonImportItems;
    private System.Windows.Forms.TextBox textBoxDetailItemPrice;
    private System.Windows.Forms.TextBox textBoxDetailItemName;
    private System.Windows.Forms.TextBox textBoxDetailItemCode;
    private System.Windows.Forms.Button buttonExportItems;
    private System.Windows.Forms.GroupBox groupBoxMasterItemDetail;
    private System.Windows.Forms.Label labelDiscount;
    private System.Windows.Forms.Label labelPrice;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.Label labelCode;
    private System.Windows.Forms.TextBox textBoxDetailItemDiscountAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn MasterItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn MasterItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn MasterItemPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn MasterItemDiscount;
    private System.Windows.Forms.Label labelDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountAmount;
    private System.Windows.Forms.TextBox textBoxDetailItemDiscountPercent;
    private System.Windows.Forms.Label labelPriceRupiah;
    private System.Windows.Forms.GroupBox groupBoxImport;
    private System.Windows.Forms.GroupBox groupBoxExport;
    private System.Windows.Forms.Label labelDiscountRupiah;
    private System.Windows.Forms.Button buttonBrowseImportFile;
    private System.Windows.Forms.TextBox textBoxFilePathImport;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.DataGridView dataGridViewUploadConfirm;
    private System.Windows.Forms.Button buttonDelete;
    private System.Windows.Forms.TabPage tabPageLogin;
    private System.Windows.Forms.GroupBox groupBoxLogin;
    private System.Windows.Forms.TextBox textBoxPassword;
    private System.Windows.Forms.TextBox textBoxUsername;
    private System.Windows.Forms.Label labelPassword;
    private System.Windows.Forms.Label labelUsername;
    private System.Windows.Forms.Button buttonLogin;
    private System.Windows.Forms.StatusStrip statusStripInformation;
    private System.Windows.Forms.Button buttonCancelSave;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemUploadCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemUploadName;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemUploadPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemUploadDiscount;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemUploadStatus;
    private System.Windows.Forms.Button buttonClearCart;
  }
}


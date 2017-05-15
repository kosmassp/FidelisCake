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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
      this.menuStripMain = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.transaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.penjualanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.daftarBarangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.daftarUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.laporanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.laporanTransaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControlPage = new System.Windows.Forms.TabControl();
      this.tabPageLogin = new System.Windows.Forms.TabPage();
      this.groupBoxLogin = new System.Windows.Forms.GroupBox();
      this.labelCannotLogin = new System.Windows.Forms.Label();
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
      this.tabPageProductMaster = new System.Windows.Forms.TabPage();
      this.buttonCancelSave = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.groupBoxExportImport = new System.Windows.Forms.GroupBox();
      this.buttonImport = new System.Windows.Forms.Button();
      this.buttonExportItems = new System.Windows.Forms.Button();
      this.groupBoxMasterItemDetail = new System.Windows.Forms.GroupBox();
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
      this.buttonUpdate = new System.Windows.Forms.Button();
      this.buttonAdd = new System.Windows.Forms.Button();
      this.tabPageReport = new System.Windows.Forms.TabPage();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.dataGridViewLaporan = new System.Windows.Forms.DataGridView();
      this.groupBoxReportFilter = new System.Windows.Forms.GroupBox();
      this.labelLaporanStart = new System.Windows.Forms.Label();
      this.buttonShowReportDetail = new System.Windows.Forms.Button();
      this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
      this.buttonShowReportSummary = new System.Windows.Forms.Button();
      this.dateTimePickerStop = new System.Windows.Forms.DateTimePicker();
      this.labelLaporanEnd = new System.Windows.Forms.Label();
      this.statusStripInformation = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusCurrentDate = new System.Windows.Forms.ToolStripStatusLabel();
      this.timerDisplayDate = new System.Windows.Forms.Timer(this.components);
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
      this.tabPageProductMaster.SuspendLayout();
      this.groupBoxExportImport.SuspendLayout();
      this.groupBoxMasterItemDetail.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).BeginInit();
      this.tabPageReport.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporan)).BeginInit();
      this.groupBoxReportFilter.SuspendLayout();
      this.statusStripInformation.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStripMain
      // 
      this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.transaksiToolStripMenuItem,
            this.editToolStripMenuItem,
            this.laporanToolStripMenuItem});
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
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.daftarBarangToolStripMenuItem,
            this.daftarUserToolStripMenuItem});
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
      // daftarUserToolStripMenuItem
      // 
      this.daftarUserToolStripMenuItem.Name = "daftarUserToolStripMenuItem";
      this.daftarUserToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
      this.daftarUserToolStripMenuItem.Text = "Daftar User";
      this.daftarUserToolStripMenuItem.Click += new System.EventHandler(this.daftarUserToolStripMenuItem_Click);
      // 
      // laporanToolStripMenuItem
      // 
      this.laporanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.laporanTransaksiToolStripMenuItem});
      this.laporanToolStripMenuItem.Name = "laporanToolStripMenuItem";
      this.laporanToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
      this.laporanToolStripMenuItem.Text = "Laporan";
      // 
      // laporanTransaksiToolStripMenuItem
      // 
      this.laporanTransaksiToolStripMenuItem.Name = "laporanTransaksiToolStripMenuItem";
      this.laporanTransaksiToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
      this.laporanTransaksiToolStripMenuItem.Text = "Laporan Transaksi";
      this.laporanTransaksiToolStripMenuItem.Click += new System.EventHandler(this.laporanTransaksiToolStripMenuItem_Click);
      // 
      // tabControlPage
      // 
      this.tabControlPage.Controls.Add(this.tabPageLogin);
      this.tabControlPage.Controls.Add(this.tabPageCashier);
      this.tabControlPage.Controls.Add(this.tabPageProductMaster);
      this.tabControlPage.Controls.Add(this.tabPageReport);
      this.tabControlPage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlPage.Location = new System.Drawing.Point(0, 24);
      this.tabControlPage.Name = "tabControlPage";
      this.tabControlPage.SelectedIndex = 0;
      this.tabControlPage.Size = new System.Drawing.Size(1108, 683);
      this.tabControlPage.TabIndex = 1;
      this.tabControlPage.SelectedIndexChanged += new System.EventHandler(this.tabControlPage_SelectedIndexChanged);
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
      this.groupBoxLogin.Controls.Add(this.labelCannotLogin);
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
      // labelCannotLogin
      // 
      this.labelCannotLogin.AutoSize = true;
      this.labelCannotLogin.ForeColor = System.Drawing.Color.Red;
      this.labelCannotLogin.Location = new System.Drawing.Point(93, 114);
      this.labelCannotLogin.Name = "labelCannotLogin";
      this.labelCannotLogin.Size = new System.Drawing.Size(0, 13);
      this.labelCannotLogin.TabIndex = 5;
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
      this.textBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPassword_KeyPress);
      this.textBoxPassword.Enter += new System.EventHandler(this.textBoxPassword_Enter);
      // 
      // textBoxUsername
      // 
      this.textBoxUsername.Location = new System.Drawing.Point(96, 43);
      this.textBoxUsername.Name = "textBoxUsername";
      this.textBoxUsername.Size = new System.Drawing.Size(223, 20);
      this.textBoxUsername.TabIndex = 0;
      this.textBoxUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUsername_KeyPress);
      this.textBoxUsername.Enter += new System.EventHandler(this.textBoxUsername_Enter);
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
      this.dataGridViewCart.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCart_CellValueChanged);
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
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemPrice.DefaultCellStyle = dataGridViewCellStyle5;
      this.CartItemPrice.FillWeight = 120F;
      this.CartItemPrice.HeaderText = "Harga Barang";
      this.CartItemPrice.Name = "CartItemPrice";
      this.CartItemPrice.ReadOnly = true;
      this.CartItemPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemPrice.Width = 120;
      // 
      // CartItemDiscount
      // 
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemDiscount.DefaultCellStyle = dataGridViewCellStyle6;
      this.CartItemDiscount.FillWeight = 120F;
      this.CartItemDiscount.HeaderText = "Diskon";
      this.CartItemDiscount.Name = "CartItemDiscount";
      this.CartItemDiscount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.CartItemDiscount.Width = 120;
      // 
      // CartItemSubtotal
      // 
      dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.CartItemSubtotal.DefaultCellStyle = dataGridViewCellStyle7;
      this.CartItemSubtotal.FillWeight = 120F;
      this.CartItemSubtotal.HeaderText = "Total";
      this.CartItemSubtotal.Name = "CartItemSubtotal";
      this.CartItemSubtotal.ReadOnly = true;
      this.CartItemSubtotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
      // buttonClearCart
      // 
      this.buttonClearCart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClearCart.Location = new System.Drawing.Point(525, 31);
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
      this.textBoxNotes.Size = new System.Drawing.Size(218, 63);
      this.textBoxNotes.TabIndex = 1;
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
      this.textBoxPayment.TabIndex = 0;
      this.textBoxPayment.Click += new System.EventHandler(this.textBoxPayment_Click);
      this.textBoxPayment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPayment_KeyUp);
      // 
      // textBoxTotal
      // 
      this.textBoxTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTotal.Location = new System.Drawing.Point(316, 31);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new System.Drawing.Size(183, 20);
      this.textBoxTotal.TabIndex = 4;
      this.textBoxTotal.TextChanged += new System.EventHandler(this.textBoxTotal_TextChanged);
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
      this.buttonCheckout.TabIndex = 3;
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
      this.dataGridViewItemList.Size = new System.Drawing.Size(414, 589);
      this.dataGridViewItemList.TabIndex = 1;
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
      dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
      this.ItemPrice.DefaultCellStyle = dataGridViewCellStyle8;
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
      this.panelFilter.Size = new System.Drawing.Size(414, 43);
      this.panelFilter.TabIndex = 0;
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
      this.textBoxFilter.TabIndex = 0;
      this.textBoxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyUp);
      // 
      // tabPageProductMaster
      // 
      this.tabPageProductMaster.Controls.Add(this.buttonCancelSave);
      this.tabPageProductMaster.Controls.Add(this.buttonSave);
      this.tabPageProductMaster.Controls.Add(this.buttonDelete);
      this.tabPageProductMaster.Controls.Add(this.groupBoxExportImport);
      this.tabPageProductMaster.Controls.Add(this.groupBoxMasterItemDetail);
      this.tabPageProductMaster.Controls.Add(this.dataGridViewMasterItemList);
      this.tabPageProductMaster.Controls.Add(this.buttonUpdate);
      this.tabPageProductMaster.Controls.Add(this.buttonAdd);
      this.tabPageProductMaster.Location = new System.Drawing.Point(4, 22);
      this.tabPageProductMaster.Name = "tabPageProductMaster";
      this.tabPageProductMaster.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageProductMaster.Size = new System.Drawing.Size(1100, 657);
      this.tabPageProductMaster.TabIndex = 1;
      this.tabPageProductMaster.Text = "Daftar Barang";
      this.tabPageProductMaster.UseVisualStyleBackColor = true;
      // 
      // buttonCancelSave
      // 
      this.buttonCancelSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancelSave.Location = new System.Drawing.Point(927, 606);
      this.buttonCancelSave.Name = "buttonCancelSave";
      this.buttonCancelSave.Size = new System.Drawing.Size(164, 28);
      this.buttonCancelSave.TabIndex = 10;
      this.buttonCancelSave.Text = "Batal";
      this.buttonCancelSave.UseVisualStyleBackColor = true;
      this.buttonCancelSave.Visible = false;
      // 
      // buttonSave
      // 
      this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSave.Location = new System.Drawing.Point(750, 606);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(164, 28);
      this.buttonSave.TabIndex = 0;
      this.buttonSave.Text = "Simpan";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Visible = false;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      // 
      // buttonDelete
      // 
      this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDelete.BackgroundImage = global::InventoryAndSales.Properties.Resources.Remove;
      this.buttonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonDelete.Location = new System.Drawing.Point(591, 134);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(40, 40);
      this.buttonDelete.TabIndex = 5;
      this.buttonDelete.UseVisualStyleBackColor = true;
      this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
      // 
      // groupBoxExportImport
      // 
      this.groupBoxExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxExportImport.Controls.Add(this.buttonImport);
      this.groupBoxExportImport.Controls.Add(this.buttonExportItems);
      this.groupBoxExportImport.Location = new System.Drawing.Point(653, 258);
      this.groupBoxExportImport.Name = "groupBoxExportImport";
      this.groupBoxExportImport.Size = new System.Drawing.Size(441, 84);
      this.groupBoxExportImport.TabIndex = 8;
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
      this.groupBoxMasterItemDetail.Location = new System.Drawing.Point(653, 28);
      this.groupBoxMasterItemDetail.Name = "groupBoxMasterItemDetail";
      this.groupBoxMasterItemDetail.Size = new System.Drawing.Size(441, 199);
      this.groupBoxMasterItemDetail.TabIndex = 7;
      this.groupBoxMasterItemDetail.TabStop = false;
      this.groupBoxMasterItemDetail.Text = "Detail Barang";
      // 
      // buttonOkEdit
      // 
      this.buttonOkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOkEdit.Location = new System.Drawing.Point(274, 164);
      this.buttonOkEdit.Name = "buttonOkEdit";
      this.buttonOkEdit.Size = new System.Drawing.Size(75, 23);
      this.buttonOkEdit.TabIndex = 17;
      this.buttonOkEdit.Text = "OK";
      this.buttonOkEdit.UseVisualStyleBackColor = true;
      this.buttonOkEdit.Click += new System.EventHandler(this.buttonOkEdit_Click);
      // 
      // buttonCancelEdit
      // 
      this.buttonCancelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancelEdit.Location = new System.Drawing.Point(357, 164);
      this.buttonCancelEdit.Name = "buttonCancelEdit";
      this.buttonCancelEdit.Size = new System.Drawing.Size(75, 23);
      this.buttonCancelEdit.TabIndex = 16;
      this.buttonCancelEdit.Text = "Batal";
      this.buttonCancelEdit.UseVisualStyleBackColor = true;
      this.buttonCancelEdit.Click += new System.EventHandler(this.buttonCancelEdit_Click);
      // 
      // labelDiscountRupiah
      // 
      this.labelDiscountRupiah.AutoSize = true;
      this.labelDiscountRupiah.Location = new System.Drawing.Point(182, 110);
      this.labelDiscountRupiah.Name = "labelDiscountRupiah";
      this.labelDiscountRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelDiscountRupiah.TabIndex = 15;
      this.labelDiscountRupiah.Text = "Rupiah";
      // 
      // labelPriceRupiah
      // 
      this.labelPriceRupiah.AutoSize = true;
      this.labelPriceRupiah.Location = new System.Drawing.Point(182, 82);
      this.labelPriceRupiah.Name = "labelPriceRupiah";
      this.labelPriceRupiah.Size = new System.Drawing.Size(41, 13);
      this.labelPriceRupiah.TabIndex = 14;
      this.labelPriceRupiah.Text = "Rupiah";
      // 
      // labelDiscountPercent
      // 
      this.labelDiscountPercent.AutoSize = true;
      this.labelDiscountPercent.Location = new System.Drawing.Point(182, 138);
      this.labelDiscountPercent.Name = "labelDiscountPercent";
      this.labelDiscountPercent.Size = new System.Drawing.Size(15, 13);
      this.labelDiscountPercent.TabIndex = 13;
      this.labelDiscountPercent.Text = "%";
      // 
      // radioButtonDiscountPercent
      // 
      this.radioButtonDiscountPercent.AutoSize = true;
      this.radioButtonDiscountPercent.Enabled = false;
      this.radioButtonDiscountPercent.Location = new System.Drawing.Point(76, 139);
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
      this.radioButtonDiscountAmount.Location = new System.Drawing.Point(76, 110);
      this.radioButtonDiscountAmount.Name = "radioButtonDiscountAmount";
      this.radioButtonDiscountAmount.Size = new System.Drawing.Size(14, 13);
      this.radioButtonDiscountAmount.TabIndex = 11;
      this.radioButtonDiscountAmount.UseVisualStyleBackColor = true;
      this.radioButtonDiscountAmount.CheckedChanged += new System.EventHandler(this.radioButtonDiscount_CheckedChanged);
      // 
      // textBoxDetailItemDiscountPercent
      // 
      this.textBoxDetailItemDiscountPercent.Enabled = false;
      this.textBoxDetailItemDiscountPercent.Location = new System.Drawing.Point(97, 135);
      this.textBoxDetailItemDiscountPercent.Name = "textBoxDetailItemDiscountPercent";
      this.textBoxDetailItemDiscountPercent.Size = new System.Drawing.Size(79, 20);
      this.textBoxDetailItemDiscountPercent.TabIndex = 4;
      this.textBoxDetailItemDiscountPercent.TextChanged += new System.EventHandler(this.textBoxDiscountPercent_TextChanged);
      // 
      // labelDiscount
      // 
      this.labelDiscount.AutoSize = true;
      this.labelDiscount.Location = new System.Drawing.Point(19, 110);
      this.labelDiscount.Name = "labelDiscount";
      this.labelDiscount.Size = new System.Drawing.Size(40, 13);
      this.labelDiscount.TabIndex = 9;
      this.labelDiscount.Text = "Diskon";
      // 
      // textBoxDetailItemCode
      // 
      this.textBoxDetailItemCode.Enabled = false;
      this.textBoxDetailItemCode.Location = new System.Drawing.Point(76, 23);
      this.textBoxDetailItemCode.Name = "textBoxDetailItemCode";
      this.textBoxDetailItemCode.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemCode.TabIndex = 0;
      // 
      // labelPrice
      // 
      this.labelPrice.AutoSize = true;
      this.labelPrice.Location = new System.Drawing.Point(19, 82);
      this.labelPrice.Name = "labelPrice";
      this.labelPrice.Size = new System.Drawing.Size(36, 13);
      this.labelPrice.TabIndex = 8;
      this.labelPrice.Text = "Harga";
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(19, 54);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(35, 13);
      this.labelName.TabIndex = 7;
      this.labelName.Text = "Nama";
      // 
      // labelCode
      // 
      this.labelCode.AutoSize = true;
      this.labelCode.Location = new System.Drawing.Point(19, 26);
      this.labelCode.Name = "labelCode";
      this.labelCode.Size = new System.Drawing.Size(32, 13);
      this.labelCode.TabIndex = 6;
      this.labelCode.Text = "Kode";
      // 
      // textBoxDetailItemDiscountAmount
      // 
      this.textBoxDetailItemDiscountAmount.Enabled = false;
      this.textBoxDetailItemDiscountAmount.Location = new System.Drawing.Point(97, 107);
      this.textBoxDetailItemDiscountAmount.Name = "textBoxDetailItemDiscountAmount";
      this.textBoxDetailItemDiscountAmount.Size = new System.Drawing.Size(79, 20);
      this.textBoxDetailItemDiscountAmount.TabIndex = 3;
      this.textBoxDetailItemDiscountAmount.TextChanged += new System.EventHandler(this.textBoxDiscountAmount_TextChanged);
      // 
      // textBoxDetailItemName
      // 
      this.textBoxDetailItemName.Enabled = false;
      this.textBoxDetailItemName.Location = new System.Drawing.Point(76, 51);
      this.textBoxDetailItemName.Name = "textBoxDetailItemName";
      this.textBoxDetailItemName.Size = new System.Drawing.Size(285, 20);
      this.textBoxDetailItemName.TabIndex = 1;
      // 
      // textBoxDetailItemPrice
      // 
      this.textBoxDetailItemPrice.Enabled = false;
      this.textBoxDetailItemPrice.Location = new System.Drawing.Point(76, 79);
      this.textBoxDetailItemPrice.Name = "textBoxDetailItemPrice";
      this.textBoxDetailItemPrice.Size = new System.Drawing.Size(100, 20);
      this.textBoxDetailItemPrice.TabIndex = 2;
      // 
      // dataGridViewMasterItemList
      // 
      this.dataGridViewMasterItemList.AllowUserToAddRows = false;
      this.dataGridViewMasterItemList.AllowUserToResizeRows = false;
      this.dataGridViewMasterItemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewMasterItemList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewMasterItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMasterItemList.Enabled = false;
      this.dataGridViewMasterItemList.Location = new System.Drawing.Point(22, 28);
      this.dataGridViewMasterItemList.Name = "dataGridViewMasterItemList";
      this.dataGridViewMasterItemList.RowHeadersVisible = false;
      this.dataGridViewMasterItemList.Size = new System.Drawing.Size(546, 606);
      this.dataGridViewMasterItemList.TabIndex = 0;
      this.dataGridViewMasterItemList.SelectionChanged += new System.EventHandler(this.dataGridViewMasterItemList_SelectionChanged);
      // 
      // buttonUpdate
      // 
      this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonUpdate.BackgroundImage = global::InventoryAndSales.Properties.Resources.Edit;
      this.buttonUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonUpdate.Location = new System.Drawing.Point(591, 81);
      this.buttonUpdate.Name = "buttonUpdate";
      this.buttonUpdate.Size = new System.Drawing.Size(40, 40);
      this.buttonUpdate.TabIndex = 7;
      this.buttonUpdate.UseVisualStyleBackColor = true;
      this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
      // 
      // buttonAdd
      // 
      this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAdd.BackgroundImage = global::InventoryAndSales.Properties.Resources.Add;
      this.buttonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.buttonAdd.Location = new System.Drawing.Point(591, 28);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new System.Drawing.Size(40, 40);
      this.buttonAdd.TabIndex = 6;
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
      // 
      // tabPageReport
      // 
      this.tabPageReport.Controls.Add(this.label5);
      this.tabPageReport.Controls.Add(this.label4);
      this.tabPageReport.Controls.Add(this.label3);
      this.tabPageReport.Controls.Add(this.label2);
      this.tabPageReport.Controls.Add(this.label1);
      this.tabPageReport.Controls.Add(this.dataGridViewLaporan);
      this.tabPageReport.Controls.Add(this.groupBoxReportFilter);
      this.tabPageReport.Location = new System.Drawing.Point(4, 22);
      this.tabPageReport.Name = "tabPageReport";
      this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReport.Size = new System.Drawing.Size(1100, 657);
      this.tabPageReport.TabIndex = 3;
      this.tabPageReport.Text = "Laporan";
      this.tabPageReport.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(19, 410);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(161, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Total Transaksi (Setelah Diskon)";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(19, 297);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(124, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Total Discount Diberikan";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(19, 381);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(88, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Total Roti Terjual";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(19, 351);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(115, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Total Jenis Roti Terjual";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(18, 322);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(166, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Total Transaksi (Sebelum Diskon)";
      // 
      // dataGridViewLaporan
      // 
      this.dataGridViewLaporan.AllowUserToAddRows = false;
      this.dataGridViewLaporan.AllowUserToDeleteRows = false;
      this.dataGridViewLaporan.AllowUserToOrderColumns = true;
      this.dataGridViewLaporan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewLaporan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewLaporan.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.dataGridViewLaporan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLaporan.Location = new System.Drawing.Point(265, 22);
      this.dataGridViewLaporan.Name = "dataGridViewLaporan";
      this.dataGridViewLaporan.ReadOnly = true;
      this.dataGridViewLaporan.RowHeadersVisible = false;
      this.dataGridViewLaporan.Size = new System.Drawing.Size(815, 629);
      this.dataGridViewLaporan.TabIndex = 7;
      // 
      // groupBoxReportFilter
      // 
      this.groupBoxReportFilter.Controls.Add(this.labelLaporanStart);
      this.groupBoxReportFilter.Controls.Add(this.buttonShowReportDetail);
      this.groupBoxReportFilter.Controls.Add(this.dateTimePickerStart);
      this.groupBoxReportFilter.Controls.Add(this.buttonShowReportSummary);
      this.groupBoxReportFilter.Controls.Add(this.dateTimePickerStop);
      this.groupBoxReportFilter.Controls.Add(this.labelLaporanEnd);
      this.groupBoxReportFilter.Location = new System.Drawing.Point(17, 16);
      this.groupBoxReportFilter.Name = "groupBoxReportFilter";
      this.groupBoxReportFilter.Size = new System.Drawing.Size(242, 124);
      this.groupBoxReportFilter.TabIndex = 6;
      this.groupBoxReportFilter.TabStop = false;
      this.groupBoxReportFilter.Text = ",";
      // 
      // labelLaporanStart
      // 
      this.labelLaporanStart.AutoSize = true;
      this.labelLaporanStart.Location = new System.Drawing.Point(25, 26);
      this.labelLaporanStart.Name = "labelLaporanStart";
      this.labelLaporanStart.Size = new System.Drawing.Size(71, 13);
      this.labelLaporanStart.TabIndex = 2;
      this.labelLaporanStart.Text = "Dari Tanggal:";
      // 
      // buttonShowReportDetail
      // 
      this.buttonShowReportDetail.Location = new System.Drawing.Point(121, 84);
      this.buttonShowReportDetail.Name = "buttonShowReportDetail";
      this.buttonShowReportDetail.Size = new System.Drawing.Size(95, 23);
      this.buttonShowReportDetail.TabIndex = 5;
      this.buttonShowReportDetail.Text = "Lihat Detail";
      this.buttonShowReportDetail.UseVisualStyleBackColor = true;
      this.buttonShowReportDetail.Click += new System.EventHandler(this.buttonShowReportDetail_Click);
      // 
      // dateTimePickerStart
      // 
      this.dateTimePickerStart.CustomFormat = "dd  MMM yyyy";
      this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStart.Location = new System.Drawing.Point(104, 22);
      this.dateTimePickerStart.MaxDate = new System.DateTime(2115, 12, 31, 0, 0, 0, 0);
      this.dateTimePickerStart.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerStart.Name = "dateTimePickerStart";
      this.dateTimePickerStart.Size = new System.Drawing.Size(112, 20);
      this.dateTimePickerStart.TabIndex = 0;
      // 
      // buttonShowReportSummary
      // 
      this.buttonShowReportSummary.Location = new System.Drawing.Point(12, 84);
      this.buttonShowReportSummary.Name = "buttonShowReportSummary";
      this.buttonShowReportSummary.Size = new System.Drawing.Size(95, 23);
      this.buttonShowReportSummary.TabIndex = 4;
      this.buttonShowReportSummary.Text = "Lihat Summary";
      this.buttonShowReportSummary.UseVisualStyleBackColor = true;
      this.buttonShowReportSummary.Click += new System.EventHandler(this.buttonShowReportSummary_Click);
      // 
      // dateTimePickerStop
      // 
      this.dateTimePickerStop.CustomFormat = "dd  MMM yyyy";
      this.dateTimePickerStop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStop.Location = new System.Drawing.Point(104, 48);
      this.dateTimePickerStop.MaxDate = new System.DateTime(2115, 12, 31, 0, 0, 0, 0);
      this.dateTimePickerStop.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerStop.Name = "dateTimePickerStop";
      this.dateTimePickerStop.Size = new System.Drawing.Size(112, 20);
      this.dateTimePickerStop.TabIndex = 1;
      // 
      // labelLaporanEnd
      // 
      this.labelLaporanEnd.AutoSize = true;
      this.labelLaporanEnd.Location = new System.Drawing.Point(9, 52);
      this.labelLaporanEnd.Name = "labelLaporanEnd";
      this.labelLaporanEnd.Size = new System.Drawing.Size(87, 13);
      this.labelLaporanEnd.TabIndex = 3;
      this.labelLaporanEnd.Text = "Sampai Tanggal:";
      // 
      // statusStripInformation
      // 
      this.statusStripInformation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusCurrentDate});
      this.statusStripInformation.Location = new System.Drawing.Point(0, 707);
      this.statusStripInformation.Name = "statusStripInformation";
      this.statusStripInformation.Size = new System.Drawing.Size(1108, 22);
      this.statusStripInformation.TabIndex = 2;
      this.statusStripInformation.Text = "statusStrip1";
      // 
      // toolStripStatusCurrentDate
      // 
      this.toolStripStatusCurrentDate.Name = "toolStripStatusCurrentDate";
      this.toolStripStatusCurrentDate.Size = new System.Drawing.Size(77, 17);
      this.toolStripStatusCurrentDate.Text = "{DateDisplay}";
      // 
      // timerDisplayDate
      // 
      this.timerDisplayDate.Enabled = true;
      this.timerDisplayDate.Interval = 1000;
      this.timerDisplayDate.Tick += new System.EventHandler(this.timerDisplayDate_Tick);
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
      this.tabPageProductMaster.ResumeLayout(false);
      this.groupBoxExportImport.ResumeLayout(false);
      this.groupBoxMasterItemDetail.ResumeLayout(false);
      this.groupBoxMasterItemDetail.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMasterItemList)).EndInit();
      this.tabPageReport.ResumeLayout(false);
      this.tabPageReport.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporan)).EndInit();
      this.groupBoxReportFilter.ResumeLayout(false);
      this.groupBoxReportFilter.PerformLayout();
      this.statusStripInformation.ResumeLayout(false);
      this.statusStripInformation.PerformLayout();
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
    private System.Windows.Forms.TabPage tabPageProductMaster;
    private System.Windows.Forms.GroupBox groupBoxItemList;
    private System.Windows.Forms.TextBox textBoxFilter;
    private System.Windows.Forms.Label labelFilter;
    private System.Windows.Forms.DataGridView dataGridViewItemList;
    private System.Windows.Forms.GroupBox groupBoxCart;
    private System.Windows.Forms.DataGridView dataGridViewCart;
    private System.Windows.Forms.GroupBox groupBoxSummary;
    private System.Windows.Forms.TextBox textBoxChanges;
    private System.Windows.Forms.TextBox textBoxPayment;
    private System.Windows.Forms.TextBox textBoxTotal;
    private System.Windows.Forms.Label labelChanges;
    private System.Windows.Forms.Label labelPayment;
    private System.Windows.Forms.Button buttonCheckout;
    private System.Windows.Forms.Label labelTotal;
    private System.Windows.Forms.Label labelNotes;
    private System.Windows.Forms.TextBox textBoxNotes;
    private System.Windows.Forms.Panel panelFilter;
    private System.Windows.Forms.DataGridView dataGridViewMasterItemList;
    private System.Windows.Forms.Button buttonUpdate;
    private System.Windows.Forms.Button buttonAdd;
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
    private System.Windows.Forms.Label labelDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountPercent;
    private System.Windows.Forms.RadioButton radioButtonDiscountAmount;
    private System.Windows.Forms.TextBox textBoxDetailItemDiscountPercent;
    private System.Windows.Forms.Label labelPriceRupiah;
    private System.Windows.Forms.GroupBox groupBoxExportImport;
    private System.Windows.Forms.Label labelDiscountRupiah;
    private System.Windows.Forms.Button buttonSave;
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
    private System.Windows.Forms.Button buttonClearCart;
    private System.Windows.Forms.Label labelCannotLogin;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemQuantity;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemDiscount;
    private System.Windows.Forms.DataGridViewTextBoxColumn CartItemSubtotal;
    private System.Windows.Forms.Button buttonImport;
    private System.Windows.Forms.Button buttonCancelEdit;
    private System.Windows.Forms.Button buttonOkEdit;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
    private System.Windows.Forms.DataGridViewTextBoxColumn ItemPrice;
    private System.Windows.Forms.DataGridViewButtonColumn ItemAdd;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCurrentDate;
    private System.Windows.Forms.Timer timerDisplayDate;
    private System.Windows.Forms.ToolStripMenuItem daftarUserToolStripMenuItem;
    private System.Windows.Forms.TabPage tabPageReport;
    private System.Windows.Forms.ToolStripMenuItem laporanToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem laporanTransaksiToolStripMenuItem;
    private System.Windows.Forms.Button buttonShowReportSummary;
    private System.Windows.Forms.Label labelLaporanEnd;
    private System.Windows.Forms.Label labelLaporanStart;
    private System.Windows.Forms.DateTimePicker dateTimePickerStop;
    private System.Windows.Forms.DateTimePicker dateTimePickerStart;
    private System.Windows.Forms.GroupBox groupBoxReportFilter;
    private System.Windows.Forms.Button buttonShowReportDetail;
    private System.Windows.Forms.DataGridView dataGridViewLaporan;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
  }
}


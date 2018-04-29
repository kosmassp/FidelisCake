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
      this.menuStripMain = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.transaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.penjualanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.printLastReceiptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.printUlangTransaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.daftarBarangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.daftarUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.laporanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.laporanTransaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControlPage = new System.Windows.Forms.TabControl();
      this.tabPageLogin = new System.Windows.Forms.TabPage();
      this.tabPageCashier = new System.Windows.Forms.TabPage();
      this.tabPageProductMaster = new System.Windows.Forms.TabPage();
      this.tabPageReport = new System.Windows.Forms.TabPage();
      this.tabPageUserMaster = new System.Windows.Forms.TabPage();
      this.statusStripInformation = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusCurrentDate = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelEmpty = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelActiveUser = new System.Windows.Forms.ToolStripStatusLabel();
      this.timerDisplayDate = new System.Windows.Forms.Timer(this.components);
      this.loginPage1 = new InventoryAndSales.GUI.Page.LoginPage();
      this.cashierPage1 = new InventoryAndSales.GUI.Page.CashierPage();
      this.masterProductPage1 = new InventoryAndSales.GUI.Page.MasterProductPage();
      this.reportDisplayPage1 = new InventoryAndSales.GUI.Page.ReportDisplayPage();
      this.masterUserPage1 = new InventoryAndSales.GUI.Page.MasterUserPage();
      this.ubahTransaksiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStripMain.SuspendLayout();
      this.tabControlPage.SuspendLayout();
      this.tabPageLogin.SuspendLayout();
      this.tabPageCashier.SuspendLayout();
      this.tabPageProductMaster.SuspendLayout();
      this.tabPageReport.SuspendLayout();
      this.tabPageUserMaster.SuspendLayout();
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
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // transaksiToolStripMenuItem
      // 
      this.transaksiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.penjualanToolStripMenuItem,
            this.printLastReceiptToolStripMenuItem,
            this.printUlangTransaksiToolStripMenuItem,
            this.ubahTransaksiToolStripMenuItem});
      this.transaksiToolStripMenuItem.Name = "transaksiToolStripMenuItem";
      this.transaksiToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
      this.transaksiToolStripMenuItem.Text = "Transaksi";
      // 
      // penjualanToolStripMenuItem
      // 
      this.penjualanToolStripMenuItem.Name = "penjualanToolStripMenuItem";
      this.penjualanToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
      this.penjualanToolStripMenuItem.Text = "Penjualan";
      this.penjualanToolStripMenuItem.Click += new System.EventHandler(this.penjualanToolStripMenuItem_Click);
      // 
      // printLastReceiptToolStripMenuItem
      // 
      this.printLastReceiptToolStripMenuItem.Name = "printLastReceiptToolStripMenuItem";
      this.printLastReceiptToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
      this.printLastReceiptToolStripMenuItem.Text = "Print ulang transaksi terakhir";
      this.printLastReceiptToolStripMenuItem.Click += new System.EventHandler(this.printLastReceiptToolStripMenuItem_Click);
      // 
      // printUlangTransaksiToolStripMenuItem
      // 
      this.printUlangTransaksiToolStripMenuItem.Name = "printUlangTransaksiToolStripMenuItem";
      this.printUlangTransaksiToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
      this.printUlangTransaksiToolStripMenuItem.Text = "Print ulang transaksi ...";
      this.printUlangTransaksiToolStripMenuItem.Click += new System.EventHandler(this.printUlangTransaksiToolStripMenuItem_Click);
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
      this.tabControlPage.Controls.Add(this.tabPageUserMaster);
      this.tabControlPage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlPage.Location = new System.Drawing.Point(0, 24);
      this.tabControlPage.Name = "tabControlPage";
      this.tabControlPage.SelectedIndex = 0;
      this.tabControlPage.Size = new System.Drawing.Size(1108, 683);
      this.tabControlPage.TabIndex = 1;
      // 
      // tabPageLogin
      // 
      this.tabPageLogin.Controls.Add(this.loginPage1);
      this.tabPageLogin.Location = new System.Drawing.Point(4, 22);
      this.tabPageLogin.Name = "tabPageLogin";
      this.tabPageLogin.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageLogin.Size = new System.Drawing.Size(1100, 657);
      this.tabPageLogin.TabIndex = 2;
      this.tabPageLogin.Text = "Login";
      this.tabPageLogin.UseVisualStyleBackColor = true;
      // 
      // tabPageCashier
      // 
      this.tabPageCashier.Controls.Add(this.cashierPage1);
      this.tabPageCashier.Location = new System.Drawing.Point(4, 22);
      this.tabPageCashier.Name = "tabPageCashier";
      this.tabPageCashier.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageCashier.Size = new System.Drawing.Size(1100, 657);
      this.tabPageCashier.TabIndex = 0;
      this.tabPageCashier.Text = "Kasir";
      this.tabPageCashier.UseVisualStyleBackColor = true;
      // 
      // tabPageProductMaster
      // 
      this.tabPageProductMaster.Controls.Add(this.masterProductPage1);
      this.tabPageProductMaster.Location = new System.Drawing.Point(4, 22);
      this.tabPageProductMaster.Name = "tabPageProductMaster";
      this.tabPageProductMaster.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageProductMaster.Size = new System.Drawing.Size(1100, 657);
      this.tabPageProductMaster.TabIndex = 1;
      this.tabPageProductMaster.Text = "Daftar Barang";
      this.tabPageProductMaster.UseVisualStyleBackColor = true;
      // 
      // tabPageReport
      // 
      this.tabPageReport.Controls.Add(this.reportDisplayPage1);
      this.tabPageReport.Location = new System.Drawing.Point(4, 22);
      this.tabPageReport.Name = "tabPageReport";
      this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReport.Size = new System.Drawing.Size(1100, 657);
      this.tabPageReport.TabIndex = 3;
      this.tabPageReport.Text = "Laporan";
      this.tabPageReport.UseVisualStyleBackColor = true;
      // 
      // tabPageUserMaster
      // 
      this.tabPageUserMaster.Controls.Add(this.masterUserPage1);
      this.tabPageUserMaster.Location = new System.Drawing.Point(4, 22);
      this.tabPageUserMaster.Name = "tabPageUserMaster";
      this.tabPageUserMaster.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageUserMaster.Size = new System.Drawing.Size(1100, 657);
      this.tabPageUserMaster.TabIndex = 4;
      this.tabPageUserMaster.Text = "Daftar User";
      this.tabPageUserMaster.UseVisualStyleBackColor = true;
      // 
      // statusStripInformation
      // 
      this.statusStripInformation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusCurrentDate,
            this.toolStripStatusLabelEmpty,
            this.toolStripStatusLabelActiveUser});
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
      // toolStripStatusLabelEmpty
      // 
      this.toolStripStatusLabelEmpty.Name = "toolStripStatusLabelEmpty";
      this.toolStripStatusLabelEmpty.Size = new System.Drawing.Size(945, 17);
      this.toolStripStatusLabelEmpty.Spring = true;
      // 
      // toolStripStatusLabelActiveUser
      // 
      this.toolStripStatusLabelActiveUser.Name = "toolStripStatusLabelActiveUser";
      this.toolStripStatusLabelActiveUser.Size = new System.Drawing.Size(71, 17);
      this.toolStripStatusLabelActiveUser.Text = "{ActiveUser}";
      // 
      // timerDisplayDate
      // 
      this.timerDisplayDate.Enabled = true;
      this.timerDisplayDate.Interval = 1000;
      this.timerDisplayDate.Tick += new System.EventHandler(this.timerDisplayDate_Tick);
      // 
      // loginPage1
      // 
      this.loginPage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.loginPage1.Location = new System.Drawing.Point(3, 3);
      this.loginPage1.Name = "loginPage1";
      this.loginPage1.Size = new System.Drawing.Size(1094, 651);
      this.loginPage1.TabIndex = 0;
      // 
      // cashierPage1
      // 
      this.cashierPage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cashierPage1.Location = new System.Drawing.Point(3, 3);
      this.cashierPage1.Name = "cashierPage1";
      this.cashierPage1.Size = new System.Drawing.Size(1094, 651);
      this.cashierPage1.TabIndex = 0;
      // 
      // masterProductPage1
      // 
      this.masterProductPage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.masterProductPage1.Location = new System.Drawing.Point(3, 3);
      this.masterProductPage1.Name = "masterProductPage1";
      this.masterProductPage1.Size = new System.Drawing.Size(1094, 651);
      this.masterProductPage1.TabIndex = 0;
      // 
      // reportDisplayPage1
      // 
      this.reportDisplayPage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.reportDisplayPage1.Location = new System.Drawing.Point(3, 3);
      this.reportDisplayPage1.Name = "reportDisplayPage1";
      this.reportDisplayPage1.Size = new System.Drawing.Size(1094, 651);
      this.reportDisplayPage1.TabIndex = 0;
      // 
      // masterUserPage1
      // 
      this.masterUserPage1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.masterUserPage1.Location = new System.Drawing.Point(3, 3);
      this.masterUserPage1.Name = "masterUserPage1";
      this.masterUserPage1.Size = new System.Drawing.Size(1094, 651);
      this.masterUserPage1.TabIndex = 0;
      // 
      // ubahTransaksiToolStripMenuItem
      // 
      this.ubahTransaksiToolStripMenuItem.Name = "ubahTransaksiToolStripMenuItem";
      this.ubahTransaksiToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
      this.ubahTransaksiToolStripMenuItem.Text = "Ubah transaksi ...";
      this.ubahTransaksiToolStripMenuItem.Click += new System.EventHandler(this.ubahTransaksiToolStripMenuItem_Click);
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
      this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
      this.menuStripMain.ResumeLayout(false);
      this.menuStripMain.PerformLayout();
      this.tabControlPage.ResumeLayout(false);
      this.tabPageLogin.ResumeLayout(false);
      this.tabPageCashier.ResumeLayout(false);
      this.tabPageProductMaster.ResumeLayout(false);
      this.tabPageReport.ResumeLayout(false);
      this.tabPageUserMaster.ResumeLayout(false);
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
    private System.Windows.Forms.TabPage tabPageLogin;
    private System.Windows.Forms.StatusStrip statusStripInformation;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCurrentDate;
    private System.Windows.Forms.Timer timerDisplayDate;
    private System.Windows.Forms.ToolStripMenuItem daftarUserToolStripMenuItem;
    private System.Windows.Forms.TabPage tabPageReport;
    private System.Windows.Forms.ToolStripMenuItem laporanToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem laporanTransaksiToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelActiveUser;
    private System.Windows.Forms.TabPage tabPageUserMaster;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelEmpty;
    private System.Windows.Forms.ToolStripMenuItem printLastReceiptToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem printUlangTransaksiToolStripMenuItem;
    private InventoryAndSales.GUI.Page.LoginPage loginPage1;
    private InventoryAndSales.GUI.Page.CashierPage cashierPage1;
    private InventoryAndSales.GUI.Page.MasterProductPage masterProductPage1;
    private InventoryAndSales.GUI.Page.ReportDisplayPage reportDisplayPage1;
    private InventoryAndSales.GUI.Page.MasterUserPage masterUserPage1;
    private System.Windows.Forms.ToolStripMenuItem ubahTransaksiToolStripMenuItem;
  }
}


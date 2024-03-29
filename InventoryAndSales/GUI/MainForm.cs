﻿using InventoryAndSales.Enumeration;
using InventoryAndSales.GUI.Popup;
using InventoryAndSales.GUI.Util;
using SimpleCommon.Utility;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace InventoryAndSales.GUI
{
  public partial class MainForm : Form
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private MainFormController controller;

    private DisplayPage currentPage;

    public MainForm()
    {
      CultureInfo.DefaultThreadCurrentCulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      CultureInfo.DefaultThreadCurrentUICulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

      InitializeComponent();
      Version version = Assembly.GetEntryAssembly().GetName().Version;
      Text = Text + $" [version: {version}]";
      ControlUtility.HideTabHeader(tabControlPage);
      controller = new MainFormController(this);
      KeyPreview = true;
    }

    public void EnableMenu(int role)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<int>(EnableMenu), role);
        return;
      }
      transaksiToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Cashier);
      editToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Master);
      laporanToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Admin);
      checkKasirToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Cashier);
    }

    public void LoadCashierPage()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(LoadCashierPage));
        return;
      }
      tabControlPage.SelectedTab = tabPageCashier;
      currentPage = DisplayPage.Cashier;
      cashierPage1.Reset();
    }

    public void LoadLoginPage()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(LoadLoginPage));
        return;
      }
      tabControlPage.SelectedTab = tabPageLogin;
      currentPage = DisplayPage.Login;
      controller.Logout();
      loginPage1.Reset();
    }

    public void LoadProductMasterPage()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(LoadProductMasterPage));
        return;
      }
      tabControlPage.SelectedTab = tabPageProductMaster;
      currentPage = DisplayPage.MasterProduct;
      masterProductPage1.Reset();
    }

    public void LoadUserMasterPage()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(LoadUserMasterPage));
        return;
      }
      tabControlPage.SelectedTab = tabPageUserMaster;
      currentPage = DisplayPage.MasterUser;
      masterUserPage1.Reset();
    }

    public void UpdateActiveUser(string name)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<string>(UpdateActiveUser), name);
        return;
      }
      toolStripStatusLabelActiveUser.Text = string.Format("ActiveUser={0}", string.IsNullOrEmpty(name) ? "<None>" : name);
    }

    private void daftarBarangToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadProductMasterPage();
    }

    private void daftarUserToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadUserMasterPage();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void hapusTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (controller.RequestDeleteTransaction())
          MessageBox.Show("Transaksi dihapus.");
      }
      catch (Exception ex)
      {
        _log.Error(ex);
        MessageBox.Show("Terdapat kesalahan sistem. Tolong check kembali. ");
      }
      LoadCashierPage();
    }

    private void jumlahSetoranToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string total = controller.GetCurrentDayTotalTransaction();
      StringBuilder messageBuilder = new StringBuilder();
      messageBuilder.AppendLine("Tanggal: " + DateTime.Today.ToString("dd/MMM/yyyy"));
      messageBuilder.AppendLine("Jam: " + DateTime.Now.ToString("HH:mm:ss"));
      messageBuilder.AppendLine("Total Transaksi : " + total);
      messageBuilder.AppendLine();
      messageBuilder.AppendLine();
      messageBuilder.AppendLine("Jika terdapat perubahan transaksi, Jumlah kemungkinan tidak sesuai.");
      MessageBox.Show(messageBuilder.ToString(), "Jumlah Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void laporanTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageReport;
      reportDisplayPage1.RefreshOnDisplay();
    }

    private void loginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadLoginPage();
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (currentPage == DisplayPage.Cashier)
      {
        Keys keyCode = e.KeyCode;
        switch (keyCode)
        {
          case Keys.F5:
            cashierPage1.FocusFilter();
            break;

          case Keys.F6:
            cashierPage1.FocusPayment();
            break;

          case Keys.F7:
            cashierPage1.FocusCheckout();
            break;
        }
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      LoadLoginPage();
    }
    private void pengaturanToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SettingForm settingForm = new SettingForm();
      settingForm.Show();
    }

    private void penjualanToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadCashierPage();
    }
    private void printLastReceiptToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!controller.PrintLastReceipt())
        MessageBox.Show("Transaksi terakhir tidak ada", "Gagal Print", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
    }

    private void printUlangTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!controller.PrintReceipt())
      {
        MessageBox.Show("No Faktur tidak ditemukan");
      }
    }

    private void timerDisplayDate_Tick(object sender, EventArgs e)
    {
      toolStripStatusCurrentDate.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
    }
    private void ubahTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      controller.RequestUpdateTransaction();
      LoadCashierPage();
    }
  }
}
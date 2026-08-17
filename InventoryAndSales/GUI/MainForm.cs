using InventoryAndSales.Enumeration;
using InventoryAndSales.Business;
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
      ControlUtility.HideTabHeader(tabControlPage);
      controller = new MainFormController(this);
      RefreshWindowTitle();
      KeyPreview = true;
    }

    /// <summary>
    /// Titles the window with the shop's own name. Called again when the settings dialog closes, so
    /// a rename shows without restarting the till.
    /// </summary>
    public void RefreshWindowTitle()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(RefreshWindowTitle));
        return;
      }
      Version version = Assembly.GetEntryAssembly().GetName().Version;
      Text = $"{controller.GetShopName()} [version: {version}]";
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
      // Gated on Laporan, not Admin. It used to test the Admin bit, which meant a Supervisor held
      // the Laporan permission but still could not open the reports menu.
      laporanToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Laporan);
      checkKasirToolStripMenuItem.Visible = BusinessUtil.AllowedRole(role, AccessOption.Cashier);
    }

    public void LoadCashierPage()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(LoadCashierPage));
        return;
      }
      // Every page switch is logged: reconstructing what the operator was doing when something went
      // wrong is most of the work of answering a report from the shop.
      _log.Info("Navigating to the cashier page.");
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
      _log.Info("Navigating to the login page.");
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
      _log.Info("Navigating to the product master page.");
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
      _log.Info("Navigating to the user master page.");
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
      // Kept in step with the visible tab so the cashier hotkeys stop firing while reports are up.
      currentPage = DisplayPage.Report;
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

        // Ctrl+1/2/3 pick the payment method without leaving the keyboard. Handled before the
        // switch because they are chords, and because the digit keys alone belong to the filter box.
        if (e.Control)
        {
          switch (keyCode)
          {
            case Keys.D1:
            case Keys.NumPad1:
              cashierPage1.SelectPaymentMethod(PaymentMethod.Cash);
              e.Handled = true;
              return;

            case Keys.D2:
            case Keys.NumPad2:
              cashierPage1.SelectPaymentMethod(PaymentMethod.Edc);
              e.Handled = true;
              return;

            case Keys.D3:
            case Keys.NumPad3:
              cashierPage1.SelectPaymentMethod(PaymentMethod.Qris);
              e.Handled = true;
              return;
          }
        }

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
      // The dialog is modeless, so the title is refreshed when it closes rather than after Show.
      settingForm.FormClosed += (s, args) => RefreshWindowTitle();
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
      try
      {
        controller.RequestUpdateTransaction();
      }
      catch (Exception ex)
      {
        _log.Error(ex);
        MessageBox.Show("Terdapat kesalahan sistem. Tolong check kembali. ");
      }
      LoadCashierPage();
    }
  }
}
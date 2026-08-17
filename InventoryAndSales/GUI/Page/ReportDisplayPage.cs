using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Page
{
  public partial class ReportDisplayPage : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private ReportDisplayController controller;
    public ReportDisplayPage()
    {
      InitializeComponent();
      controller = new ReportDisplayController(this);
    }

    private void buttonShowReportSummary_Click(object sender, EventArgs e)
    {
      RunReport(() =>
      {
        tabControlSummaryReport.TabPages.Clear();
        tabControlSummaryReport.TabPages.Add(tabPageReportPerCashier);
        tabControlSummaryReport.TabPages.Add(tabPageReportPerProduct);
        tabControlSummaryReport.TabPages.Add(tabPageReportPerTransaction);
        tabControlSummaryReport.TabPages.Add(tabPageReportPerPayment);
        controller.ShowSummaryReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
        tabControlSummaryReport.SelectedTab = tabPageReportPerCashier;
        if (checkBox1.Checked)
        {
          tabControlSummaryReport.TabPages.Add(tabPageReportDetail);
          controller.ShowDetailReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
        }
      });
    }

    /// <summary>
    /// Runs a report with an hourglass and a readable message if it fails. A report is read only, so
    /// a failure here is never worse than "try again".
    /// </summary>
    private void RunReport(Action action)
    {
      Cursor previous = Cursor;
      Cursor = Cursors.WaitCursor;
      try
      {
        action();
      }
      catch (Exception ex)
      {
        _log.Error("Report failed.", ex);
        MessageBox.Show(
          "Laporan gagal dibuat. Pastikan koneksi ke database masih tersedia, lalu coba lagi.",
          "Gagal Membuat Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        Cursor = previous;
      }
    }

    /// <summary>
    /// Binds the three summary grids. Named parameters rather than an array, because the array
    /// version relied on the controller building it in exactly the right order.
    /// </summary>
    public void UpdateReportDataGridView(DataTable byProduct, DataTable byTransaction, DataTable byCashier,
                                         DataTable byPayment)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new Action<DataTable, DataTable, DataTable, DataTable>(UpdateReportDataGridView),
                         byProduct, byTransaction, byCashier, byPayment);
        return;
      }
      dataGridViewLaporanProduct.DataSource = byProduct;
      dataGridViewLaporanTransaksi.DataSource = byTransaction;
      dataGridViewLaporanKasir.DataSource = byCashier;
      dataGridViewLaporanPembayaran.DataSource = byPayment;
    }

    public void UpdateReportDetailDataGridView(DataTable dataTable)
    {
      if (InvokeRequired)
      {
        // Marshalled to this method. It used to hand off to the three-grid overload, which would
        // have bound the wrong grids had it ever been called from another thread.
        this.BeginInvoke(new DelegateUtility.OneValueHandler<DataTable>(UpdateReportDetailDataGridView), dataTable);
        return;
      }
      dataGridViewLaporanDetail.DataSource = dataTable;
    }

    private void buttonReportPerKasir_Click(object sender, EventArgs e)
    {
      RunReport(() => controller.ShowSummaryReportPerKasir(dateTimePickerStart.Value, dateTimePickerStop.Value));
    }

    private void buttonReportPerTransaksi_Click(object sender, EventArgs e)
    {
      RunReport(() => controller.ShowSummaryReportPerTransaksi(dateTimePickerStart.Value, dateTimePickerStop.Value));
    }

    private void buttonReportPerProduct_Click(object sender, EventArgs e)
    {
      RunReport(() => controller.ShowSummaryReportPerProduct(dateTimePickerStart.Value, dateTimePickerStop.Value));
    }

    private void buttonReportPerItem_Click(object sender, EventArgs e)
    {
      RunReport(() => controller.ShowSummaryReportPerDetail(dateTimePickerStart.Value, dateTimePickerStop.Value));
    }

    private void buttonReportPerPembayaran_Click(object sender, EventArgs e)
    {
      RunReport(() => controller.ShowSummaryReportPerPembayaran(dateTimePickerStart.Value, dateTimePickerStop.Value));
    }

    public void RefreshOnDisplay()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(RefreshOnDisplay));
        return;
      }
      dateTimePickerStart.Value = DateTime.Today;
      dateTimePickerStop.Value = DateTime.Today;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;
using InventoryAndSales.Utility;

namespace InventoryAndSales.GUI.Controller
{
  public class ReportDisplayController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly ReportDisplayPage control;
    private readonly ReportManager _reportManager;
    private readonly ReportService _reportService;
    private readonly ShopService _shop;
    private readonly LoginManager _loginManager;

    public ReportDisplayController(ReportDisplayPage reportDisplayPage)
    {
      control = reportDisplayPage;
      BusinessFactory factory = BusinessFactory.GetInstance();
      _reportManager = factory.ReportManager;
      _reportService = factory.ReportService;
      _shop = factory.Shop;
      _loginManager = factory.LoginManager;
    }

    public void ShowSummaryReport(DateTime start, DateTime stop)
    {
      DataTable dataTableSummaryCashier =
        DataTableUtil.GetDataTable(_reportManager.GetReportSummaryByCashier(start, stop), "SummaryReportCashier");

      DataTable dataTableSummaryTransaction =
        DataTableUtil.GetDataTable(_reportManager.GetReportSummaryByTransaction(start, stop), "SummaryReportTransaction");

      DataTable dataTableSummaryProduct =
        DataTableUtil.GetDataTable(_reportManager.GetSummaryReportProduct(start, stop), "SummaryReportProduct");

      DataTable dataTableSummaryPayment =
        DataTableUtil.GetDataTable(_reportManager.GetReportSummaryByPaymentMethod(start, stop), "SummaryReportPayment");

      control.UpdateReportDataGridView(dataTableSummaryProduct, dataTableSummaryTransaction,
                                       dataTableSummaryCashier, dataTableSummaryPayment);
    }

    public void ShowDetailReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> detailReport = _reportManager.GetDetailReport(start, stop);
      DataTable dataTableDetail = DataTableUtil.GetDataTable(detailReport, "DetailReport");

      control.UpdateReportDetailDataGridView(dataTableDetail);
    }

    public void ShowSummaryReportPerKasir(DateTime start, DateTime stop)
    {
      WriteAndOpen(_reportManager.GetReportSummaryByCashier(start, stop),
                   "SBC", "TableSummaryByCashier", "Laporan Per Kasir", start, stop);
    }

    public void ShowSummaryReportPerTransaksi(DateTime start, DateTime stop)
    {
      WriteAndOpen(_reportManager.GetReportSummaryByTransaction(start, stop),
                   "SBT", "TableSummaryByTransaction", "Laporan Per Transaksi", start, stop);
    }

    public void ShowSummaryReportPerProduct(DateTime start, DateTime stop)
    {
      WriteAndOpen(_reportManager.GetSummaryReportProduct(start, stop),
                   "SRP", "ReportPerProduct", "Laporan Penjualan Barang", start, stop);
    }

    public void ShowSummaryReportPerDetail(DateTime start, DateTime stop)
    {
      WriteAndOpen(_reportManager.GetDetailReport(start, stop),
                   "RDP", "DetailReport", "Laporan Detail Per Item", start, stop);
    }

    public void ShowSummaryReportPerPembayaran(DateTime start, DateTime stop)
    {
      WriteAndOpen(_reportManager.GetReportSummaryByPaymentMethod(start, stop),
                   "SBP", "TableSummaryByPayment", "Laporan Metode Pembayaran", start, stop);
    }

    private static string BuildFileName(string prefix, DateTime start, DateTime stop)
    {
      return string.Format("{0}{1}_{2}.html", prefix, start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
    }

    /// <summary>
    /// Writes the report into the configured folder, unpacks the DataTables assets beside it if they
    /// are not already there, then opens it.
    /// </summary>
    private void WriteAndOpen(List<Dictionary<string, string>> dataReport, string filePrefix, string tableId,
                              string title, DateTime start, DateTime stop)
    {
      if (dataReport == null || dataReport.Count == 0)
      {
        MessageBox.Show("Tidak ada data pada rentang tanggal tersebut.", "Laporan Kosong",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      string directory;
      try
      {
        directory = _reportService.PrepareReportDirectory();
      }
      catch (Exception e)
      {
        _log.Error("Could not create the report directory.", e);
        MessageBox.Show(
          "Folder laporan tidak dapat dibuat. Silahkan pilih folder lain melalui menu Pengaturan.",
          "Gagal Membuat Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      bool hasAssets = _reportService.EnsureAssets(directory);

      ReportDocument document = new ReportDocument(title, _shop.GetName(), start, stop,
                                                   GetOperatorName(), DateTime.Now,
                                                   ReportTable.From(dataReport));

      string fullPath = Path.Combine(directory, BuildFileName(filePrefix, start, stop));
      try
      {
        HtmlReportGenerator.Write(
          document, tableId, fullPath,
          hasAssets ? ReportService.StyleSheetHref : null,
          hasAssets ? ReportService.ScriptSrc : null);
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not write report '{0}'.", fullPath), e);
        MessageBox.Show(
          "Laporan gagal disimpan. Pastikan file laporan tidak sedang dibuka dan folder dapat ditulis.",
          "Gagal Membuat Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      if (!hasAssets)
      {
        MessageBox.Show(
          "Laporan dibuat, namun file pendukung tidak ditemukan sehingga fitur urut, cari dan export tidak aktif." +
          Environment.NewLine + Environment.NewLine +
          "Pastikan folder '" + ReportService.AssetSourceFolderName + "' berada di folder aplikasi.",
          "Laporan Terbatas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      _log.InfoFormat("Report '{0}' written to '{1}': {2} rows for {3} to {4}.",
                      title, fullPath, document.Table.RowCount,
                      start.ToString("yyyy-MM-dd"), stop.ToString("yyyy-MM-dd"));
      OpenReport(fullPath);
    }

    /// <summary>Who asked for the report. Empty rather than a guess if nobody is signed in.</summary>
    private string GetOperatorName()
    {
      User activeUser = _loginManager.ActiveUser;
      return activeUser == null ? string.Empty : activeUser.Name;
    }

    private static void OpenReport(string fullPath)
    {
      try
      {
        System.Diagnostics.Process.Start(fullPath);
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not open report '{0}'.", fullPath), e);
        MessageBox.Show(
          "Laporan tersimpan di:" + Environment.NewLine + fullPath + Environment.NewLine + Environment.NewLine +
          "Namun tidak dapat dibuka otomatis. Silahkan buka manual.",
          "Laporan Tersimpan", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }
  }
}

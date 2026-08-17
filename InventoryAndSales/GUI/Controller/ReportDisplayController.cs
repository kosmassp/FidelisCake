using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Page;
using InventoryAndSales.Utility;
using SimpleCommon.UI;

namespace InventoryAndSales.GUI.Controller
{
  public class ReportDisplayController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly ReportDisplayPage control;
    private readonly ReportManager _reportManager;
    private readonly ReportService _reportService;

    public ReportDisplayController(ReportDisplayPage reportDisplayPage)
    {
      control = reportDisplayPage;
      _reportManager = BusinessFactory.GetInstance().ReportManager;
      _reportService = BusinessFactory.GetInstance().ReportService;
    }

    public void ShowSummaryReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier(start, stop);
      DataTable dataTableSummaryCashier = DataTableUtil.GetDataTable(reportSummaryByCashier, "SummaryReportCashier");

      List<Dictionary<string, string>> reportSummaryByTransaction = _reportManager.GetReportSummaryByTransaction(start, stop);
      DataTable dataTableSummaryTransaction = DataTableUtil.GetDataTable(reportSummaryByTransaction, "SummaryReportTransaction");

      List<Dictionary<string, string>> summaryReport = _reportManager.GetSummaryReportProduct(start, stop);
      DataTable dataTableSummaryProduct = DataTableUtil.GetDataTable(summaryReport, "SummaryReportProduct");

      control.UpdateReportDataGridView(dataTableSummaryProduct, dataTableSummaryTransaction, dataTableSummaryCashier);
    }

    public void ShowDetailReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> detailReport = _reportManager.GetDetailReport(start, stop);
      DataTable dataTableDetail = DataTableUtil.GetDataTable(detailReport, "DetailReport");

      control.UpdateReportDetailDataGridView(dataTableDetail);
    }

    public void ShowSummaryReportPerKasir(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> report = _reportManager.GetReportSummaryByCashier(start, stop);
      ShowSummaryReportInHtml(report, BuildFileName("SBC", start, stop), "TableSummaryByCashier", "Cashier Report");
    }

    public void ShowSummaryReportPerTransaksi(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> report = _reportManager.GetReportSummaryByTransaction(start, stop);
      ShowSummaryReportInHtml(report, BuildFileName("SBT", start, stop), "TableSummaryByTransaction", "Transaction Report");
    }

    public void ShowSummaryReportPerProduct(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> report = _reportManager.GetSummaryReportProduct(start, stop);
      ShowSummaryReportInHtml(report, BuildFileName("SRP", start, stop), "ReportPerProduct", "Product Sales Report");
    }

    public void ShowSummaryReportPerDetail(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> report = _reportManager.GetDetailReport(start, stop);
      ShowSummaryReportInHtml(report, BuildFileName("RDP", start, stop), "DetailReport", "Detail Report");
    }

    private static string BuildFileName(string prefix, DateTime start, DateTime stop)
    {
      return string.Format("{0}{1}_{2}.html", prefix, start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
    }

    /// <summary>
    /// Writes the report into the configured folder, unpacks the DataTables assets beside it if they
    /// are not already there, then opens it.
    /// </summary>
    public void ShowSummaryReportInHtml(List<Dictionary<string, string>> dataReport, string filename, string id, string title)
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

      string[] headers = dataReport[0].Keys.ToArray();
      List<string[]> dataRows = dataReport.Select(row => row.Values.ToArray()).ToList();
      string table = HtmlTableGenerator.GenerateTable(id, headers, dataRows);

      string fullPath = Path.Combine(directory, filename);
      try
      {
        HtmlReportGenerator.Write(
          title, table, fullPath,
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

      OpenReport(fullPath);
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

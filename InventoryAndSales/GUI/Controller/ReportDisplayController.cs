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
    private ReportDisplayPage control;

    private ReportManager _reportManager;
    public ReportDisplayController(ReportDisplayPage reportDisplayPage)
    {
      control = reportDisplayPage;
      _reportManager = BusinessFactory.GetInstance().ReportManager;
      //tabControlSummaryReport.TabPages.Clear();

    }


    public void ShowSummaryReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier(start, stop);
      DataTable dataTableSummaryCashier = DataTableUtil.GetDataTable(reportSummaryByCashier, "SummaryReportCashier");

      List<Dictionary<string, string>> reportSummaryByTransaction = _reportManager.GetReportSummaryByTransaction(start, stop);
      DataTable dataTableSummaryTransaction = DataTableUtil.GetDataTable(reportSummaryByTransaction, "SummaryReportTransaction");

      List<Dictionary<string, string>> summaryReport = _reportManager.GetSummaryReportProduct(start, stop);
      DataTable dataTableSummaryProduct = DataTableUtil.GetDataTable(summaryReport, "SummaryReportProduct");

      control.UpdateReportDataGridView(new DataTable[] { dataTableSummaryProduct, dataTableSummaryTransaction, dataTableSummaryCashier });

    }

    public void ShowDetailReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> detailReport = _reportManager.GetDetailReport(start, stop);
      DataTable dataTableDetail = DataTableUtil.GetDataTable(detailReport, "DetailReport");

      control.UpdateReportDetailDataGridView(dataTableDetail);
    }


    public void ShowSummaryReportPerKasir(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier(start, stop);
      string filename = string.Format("SBC{0}_{1}.html", start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
      ShowSummaryReportInHtml(reportSummaryByCashier, filename, "TableSummaryByCashier", "Cashier Report");
    }

    public void ShowSummaryReportPerTransaksi(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> byTransaction = _reportManager.GetReportSummaryByTransaction(start, stop);
      string filename = string.Format("SBT{0}_{1}.html", start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
      ShowSummaryReportInHtml(byTransaction, filename, "TableSummaryByTransaction", "Transaction Report");
    }

    public void ShowSummaryReportPerProduct(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetSummaryReportProduct(start, stop);
      string filename = string.Format("SRP{0}_{1}.html", start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
      ShowSummaryReportInHtml(reportSummaryByCashier, filename, "ReportPerProduct", "Product Sales Report");
    }

    public void ShowSummaryReportPerDetail(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetDetailReport(start, stop);
      string filename = string.Format("RDP{0}_{1}.html", start.ToString("yyyyMMdd"), stop.ToString("yyyyMMdd"));
      ShowSummaryReportInHtml(reportSummaryByCashier, filename, "DetailReport", "Detail Report");
    }

    public void ShowSummaryReportInHtml(List<Dictionary<string, string>> dataReport, string filename, string id, string title)
    {
      if (dataReport.Count > 0)
      {
        string[] headers = dataReport[0].Keys.ToArray();
        List<string[]> dataRows = new List<string[]>();
        foreach (Dictionary<string, string> dictionary in dataReport)
        {
          dataRows.Add(dictionary.Values.ToArray());
        }
        string table = HtmlTableGenerator.GenerateTable(id, headers, dataRows);
        string directory = "c:\\temp\\Report\\";
        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        string fullPath = Path.Combine(directory, filename);
        HtmlReportGenerator.Write(title, table, fullPath);
        System.Diagnostics.Process.Start(fullPath);
      }
    }
  }
}
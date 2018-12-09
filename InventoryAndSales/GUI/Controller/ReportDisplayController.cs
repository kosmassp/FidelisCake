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


    public void ShowSummaryReportInHtml( DateTime start, DateTime stop )
    {
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier( start, stop );
      string filename = string.Format("SBC{0}_{1}.html",start.ToString("yyyyMMdd"),stop.ToString("yyyyMMdd"));
      if( reportSummaryByCashier.Count > 0 )
      {
        string[] headers = reportSummaryByCashier[0].Keys.ToArray();
        List<string[]> dataRows = new List<string[]>();
        foreach( Dictionary<string, string> dictionary in reportSummaryByCashier )
        {
          dataRows.Add(dictionary.Values.ToArray());
        }
        string tableSBC = HtmlTableGenerator.GenerateTable("TableSummaryByCashier", headers, dataRows);
        string fullPath = Path.Combine("c:\\temp\\Report\\", filename);
        HtmlReportGenerator.Write("Cashier Report", tableSBC, fullPath);
        System.Diagnostics.Process.Start(fullPath);

      }

    }
  }
}
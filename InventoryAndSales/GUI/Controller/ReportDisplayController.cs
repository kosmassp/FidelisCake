﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Page;

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



  }
}
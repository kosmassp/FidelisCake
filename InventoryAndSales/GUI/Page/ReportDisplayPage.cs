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
    private ReportDisplayController controller;
    public ReportDisplayPage()
    {
      InitializeComponent();
      controller = new ReportDisplayController(this);
    }

    private void buttonShowReportSummary_Click(object sender, EventArgs e)
    {
      tabControlSummaryReport.TabPages.Clear();
      tabControlSummaryReport.TabPages.Add(tabPageReportPerCashier);
      tabControlSummaryReport.TabPages.Add(tabPageReportPerProduct);
      tabControlSummaryReport.TabPages.Add(tabPageReportPerTransaction);
      controller.ShowSummaryReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
      tabControlSummaryReport.SelectedTab = tabPageReportPerCashier;
      if (checkBox1.Checked)
      {
        tabControlSummaryReport.TabPages.Add(tabPageReportDetail);
        controller.ShowDetailReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
      }
    }

    public void UpdateReportDataGridView(DataTable[] dataTables)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueArrayHandler<DataTable>(UpdateReportDataGridView), dataTables);
        return;
      }
      dataGridViewLaporanProduct.DataSource = dataTables[0];
      dataGridViewLaporanTransaksi.DataSource = dataTables[1];
      dataGridViewLaporanKasir.DataSource = dataTables[2];
      //HtmlDocument      
    }

    public void UpdateReportDetailDataGridView(DataTable dataTable)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueArrayHandler<DataTable>(UpdateReportDataGridView), dataTable);
        return;
      }
      dataGridViewLaporanDetail.DataSource = dataTable;
    }

    private void buttonReportHtml_Click( object sender, EventArgs e )
    {
      controller.ShowSummaryReportInHtml( dateTimePickerStart.Value, dateTimePickerStop.Value );
    }
  }
}

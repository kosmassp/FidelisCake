using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI
{
  public partial class TransactionHistory : Form
  {
    private ViewManager _viewManager;
    public string SelectedTransactionId { get; private set; }
    public string SelectedTransactionFactur { get; private set; }
    public TransactionHistory()
    {
      InitializeComponent();
      _viewManager = BusinessFactory.GetInstance().ViewManager;
      this.DialogResult = DialogResult.Cancel;
      dateTimePickerFrom.Value = DateTime.Now;
      dateTimePickerTo.Value = DateTime.Now;
    }

    private void buttonSearch_Click(object sender, EventArgs e)
    {
      List<Dictionary<string, string>> summaryReport = _viewManager.GetTransaction(dateTimePickerFrom.Value, dateTimePickerTo.Value);
      DataTable dataTableSummaryProduct = DataTableUtil.GetDataTable(summaryReport, "Transaction History");
      UpdateDataGridView(dataTableSummaryProduct);
    }


    public void UpdateDataGridView(DataTable dataTable)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<DataTable>(UpdateDataGridView), dataTable);
        return;
      }
      dataGridViewSearch.DataSource = dataTable;
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void buttonNext_Click(object sender, EventArgs e)
    {
      DataGridViewSelectedRowCollection selectedRows = dataGridViewSearch.SelectedRows;
      if (selectedRows.Count != 1)
      {
        MessageBox.Show("Silahkan pilih 1 data");
      }
      else
      {
        DataGridViewRow selectedTransaction = selectedRows[0];
        SelectedTransactionId = selectedTransaction.Cells["Id"].Value.ToString();
        SelectedTransactionFactur = selectedTransaction.Cells["Factur"].Value.ToString();
        this.DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}

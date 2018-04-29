using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.GUI.Popup
{
  public partial class TransactionUpdateForm : Form
  {
    public TransactionUpdateForm(string facturNumber, User supervisor)
    {
      InitializeComponent();
      transactionUpdatePage1.Init(facturNumber, supervisor);
    }

    private void transactionUpdatePage1_BackClick(object sender, EventArgs e)
    {
      Close();
    }

    private void transactionUpdatePage1_CheckoutSucceed(object sender, EventArgs e)
    {
      Close();
    }

    private void TransactionUpdateForm_FormClosing(object sender, FormClosingEventArgs e)
    {

    }
  }
}

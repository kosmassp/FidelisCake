using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventoryAndSales.GUI
{
  public partial class ReprintReceipt : Form
  {
    public string FacturNumber;
    public bool IsPrintPressed;
    public ReprintReceipt()
    {
      InitializeComponent();

    }

    private void buttonPrint_Click(object sender, EventArgs e)
    {
      FacturNumber = textBoxFacturNumber.Text;
      IsPrintPressed = true;
      Close();
    }

  }
}

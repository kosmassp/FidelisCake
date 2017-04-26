using System;
using System.ComponentModel;
using System.Windows.Forms;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI
{
  public partial class MainForm : Form
  {
    private MainFormController controller;

    public MainForm()
    {
      InitializeComponent();
      ControlUtility.HideTabHeader(tabControlPage);
      controller = new MainFormController(this);
      controller.Initialize();

    }

    private void buttonCheckout_Click(object sender, System.EventArgs e)
    {
      string validationMsg = ValidateInput();
      if (!string.IsNullOrEmpty(validationMsg))
      {
        MessageBox.Show(validationMsg);
        return;
      }
      string errorMessage = controller.Checkout(decimal.Parse(textBoxPayment.Text), textBoxNotes.Text);
      if(string.IsNullOrEmpty(errorMessage))
      {
        controller.PrintPaymentNote();
        controller.NewCart();
      }
      else
      {
        MessageBox.Show(string.Format("{0}\n{1}", errorMessage, "Silahkan Coba Lagi"));
      }
    }

    private string ValidateInput()
    {
      decimal result;
      string payment = textBoxPayment.Text;
      if (!decimal.TryParse(payment,out result))
      {
        return "Pembayaran Tidak Valid";
      }
      return string.Empty;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageLogin;
    }

    private void daftarBarangToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageInventoryList;
    }

    private void penjualanToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageCashier;
    }

    private void loginToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageLogin;
    }

    private void buttonLogin_Click(object sender, EventArgs e)
    {
      bool success = controller.Login(textBoxUsername.Text, textBoxPassword.Text);
      if(success)
      {
        tabControlPage.SelectedTab = tabPageCashier;
      }
    }

    private void textBoxDiscountPercent_TextChanged(object sender, EventArgs e)
    {
      radioButtonDiscountPercent.Checked = true;
    }

    private void textBoxDiscountAmount_TextChanged(object sender, EventArgs e)
    {
      radioButtonDiscountAmount.Checked = true;
    }

    private void dataGridViewItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      var senderGrid = (DataGridView) sender;
      if( senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
      {
        DataGridViewRow gridViewRow = senderGrid.Rows[e.RowIndex];
        var itemsView = (Item) gridViewRow.DataBoundItem;
        AddToCart(itemsView);
      }
      
    }

    public void RefreshDataGridViewCart()
    {
      if(InvokeRequired)
      {
        //this.BeginInvoke( new VoidHandler ())
        return;
      }

    }

    private void AddToCart(Item itemView)
    {
      bool success = controller.AddToCart(itemView);
    }

    private void buttonClearCart_Click(object sender, EventArgs e)
    {
      DialogResult dr = MessageBox.Show("Apakah Anda Yakin akan membersihkan Keranjang ? Semua barang akan terhapus dari layar ?",
                      "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
      if(dr == DialogResult.Yes)
      {
        controller.NewCart();
      } 
    }

    private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
    {

    }

  }
}

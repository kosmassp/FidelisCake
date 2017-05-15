using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business.Enum;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Model;
using InventoryAndSales.GUI.Utility;
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
    }

    private void buttonCheckout_Click(object sender, System.EventArgs e)
    {
      string validationMsg = ValidateInput( textBoxPayment, "Pembayaran Tidak Valid");
      if (!string.IsNullOrEmpty(validationMsg))
      {
        MessageBox.Show(validationMsg);
        return;
      }
      string errorMessage = controller.Checkout(decimal.Parse(textBoxPayment.Text), textBoxNotes.Text);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        MessageBox.Show(string.Format("{0}\n{1}", errorMessage, "Silahkan Coba Lagi"));
      }
    }

    private string ValidateInput(TextBox textBox, string errorMessage)
    {
      decimal result;
      string payment = textBox.Text;
      if (!decimal.TryParse(payment, out result))
      {
        return errorMessage;
      }
      return string.Empty;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageLogin;
      OnLoginActivated();
    }

    private void daftarBarangToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageProductMaster;
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
      if (success)
      {
        tabControlPage.SelectedTab = tabPageCashier;
      }
      else
      {
        labelCannotLogin.Text = "Username atau password tidak benar";
      }
    }

    private void textBoxDiscountPercent_TextChanged(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(textBoxDetailItemDiscountPercent.Text))
        radioButtonDiscountPercent.Checked = true;
    }

    private void textBoxDiscountAmount_TextChanged(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(textBoxDetailItemDiscountAmount.Text))
        radioButtonDiscountAmount.Checked = true;
    }

    private void dataGridViewItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      var senderGrid = (DataGridView)sender;
      if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
      {
        var itemsView = _itemDictRowIdToItem[e.RowIndex];
        AddToCart(itemsView);
      }

    }

    public void RefreshDataGridViewCart()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(RefreshDataGridViewCart));
        return;
      }

    }

    private void AddToCart(Product productView)
    {
      controller.AddToCart(productView);

    }

    private void buttonClearCart_Click(object sender, EventArgs e)
    {
      DialogResult dr = MessageBox.Show("Apakah Anda Yakin akan membersihkan Keranjang ? Semua barang akan terhapus dari layar ?",
                      "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
      if (dr == DialogResult.Yes)
      {
        controller.NewCart();
      }
    }

    private void tabControlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (tabControlPage.SelectedTab == tabPageLogin)
      {
        OnLoginActivated();
      }
      else if (tabControlPage.SelectedTab == tabPageCashier)
      {
        OnCashierActivated();
      }
      else if (tabControlPage.SelectedTab == tabPageProductMaster)
      {
        OnInventoryListActivated();
      }
    }

    private void OnLoginActivated()
    {
      controller.Logout();
      textBoxUsername.Text = string.Empty;
      textBoxPassword.Text = string.Empty;
      labelCannotLogin.Text = string.Empty;
      textBoxUsername.Focus();
    }

    private void textBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        textBoxPassword.Focus();
      }
    }

    private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        buttonLogin_Click(sender, null);
      }
    }

    private void textBoxUsername_Enter(object sender, EventArgs e)
    {
      textBoxUsername.SelectAll();
    }

    private void textBoxPassword_Enter(object sender, EventArgs e)
    {
      textBoxPassword.SelectAll();
    }

    private void OnCashierActivated()
    {
      textBoxFilter.Text = string.Empty;
      ReloadItemList();
      ResetCart();
    }

    private Dictionary<int, Product> _itemDictRowIdToItem = new Dictionary<int, Product>();
    private void ReloadItemList()
    {
      List<Product> items = controller.GetItems();
      dataGridViewItemList.Rows.Clear();
      _itemDictRowIdToItem.Clear();
      foreach (Product item in items)
      {
        int rowId = dataGridViewItemList.Rows.Add(item.Code, item.Name, item.NetPrice.ToString(Constant.DISPLAY_CURRENCY), "+");
        _itemDictRowIdToItem[rowId] = item;
      }
    }

    public void ResetCart()
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.VoidHandler(ResetCart));
        return;
      }
      dataGridViewCart.Rows.Clear();
      _cartDictItemToRow.Clear();
      _cartDictRowToItem.Clear();

      textBoxPayment.Text = 0.ToString();
      textBoxTotal.Text = 0.ToString();
      textBoxChanges.Text = 0.ToString();
      textBoxNotes.Text = string.Empty;
      textBoxFilter.Text = string.Empty;

    }

    private Dictionary<int, int> _cartDictItemToRow = new Dictionary<int, int>();
    private Dictionary<int, Product> _cartDictRowToItem = new Dictionary<int, Product>();
    public void UpdateDataGridViewCart(Product product, int quantity)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.TwoValueHandler<Product, int>(UpdateDataGridViewCart), product, quantity);
        return;
      }
      if (_cartDictItemToRow.ContainsKey(product.Id))
      {
        _isUpdatingItemQuantity = true;
        dataGridViewCart.Rows[_cartDictItemToRow[product.Id]].Visible = quantity > 0;
        dataGridViewCart.Rows[_cartDictItemToRow[product.Id]].Cells["CartItemQuantity"].Value = quantity;
        dataGridViewCart.Rows[_cartDictItemToRow[product.Id]].Cells["CartItemSubtotal"].Value = (quantity * product.NetPrice).ToString(Constant.DISPLAY_CURRENCY);
        _isUpdatingItemQuantity = false;
      }
      else
      {
        int rowId = dataGridViewCart.Rows.Add(product.Code, product.Name, quantity, product.Price.ToString(Constant.DISPLAY_CURRENCY), product.DiscountAmount.ToString(Constant.DISPLAY_CURRENCY), (quantity * (product.Price - product.DiscountAmount)).ToString(Constant.DISPLAY_CURRENCY));
        _cartDictItemToRow.Add(product.Id, rowId);
        _cartDictRowToItem.Add(rowId, product);
      }
    }
    public void UpdateTotal(decimal total)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<decimal>(UpdateTotal), total);
        return;
      }
      textBoxTotal.Text = total.ToString(Constant.DISPLAY_CURRENCY);
    }

    private bool _isUpdatingItemQuantity;
    private void dataGridViewCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || e.ColumnIndex < 0)
        return;
      if (e.ColumnIndex == dataGridViewCart.Columns["CartItemQuantity"].Index)
      {
        if (_isUpdatingItemQuantity)
          return;
        object value = dataGridViewCart.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        string stringValue = value as string;
        int intValue = string.IsNullOrEmpty(stringValue) ? (int)value : int.Parse(stringValue);
        Product product = _cartDictRowToItem[e.RowIndex];
        controller.UpdateCart(product, intValue);
      }
    }

    private void OnInventoryListActivated()
    {
      OnEditMasterItemMode(false);
    }

    private void dataGridViewMasterItemList_SelectionChanged(object sender, EventArgs e)
    {
      DataGridViewSelectedCellCollection selectedCells = dataGridViewMasterItemList.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewMasterItemList.Rows[viewCell.RowIndex];
        var item = viewRow.DataBoundItem as Product;
        if (item != null)
        {
          UpdateDetailBarang(item);
        }
      }
    }

    private Product _currentProductSelection;
    private void UpdateDetailBarang(Product product)
    {
      if (_currentProductSelection == product)
        return;
      _currentProductSelection = product;
      textBoxDetailItemName.Text = product.Name;
      textBoxDetailItemCode.Text = product.Code;
      textBoxDetailItemPrice.Text = product.Price.ToString("#.##");
      textBoxDetailItemDiscountAmount.Text = string.Empty;
      textBoxDetailItemDiscountPercent.Text = string.Empty;
      if (product.Discount > 0)
      {
        textBoxDetailItemDiscountAmount.Text = product.Discount.ToString("#.##"); ;
      }
      else if (product.Discount < 0)
      {
        textBoxDetailItemDiscountPercent.Text = (-product.Discount).ToString("#.##");
      }
      else
      {
        radioButtonDiscountAmount.Checked = false;
        radioButtonDiscountPercent.Checked = false;
      }
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditMode)
      {
        OnEditMasterItemMode(true);
        isAdding = true;
        textBoxDetailItemName.Text = string.Empty;
        textBoxDetailItemCode.Text = string.Empty;
        textBoxDetailItemPrice.Text = string.Empty;
        textBoxDetailItemDiscountPercent.Text = string.Empty;
        textBoxDetailItemDiscountAmount.Text = string.Empty;
        radioButtonDiscountAmount.Checked = false;
        radioButtonDiscountPercent.Checked = false;
 
        return;
      }
    }

    private void GetItemDetail(out string code, out string name, out decimal price, out decimal discount)
    {
      code = textBoxDetailItemCode.Text;
      name = textBoxDetailItemName.Text;
      price = decimal.Parse(textBoxDetailItemPrice.Text);
      discount = 0;
      if (radioButtonDiscountAmount.Checked && !string.IsNullOrEmpty(textBoxDetailItemDiscountAmount.Text))
        discount = decimal.Parse(textBoxDetailItemDiscountAmount.Text);
      if (radioButtonDiscountPercent.Checked && !string.IsNullOrEmpty(textBoxDetailItemDiscountPercent.Text))
        discount = -decimal.Parse(textBoxDetailItemDiscountPercent.Text);
    }

    //TODO need to think if we need to move this in the controller level.
    private string ValidateUniqueItem()
    {
      var code = textBoxDetailItemCode.Text;
      var name = textBoxDetailItemName.Text;
      bool codeExist = false, nameExist = false;
      List<Product> items = controller.GetItems();
      foreach (Product item in items)
      {
        if (item.Code == code)
        {
          codeExist = true;
          break;
        }
        if (item.Name == name)
        {
          nameExist = true;
          break;
        }
      }

      if (codeExist)
        return "Kode barang sudah ada.";
      if (nameExist)
        return "Nama barang sudah ada.";

      return string.Empty;
    }

    private bool isOnAddEditMode = false;
    private bool isUpdating = false;
    private bool isAdding = false;
    private void buttonUpdate_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditMode)
      {
        OnEditMasterItemMode(true);
        isUpdating = true;
        return;
      }
    }

    private void OnEditMasterItemMode(bool edit)
    {
      isOnAddEditMode = edit;
      buttonAdd.Visible = !edit;
      buttonUpdate.Visible = !edit;
      buttonDelete.Visible = !edit;
      buttonOkEdit.Visible = edit;
      buttonCancelEdit.Visible = edit;
      textBoxDetailItemName.Enabled = edit;
      textBoxDetailItemCode.Enabled = edit;
      textBoxDetailItemPrice.Enabled = edit;
      radioButtonDiscountAmount.Enabled = edit;
      radioButtonDiscountPercent.Enabled = edit;
      textBoxDetailItemDiscountPercent.Enabled = edit && radioButtonDiscountPercent.Checked;
      textBoxDetailItemDiscountAmount.Enabled = edit && radioButtonDiscountAmount.Checked;
      dataGridViewMasterItemList.Enabled = !edit;
      dataGridViewMasterItemList.ForeColor = edit ? Color.Gray : Color.Black;

      List<Product> items = controller.GetItems();
      dataGridViewMasterItemList.DataSource = items;


    }

    private string ValidateDetailItemInput()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(string.IsNullOrEmpty(textBoxDetailItemCode.Text) ?  "Harap isi kode barang" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxDetailItemName.Text) ?  "Harap isi nama barang": string.Empty);
      sb.AppendLine(ValidateInput(textBoxDetailItemPrice, "Harga tidak valid"));
      if (radioButtonDiscountAmount.Checked && !string.IsNullOrEmpty(textBoxDetailItemDiscountAmount.Text))
        sb.AppendLine(ValidateInput(textBoxDetailItemDiscountAmount, "Nilai diskon rupiah tidak valid"));
      if (radioButtonDiscountPercent.Checked && !string.IsNullOrEmpty(textBoxDetailItemDiscountPercent.Text))
        sb.AppendLine(ValidateInput(textBoxDetailItemDiscountPercent, "Nilai diskon persent tidak valid"));
      return sb.ToString().Trim();
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      if (_currentProductSelection != null)
      {
        DialogResult dr = MessageBox.Show(
          string.Format("Apakah anda benar ingin menghapus Product {0}", _currentProductSelection.Name),
          "Konfirmasi Hapus",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        if(dr == DialogResult.OK)
        {
          controller.RemoveItem(_currentProductSelection);
        }
      }
    }

    private void textBoxPayment_KeyUp(object sender, KeyEventArgs e)
    {
      if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
          (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
          e.KeyCode == Keys.Decimal)
      {
        RecalculateChanges();
      }
    }

    private void RecalculateChanges()
    {
      string payment = textBoxPayment.Text;
      decimal paid;
      if (decimal.TryParse(payment, out paid))
      {
        decimal total;
        if (decimal.TryParse(textBoxTotal.Text, out total))
          textBoxChanges.Text = (paid - total).ToString(Constant.DISPLAY_CURRENCY);
      }
    }

    private void textBoxTotal_TextChanged(object sender, EventArgs e)
    {
      string payment = textBoxPayment.Text;
      decimal paid;
      if (decimal.TryParse(payment, out paid))
      {
        decimal total;
        if (decimal.TryParse(textBoxTotal.Text, out total))
          if (paid < total)
          {
            textBoxPayment.Text = total.ToString();
            RecalculateChanges();
          }
      }

    }

    private void textBoxPayment_Click(object sender, EventArgs e)
    {
      textBoxPayment.SelectAll();
    }

    private void radioButtonDiscount_CheckedChanged(object sender, EventArgs e)
    {
      if (radioButtonDiscountAmount.Enabled)
        textBoxDetailItemDiscountAmount.Enabled = radioButtonDiscountAmount.Checked;
      if (radioButtonDiscountPercent.Enabled)
        textBoxDetailItemDiscountPercent.Enabled = radioButtonDiscountPercent.Checked;
    }

    private void buttonOkEdit_Click(object sender, EventArgs e)
    {
      string errorMsg = ValidateDetailItemInput();
      if (!string.IsNullOrEmpty(errorMsg))
      {
        MessageBox.Show(errorMsg);
        return;
      }

      string code;
      string name;
      decimal price;
      decimal discount;
      GetItemDetail(out code, out name, out price, out discount);
      if (isUpdating)
      {
        controller.UpdateItem(_currentProductSelection, code, name, price, discount);
        isUpdating = false;
      }
      else if (isAdding)
      {
        controller.AddItem(code, name, price, discount);
        isAdding = false;
      }
      OnEditMasterItemMode(false);
    }

    private void buttonCancelEdit_Click(object sender, EventArgs e)
    {
      OnEditMasterItemMode(false);
      _currentProductSelection = null;
      DataGridViewSelectedCellCollection selectedCells = dataGridViewMasterItemList.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewMasterItemList.Rows[viewCell.RowIndex];
        var item = viewRow.DataBoundItem as Product;
        if (item != null)
        {
          UpdateDetailBarang(item);
        }
      }

    }

    public void EnableMenu(int role)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<int>(EnableMenu), role);
        return;
      }
      transaksiToolStripMenuItem.Visible = (((MenuAccess)role & MenuAccess.Cashier) == MenuAccess.Cashier);
      editToolStripMenuItem.Visible = (((MenuAccess)role & MenuAccess.Master) == MenuAccess.Master);
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
    }

    private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
    {
      FilterItemView(textBoxFilter.Text);
    }

    private void FilterItemView(string filter)
    {
      foreach (KeyValuePair<int, Product> rowIdProduct in _itemDictRowIdToItem)
      {
        if(  rowIdProduct.Value.Name.ToLower().Contains(filter.ToLower())
          || rowIdProduct.Value.Code.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase)
          )
        {
          dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
        }
        else
          dataGridViewItemList.Rows[rowIdProduct.Key].Visible = false;
      }
    }

    private void timerDisplayDate_Tick(object sender, EventArgs e)
    {
      toolStripStatusCurrentDate.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
    }

    private void daftarUserToolStripMenuItem_Click(object sender, EventArgs e)
    {
      //tabControlPage.SelectedTab = tabPageUserMaster;
    }

    private void laporanTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageReport;
      
    }

    private void buttonShowReportSummary_Click(object sender, EventArgs e)
    {
      controller.ShowSummaryReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
    }

    private void buttonShowReportDetail_Click(object sender, EventArgs e)
    {
      controller.ShowDetailReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
    }

    public void UpdateReportDataGridView(DataTable dataTable )
    {
      if(InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<DataTable>(UpdateReportDataGridView), dataTable);
        return;
      }
      dataGridViewLaporan.DataSource = dataTable;
    }
  }

}

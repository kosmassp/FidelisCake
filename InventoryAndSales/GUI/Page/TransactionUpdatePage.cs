using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Controller;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Page
{
  public partial class TransactionUpdatePage : UserControl
  {
    public event EventHandler CheckoutSucceed;
    public event EventHandler BackClick;

    private TransactionUpdateController controller;
    public TransactionUpdatePage()
    {
      InitializeComponent();
      controller = new TransactionUpdateController(this);
    }

    public void Init(string facturNumber, User user)
    {
      controller.Init(facturNumber, user);
      textBoxTransactionID.Text = controller.OriginalTransaction.Id.ToString();
      Reset();
    }

    public void Reset()
    {
      textBoxFilter.Text = string.Empty;

      ReloadItemList();
      bool byBarcode;
      FilterItemView(string.Empty, out byBarcode);
      controller.NewCart();
      controller.ResetByTransaction();
      textBoxFilter.Focus();
    }

    private Dictionary<int, int> _cartDictItemToRow = new Dictionary<int, int>();
    private Dictionary<int, Product> _cartDictRowToItem = new Dictionary<int, Product>();
    private Dictionary<int, Product> _itemDictRowIdToItem = new Dictionary<int, Product>();
    private bool _isUpdatingItemQuantity;

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

    private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
    {
      bool byBarcode;
      if( e.KeyData == Keys.Add )
      {
        var selectedRows = dataGridViewItemList.SelectedRows;
        if (selectedRows.Count != 1)
          return;
        var selectedRow = selectedRows[0];
        AddToCart(_itemDictRowIdToItem[selectedRow.Index]);
        textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('+'),1);
      }
      else if (e.KeyData == Keys.Down)
      {
        SelectNextVisibleRow();
      }
      else if (e.KeyData == Keys.Up)
      {
        SelectPrevVisibleRow();
      }
      else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
      {
        e.Handled = true;
      }
      else
      {
        Product theOnlyProduct = FilterItemView(textBoxFilter.Text.Trim(), out byBarcode);
        //Comment here if barcode needed to be keypress
        if (theOnlyProduct != null && e.KeyData == Keys.Enter)
        {
          AddToCart(theOnlyProduct);
          if (byBarcode) //clear filter search
          {
            textBoxFilter.Text = string.Empty;
            FilterItemView(string.Empty, out byBarcode);
          }
        }
        e.Handled = true;
      }
    }

    private void SelectNextVisibleRow() //Down Arrow
    {
      var selectedRows = dataGridViewItemList.SelectedRows;
      int selectedIndex = 0;
      if (selectedRows.Count > 0)
      {
        var selectedRow = selectedRows[0];
        selectedIndex = selectedRow.Index;
      }
      int stopLoop = selectedIndex;
      dataGridViewItemList.ClearSelection();
      while(true)
      {
        selectedIndex++;
        if (selectedIndex >= dataGridViewItemList.Rows.Count)
          selectedIndex = 0;
        DataGridViewRow row = dataGridViewItemList.Rows[selectedIndex];
        if (row.Visible)
        {
          row.Selected = true;
          dataGridViewItemList.FirstDisplayedScrollingRowIndex = selectedIndex;
          break;
        }
        if(stopLoop == selectedIndex)
          return;
      }
    }

    private void SelectPrevVisibleRow()
    {
      var selectedRows = dataGridViewItemList.SelectedRows;
      int selectedIndex = 0;
      if (selectedRows.Count > 0)
      {
        var selectedRow = selectedRows[0];
        selectedIndex = selectedRow.Index;
      }
      int stopLoop = selectedIndex;
      dataGridViewItemList.ClearSelection();
      while (true)
      {
        selectedIndex--;
        if (selectedIndex < 0 )
          selectedIndex = dataGridViewItemList.Rows.Count - 1;
        DataGridViewRow row = dataGridViewItemList.Rows[selectedIndex];
        if (row.Visible)
        {
          row.Selected = true;
          dataGridViewItemList.FirstDisplayedScrollingRowIndex = selectedIndex;
          break;
        }
        if (stopLoop == selectedIndex)
          return;
      }
    }

    private Product FilterItemView(string filter, out bool byBarcode)
    {
      int filterCountResult = 0;
      Product lastProduct = null;
      byBarcode = false;
      int selectedIndex = -1;
      foreach (KeyValuePair<int, Product> rowIdProduct in _itemDictRowIdToItem)
      {
        Product product = rowIdProduct.Value;
        if (!string.IsNullOrEmpty(filter))
        {
          if (!string.IsNullOrEmpty(product.Barcode) && product.Barcode.Equals(filter))
          {
            dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
            byBarcode = true;
            filterCountResult++;
            lastProduct = product;
            if (selectedIndex < 0)
              selectedIndex = rowIdProduct.Key;
          }
          else if (product.Name.ToLower().Contains(filter.ToLower())
                   || product.Code.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
          {
            dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
            filterCountResult++;
            lastProduct = product;
            if (selectedIndex < 0)
              selectedIndex = rowIdProduct.Key;
          }
          else
            dataGridViewItemList.Rows[rowIdProduct.Key].Visible = false;
        }
        else
        {
          dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
          if (selectedIndex < 0)
            selectedIndex = rowIdProduct.Key;
        }
      }

      if (selectedIndex >= 0)
      {
        dataGridViewItemList.ClearSelection();
        dataGridViewItemList.Rows[selectedIndex].Selected = true;
        dataGridViewItemList.FirstDisplayedScrollingRowIndex = selectedIndex;
      }
      if (filterCountResult == 1)
      {
        return lastProduct;
      }
      return null;
    }

    private void dataGridViewCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

    private void buttonClearCart_Click(object sender, EventArgs e)
    {
      DialogResult dr = MessageBox.Show("Apakah Anda Yakin akan membersihkan Keranjang ? Semua barang akan terhapus dari layar ?",
                "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
      if (dr == DialogResult.Yes)
      {
        controller.NewCart();
        textBoxFilter.Focus();
      }

    }

    private void AddToCart(Product productView)
    {
      controller.AddToCart(productView);
    }

    private void buttonCheckout_Click(object sender, EventArgs e)
    {
      string validationMsg = ValidateInput(textBoxPayment, "Pembayaran Tidak Valid");
      if (!string.IsNullOrEmpty(validationMsg))
      {
        MessageBox.Show(validationMsg);
        return;
      }
      string successMessage;
      string errorMessage = controller.Checkout(decimal.Parse(textBoxPayment.Text), textBoxNotes.Text, out successMessage);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        MessageBox.Show(string.Format("Transaksi Gagal.\n{0}\n\n\n{1}", errorMessage, "Silahkan Coba Lagi"));
      }
      else
      {
        if (!string.IsNullOrEmpty(successMessage))
        {
          MessageBox.Show(successMessage);
          textBoxFilter.Focus();
        }
        if (CheckoutSucceed != null)
          CheckoutSucceed(this, null);

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

    private void textBoxPayment_TextChanged(object sender, EventArgs e)
    {
      RecalculateChanges();
    }

    private void textBoxPayment_KeyUp(object sender, KeyEventArgs e)
    {
      RecalculateChanges();
    }

    private void RecalculateChanges()
    {
      if (string.IsNullOrEmpty(textBoxPayment.Text))
      {
        textBoxPayment.Text = "0";
        textBoxPayment.SelectAll();
      }
      string payment = textBoxPayment.Text;
      decimal paid;
      if (decimal.TryParse(payment, out paid))
      {
        decimal total;
        if (decimal.TryParse(textBoxTotal.Text, out total))
          textBoxChanges.Text = (paid - total).ToString(Constant.DISPLAY_CURRENCY);
      }
    }

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

    private void dataGridViewItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      var senderGrid = (DataGridView)sender;
      if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
      {
        var itemsView = _itemDictRowIdToItem[e.RowIndex];
        AddToCart(itemsView);
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

    public void FocusFilter()
    {
      textBoxFilter.Focus();
    }

    public void FocusPayment()
    {
      textBoxPayment.Focus();
      textBoxPayment.SelectAll();
    }

    public void FocusCheckout()
    {
      buttonCheckout.PerformClick();
    }

    private void textBoxPayment_Click(object sender, EventArgs e)
    {
      textBoxPayment.SelectAll();
    }

    private void buttonReset_Click(object sender, EventArgs e)
    {
      Reset();
    }

    private void buttonBack_Click(object sender, EventArgs e)
    {
      controller.Unload();
      if (BackClick != null)
        BackClick(this, null);
    }
  }
}

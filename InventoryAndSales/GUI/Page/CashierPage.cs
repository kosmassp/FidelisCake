using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Controller;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Page
{
  public partial class CashierPage : UserControl
  {
    private CashierController controller;
    public CashierPage()
    {
      InitializeComponent();
      controller = new CashierController(this);
    }

    public void Reset()
    {
      textBoxFilter.Text = string.Empty;

      ReloadItemList();
      bool byBarcode;
      FilterItemView(string.Empty, out byBarcode);
      controller.NewCart();
      // Picks up terminals added or removed in settings since this screen was last shown.
      ResetPaymentMethod();
      // Held baskets survive leaving and returning to this screen - they only end with the session.
      RefreshHeldCarts();
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


    private void dataGridViewItemList_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '+')
      {
        var selectedRows = dataGridViewItemList.SelectedRows;
        if (selectedRows.Count != 1)
          return;
        var selectedRow = selectedRows[0];
        AddToCart(_itemDictRowIdToItem[selectedRow.Index]);
        if (textBoxFilter.Text.IndexOf('+') >= 0)
          textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('+'), 1);
        //e.Handled = true;
      }
      else if (e.KeyChar == '-')
      {
        var selectedRows = dataGridViewItemList.SelectedRows;
        if (selectedRows.Count != 1)
          return;
        var selectedRow = selectedRows[0];
        RemoveFromCart(_itemDictRowIdToItem[selectedRow.Index]);
        if (textBoxFilter.Text.IndexOf('-') >= 0)
          textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('-'), 1);
        //e.Handled = true;
      }
    }

    private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '+')
      {
        var selectedRows = dataGridViewItemList.SelectedRows;
        if (selectedRows.Count != 1)
          return;
        var selectedRow = selectedRows[0];
        AddToCart(_itemDictRowIdToItem[selectedRow.Index]);
        if (textBoxFilter.Text.IndexOf('+') >=0 )
          textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('+'), 1);
        //e.Handled = true;
      }
      else if (e.KeyChar == '-')
      {
        var selectedRows = dataGridViewItemList.SelectedRows;
        if (selectedRows.Count != 1)
          return;
        var selectedRow = selectedRows[0];
        RemoveFromCart(_itemDictRowIdToItem[selectedRow.Index]);
        if (textBoxFilter.Text.IndexOf('-') >= 0)
          textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('-'), 1);
        //e.Handled = true;
      }
    }

    private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
    {
      bool byBarcode;
      if (textBoxFilter.Text.IndexOf('+') >= 0)
      {
        textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('+'), 1);
        e.Handled = false;
        return;
      }
      if (textBoxFilter.Text.IndexOf('-') >= 0)
      {
        textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.IndexOf('-'), 1);
        e.Handled = false;
        return;
      }
      //if (e.KeyData == Keys.Add || e.KeyCode == Keys.Add)
      //{
      //  var selectedRows = dataGridViewItemList.SelectedRows;
      //  if (selectedRows.Count != 1)
      //    return;
      //  var selectedRow = selectedRows[0];
      //  AddToCart(_itemDictRowIdToItem[selectedRow.Index]);
      //}
      //else 
        if (e.KeyData == Keys.Down)
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
          e.Handled = true;
        }
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
      bool flagChange = false;
      foreach (KeyValuePair<int, Product> rowIdProduct in _itemDictRowIdToItem)
      {
        bool lastState = dataGridViewItemList.Rows[rowIdProduct.Key].Visible;
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
          // Null tolerant: Code and Name can be empty on rows created by a CSV import or edited
          // directly in the database.
          else if ((product.Name ?? string.Empty).IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0
                   || (product.Code ?? string.Empty).StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
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
        if (!flagChange && lastState != dataGridViewItemList.Rows[rowIdProduct.Key].Visible)
          flagChange = true;
      }

      if (selectedIndex >= 0 && flagChange)
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

    private void RemoveFromCart(Product productView)
    {
      controller.RemoveFromCart(productView);
    }

    private bool _loadingPaymentMethod;

    /// <summary>The terminal/provider row in <c>tableLayoutFields</c>, and its height when shown.</summary>
    private const int ReferenceRowIndex = 1;
    private const float ReferenceRowHeight = 27F;

    /// <summary>
    /// An entry in the method combo. Keeps the label and the value together so the screen never has
    /// to match on display text.
    /// </summary>
    private class MethodChoice
    {
      public PaymentMethod Method { get; private set; }
      public string Label { get; private set; }

      public MethodChoice(PaymentMethod method, string label)
      {
        Method = method;
        Label = label;
      }

      public override string ToString()
      {
        return Label;
      }
    }

    /// <summary>
    /// Rebuilds the method list and puts the screen back on cash.
    ///
    /// A method whose list is empty is left out rather than shown-but-broken: choosing it could
    /// never lead to a completed sale.
    /// </summary>
    private void ResetPaymentMethod()
    {
      _loadingPaymentMethod = true;
      try
      {
        comboBoxPaymentMethod.Items.Clear();
        comboBoxPaymentMethod.Items.Add(new MethodChoice(PaymentMethod.Cash, "Tunai (Ctrl+1)"));
        if (controller.IsMethodAvailable(PaymentMethod.Edc))
          comboBoxPaymentMethod.Items.Add(new MethodChoice(PaymentMethod.Edc, "EDC (Ctrl+2)"));
        if (controller.IsMethodAvailable(PaymentMethod.Qris))
          comboBoxPaymentMethod.Items.Add(new MethodChoice(PaymentMethod.Qris, "QRIS (Ctrl+3)"));
        if (controller.IsMethodAvailable(PaymentMethod.Transfer))
          comboBoxPaymentMethod.Items.Add(new MethodChoice(PaymentMethod.Transfer, "Transfer (Ctrl+4)"));

        comboBoxPaymentMethod.SelectedIndex = 0;
      }
      finally
      {
        _loadingPaymentMethod = false;
      }
      ApplyPaymentMethod();
    }

    private PaymentMethod SelectedMethod
    {
      get
      {
        MethodChoice choice = comboBoxPaymentMethod.SelectedItem as MethodChoice;
        return choice == null ? PaymentMethod.Cash : choice.Method;
      }
    }

    /// <summary>
    /// The terminal or provider name the cashier picked, or empty for cash.
    ///
    /// A QRIS entry displays as "GoPay (Statis)", so the name is taken from the object rather than
    /// from what is on screen.
    /// </summary>
    private string SelectedReference
    {
      get
      {
        object selected = comboBoxReference.SelectedItem;
        if (selected == null)
          return string.Empty;
        QrisProvider provider = selected as QrisProvider;
        return provider != null ? provider.Name : selected.ToString();
      }
    }

    /// <summary>
    /// Picks a method from the keyboard. Does nothing if it is not on offer, but says why - a
    /// shortcut that silently ignores you is worse than one that explains itself.
    /// </summary>
    public void SelectPaymentMethod(PaymentMethod method)
    {
      for (int i = 0; i < comboBoxPaymentMethod.Items.Count; i++)
      {
        MethodChoice choice = comboBoxPaymentMethod.Items[i] as MethodChoice;
        if (choice != null && choice.Method == method)
        {
          comboBoxPaymentMethod.SelectedIndex = i;
          return;
        }
      }

      string what = method == PaymentMethod.Qris ? "provider QRIS"
        : method == PaymentMethod.Transfer ? "rekening tujuan transfer"
        : "terminal EDC";
      MessageBox.Show(
        string.Format("Belum ada {0} yang terdaftar.{1}{1}Tambahkan melalui menu Pengaturan.", what, Environment.NewLine),
        "Metode Tidak Tersedia", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Shapes the payment fields around the chosen method: cash takes an amount and gives change,
    /// everything else takes the exact total and needs to record where it came through.
    /// </summary>
    private void ApplyPaymentMethod()
    {
      PaymentMethod method = SelectedMethod;
      bool exact = PaymentDetail.IsExactAmount(method);
      bool qris = method == PaymentMethod.Qris;
      bool transfer = method == PaymentMethod.Transfer;

      labelReference.Visible = exact;
      comboBoxReference.Visible = exact;
      // Collapse the row rather than leaving a gap where the terminal would have been: cash is the
      // common case and the remaining fields should sit straight under the method.
      tableLayoutFields.RowStyles[ReferenceRowIndex].Height = exact ? ReferenceRowHeight : 0F;
      labelReference.Text = qris ? "Provider" : transfer ? "Rekening" : "Terminal";
      textBoxPayment.ReadOnly = exact;

      if (exact)
      {
        _loadingPaymentMethod = true;
        try
        {
          comboBoxReference.Items.Clear();
          // QRIS entries go in as objects so the code type shows in the list and the name can be
          // read back without parsing the label.
          if (qris)
            foreach (QrisProvider provider in controller.GetQrisProviders())
              comboBoxReference.Items.Add(provider);
          else if (transfer)
            foreach (string bank in controller.GetTransferBanks())
              comboBoxReference.Items.Add(bank);
          else
            foreach (string terminal in controller.GetEdcTerminals())
              comboBoxReference.Items.Add(terminal);
          if (comboBoxReference.Items.Count > 0)
            comboBoxReference.SelectedIndex = 0;
        }
        finally
        {
          _loadingPaymentMethod = false;
        }

        decimal total = controller.GetCartTotal();
        textBoxPayment.Text = total.ToString(Constant.DISPLAY_CURRENCY);
        textBoxChanges.Text = 0.ToString(Constant.DISPLAY_CURRENCY);
      }
      else
      {
        RecalculateChanges();
      }
    }

    private void comboBoxPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (_loadingPaymentMethod)
        return;
      ApplyPaymentMethod();
    }

    #region Held carts

    /// <summary>
    /// Rebuilds the held-basket list. Called after every hold, recall or discard so the dropdown
    /// always matches what is actually set aside.
    /// </summary>
    private void RefreshHeldCarts()
    {
      List<HeldCart> held = controller.GetHeldCarts();

      comboBoxHeldCart.Items.Clear();
      foreach (HeldCart cart in held)
        comboBoxHeldCart.Items.Add(cart);
      if (comboBoxHeldCart.Items.Count > 0)
        comboBoxHeldCart.SelectedIndex = 0;

      bool any = held.Count > 0;
      comboBoxHeldCart.Enabled = any;
      buttonRecallCart.Enabled = any;
      buttonDiscardHeldCart.Enabled = any;
    }

    private HeldCart SelectedHeldCart
    {
      get { return comboBoxHeldCart.SelectedItem as HeldCart; }
    }

    private void buttonHoldCart_Click(object sender, EventArgs e)
    {
      HeldCart held;
      // The notes box doubles as the label, so a cashier can tag a basket without another dialog.
      string problem = controller.HoldCart(textBoxNotes.Text, out held);
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Disimpan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      RefreshHeldCarts();
      textBoxFilter.Focus();
      MessageBox.Show(string.Format("Keranjang disimpan di nomor {0}.", held.Slot), "Tersimpan",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void buttonRecallCart_Click(object sender, EventArgs e)
    {
      HeldCart selected = SelectedHeldCart;
      if (selected == null)
        return;

      HeldCart recalled;
      string problem = controller.RecallCart(selected.Slot, out recalled);
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Diambil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      // The note travels with the basket, so whoever picks it up sees what it was tagged with.
      textBoxNotes.Text = recalled.Label;
      RefreshHeldCarts();
      ApplyPaymentMethod();
      textBoxFilter.Focus();
    }

    private void buttonDiscardHeldCart_Click(object sender, EventArgs e)
    {
      HeldCart selected = SelectedHeldCart;
      if (selected == null)
        return;

      DialogResult confirm = MessageBox.Show(
        string.Format("Hapus keranjang tersimpan nomor {0}?", selected.Slot) +
        Environment.NewLine + Environment.NewLine + selected.ToString(),
        "Konfirmasi Hapus", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
      if (confirm != DialogResult.OK)
        return;

      controller.DiscardHeldCart(selected.Slot);
      RefreshHeldCarts();
    }

    #endregion

    private void buttonCheckout_Click(object sender, EventArgs e)
    {
      // Only cash needs the typed amount to make sense; the others take the total regardless.
      PaymentMethod method = SelectedMethod;
      decimal tendered = 0;
      if (!PaymentDetail.IsExactAmount(method))
      {
        string validationMsg = ValidateInput(textBoxPayment, "Pembayaran Tidak Valid");
        if (!string.IsNullOrEmpty(validationMsg))
        {
          MessageBox.Show(validationMsg);
          return;
        }
        tendered = decimal.Parse(textBoxPayment.Text);
      }

      string successMessage;
      string errorMessage = controller.Checkout(
        method,
        tendered,
        SelectedReference,
        textBoxNotes.Text,
        out successMessage);
      if (!string.IsNullOrEmpty(errorMessage))
      {
        MessageBox.Show(string.Format("Transaksi Gagal.\n{0}\n\n\n{1}", errorMessage, "Silahkan Coba Lagi"));
      }
      else
        if (!string.IsNullOrEmpty(successMessage))
        {
          MessageBox.Show(successMessage);
          textBoxFilter.Focus();
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
      // A card or QRIS payment is always the exact total with no change; ApplyPaymentMethod owns
      // those two boxes then, and blanking the amount back to zero here would fight it.
      if (PaymentDetail.IsExactAmount(SelectedMethod))
        return;

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
      // For a card payment the amount tracks the total, so it has to follow every cart change.
      ApplyPaymentMethod();
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

  }
}

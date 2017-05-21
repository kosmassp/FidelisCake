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
      comboBoxRoleMaster.DataSource = Enum.GetValues(typeof(RoleOptions));
    }

    private void buttonCheckout_Click(object sender, System.EventArgs e)
    {
      string validationMsg = ValidateInput( textBoxPayment, "Pembayaran Tidak Valid");
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
        if (!string.IsNullOrEmpty(successMessage))
      
      {
        MessageBox.Show(successMessage);
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
        OnProductMasterActivated();
      } 
      else if (tabControlPage.SelectedTab == tabPageUserMaster)
      {
        OnUserMasterActivated();
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
        e.Handled = true;
        textBoxPassword.Focus();
      }
    }

    private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        e.Handled = true;
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
      bool byBarcode;
      FilterItemView(string.Empty, out byBarcode);
      controller.NewCart();
      textBoxFilter.Focus();
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

    private void OnProductMasterActivated()
    {
      OnEditMasterItem(false);
    }

    private void OnUserMasterActivated()
    {
      OnEditMasterUser(false);
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
      else
      {
        ClearFieldItemDetail();
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
      textBoxDetailItemBarcode.Text = product.Barcode;
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

    private void buttonAddProduct_Click(object sender, EventArgs e)
    {
      if (!isOnProductAddEditMode)
      {
        OnEditMasterItem(true);
        isAddingProduct = true;
        ClearFieldItemDetail();

        return;
      }
    }

    private void ClearFieldItemDetail()
    {
      textBoxDetailItemName.Text = string.Empty;
      textBoxDetailItemCode.Text = string.Empty;
      textBoxDetailItemBarcode.Text = string.Empty;
      textBoxDetailItemPrice.Text = string.Empty;
      textBoxDetailItemDiscountPercent.Text = string.Empty;
      textBoxDetailItemDiscountAmount.Text = string.Empty;
      radioButtonDiscountAmount.Checked = false;
      radioButtonDiscountPercent.Checked = false;
    }

    private void GetItemDetail(out string code, out string barcode, out string name, out decimal price, out decimal discount)
    {
      code = textBoxDetailItemCode.Text;
      barcode = textBoxDetailItemBarcode.Text;
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
      var barcode = textBoxDetailItemBarcode.Text;
      bool codeExist = false, nameExist = false, barcodeExist = false;
      List<Product> items = controller.GetItems();
      foreach (Product item in items)
      {
        if (_currentProductSelection != null 
          && item.Code == _currentProductSelection.Code
          && item.Name == _currentProductSelection.Name
          && item.Barcode == _currentProductSelection.Barcode
          )
          continue;
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
        if (!string.IsNullOrEmpty(item.Barcode) && !string.IsNullOrEmpty(barcode ) && item.Barcode == barcode)
        {
          barcodeExist = true;
          break;
        }
      }

      if (barcodeExist)
        return "Kode barang sudah ada.";
      if (codeExist)
        return "Kode barang sudah ada.";
      if (nameExist)
        return "Nama barang sudah ada.";

      return string.Empty;
    }

    private bool isOnProductAddEditMode = false;
    private bool isUpdatingProduct = false;
    private bool isAddingProduct = false;
    private void buttonEditProduct_Click(object sender, EventArgs e)
    {
      if (!isOnProductAddEditMode)
      {
        OnEditMasterItem(true);
        isUpdatingProduct = true;
        return;
      }
    }

    private void OnEditMasterItem(bool edit)
    {
      isOnProductAddEditMode = edit;
      buttonAdd.Visible = !edit;
      buttonUpdate.Visible = !edit;
      buttonDelete.Visible = !edit;
      buttonOkEdit.Visible = edit;
      buttonCancelEdit.Visible = edit;
      textBoxDetailItemName.Enabled = edit;
      textBoxDetailItemCode.Enabled = edit;
      textBoxDetailItemBarcode.Enabled = edit;
      textBoxDetailItemPrice.Enabled = edit;
      radioButtonDiscountAmount.Enabled = edit;
      radioButtonDiscountPercent.Enabled = edit;
      textBoxDetailItemDiscountPercent.Enabled = edit && radioButtonDiscountPercent.Checked;
      textBoxDetailItemDiscountAmount.Enabled = edit && radioButtonDiscountAmount.Checked;
      dataGridViewMasterItemList.Enabled = !edit;
      dataGridViewMasterItemList.ForeColor = edit ? Color.Gray : Color.Black;

      if (!edit)
      {
        List<Product> items = controller.GetItems();
        dataGridViewMasterItemList.DataSource = null;
        dataGridViewMasterItemList.DataSource = items;
      }
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
      sb.AppendLine(ValidateUniqueItem());
      return sb.ToString().Trim();
    }

    private void buttonDeleteProduct_Click(object sender, EventArgs e)
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
          OnEditMasterItem(false);
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
      //string payment = textBoxPayment.Text;
      //decimal paid;
      //if (decimal.TryParse(payment, out paid))
      //{
      //  decimal total;
      //  if (decimal.TryParse(textBoxTotal.Text, out total))
      //    if (paid < total)
      //    {
      //      textBoxPayment.Text = total.ToString();
      //      RecalculateChanges();
      //    }
      //}
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
      string barcode;
      GetItemDetail(out code, out barcode, out name, out price, out discount);
      if (isUpdatingProduct)
      {
        controller.UpdateItem(_currentProductSelection, code, barcode, name, price, discount);
        isUpdatingProduct = false;
      }
      else if (isAddingProduct)
      {
        controller.AddItem(code, barcode, name, price, discount);
        isAddingProduct = false;
      }
      OnEditMasterItem(false);
    }

    private void buttonCancelEdit_Click(object sender, EventArgs e)
    {
      OnEditMasterItem(false);
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

    public void UpdateActiveUser(string name)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<string>(UpdateActiveUser), name);
        return;
      }
      toolStripStatusLabelActiveUser.Text = string.Format("ActiveUser={0}", string.IsNullOrEmpty(name) ? "<Unknown>" : name);
    }

    public void EnableMenu(int role)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<int>(EnableMenu), role);
        return;
      }
      transaksiToolStripMenuItem.Visible = AllowedRole(role, MenuAccess.Cashier);
      editToolStripMenuItem.Visible = AllowedRole(role, MenuAccess.Master);
      laporanToolStripMenuItem.Visible = AllowedRole(role, MenuAccess.Admin);
    }

    private bool AllowedRole(int role, MenuAccess access)
    {
      return (((MenuAccess)role & access) == access);
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
    }

    private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
    {
      bool byBarcode;
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
    private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
    {
      //if (e.KeyChar == (char)'\r')
      //{
      //  bool byBarcode;
      //  Product theOnlyProduct = FilterItemView(textBoxFilter.Text.Trim(), out byBarcode);
      //  AddToCart(theOnlyProduct);
      //  if (byBarcode) //clear filter search
      //  {
      //    textBoxFilter.Text = string.Empty;
      //    FilterItemView(string.Empty, out byBarcode);
      //  }
      //}
    }


    private Product FilterItemView(string filter, out bool byBarcode)
    {
      int filterCountResult = 0;
      Product lastProduct = null;
      byBarcode = false;
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
          }
          else if (product.Name.ToLower().Contains(filter.ToLower())
                   || product.Code.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase))
          {
            dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
            filterCountResult++;
            lastProduct = product;
          }
          else
            dataGridViewItemList.Rows[rowIdProduct.Key].Visible = false;
        }
        else
          dataGridViewItemList.Rows[rowIdProduct.Key].Visible = true;
      }

      if(filterCountResult == 1)
      {
        return lastProduct;
      }
      return null;
    }

    private void timerDisplayDate_Tick(object sender, EventArgs e)
    {
      toolStripStatusCurrentDate.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
    }

    private void daftarUserToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageUserMaster;
    }

    private void laporanTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
    {
      tabControlPage.SelectedTab = tabPageReport;
    }

    private void buttonShowReportSummary_Click(object sender, EventArgs e)
    {
      controller.ShowSummaryReport(dateTimePickerStart.Value, dateTimePickerStop.Value);
    }

    public void UpdateReportDataGridView(DataTable[] dataTables)
    {
      if(InvokeRequired)
      {
        this.BeginInvoke(new DelegateUtility.OneValueArrayHandler<DataTable>(UpdateReportDataGridView), dataTables);
        return;
      }
      dataGridViewLaporan.DataSource = dataTables[0];
      dataGridViewLaporanDetail.DataSource = dataTables[1];
      dataGridViewLaporanKasir.DataSource = dataTables[2];
    }


    private bool isOnAddEditUser = false;
    private bool isUpdatingUser = false;
    private bool isAddingUser = false;

    private void OnEditMasterUser(bool edit)
    {
      isOnAddEditUser = edit;
      buttonAddUserMaster.Visible = !edit;
      buttonEditUserMaster.Visible = !edit;
      buttonDeleteUserMaster.Visible = !edit;
      buttonOkUserMaster.Visible = edit;
      buttonCancelUserMaster.Visible = edit;
      textBoxUsernameMaster.Enabled = edit;
      textBoxNameMaster.Enabled = edit;
      textBoxPasswordMaster.Enabled = edit;
      textBoxRePasswordMaster.Enabled = edit;

      dataGridViewUserMaster.Enabled = !edit;
      dataGridViewUserMaster.ForeColor = edit ? Color.Gray : Color.Black;

      if (!edit)
      {
        List<User> users = controller.GetUsers();
        dataGridViewUserMaster.DataSource = null;
        dataGridViewUserMaster.DataSource = users;
      }
    }



    private string ValidateDetailUser()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(string.IsNullOrEmpty(textBoxUsernameMaster.Text) ? "Harap isi username" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxNameMaster.Text) ? "Harap isi nama user" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxPasswordMaster.Text) ? "Harap isi password" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxRePasswordMaster.Text) ? "Harap isi re-password " : string.Empty);
      sb.AppendLine(textBoxPasswordMaster.Text != textBoxRePasswordMaster.Text ? "Password tidak sesuai dengan re-password" : string.Empty);
      return sb.ToString().Trim();
    }


    private User _currentUserSelection;
    private void buttonOkUserMaster_Click(object sender, EventArgs e)
    {
      string errorMsg = ValidateDetailUser();
      if (!string.IsNullOrEmpty(errorMsg))
      {
        MessageBox.Show(errorMsg);
        return;
      }

      string username;
      string name;
      string password;
      int role;
      GetUserDetail(out username, out name, out password, out role);
      if (isUpdatingUser)
      {
        controller.UpdateUser(_currentUserSelection, username, name, password, role);
        isUpdatingUser = false;
      }
      else if (isAddingUser)
      {
        controller.AddUser(username, name, password, role);
        isAddingUser = false;
      }
      OnEditMasterUser(false);
    }

    private void GetUserDetail(out string username, out string name, out string password, out int role)
    {
      username = textBoxUsernameMaster.Text;
      name = textBoxNameMaster.Text;
      password = textBoxPasswordMaster.Text;
      RoleOptions selectedRole = (RoleOptions)comboBoxRoleMaster.SelectedValue;
      role = (int) selectedRole;
    }

    private void buttonCancelUserMaster_Click(object sender, EventArgs e)
    {
      OnEditMasterUser(false);
      _currentUserSelection = null;
      DataGridViewSelectedCellCollection selectedCells = dataGridViewUserMaster.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewUserMaster.Rows[viewCell.RowIndex];
        var user = viewRow.DataBoundItem as User;
        if (user != null)
        {
          UpdateDetailUser(user);
        }
      }
    }

    private void UpdateDetailUser(User user)
    {
      if (_currentUserSelection == user)
        return;
      _currentUserSelection = user;
      textBoxUsernameMaster.Text = user.Username;
      string password = user.Password;
      if (!string.IsNullOrEmpty(password) && password.Length > 8) 
      {
        password = password.Substring(0, 8);
      }
      textBoxPasswordMaster.Text = password;
      textBoxRePasswordMaster.Text = password;
      textBoxNameMaster.Text = user.Name;
      comboBoxRoleMaster.SelectedItem = (RoleOptions) user.Role;

    }


    private void buttonAddUserMaster_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditUser)
      {
        OnEditMasterUser(true);
        isAddingUser = true;
        ClearFieldUser();
        return;
      }
    }

    private void ClearFieldUser()
    {
      textBoxUsernameMaster.Text = string.Empty;
      textBoxNameMaster.Text = string.Empty;
      textBoxPasswordMaster.Text = string.Empty;
      textBoxRePasswordMaster.Text = string.Empty;
    }

    private void buttonEditUserMaster_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditUser)
      {
        OnEditMasterUser(true);
        isUpdatingUser = true;
        return;
      }
    }

    private void buttonDeleteUserMaster_Click(object sender, EventArgs e)
    {
      if (_currentUserSelection != null)
      {
        DialogResult dr = MessageBox.Show(
          string.Format("Apakah anda benar ingin menghapus User {0}", _currentUserSelection.Name),
          "Konfirmasi Hapus",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        if (dr == DialogResult.OK)
        {
          controller.DeleteUser(_currentUserSelection);
          OnEditMasterUser(false);
        }
      }
    }

    private void dataGridViewUserMaster_SelectionChanged(object sender, EventArgs e)
    {
      DataGridViewSelectedCellCollection selectedCells = dataGridViewUserMaster.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewUserMaster.Rows[viewCell.RowIndex];
        var user = viewRow.DataBoundItem as User;
        if (user != null)
        {
          UpdateDetailUser(user);
        }
      }
      else
      {
        ClearFieldUser();
      }

    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

  }

}

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

namespace InventoryAndSales.GUI.Page
{
  public partial class MasterProductPage : UserControl
  {
    private MasterProductController controller;
    private Product _currentProductSelection;

    public MasterProductPage()
    {
      InitializeComponent();
      controller = new MasterProductController(this);
    }

    public void Reset()
    {
      OnEditMasterItem(false);
    }

    private Dictionary<int, Product> _itemDictRowIdToItem = new Dictionary<int, Product>();
    private string GenerateCode(string name)
    {
      string prefix = GeneratePrefix(name);
      int i = 0;
      string codeGenerated = prefix;
      while (i++ < 99999 && prefix.Length < 5)
      {
        codeGenerated = prefix + i.ToString().PadLeft(5 - prefix.Length, '0');
        //check if exitst
        bool exists = _itemDictRowIdToItem.Values.Any(product => product.Code.Equals(codeGenerated, StringComparison.InvariantCultureIgnoreCase));
        if (!exists) break;
      }
      return codeGenerated;
    }

    private string GeneratePrefix(string name)
    {
      string[] names = name.Split(' ');
      string prefix = string.Empty;
      foreach (string n in names)
      {
        string trim = n.Trim();
        if (trim.Length > 0 && char.IsLetter(trim[0]))
          prefix += trim[0];
      }
      return prefix.ToUpper();
    }

    private void buttonGenerateCode_Click(object sender, EventArgs e)
    {
      textBoxDetailItemCode.Text = GenerateCode(textBoxDetailItemName.Text);
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


    private volatile bool _isChanging;
    private void radioButtonDiscount_CheckedChanged(object sender, EventArgs e)
    {
      if (!_isChanging)
      {
        _isChanging = true;
        if (radioButtonDiscountAmount.Enabled)
          textBoxDetailItemDiscountAmount.Enabled = radioButtonDiscountAmount.Checked;
        if (radioButtonDiscountPercent.Enabled)
          textBoxDetailItemDiscountPercent.Enabled = radioButtonDiscountPercent.Checked;
        _isChanging = false;
      }
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
      UpdateSelectedProduct();
    }

    private void UpdateSelectedProduct()
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


    private bool IsCodeFromName(string name, string code)
    {
      string prefix = GeneratePrefix(name);
      if (prefix.Length <= code.Length)
      {
        int num;
        string codePrefix = code.Substring(0, prefix.Length);
        string numeric = code.Substring(prefix.Length, code.Length - prefix.Length);
        if (codePrefix == prefix && int.TryParse(numeric, out num))
          return true;
        else
          return false;
      }
      return code.StartsWith(prefix) || prefix.StartsWith(code);
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
      //checkBoxAutoGenerate.Checked = true;
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
        if (!string.IsNullOrEmpty(item.Barcode) && !string.IsNullOrEmpty(barcode) && item.Barcode == barcode)
        {
          barcodeExist = true;
          break;
        }
      }

      if (barcodeExist)
        return "Barcode barang sudah ada.";
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
        UpdateSelectedProduct();
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
      //checkBoxAutoGenerate.Enabled = edit;
      textBoxDetailItemBarcode.Enabled = edit;
      textBoxDetailItemPrice.Enabled = edit;
      radioButtonDiscountAmount.Enabled = edit;
      radioButtonDiscountPercent.Enabled = edit;
      textBoxDetailItemDiscountPercent.Enabled = edit && radioButtonDiscountPercent.Checked;
      textBoxDetailItemDiscountAmount.Enabled = edit && radioButtonDiscountAmount.Checked;
      dataGridViewMasterItemList.Enabled = !edit;
      dataGridViewMasterItemList.ForeColor = edit ? Color.Gray : Color.Black;

      if (!edit) //on edit item done
      {
        List<Product> items = controller.GetItems();
        dataGridViewMasterItemList.DataSource = null;
        dataGridViewMasterItemList.DataSource = items;
      }
    }

    private string ValidateDetailItemInput()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(string.IsNullOrEmpty(textBoxDetailItemCode.Text) ? "Harap isi kode barang" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxDetailItemName.Text) ? "Harap isi nama barang" : string.Empty);
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
        if (dr == DialogResult.OK)
        {
          controller.RemoveItem(_currentProductSelection);
          OnEditMasterItem(false);
        }
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

    private void UpdateDetailBarang(Product product)
    {
      if (_currentProductSelection == product)
        return;
      _currentProductSelection = product;
      textBoxDetailItemName.Text = product.Name;
      textBoxDetailItemCode.Text = product.Code;
      //checkBoxAutoGenerate.Checked = IsCodeFromName(product.Name, product.Code);
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

    private void dataGridViewMasterItemList_Click(object sender, EventArgs e)
    {
      UpdateSelectedProduct();
    }

  }
}

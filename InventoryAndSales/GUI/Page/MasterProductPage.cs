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
using InventoryAndSales.Database.DataTable;
using System.IO;

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
      comboBoxSort.Items.Clear();
      comboBoxSort.Items.AddRange(controller.GetSortableColumns().ToArray());
      //comboBoxSort.SelectedIndex = 0;
    }

    public void Reset()
    {
      isUpdatingProduct = false;
      isAddingProduct = false;
      OnEditMasterItem(false);
    }

    private string GenerateCode(string name)
    {
      string prefix = GeneratePrefix(name);
      int i = 0;
      HashSet<string> _existingCode = new HashSet<string>();
      List<Product> items = controller.GetItems(string.Empty, string.Empty);
      foreach( Product item in items )
      {
        _existingCode.Add(item.Code);
      }
      string codeGenerated = prefix;
      while (i++ < 99999 && prefix.Length < 5)
      {
        codeGenerated = prefix + i.ToString().PadLeft(5 - prefix.Length, '0');
        //check if exitst
        bool exists = _existingCode.Contains( codeGenerated );
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
      }
      else if (isAddingProduct)
      {
        controller.AddItem(code, barcode, name, price, discount);
      }
      isUpdatingProduct = false;
      isAddingProduct = false;
      OnEditMasterItem(false);
    }

    private void buttonCancelEdit_Click(object sender, EventArgs e)
    {
      isUpdatingProduct = false;
      isAddingProduct = false;
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
        isUpdatingProduct = false;
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
      List<Product> items = controller.GetItems(string.Empty, comboBoxSort.SelectedItem.ToString());
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
        isAddingProduct = false;
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

      if (!edit && comboBoxSort != null) //on edit item done
      {
        if (comboBoxSort.SelectedItem == null)
          comboBoxSort.SelectedIndex = 0;
        string orderBy = comboBoxSort.SelectedItem?.ToString();
        List<Product> items = controller.GetItems(_searchedText, orderBy);
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
          isUpdatingProduct = false;
          isAddingProduct = false;
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

    private void textBoxSearch_TextChanged(object sender, EventArgs e)
    {

    }

    private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      Reset();
    }

    private string _searchedText = string.Empty;
    private void buttonSearch_Click(object sender, EventArgs e)
    {
      _searchedText = textBoxSearch.Text;
      Reset();
    }

    private void buttonExportItems_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog sfd = new SaveFileDialog())
      {
        sfd.Filter = "CSV Files|*.csv";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
          StringBuilder csvContent = new StringBuilder();

          // Write the header row
          var columnNames = new string[]
          {
            "Id","Code","Barcode","Name","Price","Discount"
          };
          csvContent.AppendLine(string.Join(",", columnNames));

          // Write the data rows
          List<Product> products = controller.GetItemsForExport();
          foreach (Product p in products)
          {
            // Manually build each line using the selected properties.
            var fields = new string[]
            {
                QuoteField(p.Id.ToString()),
                QuoteField(p.Code),
                QuoteField(p.Barcode),
                QuoteField(p.Name),
                QuoteField(p.Price.ToString()),
                QuoteField(p.Discount.ToString())
            };
            
            csvContent.AppendLine(string.Join(",", fields));
          }

          // Write to file
          File.WriteAllText(sfd.FileName, csvContent.ToString(), Encoding.UTF8);
          MessageBox.Show("File saved successfully!", "CSV Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }

    private string QuoteField(string field)
    {
      if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
      {
        field = field.Replace("\"", "\"\"");
        return $"\"{field}\"";
      }
      return field;
    }

    // Import products from CSV file.
    private List<Product> ImportProductsFromCsv(string filePath)
    {
      List<Product> products = new List<Product>();

      // Read all lines from the CSV file.
      string[] lines = File.ReadAllLines(filePath);
      if (lines.Length < 2)
        return products; // No data if there's only the header.

      // Skip the header row.
      for (int i = 1; i < lines.Length; i++)
      {
        string line = lines[i];
        if (string.IsNullOrWhiteSpace(line))
          continue;

        string[] fields = ParseCsvLine(line);
        // Expecting exactly 6 fields: Id, Code, Barcode, Name, Price, Discount.
        if (fields.Length < 6)
          continue;

        try
        {
          Product product = new Product
          {
            Id = fields[0] == "" ? 0 : int.Parse(fields[0]),
            Code = fields[1],
            Barcode = fields[2],
            Name = fields[3],
            Price = decimal.Parse(fields[4]),
            Discount = decimal.Parse(fields[5])
          };

          products.Add(product);
        }
        catch (Exception ex)
        {
          // Handle any conversion errors (e.g., log or notify the user).
          MessageBox.Show($"Error parsing line {i + 1}: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      return products;
    }

    // A basic CSV line parser that supports quotes and escaped quotes.
    private string[] ParseCsvLine(string line)
    {
      List<string> fields = new List<string>();
      bool inQuotes = false;
      StringBuilder field = new StringBuilder();
      for (int i = 0; i < line.Length; i++)
      {
        char c = line[i];
        if (c == '\"')
        {
          // If in quotes and next char is also a quote, then it's an escaped quote.
          if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
          {
            field.Append('\"');
            i++; // Skip the escaped quote.
          }
          else
          {
            inQuotes = !inQuotes; // Toggle the quoting flag.
          }
        }
        else if (c == ',' && !inQuotes)
        {
          fields.Add(field.ToString());
          field.Clear();
        }
        else
        {
          field.Append(c);
        }
      }
      fields.Add(field.ToString());
      return fields.ToArray();
    }

    private void buttonImport_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog ofd = new OpenFileDialog())
      {
        ofd.Filter = "CSV Files|*.csv";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
          List<Product> products = ImportProductsFromCsv(ofd.FileName);

          // Pass the imported products to your controller.
          controller.SetItemForImport(products);

          MessageBox.Show("File imported successfully!", "CSV Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      }
    }
  }
}

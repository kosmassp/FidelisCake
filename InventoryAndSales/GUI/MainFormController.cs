using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI
{
  public class MainFormController
  {
    private MainForm _mainForm;
    private CashierManager _cashierManager;
    private LoginManager _loginManager;
    private MasterManager _masterManager;
    private ReportManager _reportManager;
    public MainFormController(MainForm mainForm)
    {
      _mainForm = mainForm;
      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _reportManager = BusinessFactory.GetInstance().ReportManager;

      //Event
      _cashierManager.CartChange += CartChange;
    }

    private void CartChange(object sender, KeyValuePair<Product, int> args)
    {
      _mainForm.UpdateDataGridViewCart(args.Key, args.Value);
      decimal x, y;
      decimal total = _cashierManager.GetCartTotal(out x, out y);
      _mainForm.UpdateTotal(total);
    }

    public string Checkout(decimal payment, string notes, out string successMessage)
    {
      successMessage = string.Empty;
      if(payment < 0)
        return "Pembayaran kurang dari 0";

      decimal x, y;
      decimal total = _cashierManager.GetCartTotal(out x, out y);
      if (total <= 0)
        return "Tidak ada pembelian. Silahkan tambahkan item yang dibeli";
      decimal changes = payment - total;

      if (changes < 0)
        return "Pembayaran kurang dari harga yang harus dibayarkan.";

      try
      {
        Transaction transaction;
        List<TransactionDetail> transactionDetails;
        _cashierManager.Checkout(payment, notes, _loginManager.ActiveUser.Id, 1, out transaction, out transactionDetails );
        PrintPaymentNote(transaction, transactionDetails);
        successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ", changes.ToString(Constant.DISPLAY_CURRENCY));
        NewCart();
      }
      catch(Exception e)
      {
        return e.Message;
      }
      return string.Empty;
    }

    public bool Login(string username, string password)
    {
      bool loginSuccess = _loginManager.Login(username, password);
      if (loginSuccess)
      {
        User activeUser = _loginManager.ActiveUser;

        _mainForm.EnableMenu(activeUser.Role);
        _mainForm.UpdateActiveUser(activeUser.Name);
      }
      return loginSuccess;
    }

    List<Product> _items;
    public List<Product> GetItems()
    {
      if( _items == null || _items.Count == 0)
        _items = _masterManager.GetAllProduct();
      return _items;
    }

    private Font _printFont = new Font("Arial", 10);
    private const string lineSeparator = "======================================"; 
    public void PrintPaymentNote(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      List<StringPrint> stringToPrint = new List<StringPrint>();
      StringFormat centerString = new StringFormat();
      centerString.Alignment = StringAlignment.Center;
      StringFormat leftString = new StringFormat();
      centerString.Alignment = StringAlignment.Near;
      //Todo customize this
      stringToPrint.Add(new StringPrint("FIDELIS CAKE AND BAKERY", centerString));
      stringToPrint.Add(new StringPrint("JL MAYJEND SUTOYO", centerString));
      stringToPrint.Add(new StringPrint("BANJARNEGARA", centerString));
      stringToPrint.Add(new StringPrint("(0286) 595180", centerString));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint("TRX:" + transaction.Factur, centerString));
      stringToPrint.Add(new StringPrint(lineSeparator, centerString));

      foreach (TransactionDetail tDetail in transactionDetails)
      {
        stringToPrint.Add(new StringPrint(tDetail.ProductName, leftString));
        stringToPrint.Add(new StringPrint(tDetail.Quantity + " X " + tDetail.ProductPrice + " = " + tDetail.SubtotalPrice, leftString));
        if(tDetail.ProductDiscount > 0)
          stringToPrint.Add(new StringPrint("Discount: "+tDetail.SubtotalDiscount, leftString));
      }
      stringToPrint.Add(new StringPrint(lineSeparator, centerString));
      stringToPrint.Add(new StringPrint("TOTAL BELANJA :     " + transaction.TotalPrice, leftString));
      stringToPrint.Add(new StringPrint("TOTAL DISCOUNT:     " + transaction.TotalDiscount, leftString));
      stringToPrint.Add(new StringPrint("TOTAL         :     " + transaction.Total, leftString));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint("PEMBAYARAN    :     " + transaction.Payment, leftString));
      stringToPrint.Add(new StringPrint("KEMBALI       :     " + transaction.Exchange, leftString));
      //Todo customize this
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint(lineSeparator, centerString));
      stringToPrint.Add(new StringPrint("TERIMA KASIH", centerString));

      PrinterUtility.Print(stringToPrint, _printFont);
    }

    public void AddToCart(Product product)
    {
      _cashierManager.AddToCart(product, 1);
    }

    public void NewCart()
    {
      _cashierManager.NewCart();
      _mainForm.ResetCart();
    }

    public void UpdateCart(Product product, int value)
    {
      _cashierManager.UpdateItemCart(product, value);
    }

    public void AddItem(string code, string barcode, string name, decimal price, decimal discount)
    {
      _masterManager.AddProduct(new Product(code, barcode, name, price, discount, false));
      ResetItemList();
    }

    public void UpdateItem(Product currentProductSelection, string code, string barcode, string name, decimal price, decimal discount)
    {
      currentProductSelection.Code = code;
      currentProductSelection.Barcode = barcode;
      currentProductSelection.Name = name;
      currentProductSelection.Price = price;
      currentProductSelection.Discount = discount;
      _masterManager.UpdateProduct(currentProductSelection);
      ResetItemList();
    }

    public void RemoveItem(Product currentProductSelection)
    {
      _masterManager.DeleteProduct(currentProductSelection);
      ResetItemList();
    }

    private void ResetItemList()
    {
      _items = null;
      _items = _masterManager.GetAllProduct();
    }

    public void Logout()
    {
      _mainForm.EnableMenu(0);
      _mainForm.UpdateActiveUser("");
      _loginManager.Logout();
    }

    public void ShowSummaryReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> detailReport = _reportManager.GetDetailReport(start, stop);
      DataTable dataTableDetail = GetDataTable(detailReport, "DetailReport");

      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier(start, stop);
      DataTable dataTableDetailCashier = GetDataTable(reportSummaryByCashier, "SummaryReportCashier");

      List<Dictionary<string, string>> summaryReport = _reportManager.GetSummaryReport(start, stop);
      DataTable dataTable = GetDataTable(summaryReport, "SummaryReport");

      _mainForm.UpdateReportDataGridView(new DataTable[] {dataTable, dataTableDetail, dataTableDetailCashier});

    }

    private DataTable GetDataTable(List<Dictionary<string, string>> summaryReport, string tableName)
    {
      DataTable dataTable = new DataTable();
      if(summaryReport.Count > 0)
      {
        foreach ( string columnName in summaryReport[0].Keys )
        {
          dataTable.Columns.Add(columnName);
        }
        foreach (Dictionary<string, string> dictionary in summaryReport)
        {
          dataTable.Rows.Add(dictionary.Values.ToArray());
        }
      }
      return dataTable;
    }

    public List<User> GetUsers()
    {
      return _masterManager.GetUsers();
    }

    public void DeleteUser(User currentUserSelection)
    {
      _masterManager.DeleteUser(currentUserSelection);
    }

    public void UpdateUser(User currentUserSelection, string username, string name, string password, int role)
    {
      if ( string.IsNullOrEmpty(currentUserSelection.Password) || !currentUserSelection.Password.StartsWith(password) )
      {
        currentUserSelection.Password = HashUtility.GetEncryptedPass(password);
      }
      currentUserSelection.Name = name;
      currentUserSelection.Role = role;
      _masterManager.UpdateUser(currentUserSelection);
    }

    public void AddUser(string username, string name, string password, int role)
    {
      _masterManager.AddUser(new User(username, HashUtility.GetEncryptedPass(password), name, role, false));
    }
  }
}

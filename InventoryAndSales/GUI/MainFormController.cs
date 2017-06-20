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

    Transaction lastTransaction;
    List<TransactionDetail> lastTransactionDetails;
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
        lastTransaction = null;
        lastTransactionDetails = null;
        _cashierManager.Checkout(payment, notes, _loginManager.ActiveUser.Id, 1, out lastTransaction, out lastTransactionDetails );
        PrintPaymentNote(lastTransaction, lastTransactionDetails);
        successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ", changes.ToString(Constant.DISPLAY_CURRENCY));
        NewCart();
      }
      catch(Exception e)
      {
        return e.Message + Environment.NewLine + e.StackTrace;
      }
      return string.Empty;
    }

    public bool Login(string username, string password)
    {
      //TestPrint();
      bool loginSuccess = _loginManager.Login(username, password);
      if (loginSuccess)
      {
        User activeUser = _loginManager.ActiveUser;

        _mainForm.EnableMenu(activeUser.Role);
        _mainForm.UpdateActiveUser(activeUser.Name);
      }
      return loginSuccess;
    }

    private void TestPrint()
    {
      List<StringPrint> sp = new List<StringPrint>();
      StringFormat sf = new StringFormat();
      sf.Alignment = StringAlignment.Center;
      sp.Add(new StringPrint("FIDELIS CAKE AND BAKERY", sf));
      StringFormat sfl = new StringFormat();
      sfl.Alignment = StringAlignment.Center;
      sp.Add(new StringPrint(lineSeparator, sfl));
      PrinterUtility.Print(sp, new Font("Courier New", 9));
    }

    List<Product> _items;
    public List<Product> GetItems()
    {
      if( _items == null || _items.Count == 0)
        _items = _masterManager.GetAllProduct();
      return _items;
    }

    private Font _printFont = new Font("Courier New", 9);
    private const string lineSeparator = "================================="; 
    public void PrintPaymentNote(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      List<StringPrint> stringToPrint = new List<StringPrint>();
      StringFormat centerString = new StringFormat();
      centerString.Alignment = StringAlignment.Center;
      StringFormat leftString = new StringFormat();
      leftString.Alignment = StringAlignment.Near;
      //Todo customize this
      stringToPrint.Add(new StringPrint("FIDELIS CAKE AND BAKERY", centerString));
      stringToPrint.Add(new StringPrint("JL MAYJEND SUTOYO NO 1", centerString));
      stringToPrint.Add(new StringPrint("BANJARNEGARA", centerString));
      stringToPrint.Add(new StringPrint("(0286) 594573", centerString));
      stringToPrint.Add(new StringPrint(lineSeparator, leftString));
      stringToPrint.Add(new StringPrint(transaction.Time.ToString("dd-MM-yyyy HH:mm") + "  ID:" + transaction.Factur));
      stringToPrint.Add(new StringPrint("KASIR:" + _loginManager.ActiveUser.Name));
      stringToPrint.Add(new StringPrint(lineSeparator, leftString));

      foreach (TransactionDetail tDetail in transactionDetails)
      {
        stringToPrint.Add(new StringPrint(tDetail.ProductName, leftString));
        stringToPrint.Add(new StringPrint(tDetail.Quantity + " X " + tDetail.ProductPrice + " = " + tDetail.SubtotalPrice, leftString));
        if(tDetail.ProductDiscount > 0)
          stringToPrint.Add(new StringPrint("Discount: "+tDetail.SubtotalDiscount, leftString));
      }
      stringToPrint.Add(new StringPrint(lineSeparator, leftString));
      stringToPrint.Add(new StringPrint("TOTAL BELANJA : Rp. " + transaction.TotalPrice, leftString));
      stringToPrint.Add(new StringPrint("TOTAL DISCOUNT: Rp. " + transaction.TotalDiscount, leftString));
      stringToPrint.Add(new StringPrint("TOTAL         : Rp. " + transaction.Total, leftString));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint("PEMBAYARAN    : Rp. " + transaction.Payment, leftString));
      stringToPrint.Add(new StringPrint("KEMBALI       : Rp. " + transaction.Exchange, leftString));
      //Todo customize this
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint(lineSeparator, leftString));
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
      List<Dictionary<string, string>> reportSummaryByCashier = _reportManager.GetReportSummaryByCashier(start, stop);
      DataTable dataTableSummaryCashier = GetDataTable(reportSummaryByCashier, "SummaryReportCashier");

      List<Dictionary<string, string>> reportSummaryByTransaction = _reportManager.GetReportSummaryByTransaction(start, stop);
      DataTable dataTableSummaryTransaction = GetDataTable(reportSummaryByTransaction, "SummaryReportTransaction");

      List<Dictionary<string, string>> summaryReport = _reportManager.GetSummaryReportProduct(start, stop);
      DataTable dataTableSummaryProduct = GetDataTable(summaryReport, "SummaryReportProduct");

      _mainForm.UpdateReportDataGridView(new DataTable[] { dataTableSummaryProduct, dataTableSummaryTransaction, dataTableSummaryCashier });

    }

    public void ShowDetailReport(DateTime start, DateTime stop)
    {
      List<Dictionary<string, string>> detailReport = _reportManager.GetDetailReport(start, stop);
      DataTable dataTableDetail = GetDataTable(detailReport, "DetailReport");

      _mainForm.UpdateReportDetailDataGridView(dataTableDetail);

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

    public bool PrintLastReceipt()
    {
      if (lastTransaction != null && lastTransactionDetails != null)
      {
        PrintPaymentNote(lastTransaction, lastTransactionDetails);
        return true;
      }
      return false;

    }

    public bool PrintReceipt(string facturNumber)
    {
      List<TransactionDetail> details;
      Transaction t = _cashierManager.GetTransaction(facturNumber, out details);
      if (t != null && details != null)
      {
        PrintPaymentNote(t, details);
        return true;
      }
      return false;
    }
  }
}

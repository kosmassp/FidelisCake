using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI
{
  public class MainFormController
  {
    private MainForm _mainForm;
    private CashierManager _cashierManager;
    private LoginManager _loginManager;
    public MainFormController(MainForm mainForm)
    {
      _mainForm = mainForm;
      _loginManager = LoginManager.Instance;
      _cashierManager = CashierManager.Instance;
    }

    internal void Initialize()
    {
      //load barang
    }

    public string Checkout(decimal payment, string notes)
    {
      if(payment < 0)
        return "Pembayaran kurang dari 0";

      //decimal total = _cashierManager.GetCartTotal();
      //if (payment < total)
      //  return "Pembayaran kurang dari 0";

      try
      {
        _cashierManager.Checkout(payment, notes);
      }
      catch(Exception e)
      {
        return e.StackTrace;
      }
      return string.Empty;
    }

    public bool Login(string username, string password)
    {
      return _loginManager.Login(username, password);
    }

    private Font _printFont = new Font("Arial", 10);
    public void PrintPaymentNote()
    {
      List<StringPrint> stringToPrint = new List<StringPrint>();
      StringFormat centerString = new StringFormat();
      centerString.Alignment = StringAlignment.Center;
      //Todo customize this
      stringToPrint.Add(new StringPrint("FIDELIS CAKE AND BAKERY", centerString));
      stringToPrint.Add(new StringPrint("JL ", centerString));
      stringToPrint.Add(new StringPrint("BANJARNEGARA", centerString));
      stringToPrint.Add(new StringPrint("(0286) ", centerString));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      stringToPrint.Add(new StringPrint(DateTime.Now.ToString("dd-MM-yyyy HH:mm")));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));
      PrinterUtility.Print(stringToPrint, _printFont);
    }

    public bool AddToCart(Item item)
    {
      return _cashierManager.AddToCart(item, 1);
    }

    public void NewCart()
    {
      _cashierManager.NewCart();
    }
  }
}

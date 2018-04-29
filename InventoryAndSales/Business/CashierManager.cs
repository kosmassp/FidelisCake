using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  public class CashierManager
  {
    private readonly TransactionManager _transactionManager;
    private readonly UserManager _userManager;
    public CashierManager(TransactionManager transactionManager, UserManager userManager)
    {
      _transactionManager = transactionManager;
      _userManager = userManager;
      _cart = new Dictionary<int, TransactionDetail>();
    }



    public decimal GetCartTotal(out decimal totalPrice, out decimal totalDiscount)
    {
      totalPrice = 0;
      totalDiscount = 0;
      foreach (KeyValuePair<int, TransactionDetail> cartItem in _cart)
      {
        totalPrice += cartItem.Value.SubtotalPrice;
        totalDiscount += cartItem.Value.SubtotalDiscount;
      }
      return totalPrice - totalDiscount;
    }
    
    public delegate void CartChangeDelegate(object sender, KeyValuePair<Product, int> args);
    public event CartChangeDelegate CartChange;
    private Dictionary<int, TransactionDetail> _cart;
    private readonly object _lockCart = new object();
    public bool AddToCart(Product product, int quantity)
    {
      lock (_lockCart)
      {
        try
        {
          if (_cart.ContainsKey(product.Id))
            _cart[product.Id].UpdateQuantity(_cart[product.Id].Quantity + quantity);
          else
          {
            if(quantity > 0)
              _cart.Add(product.Id, new TransactionDetail(product, quantity));
          }
          InvokeCartChanges(product, _cart[product.Id].Quantity);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    private void InvokeCartChanges(Product product, int quantity)
    {
      if(CartChange != null)
      {
        CartChange(this, new KeyValuePair<Product, int>(product, quantity));
      }
    }

    public bool UpdateItemCart(Product product, int quantity)
    {
      lock (_lockCart)
      {
        try
        {
          if (quantity <= 0)
            return RemoveItemCart(product);
          if (_cart.ContainsKey(product.Id))
            _cart[product.Id].UpdateQuantity(quantity);
          else
            _cart.Add(product.Id, new TransactionDetail(product, quantity));
          InvokeCartChanges(product, quantity);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }
    public void NewCart()
    {
      lock (_lockCart)
      {
        //TODO: Invoke clear cart or invoke cart updated.
        //foreach (var productId in _cart.Keys)
        //{
        //  _productManager.
        //}
        _cart.Clear();
      }
    }
    public bool RemoveItemCart(Product product)
    {
      lock (_lockCart)
      {
        try
        {
          if (_cart.ContainsKey(product.Id))
            _cart.Remove(product.Id);
          InvokeCartChanges(product, 0);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    public void UpdateCheckout(Transaction _originalTransaction, decimal payment, string notes, int userId, int customerId)
    {
      List<TransactionDetail> transactionDetails;
      Transaction transaction = GenerateTransactionAndDetails(notes, payment, userId, customerId, out transactionDetails);
      _transactionManager.UpdateCompleteTransaction(_originalTransaction, transaction, transactionDetails);
      PrintPaymentNote(transaction, transactionDetails);
    }

    private string _lastFactur;
    public string GetLastFactur()
    {
      return _lastFactur;
    }

    public void Checkout(decimal payment, string notes, int userId, int customerId)
    {
      List<TransactionDetail> transactionDetails;
      Transaction transaction = GenerateTransactionAndDetails(notes, payment, userId, customerId, out transactionDetails);
      _transactionManager.SaveCompleteTransaction(transaction, transactionDetails);
      _lastFactur = transaction.Factur;
      PrintPaymentNote(transaction, transactionDetails);
    }

    private Transaction GenerateTransactionAndDetails(string notes, decimal payment, int userId, int customerId, out List<TransactionDetail> transactionDetails)
    {
      Transaction transaction = new Transaction();
      transaction.TotalPrice = 0;
      transaction.TotalDiscount = 0;
      transaction.Total = 0;
      transaction.Notes = notes;
      transaction.Time = DateTime.Now;
      transaction.Factur = GenerateFactur();
      transaction.Payment = payment;
      transaction.UserId = userId;
      transaction.CustomerId = customerId;
      transactionDetails = new List<TransactionDetail>();
      foreach (KeyValuePair<int, TransactionDetail> detail in _cart)
      {
        TransactionDetail td = detail.Value;
        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
        transactionDetails.Add(td);
      }
      transaction.Exchange = transaction.Payment - transaction.Total;
      return transaction;
    }

    private string GenerateFactur()
    {
      DateTime Now = DateTime.Now;
      string factur = ConvertToChar(Now.Year) + ConvertToChar(Now.DayOfYear) + "_" + 
                      ConvertToChar(Now.Hour) + ConvertToChar(Now.Minute) + ConvertToChar(Now.Second) + ConvertToChar(Now.Millisecond);
      return factur;

    }


    public string ConvertToChar(int value)
    {
      try
      {
        if (value >= StringNumber.Length)
        {
          return ConvertToChar(value/StringNumber.Length) + ConvertToChar(value%StringNumber.Length);
        }
        return StringNumber[value];
      }
      catch(Exception e)
      {
        return value.ToString();
      }
    }

    private string[] StringNumber = new string[]
                                      {
                                        "A", 
                                        "B",
                                        "C",
                                        "D",
                                        "E",
                                        "F",
                                        "G",
                                        "H",
                                        "I",
                                        "J",
                                        "K",
                                        "L",
                                        "M",
                                        "N",
                                        "O",
                                        "P",
                                        "Q",
                                        "R",
                                        "S",
                                        "T",
                                        "U",
                                        "V",
                                        "W",
                                        "X",
                                        "Y",
                                        "Z",
                                        "0",
                                        "1",
                                        "2",
                                        "3",
                                        "4",
                                        "5",
                                        "6",
                                        "7",
                                        "8",
                                        "9",
                                        "a",
                                        "b",
                                        "c",
                                        "d",
                                        "e",
                                        "f",
                                        "g",
                                        "h",
                                        "i",
                                        "j",
                                        "k",
                                        "l",
                                        "m",
                                        "n",
                                        "o",
                                        "p",
                                        "q",
                                        "r",
                                        "s",
                                        "t",
                                        "u",
                                        "v",
                                        "w",
                                        "x",
                                        "y",
                                        "z"
                                      };

    public Transaction GetTransaction(string facturNumber, out List<TransactionDetail> details)
    {
      return _transactionManager.GetTransaction(facturNumber, out details);
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
      PrinterUtility.Print(sp, _printFont);
    }

    private Font _printFont = new Font("Courier New", 9);
    private const string lineSeparator = "=================================";
    public void PrintPaymentNote(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      User cashier = _userManager.FindById(transaction.UserId);
      if(cashier == null)
        return;
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
      stringToPrint.Add(new StringPrint("KASIR:" + cashier.Name));
      stringToPrint.Add(new StringPrint(lineSeparator, leftString));

      foreach (TransactionDetail tDetail in transactionDetails)
      {
        stringToPrint.Add(new StringPrint(tDetail.ProductName, leftString));
        stringToPrint.Add(new StringPrint(tDetail.Quantity + " X " + tDetail.ProductPrice + " = " + tDetail.SubtotalPrice, leftString));
        if (tDetail.ProductDiscount > 0)
          stringToPrint.Add(new StringPrint("Discount: " + tDetail.SubtotalDiscount, leftString));
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

  }

}

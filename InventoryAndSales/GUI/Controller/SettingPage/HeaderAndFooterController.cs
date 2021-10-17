using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Popup.SettingPage;
using SimpleCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  class HeaderAndFooterController
  {
    private HeaderAndFooterForm _headerAndFooter;
    private CashierManager _cashierManager;

    public HeaderAndFooterController(HeaderAndFooterForm headerAndFooter)
    {
      this._headerAndFooter = headerAndFooter;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      headerAndFooter.SetPaymentNoteFont(_cashierManager.GetPrintFont());
    }

    internal string GetHeader()
    {
      return _cashierManager.GetHeaderNote();
    }

    internal string GetFooter()
    {
      return _cashierManager.GetFooterNote();
    }

    internal List<StringPrint> GetExample(string headers, string footers)
    {
      Transaction transaction = new Transaction();
      transaction.TotalPrice = 0;
      transaction.TotalDiscount = 0;
      transaction.Total = 0;
      transaction.Notes = "CONTOH";
      transaction.Time = DateTime.Now;
      transaction.Factur = "KODE_UNIK_FACTUR";
      transaction.Payment = 0;
      transaction.UserId = 0;
      transaction.CustomerId = 0;
      var transactionDetails = new List<TransactionDetail>();
      for(int i = 1; i <= 3; i++)
      {
        TransactionDetail td = new TransactionDetail();
        td.ProductName = "NAMA_PRODUK " + i;
        td.ProductId = 0;
        td.ProductDiscount = ( i / 3 ) * 500;
        td.ProductPrice = i * 5000;
        td.Quantity = i * 2;
        td.SubtotalDiscount = td.ProductDiscount * td.Quantity;
        td.SubtotalPrice = td.ProductPrice * td.Quantity;
        td.Subtotal = td.SubtotalPrice - td.SubtotalDiscount;

        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
        transactionDetails.Add(td);
      }
      transaction.Exchange = transaction.Payment - transaction.Total;

      User user = new User();
      user.Id = 0;
      user.Name = "NAMA_KASIR";
      user.Role = 1;
      return  CashierManager.GeneratePaymentNote(headers, footers, transaction, transactionDetails, user);
    }

    public void SetHeader(string text)
    {
      _cashierManager.SetHeaderNote(text);
    }

    public void SetFooter(string text)
    {
      _cashierManager.SetFooterNote(text);
    }
  }
}

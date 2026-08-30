using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Popup.SettingPage;
using SimpleCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  internal class HeaderAndFooterController
  {
    private readonly HeaderAndFooterForm _headerAndFooter;
    private readonly CashierManager _cashierManager;

    public HeaderAndFooterController(HeaderAndFooterForm headerAndFooter)
    {
      _headerAndFooter = headerAndFooter;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      // Preview is shown in the font the printer uses, so the alignment matches.
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

    public void SetHeader(string text)
    {
      _cashierManager.SetHeaderNote(text);
    }

    public void SetFooter(string text)
    {
      _cashierManager.SetFooterNote(text);
    }

    /// <summary>
    /// Renders a made-up sale through the real receipt builder, so the preview cannot drift away
    /// from what the printer produces. Nothing here touches the database or the printer.
    /// </summary>
    internal List<StringPrint> GetExample(string headers, string footers)
    {
      Transaction transaction = new Transaction
      {
        TotalPrice = 0,
        TotalDiscount = 0,
        Total = 0,
        Notes = "CONTOH",
        Time = DateTime.Now,
        Factur = "KODE_UNIK_FACTUR",
        Payment = 0,
        UserId = 0,
        CustomerId = 0,
      };

      var transactionDetails = new List<TransactionDetail>();
      for (int i = 1; i <= 3; i++)
      {
        TransactionDetail td = new TransactionDetail
        {
          ProductName = "NAMA_PRODUK " + i,
          ProductId = 0,
          // Integer division on purpose: only the third sample line carries a discount, so the
          // preview shows both a line with one and lines without.
          ProductDiscount = (i / 3) * 500,
          ProductPrice = i * 5000,
          Quantity = i * 2,
        };
        td.SubtotalDiscount = td.ProductDiscount * td.Quantity;
        td.SubtotalPrice = td.ProductPrice * td.Quantity;
        td.Subtotal = td.SubtotalPrice - td.SubtotalDiscount;

        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
        transactionDetails.Add(td);
      }
      transaction.Exchange = transaction.Payment - transaction.Total;

      return ReceiptBuilder.Build(headers, footers, transaction, transactionDetails, "NAMA_KASIR");
    }
  }
}

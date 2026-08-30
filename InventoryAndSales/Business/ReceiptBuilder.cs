using System;
using System.Collections.Generic;
using System.Drawing;

using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Turns a sale into the lines that get printed.
  ///
  /// Deliberately a pure function over its arguments: no database, no printer, no settings lookup.
  /// That is what lets the header/footer settings screen preview a made-up sale through this exact
  /// code, so what an operator sees in the preview is what the printer produces.
  /// </summary>
  public static class ReceiptBuilder
  {
    public const string LineSeparator = "=================================";

    /// <summary>Fixed width font - the column alignment below depends on it.</summary>
    public static Font CreateReceiptFont()
    {
      return new Font("Courier New", 9);
    }

    public static List<StringPrint> Build(
      string headerNotes,
      string footerNotes,
      Transaction transaction,
      List<TransactionDetail> transactionDetails,
      string cashierName)
    {
      List<StringPrint> stringToPrint = new List<StringPrint>();

      StringFormat centerString = new StringFormat { Alignment = StringAlignment.Center };
      StringFormat leftString = new StringFormat { Alignment = StringAlignment.Near };

      foreach (string header in SplitLines(headerNotes))
        stringToPrint.Add(new StringPrint(header, centerString));

      stringToPrint.Add(new StringPrint(LineSeparator, leftString));
      stringToPrint.Add(new StringPrint("TANGGAL : " + transaction.Time.ToString("dd-MM-yyyy HH:mm")));
      stringToPrint.Add(new StringPrint("FACTUR  : " + transaction.Factur));
      stringToPrint.Add(new StringPrint("KASIR   : " + cashierName));
      stringToPrint.Add(new StringPrint(LineSeparator, leftString));

      foreach (TransactionDetail tDetail in transactionDetails)
      {
        stringToPrint.Add(new StringPrint(tDetail.ProductName, leftString));
        stringToPrint.Add(new StringPrint(
          tDetail.Quantity + " x Rp." + tDetail.ProductPrice.ToString("N") + " = " + tDetail.SubtotalPrice.ToString("N"),
          leftString));
        if (tDetail.ProductDiscount > 0)
          stringToPrint.Add(new StringPrint("Discount: Rp." + tDetail.SubtotalDiscount.ToString("N"), leftString));
      }

      stringToPrint.Add(new StringPrint(LineSeparator, leftString));
      stringToPrint.Add(new StringPrint("Total Item   : Rp. " + transaction.TotalPrice.ToString("N"), leftString));
      stringToPrint.Add(new StringPrint("Total Disc   : Rp. " + transaction.TotalDiscount.ToString("N"), leftString));
      stringToPrint.Add(new StringPrint("Total Belanja: Rp. " + transaction.Total.ToString("N"), leftString));
      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));

      // A card or QRIS payment takes the exact amount, so change would always read zero. Show where
      // the money came through instead - that is what a customer or an auditor needs off the slip.
      switch (PaymentDetail.Parse(transaction.PaymentMethod))
      {
        case PaymentMethod.Edc:
          stringToPrint.Add(new StringPrint("EDC          : Rp. " + transaction.Payment.ToString("N"), leftString));
          if (!string.IsNullOrEmpty(transaction.PaymentReference))
            stringToPrint.Add(new StringPrint("Terminal     : " + transaction.PaymentReference, leftString));
          break;

        case PaymentMethod.Qris:
          stringToPrint.Add(new StringPrint("QRIS         : Rp. " + transaction.Payment.ToString("N"), leftString));
          if (!string.IsNullOrEmpty(transaction.PaymentReference))
            stringToPrint.Add(new StringPrint("Provider     : " + transaction.PaymentReference, leftString));
          string variant = PaymentDetail.DescribeVariant(transaction.PaymentVariant);
          if (!string.IsNullOrEmpty(variant))
            stringToPrint.Add(new StringPrint("Tipe QRIS    : " + variant, leftString));
          break;

        default:
          stringToPrint.Add(new StringPrint("Tunai        : Rp. " + transaction.Payment.ToString("N"), leftString));
          stringToPrint.Add(new StringPrint("Kembalian    : Rp. " + transaction.Exchange.ToString("N"), leftString));
          break;
      }

      stringToPrint.Add(new StringPrint(Environment.NewLine, centerString));

      foreach (string footer in SplitLines(footerNotes))
        stringToPrint.Add(new StringPrint(footer, centerString));

      return stringToPrint;
    }

    private static string[] SplitLines(string notes)
    {
      if (string.IsNullOrEmpty(notes))
        return new string[0];
      return notes.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
    }
  }
}

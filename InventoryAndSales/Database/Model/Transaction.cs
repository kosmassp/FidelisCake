using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InventoryAndSales.Database.Model
{
  public class Transaction : BaseObject
  {
    public long Id { get; set; }
    public string Factur { get; set; }
    public DateTime Time { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal Total { get; set; }
    public decimal Payment { get; set; }
    public decimal Exchange { get; set; }
    public string Notes { get; set; }
    public int UserId { get; set; }
    public long CustomerId { get; set; }
    public long Revision { get; set; }

    /// <summary>CASH, EDC or QRIS. Sales made before payment methods existed read as CASH.</summary>
    public string PaymentMethod { get; set; }

    /// <summary>EDC terminal, or QRIS provider. Empty for cash.</summary>
    public string PaymentReference { get; set; }

    /// <summary>STATIC or DYNAMIC for QRIS. Empty otherwise.</summary>
    public string PaymentVariant { get; set; }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id":
            return Id;
          case "Factur":
            return Factur;
          case "TransactionTime":
            return Time;
          case "TotalPrice":
            return TotalPrice;
          case "TotalDiscount":
            return TotalDiscount;
          case "Total":
            return Total;
          case "Payment":
            return Payment;
          case "Exchange":
            return Exchange;
          case "Notes":
            return Notes;
          case "UserId":
            return UserId;
          case "CustomerId":
            return CustomerId;
          case "Revision":
            return Revision;
          case "PaymentMethod":
            return PaymentMethod;
          case "PaymentReference":
            return PaymentReference;
          case "PaymentVariant":
            return PaymentVariant;
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
      }

      set
      {
        switch (columnName)
        {

          case "Id":
            Id = ToLong(value);
            break;
          case "Factur":
            Factur = ToText(value);
            break;
          case "TransactionTime":
            Time = ToDateTime(value);
            break;
          case "TotalPrice":
            TotalPrice = ToDecimal(value);
            break;
          case "TotalDiscount":
            TotalDiscount = ToDecimal(value);
            break;
          case "Total":
            Total = ToDecimal(value);
            break;
          case "Payment":
            Payment = ToDecimal(value);
            break;
          case "Exchange":
            Exchange = ToDecimal(value);
            break;
          case "Notes":
            Notes = ToText(value);
            break;
          case "UserId":
            UserId = ToInt(value);
            break;
          case "CustomerId":
            CustomerId = ToLong(value);
            break;
          case "Revision":
            Revision = ToLong(value);
            break;
          case "PaymentMethod":
            PaymentMethod = ToText(value);
            break;
          case "PaymentReference":
            PaymentReference = ToText(value);
            break;
          case "PaymentVariant":
            PaymentVariant = ToText(value);
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
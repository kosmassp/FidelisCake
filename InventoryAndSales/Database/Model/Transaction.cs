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
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
      }

      set
      {
        switch (columnName)
        {

          case "Id":
            Id = long.Parse(value.ToString());
            break;
          case "Factur":
            Factur = (string)value;
            break;
          case "TransactionTime":
            Time = (DateTime) value;
            break;
          case "TotalPrice":
            TotalPrice = (decimal)value;
            break;
          case "TotalDiscount":
            TotalDiscount = (decimal)value;
            break;
          case "Total":
            Total = (decimal)value;
            break;
          case "Payment":
            Payment = (decimal)value;
            break;
          case "Exchange":
            Exchange = (decimal)value;
            break;
          case "Notes":
            Notes = (string)value;
            break;
          case "UserId":
            UserId = (int)value;
            break;
          case "CustomerId":
            CustomerId = long.Parse(value.ToString());
            break;
          case "Revision":
            Revision = long.Parse(value.ToString());
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
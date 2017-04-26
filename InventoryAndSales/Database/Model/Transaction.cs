using System;
using System.Collections.Generic;

namespace InventoryAndSales.Database.Model
{
  public class Transaction : BaseObject
  {
    public Transaction()
    {
      TransactionDetails = new List<TransactionDetail>();
    }

    public int Id { get; set; }
    public string Factur { get; set; }
    public DateTime Time { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal Total { get; set; }
    public decimal Payment { get; set; }
    public decimal Exchange { get; set; }
    public string Notes { get; set; }
    public User Cashier { get; set; }
    public List<TransactionDetail> TransactionDetails { get; set; }

    public override Dictionary<string, object> Data
    {
      get { throw new NotImplementedException(); }
    }
  }
}
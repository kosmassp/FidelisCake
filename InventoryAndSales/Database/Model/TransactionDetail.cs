using System;
using System.Collections.Generic;

namespace InventoryAndSales.Database.Model
{
  public class TransactionDetail : BaseObject
  {
    public TransactionDetail(Item item, int quantity, Transaction transaction)
    {
      Item = item;
      Quantity = quantity;
      SubtotalDiscount = item.DiscountAmount * quantity;
      SubtotalPrice = item.Price * quantity;
      Subtotal = SubtotalPrice - SubtotalDiscount;
      Transaction = transaction;
    }

    public int Id { get; set; }
    public Item Item { get; set; }
    public int Quantity { get; set; }
    public decimal SubtotalDiscount { get; set; }
    public decimal SubtotalPrice { get; set; }
    public decimal Subtotal { get; set; }
    public Transaction Transaction { get; set; }

    public override Dictionary<string, object> Data
    {
      get { throw new NotImplementedException(); }
    }
  }
}
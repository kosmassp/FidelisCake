using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryAndSales.Database.Model
{
  public class TransactionDetail : BaseObject
  {
    public TransactionDetail()
    {
    }

    public TransactionDetail(Product product, int quantity)
      : this()
    {
      ProductName = product.Name;
      ProductId = product.Id;
      ProductDiscount = product.DiscountAmount;
      ProductPrice = product.Price;
      UpdateQuantity(quantity);
    }

    public void UpdateQuantity(int quantity)
    {
      if (quantity < 0)
        quantity = 0;
      Quantity = quantity;
      SubtotalDiscount = ProductDiscount * quantity;
      SubtotalPrice = ProductPrice * quantity;
      Subtotal = SubtotalPrice - SubtotalDiscount;
    }

    public long Id { get; set; }
    public int ProductId { get; set; }
    public decimal ProductDiscount { get; set; }
    public decimal ProductPrice { get; set; }
    //Not save. Only for display
    public string ProductName { get; set; }

    public int Quantity { get; set; }
    public decimal SubtotalDiscount { get; set; }
    public decimal SubtotalPrice { get; set; }
    public decimal Subtotal { get; set; }
    public long TransactionId { get; set; }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id":
            return Id;
          case "ProductId":
            return ProductId;
          case "ProductDiscount":
            return ProductDiscount;
          case "ProductPrice":
            return ProductPrice;
          case "Quantity":
            return Quantity;
          case "SubtotalDiscount":
            return SubtotalDiscount;
          case "SubtotalPrice":
            return SubtotalPrice;
          case "Subtotal":
            return Subtotal;
          case "TransactionId":
            return TransactionId;
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
          case "ProductId":
            ProductId = ToInt(value);
            break;
          case "ProductDiscount":
            ProductDiscount = ToDecimal(value);
            break;
          case "ProductPrice":
            ProductPrice = ToDecimal(value);
            break;
          case "Quantity":
            Quantity = ToInt(value);
            break;
          case "SubtotalDiscount":
            SubtotalDiscount = ToDecimal(value);
            break;
          case "SubtotalPrice":
            SubtotalPrice = ToDecimal(value);
            break;
          case "Subtotal":
            Subtotal = ToDecimal(value);
            break;
          case "TransactionId":
            TransactionId = ToLong(value);
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));

        }
      }
    }
  }
}
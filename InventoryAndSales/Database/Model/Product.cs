using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using InventoryAndSales.GUI.Utility;

namespace InventoryAndSales.Database.Model
{
  public class Product : BaseObject
  {
    [Browsable(false)]
    public int Id { get; set; }
    public string Code { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    [Browsable(false)]
    public bool Deleted { get; set; }
    [Browsable(false)]
    public decimal Discount { get; set; }

    public Product()  {}
    public Product(string code, string barcode, string name, decimal price, decimal discount, bool deleted)
      : this()
    {
      Code = code;
      Barcode = barcode;
      Name = name;
      Price = price;
      Discount = discount;
      Deleted = deleted;
    }

    [DisplayName("Discount")]
    public string DisplayDiscount
    {
      get
      {
        if (Discount < 0)
          return (-Discount) + " %";
        return Math.Min(Price, Discount).ToString(Constant.DISPLAY_CURRENCY);
      }
    }

    public decimal DiscountAmount
    {
      get
      {
        var discount = Discount;
        if (Discount < 0)
          discount = Price * (-Discount / 100);
        return Math.Min(Price, discount);
      }
    }

    public decimal NetPrice
    {
      get
      {
        return Price - DiscountAmount;
      }
    }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id": return Id;
          case "Code": return Code;
          case "Barcode": return Barcode;
          case "Name": return Name;
          case "Price": return Price;
          case "Discount": return Discount;
          case "Deleted": return Deleted;
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class",columnName));
      }

      set
      {
        switch (columnName)
        {
          case "Id":
            Id = (int)value;
            break;
          case "Code":
            Code = (string)value;
            break;
          case "Barcode":
            Barcode = (string)value;
            break;
          case "Name":
            Name = (string)value;
            break;
          case "Price":
            Price = (decimal)value;
            break;
          case "Discount":
            Discount = (decimal)value;
            break;
          case "Deleted":
            Deleted = (bool)value;
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }

  }
}

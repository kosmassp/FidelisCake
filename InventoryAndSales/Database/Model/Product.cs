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
    public string Name { get; set; }
    public decimal Price { get; set; }
    [Browsable(false)]
    public decimal Discount { get; set; }

    public Product()  {}
    public Product(string code, string name, decimal price, decimal discount)
      : this()
    {
      Code = code;
      Name = name;
      Price = price;
      Discount = discount;
    }

    public Product(int id, string code, string name, decimal price, decimal discount)
      : this(code, name, price, discount)
    {
      Id = id;
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
          case "Name": return Name;
          case "Price": return Price;
          case "Discount": return Discount;
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
          case "Name":
            Name = (string)value;
            break;
          case "Price":
            Price = (decimal)value;
            break;
          case "Discount":
            Discount = (decimal)value;
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }

  }
}

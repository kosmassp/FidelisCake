using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.GUI.Model
{
  public class ViewItemMaster
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    public ViewItemMaster(Product product)
    {
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

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database.Model
{
  public class Item : BaseObject
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    public decimal DiscountAmount
    {
      get
      {
        if (Discount < 0)
        {
          decimal discountPercent = Math.Max(-Discount, 0); //SubtotalDiscount is in minus, so it should less than 100
          return Price * (discountPercent / 100);
        }
        return Math.Min(Price, Discount);
      }
    }

    public override Dictionary<string, object> Data
    {
      get
      {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Id", Id);
        dict.Add("Code", Code);
        dict.Add("Name", Name);
        dict.Add("Price", Price);
        dict.Add("Discount", Discount);
        return dict;
      }
    }
  }
}

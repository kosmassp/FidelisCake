using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.GUI.Model
{
  public class ViewCartModel
  {
    public Item Item { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Subtotal { get; set; }


    public ViewCartModel(TransactionDetail detail)
    {
      
    }
  }
}

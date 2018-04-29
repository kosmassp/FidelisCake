using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;

namespace InventoryAndSales.GUI.Controller
{
  public class MasterProductController
  {
    private MasterProductPage control;
    private MasterManager _masterManager;
    public MasterProductController(MasterProductPage masterProductPage)
    {
      control = masterProductPage;
      _masterManager = BusinessFactory.GetInstance().MasterManager;

    }

    public void AddItem(string code, string barcode, string name, decimal price, decimal discount)
    {
      _masterManager.AddProduct(new Product(code, barcode, name, price, discount, false));
    }

    public void UpdateItem(Product currentProductSelection, string code, string barcode, string name, decimal price, decimal discount)
    {
      currentProductSelection.Code = code;
      currentProductSelection.Barcode = barcode;
      currentProductSelection.Name = name;
      currentProductSelection.Price = price;
      currentProductSelection.Discount = discount;
      _masterManager.UpdateProduct(currentProductSelection);
    }

    public void RemoveItem(Product currentProductSelection)
    {
      _masterManager.DeleteProduct(currentProductSelection);
    }

    public List<Product> GetItems()
    {
      return _masterManager.GetAllProduct();
    }
  }
}

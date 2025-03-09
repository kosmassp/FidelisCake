using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database.DataTable;
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

    public IList<string> GetSortableColumns()
    {
      return DataTableList.Instance.GetDataTable(typeof (Product)).Columns;
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

    public List<Product> GetItems(string nameLike, string orderBy)
    {
      return _masterManager.GetAllAvailable(nameLike, orderBy);
    }
    public List<Product> GetItemsForExport()
    {
      return _masterManager.GetAllProduct();
    }

    public void SetItemForImport(List<Product> products)
    {
      foreach (Product product in products)
      {
        if(product.Id == 0)
        {
          _masterManager.AddProduct(product);
        }
        else
        {
          _masterManager.UpdateProduct(product);
        }
      }
    }


  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  public class MasterManager
  {
    private readonly ProductManager _productManager;
    public MasterManager(ProductManager productManager)
    {
      _productManager = productManager;
    }

    public List<Product> GetAllProduct()
    {
      return _productManager.GetAll();
    }

    public void AddProduct(Product product)
    {
      _productManager.Save(product);
    }

    public void UpdateProduct(Product product)
    {
      _productManager.Update(product);
    }

    public void DeleteProduct(Product product)
    {
      _productManager.Delete(product);
    }
  }
}

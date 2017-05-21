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
    private readonly UserManager _userManager;
    public MasterManager(ProductManager productManager, UserManager userManager)
    {
      _productManager = productManager;
      _userManager = userManager;
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
      product.Deleted = true;
      _productManager.Update(product);
    }

    public List<User> GetUsers()
    {
      return _userManager.GetAll();
    }

    public void UpdateUser(User user)
    {
      _userManager.Update(user);
    }

    public void DeleteUser(User user)
    {
      user.Deleted = true;
      _userManager.Update(user);
    }

    public void AddUser(User user)
    {
      _userManager.Save(user);
    }
  }
}

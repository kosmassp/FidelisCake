using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class ProductManager : BaseManager<Product>
  {
    public ProductManager(ProductDao dao)
      : base(dao)
    {
    }


    public List<Product> GetAllAvailable(string criteria)
    {
      criteria = criteria.Replace(' ', '%');
      List<Product> items = BaseDao.FindByQuery(string.Format("WHERE Name like '%{0}%' and Deleted = '{1}'", criteria, false));
      return items;
    }
    public List<Product> GetAllAvailable(string criteria, string orderBy)
    {
      criteria = criteria.Replace(' ', '%');
      List<Product> items = BaseDao.FindByQuery(string.Format("WHERE Name like '%{0}%' and Deleted = '{1}' ", criteria, false), 
                                                orderBy);
      return items;
    }
  }
}

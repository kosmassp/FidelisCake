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

    public override List<Product> GetAll()
    {
      List<Product> items =  BaseDao.FindByQuery(string.Empty);
      //List<Product> items = new List<Product>();
      //items.Add(new Product(1, "A0001", "Roti Enak", 10000, 0));
      //items.Add(new Product( 2, "A0002", "Roti Enak Special", 15000, -5 ));
      //items.Add(new Product( 3, "A0003", "Roti Enak Keju", 12000, 2000 ));
      //items.Add(new Product( 4, "B0001", "Tart Enak", 20000, -15 ));
      //items.Add(new Product( 5, "B0002", "Tart Enak Special", 20000, -10 ));
      //items.Add(new Product( 6, "C0001", "Snack Enak", 5000, 500 ));
      //items.Add(new Product( 7, "C0002", "Snack Mahal", 50000, 0 ));
      return items;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class ItemManager : BaseManager<Item>
  {
    public ItemManager(BaseDao<Item> baseDao) : base(baseDao)
    {
    }
  }
}

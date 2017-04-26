using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class TransactionDetailManager : BaseManager<TransactionDetail>
  {
    public TransactionDetailManager(BaseDao<TransactionDetail> baseDao)
      : base(baseDao)
    {
    }
  }
}

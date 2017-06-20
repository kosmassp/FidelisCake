using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  public class TransactionDetailDao : BaseDao<TransactionDetail>
  {
    public TransactionDetailDao() 
      : base()
    {
    }

    public List<TransactionDetail> FindByTransactionId(long id)
    {
      return FindByQuery(string.Format("Where TransactionId = '{0}'", id));
    }
  }
}

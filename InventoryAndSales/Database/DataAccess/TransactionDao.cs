using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  public class TransactionDao : BaseDao<Transaction>
  {
    public TransactionDao() 
      : base()
    {
    }

    public Transaction FindByFactur(string factur)
    {
      List<Transaction> trx = this.FindByQuery(string.Format("WHERE Factur = '{0}'", factur));
      if(trx.Count > 0)
      {
        return trx[0];
      }
      return null;
    }
  }
}

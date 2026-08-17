using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
      if (string.IsNullOrEmpty(factur))
        return null;

      // AnsiText to match the column. Left to infer, a string parameter becomes Unicode, which makes
      // the server convert the column instead of the value and gives up the seek on
      // IDX_T_TRANS_FACTUR - a table scan on every reprint once a shop has years of sales.
      List<Transaction> trx = FindByQuery(
        string.Format("WHERE {0} = @factur", Dialect.Quote("Factur")),
        string.Empty,
        DbParam.AnsiText("@factur", 20, factur));
      if (trx.Count > 0)
        return trx[0];
      return null;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

      // Typed as VarChar to match the column. A string parameter would default to NVarChar, which
      // makes SQL Server convert the column instead of the value and gives up the seek on
      // IDX_T_TRANS_FACTUR - a table scan on every reprint once a shop has years of sales.
      List<Transaction> trx = FindByQuery(
        "WHERE Factur = @factur",
        string.Empty,
        new SqlParameter("@factur", SqlDbType.VarChar, 20) { Value = factur });
      if (trx.Count > 0)
        return trx[0];
      return null;
    }
  }
}

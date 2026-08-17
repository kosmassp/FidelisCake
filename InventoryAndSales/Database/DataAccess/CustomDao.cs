using System;
using System.Collections.Generic;
using System.Data.Common;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  /// <summary>
  /// The hand written reporting and browsing queries.
  ///
  /// Three conventions hold for every query here:
  ///  - only active transactions are reported (Revision = 0), so corrected and cancelled sales never
  ///    reach a total;
  ///  - the date range is a half-open interval on TransactionTime supplied as parameters, which
  ///    keeps the index usable and takes the thread culture out of the picture entirely;
  ///  - identifiers and result aliases are written through the dialect. Aliases matter more than
  ///    they look: SQL Server accepts 'Jumlah Transaksi' in single quotes, but to PostgreSQL that is
  ///    a string literal, not a name.
  ///
  /// The alias text itself is the report's column heading, so changing one renames a column in the
  /// grids and the exported HTML.
  /// </summary>
  public class CustomDao : BaseDao<CustomQuery>
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public CustomDao()
    {
    }

    // CustomQuery has no column map - every query here returns a different shape - so the inherited
    // metadata driven CRUD cannot work and must not be called.
    public override CustomQuery FindById(int id)
    {
      throw new NotSupportedException("CustomDao only runs the report queries defined on it.");
    }
    public override List<CustomQuery> FindByQuery(string whereClause)
    {
      throw new NotSupportedException("CustomDao only runs the report queries defined on it.");
    }
    public override List<CustomQuery> FindByQuery(string whereClause, string orderbyClause)
    {
      throw new NotSupportedException("CustomDao only runs the report queries defined on it.");
    }
    public override List<CustomQuery> FindByQuery(string whereClause, string orderbyClause, params DbParameter[] parameters)
    {
      throw new NotSupportedException("CustomDao only runs the report queries defined on it.");
    }
    public override bool Save(CustomQuery dataObject)
    {
      throw new NotSupportedException("CustomDao is read only.");
    }
    public override bool DeleteById(int id)
    {
      throw new NotSupportedException("CustomDao is read only.");
    }
    public override bool Delete(CustomQuery dataObject)
    {
      throw new NotSupportedException("CustomDao is read only.");
    }
    public override int Update(CustomQuery dataObject)
    {
      throw new NotSupportedException("CustomDao is read only.");
    }

    #region Query building helpers

    /// <summary>A qualified column reference, e.g. <c>t."Factur"</c>.</summary>
    private static string C(string tableAlias, string column)
    {
      return tableAlias + "." + Dialect.Quote(column);
    }

    /// <summary>A result column heading.</summary>
    private static string A(string alias)
    {
      return Dialect.QuoteAlias(alias);
    }

    private static string Table(string name)
    {
      return Dialect.Quote(name);
    }

    /// <summary>Transaction date without its time, for grouping.</summary>
    private static string TransactionDate()
    {
      return Dialect.ToDate(C("t", "TransactionTime"));
    }

    private static string ActiveInRange()
    {
      return string.Format(" WHERE {0} = 0 AND {1} >= @start AND {1} < @stop ",
                           C("t", "Revision"), C("t", "TransactionTime"));
    }

    private static string FromDetailsJoined(bool includeUser)
    {
      string sql = string.Format(
        " FROM {0} td LEFT JOIN {1} t ON (t.{2} = td.{3}) LEFT JOIN {4} p ON (p.{2} = td.{5})",
        Table("T_TRANSACTION_DETAILS"), Table("T_TRANSACTIONS"), Dialect.Quote("Id"),
        Dialect.Quote("TransactionId"), Table("M_PRODUCTS"), Dialect.Quote("ProductId"));

      if (includeUser)
        sql += string.Format(" LEFT JOIN {0} u ON (u.{1} = {2})",
                             Table("M_USERS"), Dialect.Quote("Id"), C("t", "UserId"));
      return sql;
    }

    private static string FromTransactionsJoined()
    {
      return string.Format(" FROM {0} t LEFT JOIN {1} u ON (u.{2} = {3})",
                           Table("T_TRANSACTIONS"), Table("M_USERS"), Dialect.Quote("Id"), C("t", "UserId"));
    }

    /// <summary>Cashier name, falling back for the built-in account which has no user row.</summary>
    private static string CashierName()
    {
      return string.Format("COALESCE(u.{0}, 'ADMIN')", Dialect.Quote("Name"));
    }

    /// <summary>Product name, falling back for a row deleted outside the application.</summary>
    private static string ProductName()
    {
      return string.Format("COALESCE(p.{0}, 'Telah Dihapus')", Dialect.Quote("Name"));
    }

    #endregion

    #region Reports

    public List<CustomQuery> GetReportDetailByTime(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Cashier") + "," +
        C("t", "Factur") + "," +
        C("t", "TransactionTime") + "," +
        ProductName() + " AS " + A("ProductName") + "," +
        C("td", "Quantity") + " AS " + A("Jumlah") + "," +
        C("td", "ProductPrice") + " AS " + A("Harga") + "," +
        C("td", "ProductDiscount") + " AS " + A("Diskon") + "," +
        C("td", "SubtotalPrice") + " AS " + A("Total Sebelum Diskon") + "," +
        C("td", "SubtotalDiscount") + " AS " + A("Total Diskon") + "," +
        C("td", "Subtotal") + " AS " + A("SubTotal") + "," +
        C("t", "Total") + " AS " + A("Total") +
        FromDetailsJoined(true) +
        ActiveInRange() +
        " ORDER BY " + C("t", "TransactionTime");
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByProduct(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + ProductName() + " AS " + A("ProductName") + "," +
        TransactionDate() + " AS " + A("TransactionDate") + "," +
        "COUNT(" + C("t", "Id") + ") AS " + A("Jumlah Transaksi") + "," +
        "SUM(" + C("td", "Quantity") + ") AS " + A("Jumlah Barang Terjual") + "," +
        "SUM(" + C("td", "SubtotalPrice") + ") AS " + A("Total Sebelum Diskon") + "," +
        "SUM(" + C("td", "SubtotalDiscount") + ") AS " + A("Total Diskon") + "," +
        "SUM(" + C("td", "Subtotal") + ") AS " + A("Total") +
        FromDetailsJoined(false) +
        ActiveInRange() +
        " GROUP BY " + ProductName() + ", " + TransactionDate() +
        " ORDER BY " + TransactionDate();
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByUserId(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Kasir") + "," +
        TransactionDate() + " AS " + A("Tanggal Transaksi") + "," +
        "COUNT(DISTINCT " + C("t", "Id") + ") AS " + A("Jumlah Transaksi") + "," +
        "SUM(" + C("td", "Quantity") + ") AS " + A("Jumlah Barang Terjual") + "," +
        "SUM(" + C("td", "SubtotalPrice") + ") AS " + A("Total Sebelum Diskon") + "," +
        "SUM(" + C("td", "SubtotalDiscount") + ") AS " + A("Total Diskon") + "," +
        "SUM(" + C("td", "Subtotal") + ") AS " + A("Total") +
        FromDetailsJoined(true) +
        ActiveInRange() +
        " GROUP BY " + CashierName() + ", " + TransactionDate() +
        " ORDER BY " + TransactionDate();
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Kasir") + "," +
        C("t", "Factur") + "," +
        TransactionDate() + " AS " + A("Tanggal Transaksi") + "," +
        C("t", "Total") + " AS " + A("Total") + "," +
        C("t", "Notes") + " AS " + A("Catatan") + "," +
        C("t", "Payment") + " AS " + A("Pembayaran") + "," +
        C("t", "Exchange") + " AS " + A("Kembalian") +
        FromTransactionsJoined() +
        ActiveInRange() +
        " ORDER BY " + C("t", "TransactionTime");
      return ExecuteReader(sql, DateRange(start, stop));
    }

    /// <summary>Backs the transaction picker, so Id and Factur have to come through unaliased.</summary>
    public List<CustomQuery> GetTransaction(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Kasir") + "," +
        C("t", "Id") + "," +
        C("t", "Factur") + "," +
        C("t", "TransactionTime") + " AS " + A("Tanggal Transaksi") + "," +
        C("t", "Total") + " AS " + A("Total") + "," +
        C("t", "Notes") + " AS " + A("Catatan") +
        FromTransactionsJoined() +
        ActiveInRange() +
        " ORDER BY " + C("t", "TransactionTime");
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public string GetTodaySummaryByCashier(User activeUser, DateTime date)
    {
      string sql =
        " SELECT COALESCE(SUM(" + C("t", "Total") + "), 0) AS " + A("SUMTOTAL") +
        " FROM " + Table("T_TRANSACTIONS") + " t" +
        ActiveInRange() +
        " AND " + C("t", "UserId") + " = @userId";

      List<DbParameter> parameters = new List<DbParameter>(DateRange(date, date));
      parameters.Add(DbParam.Of("@userId", activeUser.Id));

      var retValue = ExecuteReader(sql, parameters.ToArray());
      if (retValue.Count == 0)
        return "Rp. 0";
      return "Rp. " + retValue[0]["SUMTOTAL"];
    }

    #endregion

    /// <summary>
    /// Builds the half-open range [start 00:00, stop+1day 00:00). Both pickers are inclusive dates
    /// to the operator, and comparing against the raw column keeps IDX_T_TRANS_TRXTIME usable.
    /// </summary>
    private static DbParameter[] DateRange(DateTime start, DateTime stop)
    {
      DateTime from = start.Date;
      DateTime toExclusive = stop.Date.AddDays(1);
      if (toExclusive < from)
      {
        // Operator picked the dates the wrong way round; treat it as a single day.
        toExclusive = from.AddDays(1);
      }
      return new[]
      {
        DbParam.Of("@start", from),
        DbParam.Of("@stop", toExclusive),
      };
    }

    /// <summary>
    /// Projects by result-set ordinal rather than a fixed column list, because each report returns a
    /// different shape.
    /// </summary>
    protected override List<CustomQuery> ExecuteReader(String commandText, params DbParameter[] parameters)
    {
      DbConnection connection = DBFactory.GetInstance().GetConnection();
      DbTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      bool ownsConnection = activeTransaction == null;
      if (ownsConnection)
        connection.Open();
      try
      {
        List<CustomQuery> returnList = new List<CustomQuery>();
        using (DbCommand command = connection.CreateCommand())
        {
          command.CommandText = commandText;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          DBUtility.AddParameters(command, parameters);
          using (DbDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              CustomQuery t = new CustomQuery();
              for (int i = 0; i < reader.FieldCount; i++)
              {
                if (!(reader.GetValue(i) is DBNull))
                  t[reader.GetName(i)] = reader.GetValue(i);
              }
              returnList.Add(t);
            }
          }
        }
        return returnList;
      }
      catch (Exception ex)
      {
        _log.Error(string.Format("Trying to execute: {0}", commandText), ex);
        throw;
      }
      finally
      {
        if (ownsConnection)
          connection.Close();
      }
    }
  }
}

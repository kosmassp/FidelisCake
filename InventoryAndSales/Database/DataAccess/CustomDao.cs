using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  /// <summary>
  /// The hand written reporting and browsing queries.
  ///
  /// Two conventions hold for every query here:
  ///  - only active transactions are reported (Revision = 0), so corrected and cancelled sales never
  ///    reach a total;
  ///  - the date range is a half-open interval on TransactionTime supplied as parameters, which
  ///    keeps the index usable and takes the thread culture out of the picture entirely.
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
    public override List<CustomQuery> FindByQuery(string whereClause, string orderbyClause, params SqlParameter[] parameters)
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

    private const string DATE_RANGE = " AND t.TransactionTime >= @start AND t.TransactionTime < @stop ";

    private const string QUERY_REPORT_DETAIL_BY_TIME =
      " select" +
      " COALESCE(u.Name, 'ADMIN') as Cashier," +
      " t.Factur," +
      " t.TransactionTime," +
      " COALESCE(p.Name,'Telah Dihapus') as ProductName," +
      " Quantity as 'Jumlah'," +
      " td.ProductPrice as Harga," +
      " td.ProductDiscount as Diskon," +
      " SubtotalPrice as 'Total Sebelum Diskon'," +
      " SubtotalDiscount as 'Total Diskon'," +
      " Subtotal as 'SubTotal'," +
      " t.Total as 'Total'" +
      " from T_TRANSACTION_DETAILS td" +
      " left join T_TRANSACTIONS t on (t.Id = td.TransactionId)" +
      " left join M_PRODUCTS p on (p.Id = td.ProductId)" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0" + DATE_RANGE +
      " order by t.TransactionTime ";

    private const string QUERY_REPORT_SUMMARY_BY_PRODUCT =
      " select" +
      " COALESCE(p.Name,'Telah Dihapus') as ProductName," +
      " CAST(t.TransactionTime as date) as 'TransactionDate'," +
      " count(t.Id) as 'Jumlah Transaksi'," +
      " sum(Quantity) as 'Jumlah Barang Terjual'," +
      " sum(SubtotalPrice) as 'Total Sebelum Diskon'," +
      " sum(SubtotalDiscount) as 'Total Diskon'," +
      " sum(Subtotal) as 'Total'" +
      " from T_TRANSACTION_DETAILS td" +
      " left join T_TRANSACTIONS t on (t.Id = td.TransactionId)" +
      " left join M_PRODUCTS p on (p.Id = td.ProductId)" +
      " where t.Revision = 0" + DATE_RANGE +
      " group by COALESCE(p.Name,'Telah Dihapus'),CAST(t.TransactionTime as date)" +
      " order by CAST(t.TransactionTime as date)";

    private const string QUERY_REPORT_SUMMARY_BY_USER_ID =
      " select" +
      " COALESCE(u.Name,'ADMIN') as Kasir," +
      " CAST(t.TransactionTime as date) as 'Tanggal Transaksi'," +
      " count(distinct t.Id) as 'Jumlah Transaksi'," +
      " sum(Quantity) as 'Jumlah Barang Terjual'," +
      " sum(SubtotalPrice) as 'Total Sebelum Diskon'," +
      " sum(SubtotalDiscount) as 'Total Diskon'," +
      " sum(Subtotal) as 'Total'" +
      " from T_TRANSACTION_DETAILS td" +
      " left join T_TRANSACTIONS t on (t.Id = td.TransactionId)" +
      " left join M_PRODUCTS p on (p.Id = td.ProductId)" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0" + DATE_RANGE +
      " group by COALESCE(u.Name,'ADMIN'),CAST(t.TransactionTime as date)" +
      " order by CAST(t.TransactionTime as date)";

    private const string QUERY_TODAY_SUMMARY_BY_USER_ID =
      " SELECT COALESCE(SUM(t.[Total]),0) SUMTOTAL" +
      " FROM T_TRANSACTIONS t" +
      " WHERE t.UserId = @userId" +
      " and t.Revision = 0" + DATE_RANGE;

    private const string QUERY_REPORT_SUMMARY_BY_TRANSACTION =
      " select" +
      " COALESCE(u.Name,'ADMIN') as Kasir," +
      " t.Factur," +
      " CAST(t.TransactionTime as date) as 'Tanggal Transaksi'," +
      " t.Total as 'Total'," +
      " t.Notes as 'Catatan'," +
      " t.Payment as 'Pembayaran'," +
      " t.Exchange as 'Kembalian'" +
      " from T_TRANSACTIONS t" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0" + DATE_RANGE +
      " order by t.TransactionTime";

    private const string QUERY_VIEW_TRANSACTION =
      " select" +
      " COALESCE(u.Name,'ADMIN') as Kasir," +
      " t.Id," +
      " t.Factur," +
      " t.TransactionTime as 'Tanggal Transaksi'," +
      " t.Total as 'Total'," +
      " t.Notes as 'Catatan'" +
      " from T_TRANSACTIONS t" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0" + DATE_RANGE +
      " order by t.TransactionTime";

    public List<CustomQuery> GetReportSummaryByProduct(DateTime start, DateTime stop)
    {
      return ExecuteReader(QUERY_REPORT_SUMMARY_BY_PRODUCT, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      return ExecuteReader(QUERY_REPORT_SUMMARY_BY_TRANSACTION, DateRange(start, stop));
    }

    public List<CustomQuery> GetTransaction(DateTime start, DateTime stop)
    {
      return ExecuteReader(QUERY_VIEW_TRANSACTION, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportDetailByTime(DateTime start, DateTime stop)
    {
      return ExecuteReader(QUERY_REPORT_DETAIL_BY_TIME, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByUserId(DateTime start, DateTime stop)
    {
      return ExecuteReader(QUERY_REPORT_SUMMARY_BY_USER_ID, DateRange(start, stop));
    }

    public string GetTodaySummaryByCashier(User activeUser, DateTime date)
    {
      List<SqlParameter> parameters = new List<SqlParameter>(DateRange(date, date));
      parameters.Add(new SqlParameter("@userId", activeUser.Id));

      var retValue = ExecuteReader(QUERY_TODAY_SUMMARY_BY_USER_ID, parameters.ToArray());
      if (retValue.Count == 0)
        return "Rp. 0";
      return "Rp. " + retValue[0]["SUMTOTAL"];
    }

    /// <summary>
    /// Builds the half-open range [start 00:00, stop+1day 00:00). Both pickers are inclusive dates
    /// to the operator, and comparing against the raw column keeps IDX_T_TRANS_TRXTIME usable.
    /// </summary>
    private static SqlParameter[] DateRange(DateTime start, DateTime stop)
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
        new SqlParameter("@start", from),
        new SqlParameter("@stop", toExclusive),
      };
    }

    /// <summary>
    /// Projects by result-set ordinal rather than a fixed column list, because each report returns a
    /// different shape.
    /// </summary>
    protected override List<CustomQuery> ExecuteReader(String commandText, params SqlParameter[] parameters)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      bool ownsConnection = activeTransaction == null;
      if (ownsConnection)
        connection.Open();
      try
      {
        List<CustomQuery> returnList = new List<CustomQuery>();
        using (SqlCommand command = connection.CreateCommand())
        {
          command.CommandText = commandText;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          AddParameters(command, parameters);
          using (SqlDataReader reader = command.ExecuteReader())
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

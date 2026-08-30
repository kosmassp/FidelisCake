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
  ///
  /// One naming rule follows from that: a numeric column is totalled at the foot of the generated
  /// report unless its heading says it is a per-unit or averaged figure, which is spelled
  /// **"… Satuan"** or **"… Rata-rata"**. Adding a column that must not be added up means naming it
  /// that way - see <see cref="InventoryAndSales.Business.ReportTable"/>.
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

    /// <summary>
    /// How the sale was paid. A sale recorded before payment methods existed has no method and was,
    /// by definition, cash.
    /// </summary>
    private static string PaymentMethodValue()
    {
      return "COALESCE(NULLIF(" + C("t", "PaymentMethod") + ", ''), '" + PaymentMethodCash + "')";
    }

    /// <summary>EDC terminal or QRIS provider, dashed when the method does not use one.</summary>
    private static string PaymentReferenceValue()
    {
      return "COALESCE(NULLIF(" + C("t", "PaymentReference") + ", ''), '-')";
    }

    /// <summary>QRIS code type, dashed for the methods that have none.</summary>
    private static string PaymentVariantValue()
    {
      return "COALESCE(NULLIF(" + C("t", "PaymentVariant") + ", ''), '-')";
    }

    /// <summary>
    /// Items on one sale, as a scalar subquery so a transaction-level report stays one row per
    /// transaction instead of needing a GROUP BY over every selected column.
    /// </summary>
    private static string ItemCount()
    {
      return "COALESCE((SELECT SUM(d." + Dialect.Quote("Quantity") + ")" +
             " FROM " + Table("T_TRANSACTION_DETAILS") + " d" +
             " WHERE d." + Dialect.Quote("TransactionId") + " = " + C("t", "Id") + "), 0)";
    }

    /// <summary>
    /// Takings for one payment method inside a query grouped over transaction *lines*.
    ///
    /// Summed from the line subtotals rather than from the transaction total, because a sale with
    /// three lines appears three times here and summing the header total would triple it. The line
    /// subtotals of a sale add up to that same total, so the figure comes out right either way.
    /// </summary>
    private static string LineTotalForMethod(string methodCode)
    {
      return "SUM(CASE WHEN " + PaymentMethodValue() + " = '" + methodCode + "'" +
             " THEN " + C("td", "Subtotal") + " ELSE 0 END)";
    }

    /// <summary>Cash takings: everything that is not explicitly one of the electronic methods.</summary>
    private static string LineTotalForCash()
    {
      return "SUM(CASE WHEN " + PaymentMethodValue() + " NOT IN ('" + PaymentMethodEdc + "','" + PaymentMethodQris + "')" +
             " THEN " + C("td", "Subtotal") + " ELSE 0 END)";
    }

    #endregion

    #region Reports

    /// <summary>
    /// Every sold line in the range. Deliberately carries no transaction total: the header total
    /// repeats on each of its lines, and a column that must not be added up has no place in a report
    /// that totals its columns. Use <see cref="GetReportSummaryByTransaction"/> for that figure.
    /// </summary>
    public List<CustomQuery> GetReportDetailByTime(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Kasir") + "," +
        C("t", "Factur") + " AS " + A("Faktur") + "," +
        C("t", "TransactionTime") + " AS " + A("Waktu") + "," +
        PaymentMethodValue() + " AS " + A("Metode") + "," +
        ProductName() + " AS " + A("Nama Barang") + "," +
        C("td", "Quantity") + " AS " + A("Jumlah") + "," +
        C("td", "ProductPrice") + " AS " + A("Harga Satuan") + "," +
        C("td", "ProductDiscount") + " AS " + A("Diskon Satuan") + "," +
        C("td", "SubtotalPrice") + " AS " + A("Total Sebelum Diskon") + "," +
        C("td", "SubtotalDiscount") + " AS " + A("Total Diskon") + "," +
        C("td", "Subtotal") + " AS " + A("SubTotal") +
        FromDetailsJoined(true) +
        ActiveInRange() +
        " ORDER BY " + C("t", "TransactionTime");
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByProduct(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + ProductName() + " AS " + A("Nama Barang") + "," +
        TransactionDate() + " AS " + A("Tanggal Transaksi") + "," +
        "COUNT(DISTINCT " + C("t", "Id") + ") AS " + A("Jumlah Transaksi") + "," +
        "SUM(" + C("td", "Quantity") + ") AS " + A("Jumlah Barang Terjual") + "," +
        "SUM(" + C("td", "SubtotalPrice") + ") AS " + A("Total Sebelum Diskon") + "," +
        "SUM(" + C("td", "SubtotalDiscount") + ") AS " + A("Total Diskon") + "," +
        // NULLIF guards the day a line was recorded with no quantity; the row then reads blank
        // rather than failing the whole report with a divide by zero.
        "SUM(" + C("td", "Subtotal") + ") / NULLIF(SUM(" + C("td", "Quantity") + "), 0) AS " + A("Harga Rata-rata") + "," +
        "SUM(" + C("td", "Subtotal") + ") AS " + A("Total") +
        FromDetailsJoined(false) +
        ActiveInRange() +
        " GROUP BY " + ProductName() + ", " + TransactionDate() +
        " ORDER BY " + TransactionDate() + ", " + ProductName();
      return ExecuteReader(sql, DateRange(start, stop));
    }

    /// <summary>
    /// A cashier's day, split by how it was paid. The split is what a shift hand-over needs: only
    /// the cash column is money that should physically be in the drawer.
    /// </summary>
    public List<CustomQuery> GetReportSummaryByUserId(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + CashierName() + " AS " + A("Kasir") + "," +
        TransactionDate() + " AS " + A("Tanggal Transaksi") + "," +
        "COUNT(DISTINCT " + C("t", "Id") + ") AS " + A("Jumlah Transaksi") + "," +
        "SUM(" + C("td", "Quantity") + ") AS " + A("Jumlah Barang Terjual") + "," +
        "SUM(" + C("td", "SubtotalPrice") + ") AS " + A("Total Sebelum Diskon") + "," +
        "SUM(" + C("td", "SubtotalDiscount") + ") AS " + A("Total Diskon") + "," +
        LineTotalForCash() + " AS " + A("Tunai") + "," +
        LineTotalForMethod(PaymentMethodEdc) + " AS " + A("EDC") + "," +
        LineTotalForMethod(PaymentMethodQris) + " AS " + A("QRIS") + "," +
        "SUM(" + C("td", "Subtotal") + ") AS " + A("Total") +
        FromDetailsJoined(true) +
        ActiveInRange() +
        " GROUP BY " + CashierName() + ", " + TransactionDate() +
        " ORDER BY " + TransactionDate() + ", " + CashierName();
      return ExecuteReader(sql, DateRange(start, stop));
    }

    public List<CustomQuery> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + C("t", "Factur") + " AS " + A("Faktur") + "," +
        C("t", "TransactionTime") + " AS " + A("Waktu") + "," +
        CashierName() + " AS " + A("Kasir") + "," +
        PaymentMethodValue() + " AS " + A("Metode") + "," +
        PaymentReferenceValue() + " AS " + A("Referensi") + "," +
        ItemCount() + " AS " + A("Jumlah Barang") + "," +
        C("t", "TotalPrice") + " AS " + A("Total Sebelum Diskon") + "," +
        C("t", "TotalDiscount") + " AS " + A("Total Diskon") + "," +
        C("t", "Total") + " AS " + A("Total") + "," +
        C("t", "Payment") + " AS " + A("Pembayaran") + "," +
        C("t", "Exchange") + " AS " + A("Kembalian") + "," +
        C("t", "Notes") + " AS " + A("Catatan") +
        FromTransactionsJoined() +
        ActiveInRange() +
        " ORDER BY " + C("t", "TransactionTime");
      return ExecuteReader(sql, DateRange(start, stop));
    }

    /// <summary>
    /// Takings grouped by method and by the terminal or provider they came through — the figures a
    /// shop reconciles against its bank and QRIS statements.
    /// </summary>
    public List<CustomQuery> GetReportSummaryByPaymentMethod(DateTime start, DateTime stop)
    {
      string sql =
        " SELECT " + PaymentMethodValue() + " AS " + A("Metode") + "," +
        PaymentReferenceValue() + " AS " + A("Terminal / Provider") + "," +
        PaymentVariantValue() + " AS " + A("Tipe") + "," +
        "COUNT(" + C("t", "Id") + ") AS " + A("Jumlah Transaksi") + "," +
        "SUM(" + C("t", "TotalDiscount") + ") AS " + A("Total Diskon") + "," +
        "SUM(" + C("t", "Total") + ") AS " + A("Total") +
        " FROM " + Table("T_TRANSACTIONS") + " t" +
        ActiveInRange() +
        " GROUP BY " + PaymentMethodValue() + ", " + PaymentReferenceValue() + ", " + PaymentVariantValue() +
        " ORDER BY " + PaymentMethodValue() + ", " + PaymentReferenceValue();
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

    /// <summary>
    /// One cashier's takings for a day, split by how they were paid.
    ///
    /// The split matters: only the cash is money the cashier physically hands over at the end of the
    /// day. Card takings settle through the bank, so a single combined figure would overstate what
    /// should be in the drawer.
    /// </summary>
    public CashierDayTotals GetTodaySummaryByCashier(User activeUser, DateTime date)
    {
      // A sale recorded before payment methods existed has no method and was, by definition, cash,
      // so anything that is not explicitly EDC or QRIS counts as cash.
      string method = "COALESCE(" + C("t", "PaymentMethod") + ", '" + PaymentMethodCash + "')";
      string total = C("t", "Total");

      string sql =
        " SELECT" +
        "  COALESCE(SUM(CASE WHEN " + method + " NOT IN ('" + PaymentMethodEdc + "','" + PaymentMethodQris + "')" +
        "                    THEN " + total + " ELSE 0 END), 0) AS " + A("CASHTOTAL") + "," +
        "  COALESCE(SUM(CASE WHEN " + method + " = '" + PaymentMethodEdc + "' THEN " + total + " ELSE 0 END), 0) AS " + A("EDCTOTAL") + "," +
        "  COALESCE(SUM(CASE WHEN " + method + " = '" + PaymentMethodQris + "' THEN " + total + " ELSE 0 END), 0) AS " + A("QRISTOTAL") +
        " FROM " + Table("T_TRANSACTIONS") + " t" +
        ActiveInRange() +
        " AND " + C("t", "UserId") + " = @userId";

      List<DbParameter> parameters = new List<DbParameter>(DateRange(date, date));
      parameters.Add(DbParam.Of("@userId", activeUser.Id));

      var retValue = ExecuteReader(sql, parameters.ToArray());
      if (retValue.Count == 0)
        return new CashierDayTotals("0", "0", "0");
      // CustomQuery stores every column as an already-formatted string.
      return new CashierDayTotals((string)retValue[0]["CASHTOTAL"],
                                  (string)retValue[0]["EDCTOTAL"],
                                  (string)retValue[0]["QRISTOTAL"]);
    }

    private const string PaymentMethodCash = "CASH";
    private const string PaymentMethodEdc = "EDC";
    private const string PaymentMethodQris = "QRIS";

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
      using (DbScope scope = DBFactory.GetInstance().AcquireScope())
      {
        try
        {
          List<CustomQuery> returnList = new List<CustomQuery>();
          using (DbCommand command = scope.CreateCommand(commandText))
          {
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
      }
    }
  }
}

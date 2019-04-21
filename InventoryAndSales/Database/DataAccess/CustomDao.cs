using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  public class CustomDao : BaseDao<CustomQuery>
  {
    public CustomDao() 
    {
    }

    public override CustomQuery FindById(int id)
    {
      throw new NotImplementedException("This method is should not be used.");
    }
    public override List<CustomQuery> FindByQuery(string whereClause)
    {
      throw new NotImplementedException("This method is should not be used.");
    }
    public override bool Save(CustomQuery dataObject)
    {
      throw new NotImplementedException("This method is should not be used.");
    }
    public override bool DeleteById(int id)
    {
      throw new NotImplementedException("This method is should not be used.");
    }
    public override bool Delete(CustomQuery dataObject)
    {
      throw new NotImplementedException("This method is should not be used.");
    }
    public override int Update(CustomQuery dataObject)
    {
      throw new NotImplementedException("This method is should not be used.");
    }

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
      " where t.Revision = 0 and CAST(TransactionTime as date) between '{0}' and '{1}'" +
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
      " where t.Revision = 0 and CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " group by COALESCE(p.Name,'Telah Dihapus'),CAST(t.TransactionTime as date)" +
      " order by CAST(t.TransactionTime as date)";

    private const string QUERY_REPORT_SUMMARY_BY_USER_ID =
      " select"+
      " COALESCE(u.Name,'ADMIN') as Kasir,"+
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
      " where t.Revision = 0 and CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " group by COALESCE(u.Name,'ADMIN'),CAST(t.TransactionTime as date)" +
      " order by CAST(t.TransactionTime as date)";

    private const string QUERY_TODAY_SUMMARY_BY_USER_ID =
      " SELECT COALESCE(SUM([Total]),0) SUMTOTAL" +
      " FROM T_TRANSACTIONS" +
      " Where UserId = {0}" +
      " and Revision = 0" +
      " and CAST(TransactionTime as date) = '{1}'";


    private const string QUERY_REPORT_SUMMARY_BY_TRANSACTION =
      " select"+
      " COALESCE(u.Name,'ADMIN') as Kasir,"+
      " t.Factur," +
      " CAST(t.TransactionTime as date) as 'Tanggal Transaksi'," +
      " t.Total as 'Total'," +
      " t.Notes as 'Catatan'," +
      " t.Payment as 'Pembayaran'," +
      " t.Exchange as 'Kembalian'" +
      " from T_TRANSACTIONS t" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0 and CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " order by t.TransactionTime";

    private const string QUERY_VIEW_TRANSACTION =
      " select"+
      " COALESCE(u.Name,'ADMIN') as Kasir,"+
      " t.Id," +
      " t.Factur," +
      " t.TransactionTime as 'Tanggal Transaksi'," +
      " t.Total as 'Total'," +
      " t.Notes as 'Catatan'" +
      " from T_TRANSACTIONS t" +
      " left join M_USERS u on (u.Id = t.UserId)" +
      " where t.Revision = 0 and CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " order by t.TransactionTime";



    public List<CustomQuery> GetReportSummaryByProduct(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_SUMMARY_BY_PRODUCT, start.ToShortDateString(), stop.ToShortDateString()));
    }

    public List<CustomQuery> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_SUMMARY_BY_TRANSACTION, start.ToShortDateString(), stop.ToShortDateString()));
    }

    public List<CustomQuery> GetTransaction(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_VIEW_TRANSACTION, start.ToShortDateString(), stop.ToShortDateString()));
    }

    public List<CustomQuery> GetReportDetailByTime(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_DETAIL_BY_TIME, start.ToShortDateString(), stop.ToShortDateString()));
    }

    public List<CustomQuery> GetReportSummaryByUserId(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_SUMMARY_BY_USER_ID, start.ToShortDateString(), stop.ToShortDateString()));
    }

    protected override List<CustomQuery> ExecuteReader(String commandText, params SqlParameter[] parameters)
    {
      List<CustomQuery> returnList = new List<CustomQuery>();
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlCommand command = connection.CreateCommand();
      command.CommandText = commandText;
      command.Parameters.AddRange(parameters);
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      command.Transaction = activeTransaction;
      // When using CommandBehavior.CloseConnection, the connection will be closed when the 
      // IDataReader is closed.
      SqlDataReader reader = command.ExecuteReader();
      while (reader.Read())
      {
        CustomQuery t = new CustomQuery();
        //This one should pick from the query since each query will return different column.
        for (int i = 0; i < reader.FieldCount; i++)
        {
          if (!(reader.GetValue(i) is DBNull))
            t[reader.GetName(i)] = reader.GetValue(i);
        }
        returnList.Add(t);
      }
      reader.Close();
      return returnList;
    }


    public string GetTodaySummaryByCashier(User activeUser, DateTime date)
    {
      var retValue = ExecuteReader(string.Format(QUERY_TODAY_SUMMARY_BY_USER_ID, activeUser.Id, date.ToShortDateString()));
      if(retValue.Count == 0)
        return "Rp. 0";
      return "Rp. " + retValue[0]["SUMTOTAL"];
    }
  }

}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
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
      " select p.Name as ProductName, td.ProductPrice, td.ProductDiscount, t.TransactionTime, Quantity as Qty, Subtotal, SubtotalDiscount, SubtotalPrice" +
      " from T_TRANSACTION_DETAILS td" +
      " left join T_TRANSACTIONS t on (t.Id = td.TransactionId)" +
      " left join M_PRODUCTS p on (p.Id = td.ProductId)" +
      " where CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " order by t.TransactionTime ";


    private const string QUERY_REPORT_SUMMARY_BY_TIME =
      " select p.Name as ProductName, CAST(t.TransactionTime as date) as 'TransactionDate'," +
      " sum(Quantity) as Qty,sum(Subtotal) as Subtotal, Sum(SubtotalDiscount) as SubtotalDiscount," +
      " Sum(SubtotalPrice) as SubtotalPrice" +
      " from T_TRANSACTION_DETAILS td" +
      " left join T_TRANSACTIONS t on (t.Id = td.TransactionId)" +
      " left join M_PRODUCTS p on (p.Id = td.ProductId)" +
      " where CAST(TransactionTime as date) between '{0}' and '{1}'" +
      " group by p.Name,CAST(t.TransactionTime as date)" +
      " order by CAST(t.TransactionTime as date)";


    public List<CustomQuery> GetReportSummaryByTime(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_SUMMARY_BY_TIME, start.ToShortDateString(), stop.ToShortDateString()));
    }

    public List<CustomQuery> GetReportDetailByTime(DateTime start, DateTime stop)
    {
      return ExecuteReader(string.Format(QUERY_REPORT_DETAIL_BY_TIME, start.ToShortDateString(), stop.ToShortDateString()));
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
          t[reader.GetName(i)] = reader.GetValue(i);
        }
        returnList.Add(t);
      }
      reader.Close();
      return returnList;
    }



    
  }

}

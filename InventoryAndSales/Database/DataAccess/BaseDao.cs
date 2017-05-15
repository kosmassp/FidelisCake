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
  public class BaseDao<T> where T : BaseObject, new()
  {
    private readonly IDataTable _dataTable;

    public BaseDao()
    {
      _dataTable = DataTableList.Instance.GetDataTable(typeof(T));
    }

    public virtual T FindById(int id)
    {
      var result = FindByQuery(string.Format("WHERE {0}={1}", _dataTable.PrimaryKeyColumn, id));
      if (result.Count > 0)
        return result[0];
      return null;
    }

    private const string FIND_BY_QUERY = "SELECT * FROM {0} {1}";
    public virtual List<T> FindByQuery(string whereClause)
    {
      //TODO execute sql FIND_BY_ID_SQL, WE don't not to describe this everytime
      if (!string.IsNullOrEmpty(whereClause))
      {
        whereClause = whereClause.Trim();
        if (!whereClause.StartsWith("WHERE", true, CultureInfo.InvariantCulture))
          whereClause = "WHERE " + whereClause;
      }
      string preparedSql = string.Format(FIND_BY_QUERY, _dataTable.TableName, whereClause);
      return ExecuteReader(preparedSql);
    }

    private const string INSERT_SQL = "INSERT INTO {0}({1}) VALUES ({2})";
    public virtual bool Save(T dataObject)
    {
      StringBuilder columns = new StringBuilder();
      StringBuilder values = new StringBuilder();
      bool first = true;
      foreach (string column in _dataTable.Columns)
      {
        if( column == _dataTable.PrimaryKeyColumn) 
          continue;
        if(first)
        {
          columns.AppendFormat("{0}", column);
          values.AppendFormat("'{0}'", dataObject[column]);
          first = false;
        }
        else
        {
          columns.AppendFormat(",{0}", column);
          values.AppendFormat(",'{0}'", dataObject[column]);
        }
      }


      string insertSql = string.Format(INSERT_SQL, _dataTable.TableName, columns, values);
      int insert = ExecuteNonQuery(insertSql);
      if (insert > 0)
      {
        //SqlServer
        var lastId = ExecuteScalar("SELECT SCOPE_IDENTITY()");
        //Mysql
        //dataObject[_dataTable.PrimaryKeyColumn] = ExecuteScalar("SELECT LAST_INSERT_ID();"); 
        dataObject[_dataTable.PrimaryKeyColumn] = int.Parse(lastId.ToString());
      }

      return insert > 0;
    }
    private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE {2} = {3}";
    public virtual int Update(T dataObject)
    {
      StringBuilder columnValuePair = new StringBuilder();

      bool first = true;
      foreach (string column in _dataTable.Columns)
      {
        if( column == _dataTable.PrimaryKeyColumn) 
          continue;
        if(first)
        {
          columnValuePair.AppendFormat("{0}='{1}'", column, dataObject[column]);
          first = false;
        }
        else
        {
          columnValuePair.AppendFormat(",{0}='{1}'", column, dataObject[column]);
        }
      }


      string updateSql = string.Format(UPDATE_SQL, _dataTable.TableName, columnValuePair, _dataTable.PrimaryKeyColumn, dataObject[_dataTable.PrimaryKeyColumn]);
      return ExecuteNonQuery(updateSql);
    }

    public virtual bool Delete(T dataObject)
    {
      return DeleteById((int)dataObject[_dataTable.PrimaryKeyColumn]);
    }

    private const string DELETE_SQL = "DELETE FROM {0} WHERE {1} = {2}";
    public virtual bool DeleteById(int id)
    {
      //TODO execute sql FIND_BY_ID_SQL, WE don't not to describe this everytime
      string preparedSql = string.Format(DELETE_SQL, _dataTable.TableName, _dataTable.PrimaryKeyColumn, id);
      int delete = ExecuteNonQuery(preparedSql);
      return delete > 0;
    }

    protected virtual List<T> ExecuteReader(String commandText, params SqlParameter[] parameters)
    {
      List<T> returnList = new List<T>();
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
        T t = new T();
        //This one should pick from dataTable so some new column or unspecified column in the code will be ignored.
        foreach (string columnName in _dataTable.Columns)
        {
          t[columnName] = reader[columnName];
        }
        returnList.Add(t);
      }
      reader.Close();
      return returnList;
    }


    protected int ExecuteNonQuery(string nonQueryCommand)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      SqlCommand command = connection.CreateCommand();
      command.CommandText = nonQueryCommand;
      command.Transaction = activeTransaction;
      int result = command.ExecuteNonQuery();
      if (activeTransaction == null)
        connection.Close();
      return result;
    }

    protected object ExecuteScalar(string scalarCommand)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      SqlCommand command = connection.CreateCommand();
      command.CommandText = scalarCommand;
      command.Transaction = activeTransaction;
      var obj = command.ExecuteScalar();
      if (activeTransaction == null)
        connection.Close();
      return obj;
    }

    /*
    protected bool ExecuteNonQueries(string[] nonQueryCommands)
    {
      return ExecuteNonQueries(IsolationLevel.ReadCommitted, nonQueryCommands);
    }
    protected bool ExecuteNonQueries(IsolationLevel isolationLevel, string[] nonQueryCommands)
    {
      bool commandSuccess = false;
      using (SqlConnection connection = DBFactory.GetInstance().GetConnection())
      {
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        SqlTransaction transaction = connection.BeginTransaction(isolationLevel);
        command.Transaction = transaction;
        try
        {
          foreach (string nonQueryCommand in nonQueryCommands)
          {
            command.CommandText = nonQueryCommand;
            command.ExecuteNonQuery();
          }
          transaction.Commit();
          commandSuccess = true;
        }
        catch (Exception e)
        {
          try
          {
            transaction.Rollback();
          }
          catch (SqlException ex)
          {
            throw;
          }
        }
      }
      return commandSuccess;
    }
    */

  }
}

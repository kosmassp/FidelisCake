using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database
{
  public class DBUtility
  {
    public static void CheckForUpdate()
    {
      UpdateTableTransaction();
    }

    private static void UpdateTableTransaction()
    {
      string tableName = "T_TRANSACTIONS";
      string columnName = "Revision";
      if (!IsColumnExist(tableName, columnName)) //Revision will link to transaction id. Revision null when it is new. Revision -1 means deleted.
      {
        ExecuteNonQuery(string.Format("ALTER TABLE {0} ADD {1} bigint NULL", tableName, columnName));
        ExecuteNonQuery(string.Format("UPDATE {0} set {1} = 0 where {1} is NULL", tableName, columnName));
      }
      //columnName = "Deleted";
      //if (!IsColumnExist(tableName, columnName))
      //{
      //  ExecuteNonQuery(string.Format("ALTER TABLE {0} ADD {1} bit NULL", tableName, columnName));
      //  ExecuteNonQuery(string.Format("UPDATE {0} set {1} = 0 where {1} is NULL", tableName, columnName));
      //}
      //if (IsColumnExist(tableName, columnName))
      //{
      //  ExecuteNonQuery(string.Format("ALTER TABLE {0} DROP COLUMN {1}", tableName, columnName));
      //}
      //columnName = "Revision";
      //if (IsColumnExist(tableName, columnName))
      //{
      //  ExecuteNonQuery(string.Format("ALTER TABLE {0} DROP COLUMN {1}", tableName, columnName));
      //}
      //if (!IsColumnExist(tableName, columnName))
      //{
      //  ExecuteNonQuery(string.Format("ALTER TABLE {0} ADD {1} bigint NULL", tableName, columnName));
      //  ExecuteNonQuery(string.Format("UPDATE {0} set {1} = 0 where {1} is NULL", tableName, columnName));
      //}
    }

    private static bool IsColumnExist(string tableName, string columnName)
    {
      bool exists = true;
      try
      {
        //var result = ExecuteScalar(string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'", tableName, columnName));
        var obj = ExecuteScalar(string.Format("SELECT {0} from {1}", columnName, tableName)); //barbaric ways
      }
      catch(Exception e)
      {
        exists = false;
      }
      return exists;
    }


    internal static int ExecuteNonQuery(string nonQueryCommand)
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

    internal static object ExecuteScalar(string scalarCommand)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      SqlCommand command = connection.CreateCommand();
      command.CommandText = scalarCommand;
      command.Transaction = activeTransaction;
      object obj = command.ExecuteScalar();
      if (activeTransaction == null)
        connection.Close();
      return obj;
    }

    /*
    internal  bool ExecuteNonQueries(string[] nonQueryCommands)
    {
      return ExecuteNonQueries(IsolationLevel.ReadCommitted, nonQueryCommands);
    }
    internal  bool ExecuteNonQueries(IsolationLevel isolationLevel, string[] nonQueryCommands)
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

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
      //CheckTable();
      UpdateTableTransaction();
    }

    private static void CheckTable()
    {
      //if(CheckIfTableExist()) return;
      StringBuilder sb = new StringBuilder();
      sb.Append( "  CREATE TABLE [dbo].[M_PRODUCTS](                                                  " );
      sb.Append( "      [Id] [int] IDENTITY(1,1) NOT NULL,                                            " );
      sb.Append( "      [Code] [varchar](10) NULL,                                                    " );
      sb.Append( "      [Name] [varchar](70) NOT NULL,                                                " );
      sb.Append( "      [Price] [decimal](18, 0) NOT NULL,                                            " );
      sb.Append( "      [Discount] [decimal](18, 0) NULL,                                             " );
      sb.Append( "      [Deleted] [bit] NOT NULL CONSTRAINT [DF_M_PRODUCTS_Deleted]  DEFAULT ((0)),   " );
      sb.Append( "      [Barcode] [varchar](20) NULL                                                  " );
      sb.Append( "  )                                                                                 " );
      ExecuteNonQuery(sb.ToString());

      sb = new StringBuilder();
      sb.Append( "  CREATE TABLE [dbo].[M_USERS](                                                     " );
      sb.Append( "      [Id] [int] IDENTITY(1,1) NOT NULL,                                            " );
      sb.Append( "      [Username] [varchar](50) NULL,                                                " );
      sb.Append( "      [Role] [int] NULL,                                                            " );
      sb.Append( "      [Deleted] [bit] NOT NULL CONSTRAINT [DF_M_USERS_Deleted]  DEFAULT ((0)),      " );
      sb.Append( "      [Name] [varchar](50) NULL,                                                    " );
      sb.Append( "      [Password] [varchar](256) NULL                                                " );
      sb.Append( "  )                                                                                 " );
      ExecuteNonQuery( sb.ToString() );

      sb = new StringBuilder();
      sb.Append( "  CREATE TABLE [dbo].[T_TRANSACTION_DETAILS](                                       " );
      sb.Append( "      [Id] [bigint] IDENTITY(1,1) NOT NULL,                                         " );
      sb.Append( "      [ProductId] [int] NULL,                                                       " );
      sb.Append( "      [Quantity] [int] NULL,                                                        " );
      sb.Append( "      [ProductDiscount] [decimal](18, 0) NULL,                                      " );
      sb.Append( "      [ProductPrice] [decimal](18, 0) NULL,                                         " );
      sb.Append( "      [Subtotal] [decimal](18, 0) NULL,                                             " );
      sb.Append( "      [TransactionId] [bigint] NULL,                                                " );
      sb.Append( "      [SubtotalDiscount] [decimal](18, 0) NULL,                                     " );
      sb.Append( "      [SubtotalPrice] [decimal](18, 0) NULL                                         " );
      sb.Append( "  )                                                                                 " );
      ExecuteNonQuery( sb.ToString() );

      sb = new StringBuilder();
      sb.Append( "  CREATE TABLE [dbo].[T_TRANSACTIONS](                                              " );
      sb.Append( "      [Id] [bigint] IDENTITY(1,1) NOT NULL,                                         " );
      sb.Append( "      [TotalPrice] [decimal](18, 0) NULL,                                           " );
      sb.Append( "      [TotalDiscount] [decimal](18, 0) NULL,                                        " );
      sb.Append( "      [Total] [decimal](18, 0) NULL,                                                " );
      sb.Append( "      [Notes] [varchar](100) NULL,                                                  " );
      sb.Append( "      [TransactionTime] [datetime] NULL,                                            " );
      sb.Append( "      [Payment] [decimal](18, 0) NULL,                                              " );
      sb.Append( "      [Exchange] [decimal](18, 0) NULL,                                             " );
      sb.Append( "      [UserId] [int] NULL,                                                          " );
      sb.Append( "      [Factur] [varchar](18) NULL,                                                  " );
      sb.Append( "      [CustomerId] [bigint] NULL                                                    " );
      sb.Append( "  )                                                                                 " );
      ExecuteNonQuery( sb.ToString() );

      sb = new StringBuilder();
      sb.Append( "  CREATE TABLE [dbo].[M_CUSTOMERS](                                                 " );
      sb.Append( "      [Id] [int] IDENTITY(1,1) NOT NULL,                                            " );
      sb.Append( "      [Name] [varchar](50) NULL,                                                    " );
      sb.Append( "      [Address] [varchar](50) NULL,                                                 " );
      sb.Append( "      [Phone] [varchar](50) NULL,                                                   " );
      sb.Append( "      [Type] [int] NULL                                                             " );
      sb.Append( "  )                                                                                 " );
      ExecuteNonQuery( sb.ToString() );
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

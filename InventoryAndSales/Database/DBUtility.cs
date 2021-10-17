using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database
{
  public class DBUtility
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static void CheckForDatabaseTable()
    {
      _log.Info("Create Table if not exists");
      CheckTable();
      _log.Info("Update missing column");
      UpdateTableTransaction();
      _log.Info("Create index");
      CheckIndex();
    }

    private static void CheckIndex()
    {
      try
      {
        if (!IsIndexExist("T_TRANSACTIONS","IDX_T_TRANS_TRXTIME"))
        {
          var create_index = "CREATE NONCLUSTERED INDEX[IDX_T_TRANS_TRXTIME] ON[dbo].[T_TRANSACTIONS] ( [TransactionTime] DESC )";
          ExecuteNonQuery(create_index);
        }
        if (!IsIndexExist("T_TRANSACTIONS", "IDX_T_TRANS_FACTUR"))
        {
          var create_index = "CREATE UNIQUE NONCLUSTERED INDEX[IDX_T_TRANS_FACTUR] ON[dbo].[T_TRANSACTIONS] ( [Factur] ASC )";
          ExecuteNonQuery(create_index);
        }
        if (!IsIndexExist("T_TRANSACTIONS", "IDX_T_TRDETAIL_TRX_ID"))
        {
          var create_index = "CREATE NONCLUSTERED INDEX [IDX_T_TRDETAIL_TRX_ID] ON [dbo].[T_TRANSACTION_DETAILS] ( [TransactionId] DESC )";
          ExecuteNonQuery(create_index);
        }
      }
      catch (Exception e)
      {
        _log.Error("Create Index Failed");
        _log.Error(e);
      }
    }

    public static void CheckForDatabaseRow()
    {
      UpsertSettingRow();

    }

    private static void UpsertSettingRow()
    {
      string SETTINGS_QUERY = "SELECT * FROM M_SETTINGS WHERE [KEY] = '{0}' AND [GROUP] = '{1}'";
      string SETTINGS_INSERT = "INSERT INTO M_SETTINGS([KEY], [GROUP], [VALUE], [DEFAULT]) VALUES ('{0}','{1}','{2}','{2}')";

      string query = string.Format(SETTINGS_QUERY, "HEADER", "GENERAL");
      var result = ExecuteScalar(query);
      if(result == null)
      {
        string insert = string.Format(SETTINGS_INSERT, "HEADER", "GENERAL", 
          "FIDELIS CAKE AND BAKERY" + "%NEW_LINE%" +
          "JL MAYJEND SUTOYO NO 1" + "%NEW_LINE%" +
          "BANJARNEGARA" + "%NEW_LINE%" +
          "(0286) 594573");
        ExecuteNonQuery(insert);
      }

      query = string.Format(SETTINGS_QUERY, "FOOTER", "GENERAL");
      result = ExecuteScalar(query);
      if(result == null)
      {
        string insert = string.Format(SETTINGS_INSERT, "FOOTER", "GENERAL",
          "TERIMA KASIH" + "%NEW_LINE%" +
          "SELAMAT MENIKMATI");
        ExecuteNonQuery(insert);
      }
    }

    private static void CheckTable()
    {
      StringBuilder sb = new StringBuilder();
      if (!CheckIfTableExist("M_SETTINGS"))
      {                                                             
        sb.Append("  CREATE TABLE [dbo].[M_SETTINGS](       ");
        sb.Append("      [Id] [int] IDENTITY(1,1) NOT NULL, ");
        sb.Append("      [Key] [varchar](80) NOT NULL,      ");
        sb.Append("      [Group] [varchar](80) NULL,        ");
        sb.Append("      [Value] [text] NULL,               ");
        sb.Append("      [Default] [text] NOT NULL          ");
        sb.Append("  )                                      ");
        ExecuteNonQuery(sb.ToString());
      }

      if (!CheckIfTableExist("M_PRODUCTS"))
      {
        sb.Append("  CREATE TABLE [dbo].[M_PRODUCTS](                                                  ");
        sb.Append("      [Id] [int] IDENTITY(1,1) NOT NULL,                                            ");
        sb.Append("      [Code] [varchar](10) NULL,                                                    ");
        sb.Append("      [Name] [varchar](70) NOT NULL,                                                ");
        sb.Append("      [Price] [decimal](18, 0) NOT NULL,                                            ");
        sb.Append("      [Discount] [decimal](18, 0) NULL,                                             ");
        sb.Append("      [Deleted] [bit] NOT NULL CONSTRAINT [DF_M_PRODUCTS_Deleted]  DEFAULT ((0)),   ");
        sb.Append("      [Barcode] [varchar](20) NULL                                                  ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }

      if (!CheckIfTableExist("M_USERS"))
      {
        sb = new StringBuilder();
        sb.Append("  CREATE TABLE [dbo].[M_USERS](                                                     ");
        sb.Append("      [Id] [int] IDENTITY(1,1) NOT NULL,                                            ");
        sb.Append("      [Username] [varchar](50) NULL,                                                ");
        sb.Append("      [Role] [int] NULL,                                                            ");
        sb.Append("      [Deleted] [bit] NOT NULL CONSTRAINT [DF_M_USERS_Deleted]  DEFAULT ((0)),      ");
        sb.Append("      [Name] [varchar](50) NULL,                                                    ");
        sb.Append("      [Password] [varchar](256) NULL                                                ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }

      if (!CheckIfTableExist("T_TRANSACTION_DETAILS"))
      {
        sb = new StringBuilder();
        sb.Append("  CREATE TABLE [dbo].[T_TRANSACTION_DETAILS](                                       ");
        sb.Append("      [Id] [bigint] IDENTITY(1,1) NOT NULL,                                         ");
        sb.Append("      [ProductId] [int] NULL,                                                       ");
        sb.Append("      [Quantity] [int] NULL,                                                        ");
        sb.Append("      [ProductDiscount] [decimal](18, 0) NULL,                                      ");
        sb.Append("      [ProductPrice] [decimal](18, 0) NULL,                                         ");
        sb.Append("      [Subtotal] [decimal](18, 0) NULL,                                             ");
        sb.Append("      [TransactionId] [bigint] NULL,                                                ");
        sb.Append("      [SubtotalDiscount] [decimal](18, 0) NULL,                                     ");
        sb.Append("      [SubtotalPrice] [decimal](18, 0) NULL                                         ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }
      if (!CheckIfTableExist("T_TRANSACTIONS"))
      {
        sb = new StringBuilder();
        sb.Append("  CREATE TABLE [dbo].[T_TRANSACTIONS](                                              ");
        sb.Append("      [Id] [bigint] IDENTITY(1,1) NOT NULL,                                         ");
        sb.Append("      [TotalPrice] [decimal](18, 0) NULL,                                           ");
        sb.Append("      [TotalDiscount] [decimal](18, 0) NULL,                                        ");
        sb.Append("      [Total] [decimal](18, 0) NULL,                                                ");
        sb.Append("      [Notes] [varchar](100) NULL,                                                  ");
        sb.Append("      [TransactionTime] [datetime] NULL,                                            ");
        sb.Append("      [Payment] [decimal](18, 0) NULL,                                              ");
        sb.Append("      [Exchange] [decimal](18, 0) NULL,                                             ");
        sb.Append("      [UserId] [int] NULL,                                                          ");
        sb.Append("      [Factur] [varchar](18) NULL,                                                  ");
        sb.Append("      [CustomerId] [bigint] NULL                                                    ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }
      if (!CheckIfTableExist("M_CUSTOMERS"))
      {
        sb = new StringBuilder();
        sb.Append("  CREATE TABLE [dbo].[M_CUSTOMERS](                                                 ");
        sb.Append("      [Id] [int] IDENTITY(1,1) NOT NULL,                                            ");
        sb.Append("      [Name] [varchar](50) NULL,                                                    ");
        sb.Append("      [Address] [varchar](50) NULL,                                                 ");
        sb.Append("      [Phone] [varchar](50) NULL,                                                   ");
        sb.Append("      [Type] [int] NULL                                                             ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }
    }

    private static bool CheckIfTableExist(string tableName)
    {
      string query = string.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME='{0}'", tableName);
      var result = ExecuteScalar(query);
      return result != null;
    }

    private static void UpdateTableTransaction()
    {
      //CHECK COLUMN
      string tableName = "T_TRANSACTIONS";
      string columnName = "Revision";
      if (!IsColumnExist(tableName, columnName)) //Revision will link to transaction id. Revision null when it is new. Revision -1 means deleted.
      {
        ExecuteNonQuery(string.Format("ALTER TABLE {0} ADD {1} bigint NULL", tableName, columnName));
        ExecuteNonQuery(string.Format("UPDATE {0} set {1} = 0 where {1} is NULL", tableName, columnName));
      }

      //CHECK DATA TYPE
      columnName = "Factur";
      var dataType = "varchar";
      var charLength = 20;
      if (IsColumnExist(tableName, columnName) && !IsColumnTypeEquals(tableName, columnName, dataType, charLength)) //Revision will link to transaction id. Revision null when it is new. Revision -1 means deleted.
      {
        var indexName = "IDX_T_TRANS_FACTUR";
        ExecuteNonQuery( $"DROP INDEX IF EXISTS {indexName} ON {tableName}");
        ExecuteNonQuery( $"ALTER TABLE [{tableName}] ALTER COLUMN {columnName} {dataType}({charLength})");
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

    private static bool IsIndexExist(string tableName, string indexName)
    {
      bool exists = true;
      try
      {
        string checkIndex = $"SELECT * FROM SYS.INDEXES WHERE NAME = '{indexName}' AND OBJECT_ID = (SELECT OBJECT_ID FROM SYS.OBJECTS WHERE NAME = '{tableName}')";
        var result = ExecuteScalar(checkIndex);
        return result != null;
      }
      catch(Exception e)
      {
        _log.Error(e);
        exists = false;
      }
      return exists;
    }

    private static bool IsColumnExist(string tableName, string columnName)
    {
      bool exists = true;
      try
      {
        var result = ExecuteScalar(string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'", tableName, columnName));
        //var obj = ExecuteScalar(string.Format("SELECT {0} from {1}", columnName, tableName)); //barbaric ways
        return result != null;
      }
      catch(Exception e)
      {
        _log.Error(e);
        exists = false;
      }
      return exists;
    }


    private static bool IsColumnTypeEquals(string tableName, string columnName, string dataType, int charLength = 0)
    {
      bool dataTypeEquals = true;
      try
      {
        var result = ExecuteScalar(string.Format("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'", tableName, columnName));
        //var obj = ExecuteScalar(string.Format("SELECT {0} from {1}", columnName, tableName)); //barbaric ways
        dataTypeEquals = result.ToString() == dataType;
        if (dataTypeEquals && charLength > 0)
        {
          result = ExecuteScalar(string.Format("SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'", tableName, columnName));
          return (int)result == charLength;
        }
      }
      catch (Exception e)
      {
        _log.Error(e);
        dataTypeEquals = false;
      }
      return dataTypeEquals;
    }


    internal static int ExecuteNonQuery(string nonQueryCommand)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      try
      {
        SqlCommand command = connection.CreateCommand();
        command.CommandText = nonQueryCommand;
        command.CommandTimeout = 600;
        command.Transaction = activeTransaction;
        int result = command.ExecuteNonQuery();
        return result;
      }
      catch(Exception e)
      {
        _log.Error( $"Failed to run non-query {nonQueryCommand}", e);
        return -1;
      }
      finally
      {
        if (activeTransaction == null)
          connection.Close();
      }
    }

    internal static object ExecuteScalar(string scalarCommand)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      try
      {
        SqlCommand command = connection.CreateCommand();
        command.CommandText = scalarCommand;
        command.CommandTimeout = 600;
        command.Transaction = activeTransaction;
        object obj = command.ExecuteScalar();
        return obj;
      }
      catch (Exception e)
      {
        _log.Error($"Failed to run query {scalarCommand}", e);
        return null;
      }
      finally
      {
        if (activeTransaction == null)
          connection.Close();
      }
    }

  }
}

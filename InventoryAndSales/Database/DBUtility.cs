using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;

namespace InventoryAndSales.Database
{
  /// <summary>
  /// Startup schema maintenance and the low level SQL helpers.
  ///
  /// Installations are spread across many sites on different versions and there is no migration
  /// history, so the schema is reconciled on every startup instead: each step checks first and only
  /// then applies. Every step must therefore stay guarded, idempotent and additive - never drop a
  /// column an older installation may still hold data in.
  /// </summary>
  public class DBUtility
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static void CheckForDatabaseTable()
    {
      _log.Info("Create Table if not exists");
      CheckTable();
      _log.Info("Update missing column");
      UpdateTableTransaction();
      UpdateTableCustomer();
      _log.Info("Create index");
      CheckIndex();
    }

    public static void CheckForDatabaseRow()
    {
      UpsertSettingRow();
    }

    private static void CheckIndex()
    {
      try
      {
        if (!IsIndexExist("T_TRANSACTIONS", "IDX_T_TRANS_TRXTIME"))
        {
          var create_index = "CREATE NONCLUSTERED INDEX[IDX_T_TRANS_TRXTIME] ON[dbo].[T_TRANSACTIONS] ( [TransactionTime] DESC )";
          TryExecuteNonQuery(create_index);
        }
        if (!IsIndexExist("T_TRANSACTIONS", "IDX_T_TRANS_FACTUR"))
        {
          var create_index = "CREATE UNIQUE NONCLUSTERED INDEX[IDX_T_TRANS_FACTUR] ON[dbo].[T_TRANSACTIONS] ( [Factur] ASC )";
          TryExecuteNonQuery(create_index);
        }
        if (!IsIndexExist("T_TRANSACTIONS", "IDX_T_TRDETAIL_TRX_ID"))
        {
          var create_index = "CREATE NONCLUSTERED INDEX [IDX_T_TRDETAIL_TRX_ID] ON [dbo].[T_TRANSACTION_DETAILS] ( [TransactionId] DESC )";
          TryExecuteNonQuery(create_index);
        }
      }
      catch (Exception e)
      {
        _log.Error("Create Index Failed");
        _log.Error(e);
      }
    }

    /// <summary>
    /// Inserts any setting row the database does not have yet. Existing rows are left untouched, so
    /// an operator's edits survive every upgrade and a new key only has to be declared in
    /// <see cref="SettingKeys.Seed"/> to reach older installations.
    /// </summary>
    private static void UpsertSettingRow()
    {
      const string SETTINGS_QUERY = "SELECT [Id] FROM M_SETTINGS WHERE [KEY] = @key";
      const string SETTINGS_INSERT = "INSERT INTO M_SETTINGS([KEY], [GROUP], [VALUE], [DEFAULT]) VALUES (@key, @group, @value, @value)";

      foreach (SettingKeys.SettingSeed seed in SettingKeys.Seed())
      {
        try
        {
          object existing = TryExecuteScalar(SETTINGS_QUERY, new SqlParameter("@key", seed.Key));
          if (existing != null)
            continue;

          _log.InfoFormat("Seeding missing setting '{0}'.", seed.Key);
          TryExecuteNonQuery(SETTINGS_INSERT,
            new SqlParameter("@key", seed.Key),
            new SqlParameter("@group", seed.Group),
            new SqlParameter("@value", seed.Value));
        }
        catch (Exception e)
        {
          _log.Error(string.Format("Failed seeding setting '{0}'.", seed.Key), e);
        }
      }
    }

    private static void CheckTable()
    {
      if (!CheckIfTableExist("M_SETTINGS"))
      {
        StringBuilder sb = new StringBuilder();
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
        StringBuilder sb = new StringBuilder();
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
        StringBuilder sb = new StringBuilder();
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
        StringBuilder sb = new StringBuilder();
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
        StringBuilder sb = new StringBuilder();
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
        sb.Append("      [Factur] [varchar](20) NULL,                                                  ");
        sb.Append("      [CustomerId] [bigint] NULL                                                    ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }

      if (!CheckIfTableExist("M_CUSTOMERS"))
      {
        StringBuilder sb = new StringBuilder();
        sb.Append("  CREATE TABLE [dbo].[M_CUSTOMERS](                                                 ");
        sb.Append("      [Id] [int] IDENTITY(1,1) NOT NULL,                                            ");
        sb.Append("      [Name] [varchar](50) NULL,                                                    ");
        sb.Append("      [Address] [varchar](50) NULL,                                                 ");
        sb.Append("      [Phone] [varchar](50) NULL,                                                   ");
        sb.Append("      [MemberType] [int] NULL                                                       ");
        sb.Append("  )                                                                                 ");
        ExecuteNonQuery(sb.ToString());
      }
    }

    private static bool CheckIfTableExist(string tableName)
    {
      object result = TryExecuteScalar(
        "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName",
        new SqlParameter("@tableName", tableName));
      return result != null;
    }

    private static void UpdateTableTransaction()
    {
      const string tableName = "T_TRANSACTIONS";

      // Revision links a superseded transaction to the one that replaced it.
      // NULL/0 = active, > 0 = replaced by that Id, -1 = cancelled.
      if (!IsColumnExist(tableName, "Revision"))
      {
        TryExecuteNonQuery(string.Format("ALTER TABLE {0} ADD Revision bigint NULL", tableName));
        TryExecuteNonQuery(string.Format("UPDATE {0} SET Revision = 0 WHERE Revision IS NULL", tableName));
      }

      // Who cancelled a transaction and when. Older installations recorded neither, so existing
      // rows keep NULL and only cancellations made from this version onwards are attributed.
      if (!IsColumnExist(tableName, "CancelledBy"))
        TryExecuteNonQuery(string.Format("ALTER TABLE {0} ADD CancelledBy int NULL", tableName));

      if (!IsColumnExist(tableName, "CancelledAt"))
        TryExecuteNonQuery(string.Format("ALTER TABLE {0} ADD CancelledAt datetime NULL", tableName));

      // Factur started life as varchar(18) but holds an 18 digit tick count; widen it so the value
      // can never be silently truncated.
      const string columnName = "Factur";
      const string dataType = "varchar";
      const int charLength = 20;
      if (IsColumnExist(tableName, columnName) && !IsColumnTypeEquals(tableName, columnName, dataType, charLength))
      {
        const string indexName = "IDX_T_TRANS_FACTUR";
        TryExecuteNonQuery(string.Format("DROP INDEX IF EXISTS {0} ON {1}", indexName, tableName));
        TryExecuteNonQuery(string.Format("ALTER TABLE [{0}] ALTER COLUMN {1} {2}({3})", tableName, columnName, dataType, charLength));
      }
    }

    /// <summary>
    /// Early builds created M_CUSTOMERS with a column called Type while the application has always
    /// mapped it as MemberType, so reading a customer failed. Rename where the old name is present,
    /// otherwise add the column.
    /// </summary>
    private static void UpdateTableCustomer()
    {
      const string tableName = "M_CUSTOMERS";
      if (!CheckIfTableExist(tableName))
        return;
      if (IsColumnExist(tableName, "MemberType"))
        return;

      if (IsColumnExist(tableName, "Type"))
      {
        _log.Info("Renaming M_CUSTOMERS.Type to MemberType");
        TryExecuteNonQuery("EXEC sp_rename 'dbo.M_CUSTOMERS.Type', 'MemberType', 'COLUMN'");
      }
      else
      {
        _log.Info("Adding missing M_CUSTOMERS.MemberType");
        TryExecuteNonQuery(string.Format("ALTER TABLE {0} ADD MemberType int NULL", tableName));
      }
    }

    private static bool IsIndexExist(string tableName, string indexName)
    {
      object result = TryExecuteScalar(
        "SELECT NAME FROM SYS.INDEXES WHERE NAME = @indexName " +
        "AND OBJECT_ID = (SELECT OBJECT_ID FROM SYS.OBJECTS WHERE NAME = @tableName)",
        new SqlParameter("@indexName", indexName),
        new SqlParameter("@tableName", tableName));
      return result != null;
    }

    private static bool IsColumnExist(string tableName, string columnName)
    {
      object result = TryExecuteScalar(
        "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName",
        new SqlParameter("@tableName", tableName),
        new SqlParameter("@columnName", columnName));
      return result != null;
    }

    private static bool IsColumnTypeEquals(string tableName, string columnName, string dataType, int charLength = 0)
    {
      object result = TryExecuteScalar(
        "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName",
        new SqlParameter("@tableName", tableName),
        new SqlParameter("@columnName", columnName));
      if (result == null)
        return false;
      if (!string.Equals(result.ToString(), dataType, StringComparison.OrdinalIgnoreCase))
        return false;
      if (charLength <= 0)
        return true;

      object length = TryExecuteScalar(
        "SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName",
        new SqlParameter("@tableName", tableName),
        new SqlParameter("@columnName", columnName));
      if (length == null)
        return false;
      int actual;
      return int.TryParse(length.ToString(), out actual) && actual == charLength;
    }

    /// <summary>
    /// Runs a statement. Throws on failure so that a caller inside a database transaction rolls back
    /// instead of committing a partial write.
    /// </summary>
    internal static int ExecuteNonQuery(string nonQueryCommand, params SqlParameter[] parameters)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      try
      {
        using (SqlCommand command = connection.CreateCommand())
        {
          command.CommandText = nonQueryCommand;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          AddParameters(command, parameters);
          return command.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Failed to run non-query {0}", nonQueryCommand), e);
        throw;
      }
      finally
      {
        if (activeTransaction == null)
          connection.Close();
      }
    }

    /// <summary>
    /// Runs a scalar query. Throws on failure - see <see cref="ExecuteNonQuery"/>.
    /// </summary>
    internal static object ExecuteScalar(string scalarCommand, params SqlParameter[] parameters)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      if (activeTransaction == null)
        connection.Open();
      try
      {
        using (SqlCommand command = connection.CreateCommand())
        {
          command.CommandText = scalarCommand;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          AddParameters(command, parameters);
          object result = command.ExecuteScalar();
          return result == DBNull.Value ? null : result;
        }
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Failed to run query {0}", scalarCommand), e);
        throw;
      }
      finally
      {
        if (activeTransaction == null)
          connection.Close();
      }
    }

    /// <summary>
    /// Schema probing and best-effort maintenance: logs and reports failure instead of throwing,
    /// because a missing permission or an already-applied change must not stop the application from
    /// starting. Never use this for a write that matters.
    /// </summary>
    internal static int TryExecuteNonQuery(string nonQueryCommand, params SqlParameter[] parameters)
    {
      try
      {
        return ExecuteNonQuery(nonQueryCommand, parameters);
      }
      catch (Exception)
      {
        return -1;
      }
    }

    /// <summary>See <see cref="TryExecuteNonQuery"/>. Returns null when the query fails.</summary>
    internal static object TryExecuteScalar(string scalarCommand, params SqlParameter[] parameters)
    {
      try
      {
        return ExecuteScalar(scalarCommand, parameters);
      }
      catch (Exception)
      {
        return null;
      }
    }

    private static void AddParameters(SqlCommand command, SqlParameter[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      foreach (SqlParameter parameter in parameters)
      {
        if (parameter.Value == null)
          parameter.Value = DBNull.Value;
        command.Parameters.Add(parameter);
      }
    }
  }
}

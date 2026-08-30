using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Dialect;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database
{
  /// <summary>
  /// Startup schema reconciliation and the low level command helpers.
  ///
  /// Installations are spread across many sites on different versions with no migration history, so
  /// the schema is reconciled on every startup instead: each step checks first and only then
  /// applies. Every step must stay guarded, idempotent and additive - never drop a column an older
  /// installation may still hold data in.
  ///
  /// Nothing here is written for a particular database. The tables come from
  /// <see cref="DatabaseSchema"/> and the SQL to express them comes from the configured
  /// <see cref="ISqlDialect"/>.
  /// </summary>
  public class DBUtility
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private static ISqlDialect Dialect
    {
      get { return DBFactory.GetInstance().Dialect; }
    }

    public static void CheckForDatabaseTable()
    {
      _log.InfoFormat("Reconciling schema for {0}", Dialect.Name);
      CreateMissingTables();
      _log.Info("Update missing column");
      AddMissingColumns();
      RenameLegacyCustomerColumn();
      WidenFacturColumn();
      _log.Info("Create index");
      CreateMissingIndexes();
    }

    public static void CheckForDatabaseRow()
    {
      UpsertSettingRow();
      RetireSupersededManifestUrl();
    }

    #region Schema reconciliation

    private static void CreateMissingTables()
    {
      foreach (TableDefinition table in DatabaseSchema.Tables())
      {
        if (TableExists(table.Name))
          continue;
        _log.InfoFormat("Creating missing table {0}", table.Name);
        ExecuteNonQuery(Dialect.CreateTableStatement(table));
      }
    }

    private static void AddMissingColumns()
    {
      foreach (ColumnAddition addition in DatabaseSchema.ColumnAdditions())
      {
        if (!TableExists(addition.Table) || ColumnExists(addition.Table, addition.Column.Name))
          continue;

        _log.InfoFormat("Adding missing column {0}.{1}", addition.Table, addition.Column.Name);
        TryExecuteNonQuery(Dialect.AddColumnStatement(addition.Table, addition.Column));

        if (!string.IsNullOrEmpty(addition.BackfillLiteral))
          TryExecuteNonQuery(Dialect.BackfillStatement(addition.Table, addition.Column.Name, addition.BackfillLiteral));
      }
    }

    /// <summary>
    /// Early builds created M_CUSTOMERS with a column called Type while the application has always
    /// mapped it as MemberType, so reading a customer failed. Rename where the old name is present;
    /// a database created from the current schema already has the right name and skips this.
    /// </summary>
    private static void RenameLegacyCustomerColumn()
    {
      const string table = "M_CUSTOMERS";
      if (!TableExists(table) || ColumnExists(table, "MemberType"))
        return;

      if (ColumnExists(table, "Type"))
      {
        _log.Info("Renaming M_CUSTOMERS.Type to MemberType");
        TryExecuteNonQuery(Dialect.RenameColumnStatement(table, "Type", "MemberType"));
      }
      else
      {
        _log.Info("Adding missing M_CUSTOMERS.MemberType");
        TryExecuteNonQuery(Dialect.AddColumnStatement(table, ColumnDefinition.Column("MemberType", DbColumnType.Int)));
      }
    }

    /// <summary>
    /// Factur started life as varchar(18) but holds an 18 digit tick count, leaving no headroom.
    /// Widen it where the database both reports and enforces column widths.
    /// </summary>
    private static void WidenFacturColumn()
    {
      const string table = "T_TRANSACTIONS";
      const string column = "Factur";
      const int wanted = 20;

      if (!Dialect.SupportsColumnTypeInspection)
        return;
      if (!TableExists(table) || !ColumnExists(table, column))
        return;
      if (ColumnLength(table, column) >= wanted)
        return;

      ColumnDefinition widened = ColumnDefinition.Text(column, wanted);
      string alter = Dialect.AlterColumnTypeStatement(table, widened);
      if (alter == null)
        return;

      _log.InfoFormat("Widening {0}.{1} to {2} characters", table, column, wanted);
      // The unique index has to come off first on databases that will not alter an indexed column.
      TryExecuteNonQuery(Dialect.DropIndexStatement("IDX_T_TRANS_FACTUR", table));
      TryExecuteNonQuery(alter);
    }

    private static void CreateMissingIndexes()
    {
      foreach (IndexDefinition index in DatabaseSchema.Indexes())
      {
        try
        {
          if (IndexExists(index.Table, index.Name))
            continue;
          _log.InfoFormat("Creating missing index {0}", index.Name);
          TryExecuteNonQuery(Dialect.CreateIndexStatement(index));
        }
        catch (Exception e)
        {
          // An index is a performance concern, never a correctness one - never block startup for it.
          _log.Error(string.Format("Could not create index {0}", index.Name), e);
        }
      }
    }

    /// <summary>
    /// Inserts any setting row the database does not have yet. Existing rows are left untouched, so
    /// an operator's edits survive every upgrade and a new key only has to be declared in
    /// <see cref="SettingKeys.Seed"/> to reach older installations.
    /// </summary>
    private static void UpsertSettingRow()
    {
      string query = string.Format("SELECT {0} FROM {1} WHERE {2} = @key",
                                   Dialect.Quote("Id"), Dialect.Quote("M_SETTINGS"), Dialect.Quote("Key"));
      // Value and Default get their own parameters rather than repeating one: providers disagree
      // about whether a named parameter may appear twice in a statement.
      string insert = string.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}) VALUES (@key, @group, @value, @default)",
                                    Dialect.Quote("M_SETTINGS"), Dialect.Quote("Key"), Dialect.Quote("Group"),
                                    Dialect.Quote("Value"), Dialect.Quote("Default"));

      foreach (SettingKeys.SettingSeed seed in SettingKeys.Seed())
      {
        try
        {
          if (TryExecuteScalar(query, DbParam.Of("@key", seed.Key)) != null)
            continue;

          _log.InfoFormat("Seeding missing setting '{0}'.", seed.Key);
          int seeded = ExecuteNonQuery(insert,
            DbParam.Of("@key", seed.Key),
            DbParam.Of("@group", seed.Group),
            DbParam.Of("@value", seed.Value),
            DbParam.Of("@default", seed.Value));
          if (seeded <= 0)
            _log.WarnFormat("Seeding setting '{0}' affected no rows.", seed.Key);
        }
        catch (Exception e)
        {
          _log.Error(string.Format("Failed seeding setting '{0}'.", seed.Key), e);
        }
      }
    }

    /// <summary>
    /// Rewrites UPDATE_MANIFEST_URL where it still holds a retired default - the hand-edited Google
    /// Doc releases were announced in before they moved to GitHub. Only exact known defaults are
    /// rewritten; any other value is an operator's deliberate configuration and stays. Guarded and
    /// idempotent like every other reconciliation step: once rewritten, nothing matches again.
    /// </summary>
    private static void RetireSupersededManifestUrl()
    {
      // Value and Default get their own parameters rather than repeating one: providers disagree
      // about whether a named parameter may appear twice in a statement.
      string update = string.Format("UPDATE {0} SET {1} = @newValue, {2} = @newDefault WHERE {3} = @key AND {1} = @retired",
                                    Dialect.Quote("M_SETTINGS"), Dialect.Quote("Value"),
                                    Dialect.Quote("Default"), Dialect.Quote("Key"));

      foreach (string retired in SettingKeys.RetiredUpdateManifestUrls)
      {
        try
        {
          int rewritten = ExecuteNonQuery(update,
            DbParam.Of("@newValue", SettingKeys.DefaultUpdateManifestUrl),
            DbParam.Of("@newDefault", SettingKeys.DefaultUpdateManifestUrl),
            DbParam.Of("@key", SettingKeys.UpdateManifestUrl),
            DbParam.Of("@retired", retired));
          if (rewritten > 0)
            _log.InfoFormat("Update manifest address migrated from retired '{0}' to '{1}'.",
                            retired, SettingKeys.DefaultUpdateManifestUrl);
        }
        catch (Exception e)
        {
          // Best effort like the rest of reconciliation: a till that cannot migrate the address
          // still starts, it just keeps checking the old one.
          _log.Error(string.Format("Could not migrate the update manifest address from '{0}'.", retired), e);
        }
      }
    }

    #endregion

    #region Schema probes

    private static bool TableExists(string tableName)
    {
      return TryExecuteScalar(Dialect.TableExistsQuery, DbParam.Of("@tableName", tableName)) != null;
    }

    private static bool ColumnExists(string tableName, string columnName)
    {
      return TryExecuteScalar(Dialect.ColumnExistsQuery,
                              DbParam.Of("@tableName", tableName),
                              DbParam.Of("@columnName", columnName)) != null;
    }

    private static bool IndexExists(string tableName, string indexName)
    {
      return TryExecuteScalar(Dialect.IndexExistsQuery,
                              DbParam.Of("@indexName", indexName),
                              DbParam.Of("@tableName", tableName)) != null;
    }

    /// <summary>Declared length of a text column, or 0 when unknown or unlimited.</summary>
    private static int ColumnLength(string tableName, string columnName)
    {
      object result = TryExecuteScalar(Dialect.ColumnLengthQuery,
                                       DbParam.Of("@tableName", tableName),
                                       DbParam.Of("@columnName", columnName));
      if (result == null)
        return 0;
      int length;
      return int.TryParse(result.ToString(), out length) ? length : 0;
    }

    #endregion

    #region Command helpers

    /// <summary>
    /// Runs a statement. Throws on failure so that a caller inside a transaction rolls back instead
    /// of committing a partial write.
    /// </summary>
    internal static int ExecuteNonQuery(string nonQueryCommand, params DbParameter[] parameters)
    {
      return Execute(nonQueryCommand, parameters, command => command.ExecuteNonQuery());
    }

    /// <summary>Runs a scalar query. Throws on failure - see <see cref="ExecuteNonQuery"/>.</summary>
    internal static object ExecuteScalar(string scalarCommand, params DbParameter[] parameters)
    {
      return Execute(scalarCommand, parameters, command =>
      {
        object result = command.ExecuteScalar();
        return result == DBNull.Value ? null : result;
      });
    }

    /// <summary>
    /// Schema probing and best-effort maintenance: logs and reports failure instead of throwing,
    /// because a missing permission or an already-applied change must not stop the application from
    /// starting. Never use this for a write that matters.
    /// </summary>
    internal static int TryExecuteNonQuery(string nonQueryCommand, params DbParameter[] parameters)
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
    internal static object TryExecuteScalar(string scalarCommand, params DbParameter[] parameters)
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

    private static T Execute<T>(string commandText, DbParameter[] parameters, Func<DbCommand, T> run)
    {
      using (DbScope scope = DBFactory.GetInstance().AcquireScope())
      {
        try
        {
          using (DbCommand command = scope.CreateCommand(commandText))
          {
            AddParameters(command, parameters);
            return run(command);
          }
        }
        catch (Exception e)
        {
          _log.Error(string.Format("Failed to run: {0}", commandText), e);
          throw;
        }
      }
    }

    internal static void AddParameters(DbCommand command, DbParameter[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      foreach (DbParameter parameter in parameters)
      {
        if (parameter.Value == null)
          parameter.Value = DBNull.Value;
        command.Parameters.Add(parameter);
      }
    }

    #endregion
  }
}

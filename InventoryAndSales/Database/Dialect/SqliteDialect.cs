using System;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// SQLite, reached through the System.Data.SQLite provider registered in App.config.
  ///
  /// The differences that matter here:
  ///  - types are advisory. Every string is TEXT and every integer is INTEGER, so there is no column
  ///    to widen and <see cref="SupportsColumnTypeInspection"/> is false;
  ///  - the auto-numbered key must be declared INTEGER PRIMARY KEY AUTOINCREMENT, which also makes it
  ///    the only table here with a declared primary key;
  ///  - there is no boolean type: false is 0. The provider maps System.Boolean to 0/1 for both
  ///    parameters and results, so the models are unaffected;
  ///  - CAST(x AS date) is meaningless because dates are text, so date() is used instead.
  ///
  /// Requires SQLite 3.25 or later for ALTER TABLE ... RENAME COLUMN, and 3.16 or later for the
  /// pragma_table_info() table-valued function.
  /// </summary>
  public class SqliteDialect : SqlDialectBase
  {
    public const string DialectName = "Sqlite";

    public override string Name
    {
      get { return DialectName; }
    }

    public override string ProviderInvariantName
    {
      get { return "System.Data.SQLite"; }
    }

    public override string Quote(string identifier)
    {
      return "\"" + identifier.Replace("\"", "\"\"") + "\"";
    }

    public override string QuoteAlias(string alias)
    {
      return "\"" + alias.Replace("\"", "\"\"") + "\"";
    }

    /// <summary>Timestamps are stored as ISO-8601 text, so date() is the truncation.</summary>
    public override string ToDate(string expression)
    {
      return string.Format("date({0})", expression);
    }

    protected override string FalseLiteral
    {
      get { return "0"; }
    }

    protected override string MapType(ColumnDefinition column)
    {
      switch (column.Type)
      {
        case DbColumnType.Int: return "INTEGER";
        case DbColumnType.Long: return "INTEGER";
        case DbColumnType.Decimal: return "NUMERIC";
        case DbColumnType.Bool: return "INTEGER";
        // Length is not enforced, but declaring it keeps the schema self-documenting.
        case DbColumnType.String: return string.Format("VARCHAR({0})", column.Length);
        case DbColumnType.Text: return "TEXT";
        case DbColumnType.DateTime: return "DATETIME";
      }
      throw new NotSupportedException("Unmapped column type " + column.Type);
    }

    protected override string RenderIdentityColumn(ColumnDefinition column)
    {
      // AUTOINCREMENT requires exactly "INTEGER PRIMARY KEY" - not BIGINT, even for the bigint keys.
      // SQLite integers are 64 bit regardless, so nothing is lost.
      return string.Format("{0} INTEGER PRIMARY KEY AUTOINCREMENT", Quote(column.Name));
    }

    /// <summary>Column types are advisory, so there is nothing to widen.</summary>
    public override bool SupportsColumnTypeInspection
    {
      get { return false; }
    }

    public override string AlterColumnTypeStatement(string table, ColumnDefinition column)
    {
      return null;
    }

    public override string TableExistsQuery
    {
      get { return "SELECT name FROM sqlite_master WHERE type = 'table' AND name = @tableName"; }
    }

    public override string ColumnExistsQuery
    {
      get { return "SELECT name FROM pragma_table_info(@tableName) WHERE name = @columnName"; }
    }

    public override string ColumnTypeQuery
    {
      get { return "SELECT type FROM pragma_table_info(@tableName) WHERE name = @columnName"; }
    }

    public override string ColumnLengthQuery
    {
      get { return "SELECT NULL WHERE @tableName IS NOT NULL AND @columnName IS NOT NULL"; }
    }

    public override string IndexExistsQuery
    {
      get
      {
        return "SELECT name FROM sqlite_master " +
               "WHERE type = 'index' AND name = @indexName AND tbl_name = @tableName";
      }
    }

    public override string AppendIdentityRetrieval(string insertStatement, string identityColumn)
    {
      return insertStatement + "; SELECT last_insert_rowid()";
    }
  }
}

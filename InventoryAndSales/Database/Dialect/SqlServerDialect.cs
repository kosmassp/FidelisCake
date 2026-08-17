using System;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// Microsoft SQL Server. The original target, and still the default.
  /// </summary>
  public class SqlServerDialect : SqlDialectBase
  {
    public const string DialectName = "SqlServer";

    public override string Name
    {
      get { return DialectName; }
    }

    public override string ProviderInvariantName
    {
      get { return "System.Data.SqlClient"; }
    }

    public override string Quote(string identifier)
    {
      return "[" + identifier.Replace("]", "]]") + "]";
    }

    public override string QuoteAlias(string alias)
    {
      return "[" + alias.Replace("]", "]]") + "]";
    }

    public override string ToDate(string expression)
    {
      return string.Format("CAST({0} AS date)", expression);
    }

    protected override string FalseLiteral
    {
      get { return "((0))"; }
    }

    protected override string MapType(ColumnDefinition column)
    {
      switch (column.Type)
      {
        case DbColumnType.Int: return "int";
        case DbColumnType.Long: return "bigint";
        case DbColumnType.Decimal: return "decimal(18, 0)";
        case DbColumnType.Bool: return "bit";
        case DbColumnType.String: return string.Format("varchar({0})", column.Length);
        case DbColumnType.Text: return "text";
        case DbColumnType.DateTime: return "datetime";
      }
      throw new NotSupportedException("Unmapped column type " + column.Type);
    }

    protected override string RenderIdentityColumn(ColumnDefinition column)
    {
      return string.Format("{0} {1} IDENTITY(1,1) NOT NULL",
                           Quote(column.Name),
                           column.Type == DbColumnType.Long ? "bigint" : "int");
    }

    /// <summary>SQL Server spells this without the COLUMN keyword.</summary>
    public override string AddColumnStatement(string table, ColumnDefinition column)
    {
      return string.Format("ALTER TABLE {0} ADD {1} {2} NULL",
                           Quote(table), Quote(column.Name), MapType(column));
    }

    /// <summary>There is no RENAME COLUMN; the rename goes through a system procedure.</summary>
    public override string RenameColumnStatement(string table, string fromColumn, string toColumn)
    {
      return string.Format("EXEC sp_rename '{0}.{1}', '{2}', 'COLUMN'", table, fromColumn, toColumn);
    }

    /// <summary>An index name is scoped to its table here, so the table has to be named.</summary>
    public override string DropIndexStatement(string indexName, string table)
    {
      return string.Format("DROP INDEX IF EXISTS {0} ON {1}", Quote(indexName), Quote(table));
    }

    public override string AlterColumnTypeStatement(string table, ColumnDefinition column)
    {
      return string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}",
                           Quote(table), Quote(column.Name), MapType(column));
    }

    public override string TableExistsQuery
    {
      get { return "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName"; }
    }

    public override string ColumnExistsQuery
    {
      get
      {
        return "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
               "WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";
      }
    }

    public override string ColumnTypeQuery
    {
      get
      {
        return "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS " +
               "WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";
      }
    }

    public override string ColumnLengthQuery
    {
      get
      {
        return "SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS " +
               "WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";
      }
    }

    public override string IndexExistsQuery
    {
      get
      {
        return "SELECT NAME FROM SYS.INDEXES WHERE NAME = @indexName " +
               "AND OBJECT_ID = (SELECT OBJECT_ID FROM SYS.OBJECTS WHERE NAME = @tableName)";
      }
    }

    /// <summary>
    /// SCOPE_IDENTITY() must be in the same batch as the insert. Because the insert is
    /// parameterised, SqlClient sends it through sp_executesql, and a SCOPE_IDENTITY() issued as a
    /// separate command sits outside that scope and returns NULL.
    /// </summary>
    public override string AppendIdentityRetrieval(string insertStatement, string identityColumn)
    {
      return insertStatement + "; SELECT SCOPE_IDENTITY()";
    }
  }
}

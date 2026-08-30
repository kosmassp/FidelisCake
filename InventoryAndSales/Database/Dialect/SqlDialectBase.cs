using System;
using System.Collections.Generic;
using System.Text;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// The parts of a dialect that are the same everywhere: assembling a CREATE TABLE from a
  /// <see cref="TableDefinition"/>, and the handful of statements whose only variation is how a type
  /// or an identifier is spelled.
  /// </summary>
  public abstract class SqlDialectBase : ISqlDialect
  {
    public abstract string Name { get; }
    public abstract string ProviderInvariantName { get; }
    public abstract string ProviderFactoryTypeName { get; }

    public abstract string Quote(string identifier);
    public abstract string QuoteAlias(string alias);
    public abstract string ToDate(string expression);

    /// <summary>LIKE is already case-insensitive on SQL Server and, for ASCII, on SQLite.</summary>
    public virtual string CaseInsensitiveLike
    {
      get { return "LIKE"; }
    }

    public abstract string TableExistsQuery { get; }
    public abstract string ColumnExistsQuery { get; }
    public abstract string ColumnTypeQuery { get; }
    public abstract string ColumnLengthQuery { get; }
    public abstract string IndexExistsQuery { get; }
    public abstract string AppendIdentityRetrieval(string insertStatement, string identityColumn);

    public virtual bool SupportsColumnTypeInspection
    {
      get { return true; }
    }

    /// <summary>Renders a storage class as this database's type name.</summary>
    protected abstract string MapType(ColumnDefinition column);

    /// <summary>The full column fragment for an auto-numbered primary key.</summary>
    protected abstract string RenderIdentityColumn(ColumnDefinition column);

    /// <summary>How this database spells a false boolean literal in a DEFAULT clause.</summary>
    protected abstract string FalseLiteral { get; }

    public virtual string CreateTableStatement(TableDefinition table)
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("CREATE TABLE {0} (", Quote(table.Name));

      bool first = true;
      foreach (ColumnDefinition column in table.Columns)
      {
        if (!first)
          sb.Append(",");
        sb.Append(" ");
        sb.Append(RenderColumn(column));
        first = false;
      }
      sb.Append(" )");
      return sb.ToString();
    }

    protected virtual string RenderColumn(ColumnDefinition column)
    {
      if (column.IsIdentity)
        return RenderIdentityColumn(column);

      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("{0} {1}", Quote(column.Name), MapType(column));
      if (column.DefaultFalse)
        sb.AppendFormat(" DEFAULT {0}", FalseLiteral);
      sb.Append(column.Nullable ? " NULL" : " NOT NULL");
      return sb.ToString();
    }

    public virtual string AddColumnStatement(string table, ColumnDefinition column)
    {
      // Always added as nullable: existing rows have no value for it, and a NOT NULL column without
      // a default cannot be added to a populated table.
      return string.Format("ALTER TABLE {0} ADD COLUMN {1} {2} NULL",
                           Quote(table), Quote(column.Name), MapType(column));
    }

    public virtual string BackfillStatement(string table, string column, string literal)
    {
      return string.Format("UPDATE {0} SET {1} = {2} WHERE {1} IS NULL",
                           Quote(table), Quote(column), literal);
    }

    public virtual string RenameColumnStatement(string table, string fromColumn, string toColumn)
    {
      return string.Format("ALTER TABLE {0} RENAME COLUMN {1} TO {2}",
                           Quote(table), Quote(fromColumn), Quote(toColumn));
    }

    public virtual string CreateIndexStatement(IndexDefinition index)
    {
      StringBuilder columns = new StringBuilder();
      for (int i = 0; i < index.Columns.Length; i++)
      {
        if (i > 0)
          columns.Append(", ");
        columns.Append(Quote(index.Columns[i]));
        if (index.Descending)
          columns.Append(" DESC");
      }

      return string.Format("CREATE {0}INDEX {1} ON {2} ( {3} )",
                           index.Unique ? "UNIQUE " : string.Empty,
                           Quote(index.Name), Quote(index.Table), columns);
    }

    public virtual string DropIndexStatement(string indexName, string table)
    {
      return string.Format("DROP INDEX IF EXISTS {0}", Quote(indexName));
    }

    public virtual string AlterColumnTypeStatement(string table, ColumnDefinition column)
    {
      return string.Format("ALTER TABLE {0} ALTER COLUMN {1} TYPE {2}",
                           Quote(table), Quote(column.Name), MapType(column));
    }
  }
}

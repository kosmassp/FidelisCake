using System;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// PostgreSQL, reached through the Npgsql provider registered in App.config.
  ///
  /// The differences that matter here:
  ///  - identifiers are folded to lower case unless quoted, so every identifier is quoted and keeps
  ///    the exact case the schema declares - the same names the models map;
  ///  - a result alias must be double quoted; the single quotes SQL Server tolerates are string
  ///    literals here;
  ///  - auto numbering is serial/bigserial rather than IDENTITY.
  /// </summary>
  public class PostgreSqlDialect : SqlDialectBase
  {
    public const string DialectName = "PostgreSql";

    public override string Name
    {
      get { return DialectName; }
    }

    public override string ProviderInvariantName
    {
      get { return "Npgsql"; }
    }

    public override string ProviderFactoryTypeName
    {
      get { return "Npgsql.NpgsqlFactory, Npgsql"; }
    }

    public override string Quote(string identifier)
    {
      return "\"" + identifier.Replace("\"", "\"\"") + "\"";
    }

    public override string QuoteAlias(string alias)
    {
      return "\"" + alias.Replace("\"", "\"\"") + "\"";
    }

    public override string ToDate(string expression)
    {
      return string.Format("CAST({0} AS date)", expression);
    }

    /// <summary>PostgreSQL is the one of the three whose LIKE respects case.</summary>
    public override string CaseInsensitiveLike
    {
      get { return "ILIKE"; }
    }

    protected override string FalseLiteral
    {
      get { return "false"; }
    }

    protected override string MapType(ColumnDefinition column)
    {
      switch (column.Type)
      {
        case DbColumnType.Int: return "integer";
        case DbColumnType.Long: return "bigint";
        case DbColumnType.Decimal: return "numeric(18, 0)";
        case DbColumnType.Bool: return "boolean";
        case DbColumnType.String: return string.Format("varchar({0})", column.Length);
        case DbColumnType.Text: return "text";
        case DbColumnType.DateTime: return "timestamp";
      }
      throw new NotSupportedException("Unmapped column type " + column.Type);
    }

    protected override string RenderIdentityColumn(ColumnDefinition column)
    {
      // serial/bigserial create the sequence and imply NOT NULL.
      return string.Format("{0} {1}",
                           Quote(column.Name),
                           column.Type == DbColumnType.Long ? "bigserial" : "serial");
    }

    public override string TableExistsQuery
    {
      get
      {
        return "SELECT table_name FROM information_schema.tables " +
               "WHERE table_schema = current_schema() AND table_name = @tableName";
      }
    }

    public override string ColumnExistsQuery
    {
      get
      {
        return "SELECT column_name FROM information_schema.columns " +
               "WHERE table_schema = current_schema() AND table_name = @tableName AND column_name = @columnName";
      }
    }

    public override string ColumnTypeQuery
    {
      get
      {
        return "SELECT data_type FROM information_schema.columns " +
               "WHERE table_schema = current_schema() AND table_name = @tableName AND column_name = @columnName";
      }
    }

    public override string ColumnLengthQuery
    {
      get
      {
        return "SELECT character_maximum_length FROM information_schema.columns " +
               "WHERE table_schema = current_schema() AND table_name = @tableName AND column_name = @columnName";
      }
    }

    public override string IndexExistsQuery
    {
      get
      {
        return "SELECT indexname FROM pg_indexes " +
               "WHERE schemaname = current_schema() AND indexname = @indexName AND tablename = @tableName";
      }
    }

    /// <summary>
    /// RETURNING gives the generated key from the insert itself - no second statement, and no
    /// reliance on session state such as lastval().
    /// </summary>
    public override string AppendIdentityRetrieval(string insertStatement, string identityColumn)
    {
      return insertStatement + " RETURNING " + Quote(identityColumn);
    }

    /// <summary>
    /// information_schema reports the declared type as "character varying", not "varchar", so the
    /// type comparison is done on that name.
    /// </summary>
    public string VarcharTypeName
    {
      get { return "character varying"; }
    }
  }
}

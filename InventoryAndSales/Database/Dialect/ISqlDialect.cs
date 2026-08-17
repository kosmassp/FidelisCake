using System;
using System.Collections.Generic;
using System.Data.Common;
using InventoryAndSales.Database.Schema;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// Everything the data layer needs to know that differs between database products.
  ///
  /// Adding support for another database means writing one of these, not editing DBUtility, BaseDao
  /// or the queries.
  /// </summary>
  public interface ISqlDialect
  {
    /// <summary>Name used in the ConnectionProvider setting and in log messages.</summary>
    string Name { get; }

    /// <summary>ADO.NET invariant name resolved through DbProviderFactories, e.g. "Npgsql".</summary>
    string ProviderInvariantName { get; }

    // ---- identifiers and literals -------------------------------------------------------------

    /// <summary>
    /// Wraps an identifier so reserved words such as Key, Group and Default are usable and, on
    /// PostgreSQL, so its case is preserved rather than folded to lower case.
    /// </summary>
    string Quote(string identifier);

    /// <summary>Quotes a result column alias. PostgreSQL rejects the single quotes SQL Server allows.</summary>
    string QuoteAlias(string alias);

    /// <summary>Truncates a timestamp expression to a whole date.</summary>
    string ToDate(string expression);

    /// <summary>
    /// The case-insensitive pattern match operator. Plain LIKE is case-sensitive on PostgreSQL,
    /// which would silently break the product search box there.
    /// </summary>
    string CaseInsensitiveLike { get; }

    // ---- DDL ----------------------------------------------------------------------------------

    string CreateTableStatement(TableDefinition table);
    string AddColumnStatement(string table, ColumnDefinition column);
    string BackfillStatement(string table, string column, string literal);
    string RenameColumnStatement(string table, string fromColumn, string toColumn);
    string CreateIndexStatement(IndexDefinition index);
    string DropIndexStatement(string indexName, string table);

    /// <summary>
    /// Widens a column, or null when the database cannot do it. SQLite is typeless in this respect,
    /// so there is nothing to widen.
    /// </summary>
    string AlterColumnTypeStatement(string table, ColumnDefinition column);

    // ---- schema probes ------------------------------------------------------------------------
    // Each returns a statement taking the named parameters below and yielding a row when the object
    // exists.

    /// <summary>Parameters: @tableName.</summary>
    string TableExistsQuery { get; }
    /// <summary>Parameters: @tableName, @columnName.</summary>
    string ColumnExistsQuery { get; }
    /// <summary>Parameters: @tableName, @columnName. Returns the declared type name.</summary>
    string ColumnTypeQuery { get; }
    /// <summary>Parameters: @tableName, @columnName. Returns the declared length, or null.</summary>
    string ColumnLengthQuery { get; }
    /// <summary>Parameters: @indexName, @tableName.</summary>
    string IndexExistsQuery { get; }

    /// <summary>True when the database reports column types well enough to widen them.</summary>
    bool SupportsColumnTypeInspection { get; }

    // ---- inserts ------------------------------------------------------------------------------

    /// <summary>
    /// Extends an INSERT so that executing it also yields the generated key, and the caller reads it
    /// with a single ExecuteScalar.
    ///
    /// It has to be one statement. Asking for the key afterwards, as a second command, looks like it
    /// works and does not: a parameterised insert is sent as sp_executesql, which runs in its own
    /// scope, so a later SELECT SCOPE_IDENTITY() batch returns NULL and every insert silently comes
    /// back with an id of zero.
    /// </summary>
    /// <param name="identityColumn">Unquoted name of the auto-numbered key column.</param>
    string AppendIdentityRetrieval(string insertStatement, string identityColumn);
  }
}

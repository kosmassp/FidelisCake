# Directory: `InventoryAndSales/Database/Dialect/`

Namespace `InventoryAndSales.Database.Dialect`. **Everything the data layer needs to know that
differs between database products.**

Supporting another database means writing one of these, not editing `DBUtility`, `BaseDao` or the
report queries.

---

## `ISqlDialect.cs`

`public interface ISqlDialect` — the contract.

| Member | Purpose |
|---|---|
| `Name` | Value used in the `DatabaseProvider` setting: `SqlServer`, `PostgreSql`, `Sqlite`. |
| `ProviderInvariantName` | ADO.NET invariant name resolved through `DbProviderFactories`. |
| `Quote(identifier)` | Makes reserved words such as `Key`, `Group` and `Default` usable, and preserves identifier case on PostgreSQL. |
| `QuoteAlias(alias)` | A result column heading. SQL Server tolerates `'Jumlah Transaksi'`; to PostgreSQL that is a string literal, not a name. |
| `ToDate(expression)` | Truncates a timestamp to a whole date. |
| `CaseInsensitiveLike` | `LIKE`, or `ILIKE` on PostgreSQL. |
| `CreateTableStatement`, `AddColumnStatement`, `BackfillStatement`, `RenameColumnStatement`, `CreateIndexStatement`, `DropIndexStatement`, `AlterColumnTypeStatement` | DDL, rendered from a `TableDefinition` / `ColumnDefinition`. |
| `TableExistsQuery`, `ColumnExistsQuery`, `ColumnTypeQuery`, `ColumnLengthQuery`, `IndexExistsQuery` | Schema probes, each taking `@tableName` / `@columnName` / `@indexName`. |
| `SupportsColumnTypeInspection` | False where types are advisory, so there is nothing to widen. |
| `AppendIdentityRetrieval(insert, identityColumn)` | Extends an INSERT so the same statement yields the generated key. |

### Why identity retrieval is part of the insert

`AppendIdentityRetrieval` exists because the obvious alternative is wrong. A parameterised insert
travels to SQL Server as `sp_executesql`, which runs in **its own scope**; a `SELECT SCOPE_IDENTITY()`
issued afterwards as a separate command is outside that scope and returns **NULL**. Every new row
would come back with an id of zero, and every foreign key derived from it would point at nothing —
silently, because the insert itself succeeds.

This was caught by running the end-to-end harness against a real SQL Server, not by reading the code.

---

## `SqlDialectBase.cs`

`public abstract class SqlDialectBase : ISqlDialect` — the parts that are the same everywhere:
assembling a `CREATE TABLE` from a `TableDefinition`, and the statements whose only variation is how
a type or identifier is spelled. Subclasses supply `MapType`, `RenderIdentityColumn` and
`FalseLiteral`, and override the handful of statements that differ.

---

## The three dialects

| | `SqlServerDialect` | `PostgreSqlDialect` | `SqliteDialect` |
|---|---|---|---|
| Provider | `System.Data.SqlClient` | `Npgsql` | `System.Data.SQLite` |
| Quoting | `[x]` | `"x"` | `"x"` |
| Identity | `int IDENTITY(1,1)` | `serial` / `bigserial` | `INTEGER PRIMARY KEY AUTOINCREMENT` |
| Key read-back | `; SELECT SCOPE_IDENTITY()` | ` RETURNING "Id"` | `; SELECT last_insert_rowid()` |
| Bool | `bit`, default `((0))` | `boolean`, default `false` | `INTEGER`, default `0` |
| Decimal | `decimal(18,0)` | `numeric(18,0)` | `NUMERIC` |
| Timestamp | `datetime` | `timestamp` | `DATETIME` (ISO-8601 text) |
| Truncate to date | `CAST(x AS date)` | `CAST(x AS date)` | `date(x)` |
| Pattern match | `LIKE` | **`ILIKE`** | `LIKE` |
| Probes | `INFORMATION_SCHEMA`, `SYS.INDEXES` | `information_schema`, `pg_indexes` | `sqlite_master`, `pragma_table_info()` |
| Rename column | `sp_rename` | `ALTER TABLE … RENAME COLUMN` | `ALTER TABLE … RENAME COLUMN` |
| Widen a column | yes | yes | **not applicable** — types are advisory |
| Drop index | needs `ON <table>` | name only | name only |

### Product-specific notes

**PostgreSQL** folds unquoted identifiers to lower case, so every identifier is quoted and keeps the
exact case the schema declares — the same names the models map. Its `LIKE` respects case, which
without `ILIKE` would quietly stop the product search box matching anything typed in the wrong case.

**SQLite** has one integer type and returns `Int64` for everything, including flags and identities.
That is handled on the model side by the conversion helpers on `BaseObject`. `AUTOINCREMENT` requires
exactly `INTEGER PRIMARY KEY`, so the bigint keys are declared `INTEGER` — SQLite integers are 64-bit
regardless. Requires SQLite **3.25+** for `RENAME COLUMN` and **3.16+** for `pragma_table_info()`.

---

## `SqlDialectFactory.cs`

`public static class SqlDialectFactory`.

| Member | Purpose |
|---|---|
| `Create()` | Reads the `DatabaseProvider` app setting; defaults to SQL Server. |
| `Create(providerName)` | Matches by name; logs the valid names and falls back to SQL Server if unrecognised. |
| `All()` | The three dialects. |
| `ResolveProviderFactory(dialect)` | `DbProviderFactories.GetFactory`, throwing a `ConfigurationErrorsException` that names the assembly to install — whoever hits it is setting up a machine, not reading source. |

**The provider is resolved at runtime, not referenced at compile time.** The application therefore
holds no dependency on Npgsql or System.Data.SQLite and the shipped binary is identical everywhere; a
site that wants one copies the provider assembly beside the executable and uncomments its entry in
`App.config`. That is what keeps the framework-only rule intact — see
[../rules-csharp.md](../rules-csharp.md).

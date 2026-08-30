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
| `ProviderInvariantName` | ADO.NET invariant name. Only used for the optional `DbProviderFactories` route. |
| `ProviderFactoryTypeName` | Assembly-qualified factory type, e.g. `Npgsql.NpgsqlFactory, Npgsql`. **This is what the loader uses**, so a provider works by being present beside the executable. |
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
| `ResolveProviderFactory(dialect)` | Delegates to `ProviderLoader`, wrapping failure in a `ConfigurationErrorsException` — whoever hits it is setting up a machine, not reading source. |

---

## `ProviderLoader.cs`

`internal static class ProviderLoader` — finds a provider's `DbProviderFactory` **with nothing
declared in App.config**.

| Member | Signature | Purpose |
|---|---|---|
| `Resolve` | `internal static DbProviderFactory Resolve(ISqlDialect)` | Installs the resolver, then tries the factory type directly and falls back to a `DbProviderFactories` registration. |
| `Install` | `internal static void Install()` | Adds the `AssemblyResolve` handler. Idempotent, lock-guarded. |
| `ResolveBySimpleName` | `private static Assembly (object, ResolveEventArgs)` | Last chance: matches a failed bind by simple name against the application folder. |
| `FromFactoryType` | `private static DbProviderFactory (ISqlDialect)` | `Type.GetType` on `ProviderFactoryTypeName`, then reads the static `Instance` member. |
| `FromConfiguredProviders` | `private static DbProviderFactory (ISqlDialect)` | The optional `DbProviderFactories` route. |

### What this replaced

Two blocks of configuration, both now unnecessary:

- **`<system.data><DbProviderFactories>`** — every ADO.NET provider exposes a public static
  `Instance` field on its factory (the convention `DbProviderFactories` itself relies on), so the
  factory is read straight off the type.
- **`<assemblyBinding>` redirects** — Npgsql's dependencies have file versions ahead of the versions
  it was compiled against, and because there is no compile-time reference the build cannot generate
  redirects. `ResolveBySimpleName` does what a wildcard redirect would: it was observed resolving
  `System.Runtime.CompilerServices.Unsafe v4.0.5.0 → v4.0.4.1` at runtime.

A provider now works **by being present beside the executable**. Nothing to register, nothing to keep
in step, and no way for a site to get the config subtly wrong.

The direct type load is tried first so the assembly shipped beside the executable is the one used; a
`DbProviderFactories` registration still wins if a site adds one, leaving the config route available
for anyone who wants to point at a different build.

**Still no compile-time reference.** The application depends on neither Npgsql nor
System.Data.SQLite, so the shipped binary is identical everywhere — which is what keeps the
framework-only rule intact. See [../rules-csharp.md](../rules-csharp.md).

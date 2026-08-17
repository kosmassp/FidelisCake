# Directory: `InventoryAndSales/Database/Schema/`

Namespace `InventoryAndSales.Database.Schema`. **The schema the application expects, declared once
and independent of any database product.**

This exists because the schema used to be SQL Server DDL written out by hand inside `DBUtility`,
which is why supporting a second database would have meant rewriting that file. Nothing here
mentions a product; an `ISqlDialect` renders it.

---

## `DatabaseSchema.cs`

One file holding the model types and the declaration itself.

### `public enum DbColumnType`

The storage classes the application needs: `Int`, `Long`, `Decimal`, `Bool`, `String`, `Text`,
`DateTime`. Each dialect maps these to its own type names.

### `public class ColumnDefinition`

Immutable, built through named factories rather than a constructor, so a declaration reads as prose.

| Member | Purpose |
|---|---|
| `Identity(name, type)` | Auto-numbered primary key. Rendered per dialect: `IDENTITY(1,1)`, `serial`, `INTEGER PRIMARY KEY AUTOINCREMENT`. |
| `Column(name, type, nullable)` | An ordinary column. |
| `Text(name, length, nullable)` | A bounded string column. |
| `Flag(name)` | A NOT NULL boolean defaulting to false — the soft-delete columns. |

### `public class TableDefinition`

A table name plus its columns.

### `public class IndexDefinition`

Name, table, columns, `Unique`, `Descending`.

### `public class ColumnAddition`

A column an older installation may be missing, plus an optional `BackfillLiteral` for existing rows.

### `public static class DatabaseSchema`

| Member | Purpose |
|---|---|
| `Tables()` | The seven tables: `M_SETTINGS`, `M_PRODUCTS`, `M_USERS`, `T_TRANSACTION_DETAILS`, `T_TRANSACTIONS`, `M_CUSTOMERS`, `T_AUDIT_LOG`. |
| `ColumnAdditions()` | Columns added after the original release: `Revision` (backfilled to `0`), `CancelledBy`, `CancelledAt`, the payment columns. |
| `Indexes()` | `IDX_T_TRANS_TRXTIME`, `IDX_T_TRANS_FACTUR` (unique), `IDX_T_TRDETAIL_TRX_ID`, `IDX_T_AUDIT_TIME`. |

### `T_AUDIT_LOG`

`Id`, `AuditTime`, `UserId`, `UserName`, `Action`, `EntityType`, `EntityKey`, `Workstation`,
`Detail`. Written by `Business/AuditService.cs`; nothing reads it in the application — it is
investigated with SQL.

The actor's **name is stored beside the id** rather than only joined. A user row can be renamed or
soft-deleted, and a trail that stops naming its actor afterwards answers nothing. `Workstation` is
there because a shop runs more than one till.

A whole new table rather than columns on existing ones, so it needs no `ColumnAdditions()` entry:
`DBUtility` creates a missing table at startup the same way it adds a missing column.

## Changing the schema

1. Add the column here — to `Tables()` for a fresh database **and** to `ColumnAdditions()` so
   existing installations pick it up on their next launch. Both are needed: sites sit on many
   versions with no migration history.
2. Add it to the model's map in `Database/DataTable/DataTableList.cs`.
3. Add the property and both indexer arms in `Database/Model/`.

You do **not** write DDL. Every dialect renders the declaration itself.

⚠ Adding a column to `DataTableList` makes every read and write of that entity require it. If a site's
`ALTER` could plausibly fail, keep the column out of the map and write it with a targeted statement
instead, as `TransactionManager.RecordCancellationAudit` does.

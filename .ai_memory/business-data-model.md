# Data Model and Persistence

Database: **SQL Server**, catalogue `SalesInventory`, connection string in `App.config`.
Naming: `M_` = master data, `T_` = transactional data.

## Tables

### `M_PRODUCTS` — product catalogue

| Column | Type | Notes |
|---|---|---|
| `Id` | `int IDENTITY` | Primary key. |
| `Code` | `varchar(10)` | Human code, e.g. `KC001`. Searched by prefix. Unique by convention only. |
| `Name` | `varchar(70)` NOT NULL | Searched with `LIKE`. Unique by convention only. |
| `Price` | `decimal(18,0)` NOT NULL | **Scale 0 — rupiah, no fractional part.** |
| `Discount` | `decimal(18,0)` | Sign-encoded: `> 0` flat rupiah, `< 0` percentage (negated), `0` none. |
| `Deleted` | `bit` NOT NULL DEFAULT 0 | Soft delete. |
| `Barcode` | `varchar(20)` | Optional. Exact-match scan. |

### `M_USERS` — operators

| Column | Type | Notes |
|---|---|---|
| `Id` | `int IDENTITY` | Primary key. |
| `Username` | `varchar(50)` | Login name. Immutable through the UI. |
| `Password` | `varchar(256)` | Unsalted SHA-512, `BitConverter.ToString` hex with dashes (191 chars). |
| `Name` | `varchar(50)` | Display name — printed on receipts, shown in reports. |
| `Role` | `int` | Bit mask of `AccessOption`. |
| `Deleted` | `bit` NOT NULL DEFAULT 0 | Soft delete. |

### `T_TRANSACTIONS` — sale headers

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint IDENTITY` | Primary key. |
| `Factur` | `varchar(20)` | Invoice number = `DateTime.Now.Ticks` (18 digits). Uniquely indexed. |
| `TransactionTime` | `datetime` | Maps to the model property `Time`. |
| `TotalPrice` | `decimal(18,0)` | Gross — sum of line `SubtotalPrice`. |
| `TotalDiscount` | `decimal(18,0)` | Sum of line `SubtotalDiscount`. |
| `Total` | `decimal(18,0)` | Net — amount owed. |
| `Payment` | `decimal(18,0)` | Cash tendered. |
| `Exchange` | `decimal(18,0)` | `Payment - Total`. |
| `Notes` | `varchar(100)` | Free text; revisions get an auto-prefix that consumes much of the 100 chars. |
| `UserId` | `int` | Cashier. `-1` for the built-in admin. |
| `CustomerId` | `bigint` | Always `1` today. |
| `Revision` | `bigint` | `0` active, `> 0` superseded by that `Id`, `-1` cancelled. |

### `T_TRANSACTION_DETAILS` — sale lines

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint IDENTITY` | Primary key. |
| `TransactionId` | `bigint` | Header FK (no DB constraint). Indexed. |
| `ProductId` | `int` | Product FK (no DB constraint). |
| `Quantity` | `int` | Units. |
| `ProductPrice` | `decimal(18,0)` | **Snapshot** of unit price at sale time. |
| `ProductDiscount` | `decimal(18,0)` | **Snapshot** of unit discount *in rupiah* (already resolved from the percentage). |
| `SubtotalPrice` | `decimal(18,0)` | `ProductPrice × Quantity`. |
| `SubtotalDiscount` | `decimal(18,0)` | `ProductDiscount × Quantity`. |
| `Subtotal` | `decimal(18,0)` | `SubtotalPrice - SubtotalDiscount`. |

### `M_SETTINGS` — key/value configuration

| Column | Type | Notes |
|---|---|---|
| `Id` | `int IDENTITY` | Primary key. |
| `Key` | `varchar(80)` NOT NULL | **Reserved SQL word — always bracket as `[Key]`.** |
| `Group` | `varchar(80)` | **Also reserved.** |
| `Value` | `text` | Current value. |
| `Default` | `text` NOT NULL | Seeded default. Never read today. |

### `M_CUSTOMERS` — unused

`Id`, `Name`, `Address`, `Phone`, and a member-type column. See the column-name mismatch note below.

## Constraints and indexes

There are **no primary key, foreign key or unique constraints** in the database — only identity
columns and two `DEFAULT ((0))` constraints on `Deleted`. Referential integrity is maintained by
application code alone.

Three indexes are created at startup by `DBUtility.CheckIndex()`:

| Index | Table | Definition |
|---|---|---|
| `IDX_T_TRANS_TRXTIME` | `T_TRANSACTIONS` | `TransactionTime DESC` — date-range reports |
| `IDX_T_TRANS_FACTUR` | `T_TRANSACTIONS` | `UNIQUE (Factur ASC)` — faktur lookup, and the only uniqueness the DB enforces |
| `IDX_T_TRDETAIL_TRX_ID` | `T_TRANSACTION_DETAILS` | `TransactionId DESC` — loading a sale's lines |

## Money is integral

Every money column is `decimal(18,0)` — **scale 0**. Rupiah has no practical minor unit, so amounts
are whole numbers and SQL Server rounds anything else on write. A percentage discount can produce a
fraction in memory (`Price * (-Discount / 100.0m)`) that is rounded when stored, so
`SubtotalDiscount` in the database can differ slightly from a recomputation in C#. Reports read the
stored values, so they stay self-consistent.

## The hand-rolled ORM

No Entity Framework, no Dapper, no external package. Three pieces:

1. **`IDataTable`** — table name, primary key, column list.
2. **`DataTableList`** — the single registry mapping each model type to its `IDataTable`.
3. **`BaseObject.this[string columnName]`** — a hand-written `switch` on each model, used for both
   reading (`t[col] = reader[col]`) and writing (`dataObject[col]` into the SQL text).

`BaseDao<T>` generates `SELECT` / `INSERT` / `UPDATE` / `DELETE` from that metadata.

### Adding a column — all four steps

1. Add it to the physical table in `Database/DBUtility.cs` (`UpdateTableTransaction`-style guarded
   `ALTER`), and mirror it in `DDL.sql`.
2. Add it to the model's map in `Database/DataTable/DataTableList.cs`.
3. Add the property to the model in `Database/Model/`.
4. Add **both** the `get` and `set` `case` arms to that model's indexer.

Miss step 4 and it fails at runtime with `KeyNotFoundException`, not at compile time. That is the
main fragility of this design — the compiler cannot check the mapping.

### Consequences to know

- `Save` and `Update` write **every** mapped column. Always load → modify → save; never construct a
  partial entity and update it.
- `SCOPE_IDENTITY()` comes back as a `decimal`. `BaseDao.NormalizeIdentity` boxes it as an `int`
  when it fits and a `long` otherwise, because the `int`-keyed models unbox with a direct `(int)`
  cast (which throws on a boxed decimal) while the `bigint`-keyed ones parse whatever they get.
- Values are passed as `SqlParameter`s. String parameters on **indexed** lookups are explicitly
  typed `SqlDbType.VarChar` to match the columns — a default string parameter infers `NVarChar`,
  which makes SQL Server convert the column rather than the value and gives up the seek on
  `IDX_T_TRANS_FACTUR`.
- `CustomQuery` is registered with a **null** `IDataTable` — `CustomDao` projects by reader ordinal
  instead, and overrides every CRUD method to throw.

## Transaction scope

`DBFactory` holds **one process-wide ambient `SqlTransaction`**. Writers use:

```csharp
bool newTransaction = DBFactory.GetInstance().BeginTransaction();
try
{
    // writes
    if (newTransaction) DBFactory.GetInstance().CommitTransaction();
}
catch
{
    if (newTransaction) DBFactory.GetInstance().RollbackTransaction();
    throw;
}
```

`BeginTransaction` returns `false` if one is already open, so inner calls join the outer scope and
only the outermost commits. This is what makes `SaveCompleteTransaction` atomic across the header
and N detail rows.

⚠ The ambient transaction is global, not per-thread. Safe for this single-user WinForms app; a
background writer would corrupt the scope.

**Write failures now propagate.** `DBUtility.ExecuteNonQuery` / `ExecuteScalar` throw; the
best-effort variants `TryExecuteNonQuery` / `TryExecuteScalar` (returning `-1` / `null`) exist only
for schema probing and optional maintenance. Previously *every* write swallowed its exception, so a
detail row that failed to insert was silently dropped while the sale still reported success.

**Connections are closed.** `ExecuteReader` opened a connection when no ambient transaction was
running and never closed it, leaking one per read until the pool was exhausted. It now closes in a
`finally`, and commands and readers are disposed.

## Self-migrating schema

`DBUtility.CheckForDatabaseTable()` runs on every startup:

- `CheckTable()` — `CREATE TABLE` for each of the six tables when absent.
- `UpdateTableTransaction()` — adds `T_TRANSACTIONS.Revision bigint NULL` and backfills `0`;
  widens `Factur` from `varchar(18)` to `varchar(20)`, dropping `IDX_T_TRANS_FACTUR` first.
- `CheckIndex()` — creates the three indexes when absent. Failures are logged, not thrown.

`CheckForDatabaseRow()` seeds the `HEADER` and `FOOTER` settings rows.

**Migrations are guarded, idempotent and additive.** Follow that pattern: check
`IsColumnExist` / `IsIndexExist` / `IsColumnTypeEquals`, then `ALTER`, then backfill. Never drop a
column that existing installations may hold data in.

## Additional migrations

Beyond `Revision` and the `Factur` widening, startup now also applies:

| Step | What it does |
|---|---|
| `T_TRANSACTIONS.CancelledBy int NULL` | Who voided a sale. Existing rows stay NULL. |
| `T_TRANSACTIONS.CancelledAt datetime NULL` | When it was voided. |
| `M_CUSTOMERS.MemberType` | Renames the column from `Type` if that is what exists, otherwise adds it. |

The cancellation columns are deliberately **not** in the `DataTableList` map. They are written by a
targeted `UPDATE` in `TransactionManager.RecordCancellationAudit` using `TryExecuteNonQuery`, so an
installation that has not picked up the columns can still void a sale — the void is what matters,
the audit stamp is extra. Adding them to the map would make every read and write of a transaction
fail on such a database.

## Known data-model issues

**`M_CUSTOMERS` column mismatch — fixed.** `DataTableList` maps `MemberType`; `DBUtility.CheckTable`
used to create the column as `Type`, so reading a `Customer` against an app-created database would
throw. New databases now get `MemberType`, and existing ones are renamed on startup.

**SQL injection — fixed.** Every statement carrying a value now uses `SqlParameter`s: entity
save/update, product search, username lookup, faktur lookup, setting lookup, report date ranges and
the schema probes. Apostrophes in product names no longer break the statement.

⚠ **Identifiers are still concatenated**, because a column name cannot be a parameter. The only
caller-influenced one is the product sort column, which `ProductManager.SanitizeOrderBy` validates
against the mapped column list and drops if unrecognised.

⚠ **Stale DDL comment.** The commented-out block at the bottom of `DataTableList.cs` declares
`CustomerId` twice on `T_TRANSACTIONS` and omits `Revision`. `DBUtility.cs` is the authority.

**`Notes` is `varchar(100)`** and a correction spends about half of it on its automatic prefix.
`CashierManager.TrimNotes` now truncates and logs rather than letting the insert fail.

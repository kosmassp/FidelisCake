# Directory: `InventoryAndSales/Database/`

Namespace `InventoryAndSales.Database`. Owns the SQL Server connection, the ambient transaction,
and boot-time schema maintenance.

See also [../business-data-model.md](../business-data-model.md).

---

## `DBFactory.cs`

`public class DBFactory` — thread-safe lazy singleton. Composition root of the data layer **and**
the holder of the single ambient `SqlTransaction`.

### Construction

Reads `ConfigurationManager.AppSettings["ConnectionString"]`, then instantiates every DAO followed
by every manager, wiring dependencies by constructor:

```
SettingDao → SettingManager
ProductDao → ProductManager ─┐
TransactionDetailDao ────────┴→ TransactionDetailManager ─┐
TransactionDao ───────────────────────────────────────────┴→ TransactionManager
CustomerDao → CustomerManager
CustomDao   → CustomManager
UserDao     → UserManager
```

### Members

| Member | Signature | Purpose |
|---|---|---|
| `GetInstance` | `static DBFactory GetInstance()` | Double-checked-locked singleton accessor. |
| `ProductManager` … `CustomManager` | `public … { get; private set; }` | The seven managers exposed to the business layer. |
| *(DAO properties)* | `private … { get; set; }` | DAOs are deliberately not exposed outside this class. |
| `BeginTransaction` | `bool BeginTransaction()` | Opens a new connection and starts a transaction **only if none is active**. Returns `true` if it started one, `false` if one was already running. Callers use the return value to decide whether they own the commit. |
| `CommitTransaction` | `void CommitTransaction()` | Commits, disposes the transaction and connection, clears both fields. No-op if nothing is active. |
| `RollbackTransaction` | `void RollbackTransaction()` | Rolls back, same cleanup. No-op if nothing is active. |
| `GetConnection` | `SqlConnection GetConnection()` | Returns the active connection if a transaction is running, otherwise a **new** `SqlConnection` (caller opens and closes it). |
| `GetActiveTransaction` | `SqlTransaction GetActiveTransaction()` | The ambient transaction, or `null`. |

### The `BeginTransaction` contract

Every method that writes follows this pattern:

```csharp
bool newTransaction = DBFactory.GetInstance().BeginTransaction();
try
{
    // ... writes ...
    if (newTransaction) DBFactory.GetInstance().CommitTransaction();
}
catch (Exception e)
{
    if (newTransaction) DBFactory.GetInstance().RollbackTransaction();
    throw;
}
```

This gives nesting: an outer caller owns the transaction and inner calls join it. `TransactionManager`
relies on this to save a header plus N detail rows atomically.

⚠ Note: the ambient transaction is process-global, not per-thread or per-unit-of-work. Concurrent
writes from two threads would interleave into one transaction. The app is single-user WinForms, so
this holds in practice; a background writer would break it.

---

## `DBUtility.cs`

`public class DBUtility` — static. Runs at startup from `SplashForm` and provides the two raw SQL
helpers used across the data layer.

### Startup schema maintenance

| Member | Signature | Purpose |
|---|---|---|
| `CheckForDatabaseTable` | `static void CheckForDatabaseTable()` | Orchestrates: `CheckTable()` → `UpdateTableTransaction()` → `UpdateTableCustomer()` → `CheckIndex()`. Logs each phase. |
| `CheckForDatabaseRow` | `static void CheckForDatabaseRow()` | Calls `UpsertSettingRow()`. |
| `CheckTable` | `private static void CheckTable()` | `CREATE TABLE` for `M_SETTINGS`, `M_PRODUCTS`, `M_USERS`, `T_TRANSACTION_DETAILS`, `T_TRANSACTIONS`, `M_CUSTOMERS`, each guarded by `CheckIfTableExist` and each building its **own** `StringBuilder`. |
| `UpdateTableTransaction` | `private static void UpdateTableTransaction()` | Adds `Revision bigint NULL` (backfilled to `0`), `CancelledBy int NULL` and `CancelledAt datetime NULL` when missing; widens `Factur` to `varchar(20)` (dropping `IDX_T_TRANS_FACTUR` first) when the type does not match. |
| `UpdateTableCustomer` | `private static void UpdateTableCustomer()` | Renames `M_CUSTOMERS.Type` to `MemberType` where the old name exists, otherwise adds the column. The application has always mapped `MemberType`, so reading a customer used to fail against an app-created database. |
| `CheckIndex` | `private static void CheckIndex()` | Creates `IDX_T_TRANS_TRXTIME` (TransactionTime DESC), `IDX_T_TRANS_FACTUR` (unique, Factur ASC), `IDX_T_TRDETAIL_TRX_ID` (TransactionId DESC) when absent. Failures are logged, not thrown. |
| `UpsertSettingRow` | `private static void UpsertSettingRow()` | Inserts whichever rows from `SettingKeys.Seed()` the database does not have. Existing rows are untouched, so operator edits survive every upgrade. |
| `CheckIfTableExist` | `private static bool (string tableName)` | `INFORMATION_SCHEMA.TABLES` lookup. |
| `IsColumnExist` | `private static bool (string tableName, string columnName)` | `INFORMATION_SCHEMA.COLUMNS` lookup. |
| `IsColumnTypeEquals` | `private static bool (string tableName, string columnName, string dataType, int charLength = 0)` | Compares `DATA_TYPE`, and `CHARACTER_MAXIMUM_LENGTH` when `charLength > 0`. |
| `IsIndexExist` | `private static bool (string tableName, string indexName)` | `SYS.INDEXES` lookup. |

`CheckTable` previously reused one `StringBuilder` across the first two blocks without clearing it,
so on a fresh database the `M_PRODUCTS` statement was appended to the already-executed `M_SETTINGS`
text. Each block now builds its own.

`CheckTable` also creates `T_TRANSACTIONS.Factur` as `varchar(20)` directly, matching the
18-digit tick counts stored in it; `UpdateTableTransaction` still widens older databases.

The schema probes (`CheckIfTableExist`, `IsColumnExist`, `IsIndexExist`, `IsColumnTypeEquals`) are
parameterised and return a plain `bool` via `TryExecuteScalar`, replacing the earlier
`try`/`catch`-with-dead-assignment shape.

### Raw SQL helpers

| Member | Signature | Purpose |
|---|---|---|
| `ExecuteNonQuery` | `internal static int (string, params SqlParameter[])` | Runs a statement on the ambient transaction if one exists, otherwise opens/closes its own connection. `CommandTimeout = 600`. **Logs and rethrows on error.** |
| `ExecuteScalar` | `internal static object (string, params SqlParameter[])` | Same handling; maps `DBNull` to `null`. **Logs and rethrows.** |
| `TryExecuteNonQuery` | `internal static int (string, params SqlParameter[])` | Best effort — returns `-1` instead of throwing. For schema probing and optional maintenance only. |
| `TryExecuteScalar` | `internal static object (string, params SqlParameter[])` | Best effort — returns `null` instead of throwing. |
| `AddParameters` | `private static void (SqlCommand, SqlParameter[])` | Maps null values to `DBNull.Value`. |

**Write failures now propagate.** Both helpers previously swallowed their exception and returned
`-1` / `null`, so a failed write inside a transaction never triggered the caller's rollback —
`BaseDao.Save` just reported `false` and `SaveCompleteTransaction` carried on and committed, quietly
dropping a detail row from a sale. The `Try*` variants exist so that startup maintenance (creating an
index, probing for a column) can still fail harmlessly without stopping the application from
starting. **Never use them for a write that matters.**

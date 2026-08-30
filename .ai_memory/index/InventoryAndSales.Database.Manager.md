# Directory: `InventoryAndSales/Database/Manager/`

Namespace `InventoryAndSales.Database.Manager`. The repository layer between DAOs and the business
layer. Managers own transaction scope and any cross-entity hydration; DAOs own SQL.

---

## `BaseManager.cs`

`public abstract class BaseManager<T> where T : BaseObject, new()` — generic repository.
Holds `protected BaseDao<T> BaseDao`, constructor-injected.

| Member | Signature | Purpose |
|---|---|---|
| `FindById` | `virtual T FindById(int id)` | Delegates to the DAO. |
| `Save` | `virtual bool Save(T t)` | Wraps the insert in `BeginTransaction` / `CommitTransaction`, rolling back and rethrowing (`throw;`, preserving the stack trace) on error. Only commits if **it** opened the transaction. |
| `Update` | `virtual int Update(T t)` | Delegates. **No transaction wrapper.** |
| `Delete` | `virtual bool Delete(T t)` | Hard delete. Unused by the app. |
| `DeleteById` | `virtual bool DeleteById(int id)` | Hard delete. Unused by the app. |
| `GetAll` | `virtual List<T> GetAll()` | `FindByQuery(string.Empty)` — the whole table, no filter. |

⚠ Note: the class comment `//public abstract class BaseManager<T,V> when we start to use Int65 as Id`
records a known limitation — `FindById` is `int`-only, so `Transaction`/`TransactionDetail`
(`long` ids) are looked up through their own DAO methods instead.

---

## `ProductManager.cs`

`public class ProductManager : BaseManager<Product>`.

| Member | Signature | Purpose |
|---|---|---|
| `GetAllAvailable` | `List<Product> GetAllAvailable(string criteria)` | Delegates with no ordering. |
| `GetAllAvailable` | `List<Product> GetAllAvailable(string criteria, string orderBy)` | `WHERE Name LIKE @criteria AND Deleted = @deleted`. Spaces in `criteria` become `%`, so `"kue coklat"` matches `"kue besar coklat"`. |
| `SanitizeOrderBy` | `private static string (string orderBy)` | A column name cannot be a parameter, so the requested one is matched against the mapped column list and dropped (with a warning) if unrecognised. |

Inherited `GetAll()` returns deleted rows too — that is what `MasterManager.GetAllProduct()` uses.

---

## `UserManager.cs`

`public class UserManager : BaseManager<User>`.

| Member | Signature | Purpose |
|---|---|---|
| `FindByUsername` | `User FindByUsername(string username)` | Non-deleted user by name only, parameterised (`SqlDbType.VarChar(50)`). Returns the row only when **exactly one** matches — duplicates are refused rather than guessed at. |
| `GetAll` | `override List<User> GetAll()` | Excludes soft-deleted users. |

Password checking is **not** here any more. It moved to `LoginManager`, because it has to cope with
two stored hash formats and a per-user salt — neither of which can be expressed as a SQL predicate —
and because the built-in recovery account is an authentication policy, not a data-access concern.
The password no longer appears in any SQL statement. See
[../business-auth-and-roles.md](../business-auth-and-roles.md).

---

## `TransactionManager.cs`

`public class TransactionManager : BaseManager<Transaction>` — the atomic write unit for sales.
Depends on `TransactionDao` and `TransactionDetailManager`.

| Member | Signature | Purpose |
|---|---|---|
| `GetTransaction` | `Transaction GetTransaction(string factur, out List<TransactionDetail> transactionDetails)` | Header by faktur plus its lines (empty list if the header is missing). |
| `SaveCompleteTransaction` | `void SaveCompleteTransaction(Transaction transaction, List<TransactionDetail> details)` | One transaction: insert the header, stamp each detail's `TransactionId` from the generated header id, insert each detail. Rolls back and rethrows on any failure. |
| `UpdateCompleteTransaction` | `void UpdateCompleteTransaction(Transaction original, Transaction transaction, List<TransactionDetail> details)` | Revision: insert the **new** transaction, set `original.Revision = transaction.Id` and update the original, then insert the new details. All in one transaction. |
| `CancelTransaction` | `void CancelTransaction(Transaction original, int cancelledByUserId)` | Sets `Revision = -1`, updates, and stamps the audit columns. No rows are deleted. |
| `RecordCancellationAudit` | `private void (long transactionId, int cancelledByUserId)` | Targeted `UPDATE` setting `CancelledBy` / `CancelledAt`, via `TryExecuteNonQuery`. Deliberately outside the column map and tolerated if it fails, so an installation without those columns can still void a sale. |

`Revision` semantics: `0` = active, `> 0` = superseded (value is the replacement's `Id`),
`-1` = cancelled. Every report filters `Revision = 0`.

---

## `TransactionDetailManager.cs`

`public class TransactionDetailManager : BaseManager<TransactionDetail>`. Depends on
`TransactionDetailDao` and `ProductManager`.

| Member | Signature | Purpose |
|---|---|---|
| `FindByTransactionId` | `internal List<TransactionDetail> FindByTransactionId(long id)` | Loads the lines and hydrates each `ProductName` from `ProductManager.FindById`. `ProductName` is display-only and never persisted. A product that no longer exists falls back to `"Telah Dihapus"` with a warning, rather than throwing `NullReferenceException` and making an old receipt impossible to reprint. |

---

## `CustomManager.cs`

`public class CustomManager : BaseManager<CustomQuery>` — report execution and row shaping.

| Member | Signature | Purpose |
|---|---|---|
| `GetSummaryReportByProduct` | `List<Dictionary<string,string>> (DateTime, DateTime)` | Sales per product per date. |
| `GetReportSummaryByTransaction` | `List<Dictionary<string,string>> (DateTime, DateTime)` | One row per transaction. |
| `GetTransaction` | `List<Dictionary<string,string>> (DateTime, DateTime)` | Browsing view with `Id` and `Factur`. |
| `GetDetailReport` | `List<Dictionary<string,string>> (DateTime, DateTime)` | One row per line. |
| `GetReportSummaryByCashier` | `List<Dictionary<string,string>> (DateTime, DateTime)` | Sales per cashier per date, split by payment method. |
| `GetReportSummaryByPaymentMethod` | `List<Dictionary<string,string>> (DateTime, DateTime)` | Takings per method, terminal/provider/account and code type. |
| `GetTodaySummaryByCashier` | `string (User activeUser, DateTime date)` | Pre-formatted daily total. |
| `ConvertToList` | `private static List<Dictionary<string,string>> (List<CustomQuery>)` | Unwraps each `CustomQuery` into its underlying dictionary. |

Everything downstream (grids, HTML export) consumes `List<Dictionary<string,string>>`, so column
order comes from the SQL `SELECT` order and the headers are the SQL aliases.

---

## `SettingManager.cs`

`public class SettingConfigurationManager : BaseManager<SettingConfiguration>`.

| Member | Signature | Purpose |
|---|---|---|
| `FindByKey` | `List<SettingConfiguration> FindByKey(string key)` | `WHERE [KEY] = @key`. Brackets are required — `KEY` is a reserved word. Callers go through `SettingsService`, which uses `FirstOrDefault` and falls back to a default rather than throwing on a missing row. |

⚠ Note: file name and type name differ (`SettingManager.cs` → `SettingConfigurationManager`). The
type is named for `SettingConfiguration`, which itself was renamed because `Setting` clashed with
an extension method.

---

## `CustomerManager.cs`

`public class CustomerManager : BaseManager<Customer>` — no members beyond the base. Instantiated by
`DBFactory` but **never called**; the customer feature is unfinished (checkout hardcodes
`customerId = 1`).

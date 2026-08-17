# Directory: `InventoryAndSales/Database/DataAccess/`

Namespace `InventoryAndSales.Database.DataAccess`. The DAO layer — the only place that writes SQL.

**Values are passed as `SqlParameter`s**, never concatenated. That keeps a product name containing an
apostrophe from corrupting the statement, closes the injection surface, and lets the driver handle
decimal, bit and datetime conversion instead of relying on the thread culture.

⚠ **Identifiers** (table and column names) are still built into the SQL, because they cannot be
parameters. The only caller-influenced one is the product sort column, validated against the mapped
column list by `ProductManager.SanitizeOrderBy`.

⚠ On indexed lookups, string parameters are explicitly typed `SqlDbType.VarChar` to match the
columns. A default string parameter infers `NVarChar`, which makes SQL Server convert the *column*
rather than the value and gives up the seek — on `IDX_T_TRANS_FACTUR` that would turn every receipt
reprint into a table scan.

---

## `BaseDao.cs`

`public class BaseDao<T> where T : BaseObject, new()` — generic CRUD driven by `IDataTable`
metadata rather than reflection or attributes.

Field: `private readonly IDataTable _dataTable`, resolved in the constructor from
`DataTableList.Instance.GetDataTable(typeof(T))`.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `BaseDao()` | Looks up the table metadata for `T`. |
| `FindById` | `virtual T FindById(int id)` | `WHERE [pk] = @id`; returns the first row or `null`. |
| `FindByQuery` | `virtual List<T> FindByQuery(string whereClause)` | No ordering, no parameters. |
| `FindByQuery` | `virtual List<T> FindByQuery(string whereClause, string orderbyClause)` | Ordered. |
| `FindByQuery` | `virtual List<T> FindByQuery(string whereClause, string orderbyClause, params SqlParameter[] parameters)` | The one to use when the clause carries a value. Prepends `WHERE` / `ORDER BY` if the caller omitted them. |
| `Save` | `virtual bool Save(T dataObject)` | `INSERT INTO <table>(cols) VALUES (@p0, @p1, …)` over every column except the primary key, then reads `SCOPE_IDENTITY()` back into the PK. Returns `rowsAffected > 0`. |
| `Update` | `virtual int Update(T dataObject)` | `UPDATE <table> SET [col]=@pN, … WHERE [pk] = @id` over every non-PK column. |
| `Delete` | `virtual bool Delete(T dataObject)` | **Hard delete** by primary key. Not used — the business layer soft-deletes. |
| `DeleteById` | `virtual bool DeleteById(int id)` | **Hard delete**. Also unused. |
| `ToParameterValue` | `private static object (object)` | Null → empty string, preserving what the old quoted-literal SQL produced. Product `Code` and `Barcode` are read back without null checks in places, so writing NULL instead would be a behaviour change on live data. |
| `NormalizeIdentity` | `private static object (object)` | `SCOPE_IDENTITY()` returns a **decimal**; boxes it as `int` when it fits, else `long`. The int-keyed models unbox with a direct `(int)` cast, which throws on a boxed decimal, while the bigint-keyed ones parse whatever they get. |
| `ExecuteReader` | `protected virtual List<T> (string commandText, params SqlParameter[])` | Materialises one `T` per row from `_dataTable.Columns`, skipping `DBNull`. Logs and **rethrows**. |
| `AddParameters` | `protected static void (SqlCommand, SqlParameter[])` | Maps null values to `DBNull.Value`. |

**Connections are closed.** `ExecuteReader` opened a connection whenever no ambient transaction was
running and never closed it — one leaked per read until the pool was exhausted. It now closes in a
`finally`, and commands and readers are disposed.

Design consequences:

- `_dataTable.Columns` is the contract. A column added to the database but not to `DataTableList`
  is invisible; a column listed there but missing from the database throws at read time.
- `Save`/`Update` write **all** mapped columns, so a partially-populated model overwrites data with
  defaults. Always load-then-modify-then-save.
- `SCOPE_IDENTITY()` is parsed as `int`; `Transaction` and `TransactionDetail` have `long` ids and
  are assigned through their indexer, which does `long.Parse(value.ToString())`.

---

## `CustomDao.cs`

`public class CustomDao : BaseDao<CustomQuery>` — hand-written reporting and browsing SQL.
Its `_dataTable` is `null` by design (registered as `null` in `DataTableList`), so **all inherited
CRUD methods are overridden to throw `NotSupportedException`** — `FindById`, all three `FindByQuery`
overloads, `Save`, `DeleteById`, `Delete`, `Update` — with messages saying why.

| Member | Signature | Purpose |
|---|---|---|
| `GetReportSummaryByProduct` | `List<CustomQuery> (DateTime start, DateTime stop)` | Per product per date: transaction count, quantity sold, gross, discount, net. |
| `GetReportSummaryByTransaction` | `List<CustomQuery> (DateTime, DateTime)` | One row per transaction: cashier, faktur, date, total, notes, payment, change. |
| `GetReportSummaryByUserId` | `List<CustomQuery> (DateTime, DateTime)` | Per cashier per date: distinct transaction count, quantity, gross, discount, net. |
| `GetReportDetailByTime` | `List<CustomQuery> (DateTime, DateTime)` | One row per transaction line, ordered by time. |
| `GetTransaction` | `List<CustomQuery> (DateTime, DateTime)` | Browsing view: cashier, `Id`, `Factur`, timestamp, total, notes. Backs the transaction picker. |
| `GetTodaySummaryByCashier` | `string (User activeUser, DateTime date)` | `SUM(Total)` for one user on one date, returned pre-formatted as `"Rp. {n}"`, or `"Rp. 0"` when empty. |
| `ExecuteReader` | `protected override List<CustomQuery> (string, params SqlParameter[])` | Overridden to project **by reader ordinal** (`reader.GetName(i)`) rather than by a fixed column list, because every report returns a different shape. |

### Shared SQL conventions in this file

- Every report filters `WHERE t.Revision = 0` — only *active* transactions. Superseded and
  cancelled rows are excluded. See [../business-transaction-revision.md](../business-transaction-revision.md).
- Date filtering is the shared `DATE_RANGE` fragment —
  `AND t.TransactionTime >= @start AND t.TransactionTime < @stop` — with parameters built by
  `DateRange(start, stop)` as `[start.Date, stop.Date + 1 day)`. Comparing the raw column keeps
  `IDX_T_TRANS_TRXTIME` seekable, and passing parameters removes the culture dependency the old
  `CAST(... AS date) BETWEEN '{ToShortDateString()}'` form had. Reversed dates fall back to a single
  day.
- Deleted products render as `COALESCE(p.Name,'Telah Dihapus')`; the built-in admin account (no
  `M_USERS` row) renders as `COALESCE(u.Name,'ADMIN')`.
- Column aliases are Indonesian (`Kasir`, `Jumlah Transaksi`, `Total Diskon`) because they become
  the grid and HTML report headers verbatim.

---

## `TransactionDao.cs`

`public class TransactionDao : BaseDao<Transaction>`.

| Member | Signature | Purpose |
|---|---|---|
| `FindByFactur` | `Transaction FindByFactur(string factur)` | `WHERE Factur = @factur`, first row or `null`. The parameter is typed `SqlDbType.VarChar(20)` to match the column, so the unique index stays seekable. |

---

## `TransactionDetailDao.cs`

`public class TransactionDetailDao : BaseDao<TransactionDetail>`.

| Member | Signature | Purpose |
|---|---|---|
| `FindByTransactionId` | `List<TransactionDetail> FindByTransactionId(long id)` | All lines for a transaction header, `WHERE TransactionId = @transactionId`. Indexed by `IDX_T_TRDETAIL_TRX_ID`. |

---

## `ProductDao.cs`, `UserDao.cs`, `CustomerDao.cs`, `SettingDao.cs`

Empty subclasses that exist only to bind a model type to `BaseDao<T>`:

| File | Class | Model |
|---|---|---|
| `ProductDao.cs` | `ProductDao : BaseDao<Product>` | `M_PRODUCTS` |
| `UserDao.cs` | `UserDao : BaseDao<User>` | `M_USERS` |
| `CustomerDao.cs` | `CustomerDao : BaseDao<Customer>` | `M_CUSTOMERS` |
| `SettingDao.cs` | `SettingConfigurationDao : BaseDao<SettingConfiguration>` | `M_SETTINGS` |

⚠ Note: `SettingDao.cs` declares `SettingConfigurationDao` — file name and type name differ.

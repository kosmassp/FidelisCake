# Directory: `InventoryAndSales/Database/DataTable/`

Namespace `InventoryAndSales.Database.DataTable`. Table metadata — the object/relational map.

⚠ Note: the namespace segment **and** the class `DataTable` collide with `System.Data.DataTable`.
Files needing the ADO.NET type must not import this namespace, or must alias. `GUI/Util/DataTableUtil.cs`
returns `System.Data.DataTable` and deliberately sits in namespace `InventoryAndSales.GUI`.

---

## `IDataTable.cs`

`public interface IDataTable` — the contract `BaseDao<T>` programs against.

| Member | Type | Purpose |
|---|---|---|
| `TableName` | `string { get; }` | Physical table name, e.g. `M_PRODUCTS`. |
| `PrimaryKeyColumn` | `string { get; }` | Identity column name, always `Id` today. |
| `Columns` | `List<string> { get; }` | Every mapped column, primary key first. |

---

## `DataTable.cs`

`public class DataTable : IDataTable` — immutable implementation.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `DataTable(string tableName, string primaryKey, params string[] columns)` | Stores the name and PK, and builds `Columns` as `[primaryKey] + columns`. |
| `TableName` / `PrimaryKeyColumn` / `Columns` | `{ get; private set; }` | Set once at construction. |

---

## `DataTableList.cs`

`class DataTableList` (internal) — thread-safe singleton holding **the single source of truth** for
which columns the application maps for each model type.

| Member | Signature | Purpose |
|---|---|---|
| `Instance` | `static DataTableList { get; }` | Lock-guarded lazy singleton. |
| `GetDataTable` | `IDataTable GetDataTable(Type type)` | Model type → table metadata. Throws `KeyNotFoundException` for an unregistered type. |

### The registered maps

| Model | Table | Mapped columns (PK first) |
|---|---|---|
| `Product` | `M_PRODUCTS` | `Id`, `Code`, `Barcode`, `Name`, `Price`, `Discount`, `Deleted` |
| `User` | `M_USERS` | `Id`, `Username`, `Password`, `Name`, `Role`, `Deleted` |
| `Transaction` | `T_TRANSACTIONS` | `Id`, `Factur`, `TotalPrice`, `TotalDiscount`, `Total`, `Notes`, `UserId`, `TransactionTime`, `Payment`, `Exchange`, `CustomerId`, `Revision` |
| `TransactionDetail` | `T_TRANSACTION_DETAILS` | `Id`, `ProductId`, `Quantity`, `ProductDiscount`, `ProductPrice`, `SubtotalDiscount`, `SubtotalPrice`, `Subtotal`, `TransactionId` |
| `Customer` | `M_CUSTOMERS` | `Id`, `Name`, `Address`, `Phone`, `MemberType` |
| `SettingConfiguration` | `M_SETTINGS` | `Id`, `Key`, `Group`, `Value`, `Default` |
| `CustomQuery` | *(null)* | Deliberately unmapped — `CustomDao` projects by reader ordinal instead. |

### Adding a column — the four places to touch

1. The physical table (`Database/DBUtility.cs` migration block, and `DDL.sql` for reference).
2. The map in this file.
3. The model's property in `Database/Model/`.
4. Both `case` arms of that model's `this[string columnName]` indexer.

Miss step 4 and reads throw `KeyNotFoundException` at runtime, not at compile time.

⚠ Note: `M_CUSTOMERS` is mapped with column `MemberType`, but `DBUtility.CheckTable` creates the
column as `Type`. Reading a `Customer` against a database created by the app therefore fails. The
`DDL.sql` reference schema uses `MemberType`. `Customer` is not read anywhere today
(`CustomerManager` is constructed but never called), so this is latent.

⚠ Note: the file ends with a large commented-out block of legacy DDL. It is stale — the
`T_TRANSACTIONS` copy there declares `CustomerId` twice and omits `Revision`. Use
`Database/DBUtility.cs` as the authority.

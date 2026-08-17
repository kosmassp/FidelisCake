# Directory: `InventoryAndSales/Database/Model/`

Namespace `InventoryAndSales.Database.Model`. Plain entity classes. Each maps 1:1 to a table
registered in `Database/DataTable/DataTableList.cs`.

## The indexer pattern

Every model derives from `BaseObject` and implements `object this[string columnName] { get; set; }`
as a hand-written `switch`. `BaseDao<T>` uses it for both directions:

- **read** — `t[columnName] = reader[columnName]` for each mapped column
- **write** — `dataObject[column]` interpolated into the `INSERT` / `UPDATE`

This avoids reflection but means **the `switch` is the mapping**. A property with no `case` arm is
invisible to persistence; a mapped column with no `case` arm throws `KeyNotFoundException` at
runtime. Both `get` and `set` arms must be added together.

Properties marked `[Browsable(false)]` are hidden from the WinForms `DataGridView` when the model
is bound directly (product and user master pages bind `List<T>`).

---

## `BaseObject.cs`

`public abstract class BaseObject` — the contract.

| Member | Signature | Purpose |
|---|---|---|
| `this[string columnName]` | `[IndexerName("DataColumn")] abstract object { get; set; }` | Column name → value accessor. `IndexerName` renames the generated `Item` property to `DataColumn` to avoid conflicting with other members. |

---

## `Product.cs`

`public class Product : BaseObject` — a row of `M_PRODUCTS`. **Owns the pricing rules.**

| Member | Type | Purpose |
|---|---|---|
| `Id` | `int` | Identity. `[Browsable(false)]` |
| `Code` | `string` | Short human code, e.g. `KC001`. Searchable by prefix. |
| `Barcode` | `string` | Scanner code. Exact-match search. Optional. |
| `Name` | `string` | Display name. |
| `Price` | `decimal` | List price before discount. |
| `Discount` | `decimal` | **Sign-encoded.** `> 0` = flat rupiah amount; `< 0` = percentage (stored negated); `0` = none. `[Browsable(false)]` |
| `Deleted` | `bool` | Soft-delete flag. `[Browsable(false)]` |
| `DisplayDiscount` | `string { get; }` | `[DisplayName("Discount")]`. Renders `"15 %"` for percentage, otherwise the capped amount formatted with `Constant.DISPLAY_CURRENCY`. |
| `DiscountAmount` | `decimal { get; }` | **Resolved discount in rupiah.** `Discount < 0` → `Price * (-Discount / 100)`; then `Math.Min(Price, discount)` so a discount can never exceed the price. |
| `NetPrice` | `decimal { get; }` | `Price - DiscountAmount`. |

Constructors: `Product()` and `Product(string code, string barcode, string name, decimal price, decimal discount, bool deleted)`.

Mapped columns: `Id`, `Code`, `Barcode`, `Name`, `Price`, `Discount`, `Deleted`.

⚠ Note: `DisplayDiscount` returns `Math.Min(Price, Discount)` for the flat case without going
through `DiscountAmount`, so the two are consistent only because both clamp the same way.

---

## `TransactionDetail.cs`

`public class TransactionDetail : BaseObject` — a row of `T_TRANSACTION_DETAILS`. **Owns the line
subtotal rules.**

| Member | Type | Purpose |
|---|---|---|
| `Id` | `long` | Identity. |
| `ProductId` | `int` | FK to `M_PRODUCTS` (no DB constraint). |
| `ProductPrice` | `decimal` | Unit price **snapshotted at sale time**. |
| `ProductDiscount` | `decimal` | Unit discount in rupiah, snapshotted from `Product.DiscountAmount`. |
| `ProductName` | `string` | **Not persisted.** Display only; hydrated by `TransactionDetailManager.FindByTransactionId`. |
| `Quantity` | `int` | Units sold. |
| `SubtotalPrice` | `decimal` | `ProductPrice * Quantity` — gross. |
| `SubtotalDiscount` | `decimal` | `ProductDiscount * Quantity`. |
| `Subtotal` | `decimal` | `SubtotalPrice - SubtotalDiscount` — net. |
| `TransactionId` | `long` | FK to `T_TRANSACTIONS`, assigned during `SaveCompleteTransaction`. |

| Method | Signature | Purpose |
|---|---|---|
| *(ctor)* | `TransactionDetail(Product product, int quantity)` | Snapshots name, id, `DiscountAmount`, `Price`, then calls `UpdateQuantity`. |
| `UpdateQuantity` | `void UpdateQuantity(int quantity)` | Clamps negatives to `0` and recalculates all three subtotals. **The only correct way to change quantity.** |

Prices are snapshotted, so changing a product's price later does not rewrite history.

Mapped columns: `Id`, `ProductId`, `Quantity`, `ProductDiscount`, `ProductPrice`,
`SubtotalDiscount`, `SubtotalPrice`, `Subtotal`, `TransactionId` — note `ProductName` is absent.

---

## `Transaction.cs`

`public class Transaction : BaseObject` — a row of `T_TRANSACTIONS`, the sale header.

| Member | Type | Purpose |
|---|---|---|
| `Id` | `long` | Identity. |
| `Factur` | `string` | Invoice number — `DateTime.Now.Ticks`, uniquely indexed. |
| `Time` | `DateTime` | Maps to column **`TransactionTime`** (property and column names differ). |
| `TotalPrice` | `decimal` | Sum of line `SubtotalPrice` — gross. |
| `TotalDiscount` | `decimal` | Sum of line `SubtotalDiscount`. |
| `Total` | `decimal` | Sum of line `Subtotal` — net, the amount owed. |
| `Payment` | `decimal` | Cash tendered. |
| `Exchange` | `decimal` | `Payment - Total` — change given. |
| `Notes` | `string` | Free text; revisions get an auto-prefix. `varchar(100)`. |
| `UserId` | `int` | Cashier. `-1` for the built-in admin account. |
| `CustomerId` | `long` | Currently always hardcoded to `1`. |
| `Revision` | `long` | `0` active, `> 0` superseded by that `Id`, `-1` cancelled. |

`Id`, `CustomerId` and `Revision` parse via `long.Parse(value.ToString())` in the indexer because
SQL Server may hand back `int` or `decimal` depending on the column type.

---

## `User.cs`

`public class User : BaseObject` — a row of `M_USERS`.

| Member | Type | Purpose |
|---|---|---|
| `Id` | `int` | Identity. `[Browsable(false)]` |
| `Username` | `string` | Login name. |
| `Password` | `string` | SHA-512 hash as `BitConverter.ToString` hex with dashes. `[Browsable(false)]` |
| `Name` | `string` | Display name; printed on receipts and shown in reports. |
| `Role` | `int` | Bit mask of `AccessOption`. `[Browsable(false)]` |
| `RoleOption` | `RoleOptions { get; }` | `Role` cast to the enum, for the combo box. |
| `Deleted` | `bool` | Soft-delete flag. `[Browsable(false)]` |

Constructors: `User()`, `User(username, password, name, role, deleted)`,
`User(id, username, password, name, role, deleted)`.

Only `Username`, `Name` and `RoleOption` are `Browsable`, so the user master grid shows exactly
those three columns.

---

## `SettingConfiguration.cs`

`public class SettingConfiguration : BaseObject` — a row of `M_SETTINGS`, the key/value store.

| Member | Type | Purpose |
|---|---|---|
| `Id` | `int` | Identity. |
| `Key` | `string` | Setting name, e.g. `HEADER`, `FOOTER`. Reserved SQL word — always bracket it. |
| `Group` | `string` | Category, e.g. `GENERAL`. Also a reserved word. |
| `Value` | `string` | Current value (`text` column). |
| `Default` | `string` | Seeded default, for reset. Never read today. |

Named `SettingConfiguration` rather than `Setting` — see the source comment: "Name setting is
conflicted with extension."

---

## `Customer.cs`

`public class Customer : BaseObject` — a row of `M_CUSTOMERS`. Fields: `Id`, `Name`, `Address`,
`Phone`, `MemberType (int)`.

⚠ Note: unused. `CustomerManager` is wired up but never called, and checkout hardcodes
`customerId = 1`. Also see the `MemberType` vs `Type` column mismatch documented in
[InventoryAndSales.Database.DataTable.md](InventoryAndSales.Database.DataTable.md).

---

## `CustomQuery.cs`

`public class CustomQuery : BaseObject` — dynamic row bag for report queries, backed by
`Dictionary<string,string>`.

| Member | Signature | Purpose |
|---|---|---|
| `this[string columnName]` | `override object { get; set; }` | Getter returns the stored string. **The setter formats as it stores:** a `DateTime` with zero time-of-day → `"dd MMM yyyy"`, otherwise → `"dd MMM yyyy HH:mm:ss"`; anything else → `value.ToString()`. |
| `GetDict` | `Dictionary<string,string> GetDict()` | Exposes the backing dictionary to `CustomManager`. |

Because formatting happens at read time, report dates are already display-ready by the time they
reach a grid or the HTML exporter — and are strings, so grids sort them lexically, not
chronologically.

⚠ Note: the setter dereferences `value.ToString()` without a null guard; `CustomDao.ExecuteReader`
protects it by skipping `DBNull` columns.

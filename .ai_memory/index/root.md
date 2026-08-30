# Directory: `/` (solution root)

Repository root. No C# source lives here.

| File | Purpose |
|---|---|
| `InventoryAndSalesProject.sln` | Visual Studio solution. Contains `InventoryAndSales` (WinExe) and `SimpleCommon` (Library). |
| `DDL.sql` | Reference dump of the SQL Server schema for database `SalesInventory`. UTF-16 encoded. **Documentation only** — the app creates its own tables at startup via `Database/DBUtility.cs`, so this file can drift. |
| `.gitignore` | Copied from an unrelated project (`RockImager`, `FrapImageProcessor` rules are stale). The useful parts are `bin/`, `obj/`, `Debug/`, `Release/`, `*.suo`, `*.user`. |
| `temp.zip` | 5 MB untracked archive sitting in the working tree. Not referenced by the build. |
| `.vs/` | Visual Studio local state. Untracked. |

## `DDL.sql` contents

Creates six tables, matching what `DBUtility.CheckTable()` creates:

| Table | Primary key | Notes |
|---|---|---|
| `M_CUSTOMERS` | `Id int IDENTITY` | `Name`, `Address`, `Phone`, `MemberType` |
| `M_PRODUCTS` | `Id int IDENTITY` | `Code`, `Name`, `Price`, `Discount`, `Deleted` (default 0), `Barcode` |
| `M_SETTINGS` | `Id int IDENTITY` | `Key`, `Group`, `Value`, `Default` |
| `M_USERS` | `Id int IDENTITY` | `Username`, `Role`, `Deleted` (default 0), `Name`, `Password` |
| `T_TRANSACTIONS` | `Id bigint IDENTITY` | Totals, `Notes`, `TransactionTime`, `Payment`, `Exchange`, `UserId`, `CustomerId`, `Factur varchar(20)`, `Revision` |
| `T_TRANSACTION_DETAILS` | `Id bigint IDENTITY` | `ProductId`, `Quantity`, prices, `Subtotal`, `TransactionId` |

⚠ Note: the DDL declares no `PRIMARY KEY` or `FOREIGN KEY` constraints — only the identity
columns and two `DEFAULT ((0))` constraints on `Deleted`. Indexes are added at runtime by
`DBUtility.CheckIndex()`.

See [../business-data-model.md](../business-data-model.md) for the full column-by-column story.

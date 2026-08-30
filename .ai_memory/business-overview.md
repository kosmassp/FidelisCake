# Business Overview

## What this is

A single-terminal **point-of-sale and inventory application** for **Fidelis Cake and Bakery**
(Jl. Mayjend Sutoyo No. 1, Banjarnegara). Windows desktop, C# WinForms, SQL Server backend, thermal
receipt printer.

The daily job it does: a cashier signs in, searches or scans products, builds a cart, takes cash,
prints a receipt. Supervisors correct or void past sales. Admins manage products and users and pull
sales reports.

All user-facing text is **Indonesian**. "Faktur" (spelled `Factur` in code) = invoice number,
"Kasir" = cashier, "Kembalian" = change, "Nota" = receipt, "Ralat" = correction, "Laporan" = report.

## Scope

| In scope | Not in scope |
|---|---|
| Cash sales | Card, transfer or split payment |
| Per-product discount (flat or percentage) | Transaction-level discount, promotions, vouchers |
| Product catalogue with barcode | Stock levels / quantity on hand |
| Cashier attribution and daily totals | Shift management, cash drawer reconciliation |
| Sale correction and cancellation | Partial refund, returns |
| Sales reports by product / transaction / cashier | Purchasing, suppliers, cost of goods |
| Named users with role bits | Multi-store, multi-terminal, audit trail |

⚠ Despite the project name **InventoryAndSales**, there is no inventory tracking. `M_PRODUCTS` has
no quantity column and nothing decrements stock on sale. A `NewCart` comment
(`//TODO: Invoke clear cart`) hints at an abandoned intent.

Two features are wired but unfinished: **customers** (`M_CUSTOMERS`, `CustomerManager` — checkout
hardcodes `customerId = 1`) and **promotions** (`PromoPage` exists but is unreachable).

## Architecture

Four layers, each depending only on the one below.

```
      GUI/Page, GUI/Popup            views — WinForms, formatting and input only
              │
      GUI/Controller                 orchestration + input validation
              │
      Business/                      domain rules — cart, pricing, checkout, receipts
              │
      Database/Manager               repositories — transaction scope, hydration
              │
      Database/DataAccess (DAO)      SQL
              │
      SQL Server (SalesInventory)
```

`Database/Model` entities cross every layer — the same `Product` instance is bound to a grid,
carried through a controller and written by a DAO. There is no separate DTO tier.

### Composition roots

Two singletons wire everything by constructor injection:

- **`DBFactory`** (`Database/DBFactory.cs`) — connection string, seven DAOs, seven managers, and
  the single ambient `SqlTransaction`.
- **`BusinessFactory`** (`Business/BusinessFactory.cs`) — five business managers, built from
  `DBFactory`'s managers.

Every controller starts with `BusinessFactory.GetInstance().SomeManager`. That call is the only
service-locator lookup in the codebase; everything below it is injected.

### View ↔ controller contract

Each page constructs its own controller and passes `this`. The controller calls back into public
`Update*` / `Reset*` methods on the view, all of which begin with the `InvokeRequired` →
`BeginInvoke` marshalling guard (see `SimpleCommon/Utility/DelegateUtility.cs`).

The cart uses an **event** rather than direct calls: `CashierManager.CartChange` fires on every
mutation, the controller handles it and pushes the new line and total into the grid. One code path
updates the display, whether the change came from a click, a `+` key or a barcode scan.

## Application startup

1. `Program.Main` — pins culture to `en-US`, hooks the unhandled-exception logger.
2. `SplashForm` runs on a background thread:
   - `DBUtility.CheckForDatabaseTable()` — create missing tables, add `Revision`, widen `Factur`,
     create three indexes.
   - `DBUtility.CheckForDatabaseRow()` — seed the `HEADER` and `FOOTER` receipt settings.
3. On success `MainForm` opens on the login page. On failure the app logs and exits with code 1.

**The application creates and migrates its own schema.** `DDL.sql` is reference material only.

## Session flow

```
LoginPage ──Login()──► LoginManager.ActiveUser set
                            │ OnActiveUserChanged
                            ▼
                  MainFormController
                            │
      EnableMenu(role) ─────┴───── LoadCashierPage()
```

Logging out sets `ActiveUser = null` and returns to the login page. Menu visibility is recomputed
from the role bits on every change.

## Key invariants

1. **Nothing is ever hard-deleted.** Products and users carry a `Deleted` flag; transactions carry
   `Revision`. Historic sales always resolve their product and cashier names.
2. **Prices are snapshotted at sale time.** `TransactionDetail` stores `ProductPrice` and
   `ProductDiscount` as of the sale, so repricing a product never rewrites history.
3. **A transaction header and its details are written in one database transaction**, or not at all.
4. **Reports only see `Revision = 0`.** Superseded and cancelled sales are invisible to every
   report.
5. **Culture is `en-US` everywhere.** Amount parsing, amount display and the date literals embedded
   in report SQL all depend on it.
6. **Receipt layout is one static pure function** — `CashierManager.GeneratePaymentNote` — so the
   settings preview and the printer can never disagree.

## Where to read next

| Topic | Page |
|---|---|
| Tables, columns, the ORM, migrations | [business-data-model.md](business-data-model.md) |
| Login, hashing, roles, step-up authorisation | [business-auth-and-roles.md](business-auth-and-roles.md) |
| Products, codes, discounts, CSV | [business-product-master.md](business-product-master.md) |
| Cart mechanics and price formulas | [business-cart-and-pricing.md](business-cart-and-pricing.md) |
| Checkout validation and persistence | [business-checkout.md](business-checkout.md) |
| Correcting and voiding sales | [business-transaction-revision.md](business-transaction-revision.md) |
| Receipt layout and printing | [business-receipt-printing.md](business-receipt-printing.md) |
| Reports | [business-reporting.md](business-reporting.md) |
| Settings store | [business-settings.md](business-settings.md) |
| Coding conventions for new work | [rules-csharp.md](rules-csharp.md) |

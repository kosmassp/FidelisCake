# .ai_memory

Durable knowledge base for the **FidelisCake / InventoryAndSales** point-of-sale application.
Written for AI agents and new developers so they can locate code and understand rules without
re-reading the whole tree.

## Layout

| Path | Purpose |
|---|---|
| [`_index.md`](_index.md) | Master index. Every C# file in the solution, one line each, with a pointer to its directory page. **Start here when looking for a file.** |
| [`index/`](index/) | One markdown per source directory. Lists every file in that directory with its types, methods/procedures and their purpose. |
| `business-*.md` | The business logic: domain rules, formulas, workflows, invariants. Read these before changing behaviour. |
| [`rules-csharp.md`](rules-csharp.md) | Coding rules: namespaces, layering, SOLID/KISS as applied here, the framework-only dependency policy. **Read before writing code.** |

## Business logic pages

| Page | Covers |
|---|---|
| [business-overview.md](business-overview.md) | What the product is, the layer architecture, startup sequence, wiring |
| [business-data-model.md](business-data-model.md) | Tables, columns, the hand-rolled ORM, self-migration on boot |
| [business-auth-and-roles.md](business-auth-and-roles.md) | Login, password hashing, role bit flags, menu gating, supervisor step-up |
| [business-product-master.md](business-product-master.md) | Product CRUD, code generation, discount encoding, CSV import/export |
| [business-cart-and-pricing.md](business-cart-and-pricing.md) | Cart state, quantity rules, price/discount/subtotal formulas |
| [business-checkout.md](business-checkout.md) | Checkout validation, faktur generation, transaction persistence |
| [business-transaction-revision.md](business-transaction-revision.md) | Revising and cancelling transactions, the `Revision` column semantics |
| [business-receipt-printing.md](business-receipt-printing.md) | Receipt layout, printer selection, reprint paths |
| [business-reporting.md](business-reporting.md) | Report queries, in-grid vs HTML output, daily cashier total |
| [business-settings.md](business-settings.md) | `M_SETTINGS` key/value store, receipt header & footer editing |

## Writing code

[rules-csharp.md](rules-csharp.md) is the standard for new and modified code. In short:

- **Namespace = assembly name + folder path.** File name = type name. Never name a namespace segment
  or type after a framework type (`Enum`, `DataTable`, `Type`, …) — this codebase already pays for
  two such collisions.
- **No external libraries.** .NET Framework 4.6 only, plus the pre-existing `log4net`. The
  deployment is a copied folder on a shop PC; every added DLL is weight.
- **Respect the layering** — a view never touches the database, SQL lives only in
  `Database/DataAccess`.
- **SOLID and KISS** as applied to the real classes here, with the existing violations named so they
  are not copied.

## Conventions used in these documents

- File references are repo-relative, e.g. `InventoryAndSales/Business/CashierManager.cs`.
- User-facing strings in the app are **Indonesian**; this documentation is in English and quotes the
  Indonesian text where it matters (e.g. "Faktur", "Kasir", "Kembalian").
- "Faktur" (spelled `Factur` in code and DB) means *invoice / receipt number*.
- Quirks and latent bugs are called out inline under a **⚠ Note** marker. They document current
  behaviour — they are not a change request.

## Maintenance

When you add, rename or delete a C# file:

1. Update the row in [`_index.md`](_index.md).
2. Update the matching page in [`index/`](index/).
3. If the change alters a rule or formula, update the relevant `business-*.md` page.

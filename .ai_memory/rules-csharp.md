# C# Rules for This Codebase

Binding conventions for new and modified code in **FidelisCake / InventoryAndSales**.
Target: C# on **.NET Framework 4.6**, WinForms, SQL Server.

The goal is code that is **extensible, maintainable and reusable**, kept honest by **KISS** and
**SOLID**, with **no external dependencies**. Where the existing code disagrees with a rule, the
rule wins for new work; old code is corrected when you are already touching it, not in a sweeping
rewrite.

---

## 1. Dependencies — framework only

**Rule: do not add NuGet packages or third-party DLLs.** Use only what ships with .NET Framework 4.6
and is already on the machine. The deployment is a copied folder on a shop PC; every added file is
weight to install, to update and to get wrong.

Currently referenced, and the complete allowed set: `System`, `System.Core`, `System.Data`,
`System.Drawing`, `System.Windows.Forms`, `System.Web` (for `HtmlTextWriter`), `System.configuration`,
`System.Xml`, `System.Xml.Linq`, `System.Data.DataSetExtensions`, `System.Deployment`, plus the one
pre-existing exception: **`log4net`** (already shipped — keep using it, do not replace it).

Reach for the framework first:

**The one sanctioned exception is an ADO.NET provider.** PostgreSQL and SQLite are not in the
framework, so `Database/Dialect/SqlDialectFactory.cs` resolves the provider at runtime through
`DbProviderFactories` and the project references neither. The shipped binary stays framework-only;
a site that wants one installs it and edits `App.config`. Follow that pattern rather than adding a
reference.

| Temptation | Use instead |
|---|---|
| Dapper / EF / an ORM | The existing `BaseDao<T>` + `DataTableList` map, or `DbCommand` directly |
| Newtonsoft.Json | `System.Runtime.Serialization.Json`, or don't serialise |
| CsvHelper | `MasterProductPage.ParseCsvLine` already exists — extract and reuse it |
| A PDF/report library | `HtmlReportGenerator` writes HTML the browser prints |
| A DI container | Constructor injection through `DBFactory` / `BusinessFactory`, as today |
| A hashing library | `System.Security.Cryptography` (`SHA512`, `Rfc2898DeriveBytes`) |
| AutoMapper | Write the mapping; it is five lines and it is readable |

If a task genuinely cannot be done with the framework, raise it before adding the reference —
the answer is a decision, not a default.

**Corollary:** `SimpleCommon` must not reference `InventoryAndSales`. It is the reusable half; the
moment it knows about products or transactions, it stops being reusable.

---

## 2. Namespaces

**Rule: namespace = `<AssemblyName>` + folder path, exactly.**

```
InventoryAndSales/Business/CashierManager.cs      → namespace InventoryAndSales.Business
InventoryAndSales/GUI/Controller/SettingPage/…    → namespace InventoryAndSales.GUI.Controller.SettingPage
SimpleCommon/UI/ComponentWinForm/SplitButton.cs   → namespace SimpleCommon.UI.ComponentWinForm
```

One namespace per file. One public type per file. **File name = type name** — no exceptions.

### Never name a namespace segment or type after a framework type

This codebase already pays for two violations:

| Existing collision | Cost |
|---|---|
| `InventoryAndSales.Business.Enum` vs `System.Enum` | Any file needing `Enum.GetValues` must avoid importing it |
| `InventoryAndSales.Database.DataTable` (namespace **and** class) vs `System.Data.DataTable` | `DataTableUtil.cs` sits in the wrong namespace purely to escape it |

Banned as segment or type names: `Enum`, `DataTable`, `Type`, `Object`, `String`, `Task`, `Timer`,
`Path`, `Console`, `Convert`, `Math`, `Version`, `Environment`. Prefer a plural or a qualifier:
`Enums`, `TableMaps`, `ProductTableMap`.

### Existing exceptions — do not copy them

| File | Actual namespace | Expected |
|---|---|---|
| `InventoryAndSales/Utility/Constant.cs` | `InventoryAndSales.GUI.Utility` | `InventoryAndSales.Utility` |
| `InventoryAndSales/GUI/Util/DataTableUtil.cs` | `InventoryAndSales.GUI` | `InventoryAndSales.GUI.Util` |
| `InventoryAndSales/GUI/Popup/TransactionHistory.cs`, `ReprintReceipt.cs` | `InventoryAndSales.GUI` | `InventoryAndSales.GUI.Popup` |

Fix these only as part of work already touching the file — a namespace change is a cross-file edit.

### Name/file mismatches to be aware of

`SettingDao.cs` → `SettingConfigurationDao` · `SettingManager.cs` → `SettingConfigurationManager` ·
`HeaderAndFooter.cs` → `HeaderAndFooterForm` (a `UserControl`, despite `Form`) ·
`SearcheableDataView.cs` → `SearchableDataView<T>` (misspelled file) ·
`ViewCartModel.cs` → `ViewItemMaster` (dead code).

New code does not add to this list.

### `using` order

Framework (`System.*`) first, then `InventoryAndSales.*`, then `SimpleCommon.*`. Delete unused
directives — most files here carry a copy-pasted block including `System.Security.Cryptography` in
classes that do no cryptography.

---

## 3. Layering

```
GUI/Page, GUI/Popup   →  GUI/Controller  →  Business  →  Database/Manager  →  Database/DataAccess
```

**Each layer may call only the layer directly below it.**

| Rule | Meaning |
|---|---|
| A view never touches `Database.*` | No SQL, no DAO, no manager in a `Form` or `UserControl` |
| A controller never touches a DAO | Go through `Business` or `Database.Manager` |
| `Business` never touches WinForms | The one tolerated exception is `System.Drawing` types in receipt layout |
| SQL exists only in `Database/DataAccess` | Nowhere else, ever |

Views format and collect input. Controllers validate and orchestrate. `Business` owns the rules.
Managers own transaction scope. DAOs own SQL.

⚠ Known breach to *not* imitate: `AuthenticationForm` performs the authorisation decision inside the
form (its own comments say `//should not be in the UI form;`), and `BusinessUtil.AllowedRole` — an
authorisation rule — lives in `GUI/Util`.

---

## 4. SOLID, applied here

### Single responsibility

A class has one reason to change. `CashierManager` used to own the cart, checkout, revision,
cancellation **and** receipt layout; the cart is now `Cart` and the layout is `ReceiptBuilder`.
Keep going in that direction rather than adding to what is left.

Practical test: if you cannot name the class without "and", split it.

### Open/closed

Extend without editing existing code. The codebase already does this well in two places — copy them:

- **Adding a settings page:** write a `UserControl` + controller, register one line in
  `SettingForm.Initialize()`. Nothing existing changes.
- **Adding a sortable column:** add it to `DataTableList`; the sort combo box picks it up because it
  reads the column map.

Prefer registration and configuration over `switch` statements that grow with each feature.

### Liskov substitution

A subclass must be usable wherever its base is. ⚠ `CustomDao : BaseDao<CustomQuery>` still violates
this — it overrides every inherited CRUD method to throw `NotSupportedException`. That is the signal
that it never was a `BaseDao`; it wants a separate read-only query executor. Do not add another
subclass shaped like this.

### Interface segregation

Small, focused interfaces. ⚠ `ISearchable` declares three members and `SearchableDataView<T>` uses
one (`ToDisplayValues`); `ToSearchableString` and `ToDisplayKeys` are dead weight on every future
implementer. Drop members nothing calls.

### Dependency inversion

Depend on abstractions, and take dependencies through the constructor. `BaseDao<T>` depending on
`IDataTable` rather than a concrete table is the model to follow.

**Constructor injection is the house style.** `BusinessFactory` and `DBFactory` are the only two
service locators; everything below them is injected. Do not add a third singleton, and do not call
`BusinessFactory.GetInstance()` from inside a `Business` or `Database` class — only controllers do
that.

---

## 5. KISS

Simple beats clever. Some concrete calls for this codebase:

- **Delete dead code rather than commenting it out.** Git remembers. Cleared out so far:
  `CashierManager.ConvertToChar` (~55 lines), the `StringBuilder` table in `HtmlTableGenerator.cs`,
  dead `try`/`catch` shapes in `DBUtility`. Still there: the legacy DDL block in `DataTableList.cs`,
  a commented block in `LoginController`.
- **Do not duplicate a rule.** `Product.DiscountAmount` is the discount rule.
  `ViewItemMaster` copies it, which is exactly how a receipt and a screen end up disagreeing.
- **Do not copy a page.** `TransactionUpdatePage` is still a copy of `CashierPage` with small
  divergences, so every bug must be fixed twice. New shared behaviour goes into a control in
  `SimpleCommon/UI/ComponentWinForm/` (`SearchableDataView<T>` was started for exactly this).
- **Prefer a method over a flag.** `_isChanging`, `_isUpdatingItemQuantity`, `isAddingProduct` +
  `isUpdatingProduct` + `isOnProductAddEditMode` are re-entrancy and mode flags that must be kept in
  sync by hand. Where a flag is genuinely the simplest answer — `_loading` while a form fills its own
  fields — set and clear it in a `try`/`finally`.
- **Do not encode two meanings in one field** the way `Product.Discount` encodes flat-vs-percentage
  by sign. It is here, it is documented, it stays — but do not add another.
- **`Action` / `Action<T>` over custom delegates.** `DelegateUtility` predates them; new code uses
  the framework types.

---

## 6. Data access

**Nothing outside `Database/Dialect/` may name a database product.** No `SqlConnection`,
no `SqlParameter`, no `sp_rename`, no `INFORMATION_SCHEMA`. Three products are supported and the
provider is resolved at runtime; code that assumes one of them breaks the other two silently.

- **Parameterise.** Use `DbParam.Of` and the `params DbParameter[]` overloads on `FindByQuery` and
  `DBUtility`. Never interpolate a value into SQL. On an **indexed** column use
  `DbParam.AnsiText("@factur", 20, value)` — a bare string parameter infers Unicode and costs you the
  index seek.
- **Quote every identifier you write by hand** with `Dialect.Quote`. PostgreSQL folds unquoted names
  to lower case, so `WHERE Name = @n` stops matching a column created as `"Name"`. Result aliases go
  through `Dialect.QuoteAlias` — the `'Jumlah Transaksi'` form SQL Server accepts is a string literal
  elsewhere.
- **Identifiers cannot be parameters.** If a column or table name comes from anywhere near user
  input, validate it against `DataTableList` first, as `ProductManager.SanitizeOrderBy` does.
- **Write no DDL.** Declare the table or column in `Database/Schema/DatabaseSchema.cs`; the dialects
  render it.
- **Convert, do not cast, when reading a column** — `BaseObject.ToInt`, `ToBool`, `ToDecimal`. SQLite
  returns `Int64` for everything, so `(int)value` throws there.
- **Let write failures throw.** `DBUtility.ExecuteNonQuery` / `ExecuteScalar` rethrow;
  `TryExecuteNonQuery` / `TryExecuteScalar` are for schema probes and optional maintenance only.
  Never use a `Try*` for a write that matters.
- **Load → modify → save.** `Save`/`Update` write *every* mapped column, so a partially populated
  entity overwrites real data with defaults.
- **Wrap multi-row writes in a transaction** using the `BeginTransaction` / `if (newTransaction)
  Commit` pattern.
- **Adding a column means four edits** — physical table, `DataTableList`, model property, and *both*
  arms of the model's indexer. Miss the last and it fails at runtime.
- **Migrations are guarded, idempotent and additive.** Check `IsColumnExist` / `IsIndexExist`, then
  `ALTER`, then backfill. Never drop a column that live installations hold data in.
- **Soft delete only.** Set `Deleted` / `Revision`; never `DELETE`. History must stay resolvable.
- **Bracket reserved words** — `[Key]`, `[Group]`, `[Default]`.

---

## 7. UI

- **Marshal to the UI thread** in any public method a controller may call:

  ```csharp
  if (InvokeRequired)
  {
      this.BeginInvoke(new DelegateUtility.OneValueHandler<decimal>(UpdateTotal), total);
      return;
  }
  ```

- **Create controllers in `Load`, not the constructor**, and guard with `if (DesignMode) return;` —
  otherwise the Visual Studio designer tries to reach SQL Server.
- **Never hand-edit `*.Designer.cs`.** Use the designer.
- **Never show an exception to the operator.** Log the detail, show something actionable in
  Indonesian. Anything that escapes anyway lands in `Program.Application_ThreadException`, which
  reports it and keeps the till running.
- **Wrap anything slow in a wait cursor** and disable the button. Password verification alone costs a
  few hundred milliseconds.
- **User-facing strings are Indonesian**, code and comments are English.

---

## 8. Formatting, naming, culture

Match the existing style — it is consistent and there is no `.editorconfig` to argue with:

- **2-space indent**, brace on its own line (Allman).
- `PascalCase` types, methods, properties, events. `camelCase` locals and parameters.
- `_camelCase` private fields; `readonly` wherever possible.
- `UPPER_SNAKE_CASE` for `const` SQL strings and format constants (`FIND_BY_QUERY`,
  `DISPLAY_CURRENCY`).
- Interfaces prefixed `I`.
- **Never call `ToString()` / `Parse` on money or dates without a format.** The app pins `en-US`
  because amount parsing depends on it — do not undo the pin, and do not write code that silently
  relies on the machine's culture. Dates reach SQL Server as parameters, never as formatted text.
- Use `Constant.DISPLAY_CURRENCY` for on-screen amounts, `"N"` on receipts.
- Money is `decimal`, never `double` or `float`.

---

## 9. Logging and errors

```csharp
private static readonly log4net.ILog _log =
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
```

- One logger per class, that exact declaration.
- **`throw;` not `throw e;`** — the latter resets the stack trace.
- Do not swallow exceptions silently. Where tolerance is genuinely wanted, make it explicit and
  named, the way `TryExecuteNonQuery` is, and log what was skipped.
- Catch narrowly. `catch (Exception)` at a UI boundary is acceptable; deeper down it hides bugs.

---

## 10. Deployment reality

Installations are spread across many sites, each on a different version, with **no migration
history**. That shapes two rules:

- **Schema changes go through the startup reconciliation** in `DBUtility` — check first, then
  `ALTER`, then backfill. Additive only. Never drop a column an older site may hold data in.
- **New behaviour must not require a migration to have succeeded.** Where it would, degrade instead:
  write optional columns outside the entity map and tolerate their absence, as
  `TransactionManager.RecordCancellationAudit` does. A site whose `ALTER` failed must still be able
  to take money.

Likewise for anything that changes how people sign in or what they can see: default to the existing
behaviour, and let a site opt into the change. Old passwords keep working and re-hash themselves; the
recovery account stays enabled until somebody turns it off.

## 11. Checklist before committing

- [ ] Namespace matches the folder; file name matches the type; no framework-name collisions.
- [ ] New `.cs` file added to the `.csproj` `<Compile Include>` list.
- [ ] No new external dependency.
- [ ] No layer skipped; no SQL outside `Database/DataAccess`.
- [ ] New SQL is parameterised.
- [ ] New column: all four edits done.
- [ ] No rule duplicated from `Product` / `TransactionDetail` / `CashierManager`.
- [ ] Public UI methods marshal with `InvokeRequired`.
- [ ] Amounts and dates formatted explicitly.
- [ ] `.ai_memory/_index.md` and the relevant `index/*.md` updated; `business-*.md` updated if a rule
      changed.

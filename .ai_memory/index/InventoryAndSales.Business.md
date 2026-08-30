# Directory: `InventoryAndSales/Business/`

Namespace `InventoryAndSales.Business`. The business layer. Everything here is UI-agnostic and
database-agnostic — it depends on `Database.Manager` classes, never on DAOs or WinForms types
(the exception is `System.Drawing` in the receipt layout, for fonts and alignment).

Related business documentation: [../business-overview.md](../business-overview.md),
[../business-cart-and-pricing.md](../business-cart-and-pricing.md),
[../business-checkout.md](../business-checkout.md).

---

## `BusinessFactory.cs`

`public class BusinessFactory` — thread-safe lazy singleton, the composition root of the business
layer.

| Member | Signature | Purpose |
|---|---|---|
| `GetInstance` | `static BusinessFactory GetInstance()` | Double-checked-locked singleton accessor. |
| `Settings` | `SettingsService { get; private set; }` | Typed access to `M_SETTINGS`. |
| `ReportService` | `ReportService { get; private set; }` | Report folder and asset provisioning. |
| `Shop` | `ShopService { get; private set; }` | What this shop is called. |
| `Audit` | `AuditService { get; private set; }` | Who changed what. **Built first** — most of the rest reports into it. |
| `UpdateService` | `UpdateService { get; private set; }` | Whether a newer release exists. |
| `CashierManager` | `CashierManager { get; private set; }` | Checkout, revision, cancel, receipts. |
| `LoginManager` | `LoginManager { get; private set; }` | Authentication and active user. |
| `MasterManager` | `MasterManager { get; private set; }` | Product and user master data. |
| `ReportManager` | `ReportManager { get; private set; }` | Reporting queries. |
| `ViewManager` | `ViewManager { get; private set; }` | Transaction browsing. |
| *(ctor)* | `private BusinessFactory()` | Builds `SettingsService` first, then everything that needs it. |

Constructor injection throughout — this and `DBFactory` are the only two lookups.

---

## `Cart.cs`

`public class Cart` — the basket being rung up on **one** screen.

`Dictionary<int, TransactionDetail> _items` keyed by `Product.Id`, guarded by `_lockItems`. One
entry per product; quantities aggregate rather than creating extra lines.

**Each screen owns its own instance** (`CashierController` and `TransactionUpdateController` each
construct one). It previously lived on the `CashierManager` singleton, which meant the correction
window replaced whatever the cashier was ringing up and both screens reacted to each other.

| Member | Signature | Purpose |
|---|---|---|
| `CartChange` | `event CartChangeDelegate` | Raised after every change with the product and its new quantity. |
| `Add` | `bool Add(Product product, int quantityDelta)` | Adds to, or with a negative delta subtracts from, a line. A negative delta for a product not in the cart is a **no-op returning `true`** — it used to throw `KeyNotFoundException` internally. |
| `SetQuantity` | `bool SetQuantity(Product product, int quantity)` | Absolute quantity; `<= 0` removes the line. |
| `Remove` | `bool Remove(Product product)` | Drops the line, raises `CartChange` with `0`. |
| `Clear` | `void Clear()` | Empties it. Deliberately silent — the caller resets its own grid. |
| `GetTotal` | `decimal GetTotal(out decimal totalPrice, out decimal totalDiscount)` | Amount owed, plus gross and discount for the receipt. |
| `GetLines` | `List<TransactionDetail> GetLines()` | Copy of the list (not of the lines) for checkout. |
| `IsEmpty` | `bool { get; }` | Whether anything is in it. |

All mutations log and return `false` rather than throwing, so a cart error never crashes the sale
screen.

---

## `CashierManager.cs`

`public class CashierManager` — turns a `Cart` into a persisted sale and a printed receipt, and
handles correcting and voiding past sales. Stateless apart from `_lastFactur`.

Dependencies: `TransactionManager`, `UserManager`, `SettingsService`.

### Receipt notes

| Member | Signature | Purpose |
|---|---|---|
| `GetHeaderNote` / `GetFooterNote` | `string ()` | `M_SETTINGS` `HEADER` / `FOOTER`, newlines decoded. |
| `SetHeaderNote` / `SetFooterNote` | `void (string)` | Encode and persist. |

Encoding now lives in `SettingsService`, and a missing row falls back to a default instead of
throwing.

### Checkout and transactions

| Member | Signature | Purpose |
|---|---|---|
| `Checkout` | `TransactionStatus Checkout(Cart cart, decimal payment, string notes, int userId, long customerId, out string message)` | Builds and saves the sale atomically, records `_lastFactur`, then prints. Save failure → `FAILED`. Print failure → still `SUCCESS`, with a warning. |
| `UpdateCheckout` | `void UpdateCheckout(Cart cart, Transaction original, decimal payment, string notes, int userId, long customerId)` | Correction: prefixes the notes with `"Ralat Dari Transaksi: …"`, writes the new sale and links the old one. |
| `CancelTransaction` | `void CancelTransaction(string factur, int cancelledByUserId)` | Voids a sale, recording who authorised it. Throws if the faktur is unknown. |
| `GetTransaction` | `Transaction GetTransaction(string factur, out List<TransactionDetail> details)` | Delegates to `TransactionManager`. |
| `GetLastFactur` | `string ()` | Last sale of this session; in-memory only. |
| `GenerateTransactionAndDetails` | `private Transaction (…)` | Builds the header from the cart, stamps the time and faktur, computes `Exchange`. |
| `TrimNotes` | `private static string (string)` | Truncates to the `varchar(100)` column and logs, rather than letting the insert fail. |
| `GenerateFactur` | `private static string ()` | `DateTime.Now.Ticks` — 18 digits, uniquely indexed. |

### Printing

| Member | Signature | Purpose |
|---|---|---|
| `GetPrintFont` | `Font ()` | The receipt font; also used by the settings preview. |
| `PrintPaymentNote` | `void (Transaction, List<TransactionDetail>)` | Builds the lines via `ReceiptBuilder` and prints. |
| `ResolveCashierName` | `private string (int userId)` | Built-in recovery account → its name; real user → `Name`; otherwise `"ADMIN"` with a warning. **Previously returned early when the lookup failed, so sales under the recovery account printed nothing at all.** |

---

## `ReceiptBuilder.cs`

`public static class ReceiptBuilder` — a pure function producing the printable lines.

| Member | Signature | Purpose |
|---|---|---|
| `Build` | `static List<StringPrint> Build(string headerNotes, string footerNotes, Transaction, List<TransactionDetail>, string cashierName)` | Centred header → separator → date/faktur/cashier → separator → per item → separator → totals → payment and change → centred footer. |
| `CreateReceiptFont` | `static Font ()` | `Courier New` 9pt — fixed width, which the column alignment depends on. |
| `LineSeparator` | `const string` | 33 `=` characters, sized for an 80 mm roll. |
| `SplitLines` | `private static string[] (string)` | Splits on `\r\n`, `\n` or `\r`. |

No database, no printer, no settings lookup — which is what lets the settings preview render a
made-up sale through this exact code. Keep it that way.

---

## `LoginManager.cs`

`public class LoginManager` — authentication policy. Depends on `UserManager` and `SettingsService`.

| Member | Signature | Purpose |
|---|---|---|
| `ActiveUser` | `User { get; private set; }` | Signed-in user, or `null`. |
| `Login` | `bool Login(string username, string password)` | Authenticates, sets `ActiveUser`, raises `OnActiveUserChanged`. |
| `AuthenticateUsernamePassword` | `User (string password, string username)` | Validates credentials **without** touching the session — used by the supervisor approval dialog. Note the parameter order. |
| `IsBuiltInAdminAllowed` | `bool ()` | Reads `ALLOW_BUILTIN_ADMIN` (default `true`). |
| `SetBuiltInAdminAllowed` | `void (bool)` | Persists it. |
| `HasRealAdministrator` | `bool ()` | Whether any `M_USERS` account holds `AccessOption.Master`. Stops the settings page disabling the recovery account into a lockout. |
| `IsBuiltInAdminId` | `static bool (int userId)` | Whether a `UserId` is the recovery account (`-1`). |
| `BuiltInAdminDisplayName` | `static string { get; }` | Its display name. |
| `UpgradeStoredPasswordIfNeeded` | `private void (User, string plain)` | Re-hashes a legacy hash after a successful sign-in. Best effort — a failure never blocks the login. |
| `Logout` | `void ()` | Clears `ActiveUser`; does not raise the event. |
| `OnActiveUserChanged` | `event OnActiveUserDelegate` | `MainFormController` swaps pages on it. |

The built-in recovery account (`Kosmas` / `kosmas`, role 1023, id `-1`) is checked before the
database and gated on the setting. Username matching is case-insensitive, the password is exact.
Every use is logged at WARN. See [../business-auth-and-roles.md](../business-auth-and-roles.md).

---

## `SettingsService.cs` and `SettingKeys.cs`

`public class SettingsService` — typed access to `M_SETTINGS`. Every read falls back to a
caller-supplied default (then the row's own `Default`) rather than throwing, so a missing row cannot
break the feature that reads it.

| Member | Signature | Purpose |
|---|---|---|
| `GetString` / `SetString` | `string (string key, string fallback)` / `void (string, string)` | Raw value. |
| `GetBool` / `SetBool` | `bool (string key, bool fallback)` / `void (string, bool)` | Tolerates `1/0`, `yes/no`, `on/off`. |
| `GetMultiLine` / `SetMultiLine` | `string (string, string)` / `void (string, string)` | Values holding several lines. |
| `EncodeNewLines` / `DecodeNewLines` | `static string (string)` | `%NEW_LINE%` ↔ real breaks. Encoding normalises `\r\n`, `\n` and `\r`. |

`public static class SettingKeys` — the key constants, plus `Seed()` returning the rows a database is
expected to hold and `DefaultReportDirectory()`. **Adding a key here is all that is needed for an old
installation to pick it up on its next launch**, because `DBUtility.UpsertSettingRow` inserts
whichever rows are missing.

Keys: `SHOP_NAME`, `HEADER`, `FOOTER`, `EDC_TERMINALS`, `QRIS_PROVIDERS` (group `GENERAL`),
`REPORT_DIRECTORY` (`REPORT`), `ALLOW_BUILTIN_ADMIN` (`SECURITY`), `PRINTER_NAME`,
`PRINTER_PAPER_WIDTH_MM` (`PRINTER`), `UPDATE_MANIFEST_URL` (`UPDATE`).

`ConfiguredSetting(name)` seeds a key from `App.config` — used by `PRINTER_NAME` so a technician can
set it once at install time. Read **only** when the row is missing; editing `App.config` afterwards
changes nothing. `UPDATE_MANIFEST_URL` seeds through `ConfiguredManifestUrl()` instead: an
`App.config` entry wins (empty included — empty means "do not check"), but a config with **no entry
at all** seeds `DefaultUpdateManifestUrl`, the GitHub `version.txt` address baked into the build —
updates never ship the config, so an updated-in-place till must not end with checking off.
`RetiredUpdateManifestUrls` lists superseded defaults (the pre-GitHub Google Doc);
`DBUtility.RetireSupersededManifestUrl` rewrites a stored value that exactly matches one of them.

**Every `SetString` is audited**, so adding a key here makes it auditable for free.

⚠ `SetString` does nothing but log when the row does not exist, so a page that saves a **new** key
should read the value back rather than report success blindly — `ShopSettingForm` does.

---

## `AuditService.cs`

`public class AuditService` — records who changed what, into `T_AUDIT_LOG`.

| Member | Signature | Purpose |
|---|---|---|
| `Follow` | `void (LoginManager)` | Subscribes to `OnActiveUserChanged` to track the actor. |
| `Record` | `void (action, entityType, entityKey, detail)` | Against the signed-in user. |
| `RecordAs` | `void (User, action, entityType, entityKey, detail)` | Against a named user — a failed sign-in has none, and a supervisor who approved a step-up is not the person at the till. |
| `RecordLogin` / `RecordLoginFailed` / `RecordLogout` | | Session events. `RecordLogout` also clears the tracked actor, because signing out deliberately does not raise `OnActiveUserChanged`. |
| *(consts)* | | The vocabulary: `ActionLogin`, `ActionCreate`, `ActionUpdate`, `ActionDelete`, `ActionCheckout`, `ActionRevise`, `ActionCancel`, `ActionSettingChange`, `ActionUpdateApplied`; `EntityProduct`, `EntityUser`, `EntitySetting`, `EntitySale`, `EntitySession`, `EntityApplication`. |

Two rules hold, and both are load-bearing:

- **Auditing never breaks the thing it audits.** Every write is swallowed and logged, and mirrored
  into the application log as an `AUDIT` line so the trail survives even when the database is what
  is broken. A till that cannot write an audit row must still be able to take money.
- **An entry is written after the operation, never before**, so it always describes something that
  happened. Where a business transaction is still open — a CSV import runs one around the whole
  file — the entry joins it and rolls back with it. The deliberate exception is a *failed* checkout,
  recorded after the rollback because that is the case worth investigating.

Coverage comes from choke points, not scattered calls: `SettingsService.SetString` (every setting),
`MasterManager` (every product and user edit), `CashierManager` (checkout, revision, cancellation),
`LoginManager` (sign-in, failure, sign-out), `MainFormController.RequirePermission` (supervisor
approvals), `UpdateController` (an applied update).

⚠ The actor is tracked by **subscription**, not by holding a `LoginManager` — the login manager
already depends on this class. `BusinessFactory` builds the audit service first and calls `Follow`
last.

⚠ User entries deliberately exclude the password hash; an audit trail is read by more people than
the user table is.

---

## `UpdateService.cs` and `UpdateManifest.cs`

`public class UpdateService` — is there a newer release, and get it ready to install.

| Member | Signature | Purpose |
|---|---|---|
| `CurrentVersion` | `static Version` | The running assembly version. |
| `GetManifestUrl` / `SetManifestUrl` | | `UPDATE_MANIFEST_URL`. Empty switches the feature off; fresh rows seed `SettingKeys.DefaultUpdateManifestUrl` (GitHub `version.txt`) unless `App.config` says otherwise. |
| `FetchManifest` | `UpdateManifest ()` | Downloads and parses it. **Returns null for every failure** — not configured, unreachable, unparseable — because they all mean "carry on as you are". |
| `IsNewer` | `static bool (UpdateManifest)` | Strictly greater than the running version. |
| `StageUpdate` | `string (UpdateManifest, out string problem)` | Downloads the archive, verifies it against the manifest's optional `Sha256` (a mismatch — including a mistyped line — refuses before unpacking; no line skips the check), unpacks it, returns the folder holding the new files. |
| `InstallDirectory` / `WorkingRoot` | `static string` | Where the application is; where downloads go (`%LOCALAPPDATA%\FidelisCake\Update`). |

`public class UpdateManifest` — the manifest file (`version.txt` on GitHub), one `Key: value` per
line (`Version`, `Drive`, `File`, `Sha256`, `Notes`); unknown keys, blanks and `#` comments ignored.
`Parse` never throws — a mistyped release must leave the till running. `Sha256` is stored raw;
normalization (trim + lowercase) happens in `UpdateService.ChecksumAccepted`.

`ToDirectDownloadUrl` turns a Google Drive sharing link into `uc?export=download&id=…`, because a
link copied out of Drive points at a viewer page and fetching it returns HTML. A **folder** link has
no file id and is left alone: a folder cannot be fetched this way, which is why a manifest with only
`Drive:` announces the update instead of installing it.

⚠ `StageUpdate` writes only under `WorkingRoot`, never into the installation, and treats "the archive
will not open" as the signal that a Drive file was never shared publicly — that case downloads
happily as a sign-in page.

⚠ TLS 1.2 is enabled explicitly. .NET Framework 4.6 negotiates the machine's defaults, which on an
older shop PC can still be TLS 1.0; Google refuses that, so the check would fail on exactly the
machines that most need updating.

---

## `ShopService.cs`

`public class ShopService` — what this shop is called. The name titles the main window and heads
every generated report, so it cannot stay compiled in: the same build runs in more than one shop.

| Member | Signature | Purpose |
|---|---|---|
| `GetName` | `string ()` | The resolution rule below. |
| `IsNameInherited` | `bool ()` | True when the name is only being echoed from the receipt header. |
| `SetName` | `void (string)` | Stores it trimmed. |
| `ValidateName` | `string (string)` | Empty when savable, otherwise an Indonesian message. Max 60 characters. |

Resolution order, and the reason it is a rule rather than a plain setting read:

1. `SHOP_NAME`, when somebody has set one;
2. otherwise **the first non-empty line of the receipt header** — installations have been writing
   their name there for years and there is no migration history to copy it across, so `SHOP_NAME` is
   seeded **empty** and an upgrade never renames a shop behind its back;
3. otherwise `SettingKeys.DefaultShopName`.

⚠ Read the name through this class, never off the header directly: a shop that sees its name on two
screens has to see the same answer on both.

---

## `ReportService.cs`

`public class ReportService` — where reports go and how their JavaScript gets there.

| Member | Signature | Purpose |
|---|---|---|
| `GetReportDirectory` | `string ()` | Configured folder with environment variables expanded; falls back to the default if unusable. |
| `SetReportDirectory` | `void (string)` | Persists it. |
| `ValidateReportDirectory` | `string (string)` | Creates the folder and writes a probe file. Returns an Indonesian message, or empty when usable. |
| `PrepareReportDirectory` | `string ()` | Creates and returns the folder. |
| `EnsureAssets` | `bool (string reportDirectory)` | Unpacks `datatables.min.css` / `.js` into `<dir>\assets` from the shipped bundle. Idempotent. `false` means the report will still open, just without sorting, searching and export. |
| `ExtractBundle` | `private static void (string, string)` | Extracts by entry **name** only, so a crafted archive cannot write outside the target folder. |
| `GetAssetBundlePath` | `static string ()` | `reportassets.zip` next to the executable. |
| `StyleSheetHref` / `ScriptSrc` | `static string { get; }` | Relative paths a generated report uses. |

Uses `System.IO.Compression` — framework, no package.

---

## `MasterManager.cs`

Unchanged. Master data façade over `ProductManager` and `UserManager`; everything is soft-deleted.

| Member | Purpose |
|---|---|
| `GetAllProduct()` | Every product **including soft-deleted** — used by the correction screen. |
| `GetAllAvailable(criteria, orderBy)` | Non-deleted products matching a name search. |
| `AddProduct` / `UpdateProduct` / `DeleteProduct` | Insert / update / soft delete. |
| `GetUsers` / `AddUser` / `UpdateUser` / `DeleteUser` | Same for users. |

---

## `ReportManager.cs`

Thin façade over `CustomManager`. `GetSummaryReportProduct`, `GetReportSummaryByTransaction`,
`GetDetailReport`, `GetReportSummaryByCashier`, `GetReportSummaryByPaymentMethod` (each `start`/`stop`
→ `List<Dictionary<string,string>>`), and `GetTodaySummaryByCashier(User, DateTime)` → a
pre-formatted `"Rp. n"`.

---

## `ReportTable.cs`

`public class ReportTable` — a report's rows worked out into something that can be laid out. Built
with `static ReportTable From(List<Dictionary<string,string>>)`; an empty result gives an empty table,
never null.

| Member | Purpose |
|---|---|
| `Headers` | Column headings, i.e. the SQL aliases from `CustomDao`. |
| `Rows` | Values formatted for display, in `Headers` order. |
| `ColumnKinds` | `ReportColumnKind` per column — what it holds. |
| `Totals` | Formatted column total, empty where a total means nothing. |
| `HasTotals` / `RowCount` / `ColumnCount` | |

Everything is inferred from the values, because the query layer hands over stringified database
values with no types attached (`"100000.0000"` for money, `"3"` for a count):

- a column is **Number** only when *every* value in it parses, so invoice numbers stay text;
- a decimal point anywhere in the column means money → `#,##0.00`; otherwise a count → `#,##0`;
- a column is **Date** when every value matches how `CustomQuery` writes one (`dd MMM yyyy`, with or
  without `HH:mm:ss`);
- a numeric column is totalled **unless** its heading contains `Satuan` or `Rata-rata` — see the
  naming rule documented on `CustomDao`.

⚠ Adding a per-unit column to a report query without naming it that way puts a meaningless sum in the
totals row.

---

## `ReportDocument.cs`

`public class ReportDocument` — title, shop name, period, who generated it and when, plus the
`ReportTable`. Immutable; built by `ReportDisplayController` and consumed by `HtmlReportGenerator`.
`PeriodText` collapses to a single date when both ends are the same day; dates are formatted
invariantly so a saved report never depends on the machine's culture.

---

## `ReportColumnKind.cs`

`public enum ReportColumnKind` — `Text`, `Number`, `Date`. Deliberately only the distinctions the
presentation needs; the renderer maps them to alignment and wrapping.

---

## `ViewManager.cs`

`GetTransaction(DateTime start, DateTime stop)` — active transactions in a range, including `Id` and
`Factur` so the picker can return a selection.

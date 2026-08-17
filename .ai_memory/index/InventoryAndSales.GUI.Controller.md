# Directory: `InventoryAndSales/GUI/Controller/`

Namespace `InventoryAndSales.GUI.Controller`. One controller per page. Each is constructed by its
view, pulls the managers it needs from `BusinessFactory`, and holds a back-reference to the view so
it can push updates.

Rule of thumb in this codebase: **validation and orchestration live in the controller; domain rules
live in `Business/`; the view only formats and collects input.**

---

## `CashierController.cs`

`public class CashierController` — the sale screen. Depends on `LoginManager`, `CashierManager`,
`MasterManager`. **Owns its own `Cart`** and subscribes to that cart's `CartChange`.

| Member | Signature | Purpose |
|---|---|---|
| `CartChange` | `private void (object, KeyValuePair<Product,int>)` | Event handler. Pushes the changed line into the grid and recomputes the displayed total. |
| `GetItems` | `List<Product> GetItems()` | Non-deleted products, unordered — the sellable catalogue. |
| `Checkout` | `string Checkout(decimal payment, string notes, out string successMessage)` | **Returns an error message, or empty on success.** Validation order: no active user → *"Sesi telah berakhir…"*; payment `< 0`; cart total `<= 0`; change `< 0`. Then calls `CashierManager.Checkout(_cart, …)` with `PlaceholderCustomerId`. On `SUCCESS` builds *"Transaksi Berhasil. Kembalian Rp {change}"*, appends any print warning, and clears the cart. |
| `AddToCart` | `void AddToCart(Product product)` | `_cart.Add(product, 1)`. |
| `RemoveFromCart` | `void RemoveFromCart(Product product)` | `_cart.Add(product, -1)`. |
| `UpdateCart` | `void UpdateCart(Product product, int value)` | Absolute quantity. |
| `NewCart` | `void NewCart()` | Clears the cart **and** the view's grid. |

⚠ Note: `PlaceholderCustomerId = 1` — the customer feature is unfinished, and the constant is named
so that is obvious at the call site.

An unexpected failure is logged in full and reported as *"Transaksi gagal karena kesalahan sistem.
Silahkan coba lagi."*, with the cart left intact for a retry. It previously returned
`e.Message + StackTrace` straight into a message box.

---

## `TransactionUpdateController.cs`

`public class TransactionUpdateController` — the revision screen. Same shape as `CashierController`
but writes a *revision* instead of a new sale. Depends on `CashierManager`, `MasterManager`.

State: `Transaction OriginalTransaction { get; private set; }`,
`List<TransactionDetail> _originalTransactionDetails`, `User _supervisor`.

| Member | Signature | Purpose |
|---|---|---|
| `Init` | `void Init(string facturNumber, User user)` | Loads the original transaction and its lines and stores the authorising supervisor. Throws if the faktur is unknown. |
| `ResetByTransaction` | `void ResetByTransaction()` | Repopulates the cart from the original lines: builds a `ProductId → Quantity` map (summing duplicates defensively), then walks the product list calling `UpdateCart`. |
| `GetItems` | `List<Product> GetItems()` | `MasterManager.GetAllProduct()` — **includes soft-deleted products**, so lines referencing a since-withdrawn item still reload. |
| `Checkout` | `string Checkout(decimal payment, string notes, out string successMessage)` | Same validation as `CashierController`, then `CashierManager.UpdateCheckout(_cart, OriginalTransaction, …)` attributing the sale to `_supervisor.Id`. Failures are logged and reported as *"Perubahan transaksi gagal disimpan…"*. |
| `AddToCart` / `UpdateCart` / `NewCart` | *(as `CashierController`)* | Cart mutation. No `RemoveFromCart`. |
| `CartChange` | `private void (object, KeyValuePair<Product,int>)` | Pushes changes to the view. |

**This controller owns its own `Cart`.** The cart used to live on the `CashierManager` singleton and
be shared with the sale screen, so opening this window replaced whatever the cashier was ringing up
and both screens reacted to every change. `UnregisterEvent`, `Unload` and the finalizer that existed
to manage that shared subscription are gone — there is nothing shared left to release.

⚠ Note: `ResetByTransaction` replays quantities through the **live** catalogue, so a correction is
priced at today's prices, not the prices originally charged.

---

## `MasterProductController.cs`

`public class MasterProductController` — product master. Depends on `MasterManager`.

| Member | Signature | Purpose |
|---|---|---|
| `GetSortableColumns` | `IList<string> GetSortableColumns()` | `DataTableList.Instance.GetDataTable(typeof(Product)).Columns` — the sort combo box options come straight from the column map. |
| `AddItem` | `void AddItem(string code, string barcode, string name, decimal price, decimal discount)` | Creates a `Product` with `deleted: false` and saves. |
| `UpdateItem` | `void UpdateItem(Product current, string code, string barcode, string name, decimal price, decimal discount)` | Mutates the loaded instance in place, then updates. |
| `RemoveItem` | `void RemoveItem(Product current)` | Soft delete. |
| `GetItems` | `List<Product> GetItems(string nameLike, string orderBy)` | Filtered, ordered, non-deleted list. |
| `SetItemForImport` | `ImportResult SetItemForImport(List<Product> products)` | CSV import: `Id == 0` → insert, otherwise → update. **Wrapped in one database transaction**, so a failure part way through rolls the whole import back. Returns how many rows were added and updated. |
| `ImportResult` | nested `class` | `Added` / `Updated` counts, reported to the operator. |

---

## `MasterUserController.cs`

`public class MasterUserController` — user master. Depends on `MasterManager`.

| Member | Signature | Purpose |
|---|---|---|
| `GetUsers` | `List<User> GetUsers()` | Non-deleted users. |
| `AddUser` | `void AddUser(string username, string name, string password, int role)` | Hashes the password with `HashUtility.GetEncryptedPass` and saves. |
| `UpdateUser` | `void UpdateUser(User current, string username, string name, string password, int role, bool passwordChanged)` | Re-hashes **only when `passwordChanged`**. Updates `Name` and `Role`. |
| `DeleteUser` | `void DeleteUser(User current)` | Soft delete. |
| `IsUsernameTaken` | `bool IsUsernameTaken(string username, User excluding)` | Case-insensitive duplicate check — there is no unique index on the column. |

The `passwordChanged` flag comes straight from the screen, which tracks whether the password field
was touched. Previously the screen displayed the **first 8 characters of the stored hash** and the
controller re-hashed only when the typed text no longer prefixed it — which tied this decision to the
storage format and would have broken outright under the new hash format.

⚠ Note: `UpdateUser` accepts `username` but never assigns it — the username cannot be changed
through the UI.

---

## `ReportDisplayController.cs`

`public class ReportDisplayController` — reports. Depends on `ReportManager`.

| Member | Signature | Purpose |
|---|---|---|
| `ShowSummaryReport` | `void (DateTime start, DateTime stop)` | Runs the by-cashier, by-transaction and by-product reports, converts each with `DataTableUtil.GetDataTable`, and pushes them into the page as three **named** arguments (previously a positional array). |
| `ShowDetailReport` | `void (DateTime start, DateTime stop)` | Line-level report into the detail grid. |
| `ShowSummaryReportPerKasir` | `void (DateTime, DateTime)` | HTML export, file `SBC{yyyyMMdd}_{yyyyMMdd}.html`, title *Cashier Report*. |
| `ShowSummaryReportPerTransaksi` | `void (DateTime, DateTime)` | HTML export, `SBT…`, *Transaction Report*. |
| `ShowSummaryReportPerProduct` | `void (DateTime, DateTime)` | HTML export, `SRP…`, *Product Sales Report*. |
| `ShowSummaryReportPerDetail` | `void (DateTime, DateTime)` | HTML export, `RDP…`, *Detail Report*. |
| `ShowSummaryReportInHtml` | `void (List<Dictionary<string,string>> dataReport, string filename, string id, string title)` | Headers from the first row's keys, rows from its values; renders with `SimpleCommon.UI.HtmlTableGenerator`, wraps with `Utility.HtmlReportGenerator`, writes into the **configured** report folder and opens it. |
| `BuildFileName` | `private static string (string prefix, DateTime, DateTime)` | `{prefix}{yyyyMMdd}_{yyyyMMdd}.html`. |
| `OpenReport` | `private static void (string fullPath)` | Launches the file; if that fails, tells the operator where it was saved. |

The folder comes from `ReportService.PrepareReportDirectory()` and the DataTables assets are
unpacked next to it by `ReportService.EnsureAssets`. When the asset bundle is missing the report is
still written and opened — with the asset links omitted entirely — and the operator is told which
file is missing rather than being left with a silently inert page. An empty report now says so
instead of doing nothing, and write or open failures are reported with the path.

---

## `LoginController.cs`

`public class LoginController` — one method.

| Member | Signature | Purpose |
|---|---|---|
| `Login` | `bool Login(string username, string password)` | Delegates to `LoginManager.Login`. The page navigation happens through the `OnActiveUserChanged` event, not here — the commented-out block shows the earlier direct-call design. |

---

## `SettingPageController.cs`

`public class SettingPageController` — empty placeholder taking a `SettingForm`. No behaviour yet.

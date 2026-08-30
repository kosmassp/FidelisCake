# Directory: `InventoryAndSales/GUI/Popup/`

Namespace `InventoryAndSales.GUI.Popup` — except `ReprintReceipt` and `TransactionHistory`, which
sit in `InventoryAndSales.GUI` despite living in this folder.

Modal and modeless dialogs. `*.Designer.cs` / `*.resx` are generated layout.

---

## `AuthenticationForm.cs`

`public partial class AuthenticationForm : Form` — supervisor step-up. Constructed with the
`AccessOption` the caller requires; returns `DialogResult.OK` only when the entered credentials both
authenticate **and** carry that permission.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `AuthenticationForm(AccessOption accessOption)` | Stores the required permission, grabs `LoginManager`, presets `DialogResult = Cancel`, zeroes the failure counter. |
| `AuthenticatedUser` | `User { get; private set; }` | The supervisor who approved, read by the caller after OK. |
| `buttonAuthenticate_Click` | `private void (object, EventArgs)` | `LoginManager.AuthenticateUsernamePassword` (does **not** change the active session), then `BusinessUtil.AllowedRole`. Success → `DialogResult.OK` and close. Failure → label reads *"Akses Ditolak"*. |
| `buttonBack_Click` | `private void (object, EventArgs)` | Closes, leaving `DialogResult.Cancel`. |

Used by `MainFormController.RequestUpdateTransaction` and `RequestDeleteTransaction` when the signed-in
cashier lacks `AccessOption.Master`.

Failures are logged and the attempt number is shown (*"Akses Ditolak. (percobaan ke-N)"*); the
password box is cleared and refocused. The message is identical whether the credentials were wrong or
the account merely lacks the permission, so it gives nothing away. `_failed` was previously formatted
into a string with no placeholder, so the count was computed and discarded.

⚠ Note: still no lockout or throttling on repeated attempts.

⚠ Note: the authorisation decision still lives in the form rather than a controller. The
permission-plus-step-up flow used by the menus is now consolidated in
`MainFormController.RequirePermission`.

---

## `TransactionHistory.cs`

`public partial class TransactionHistory : Form` (namespace `InventoryAndSales.GUI`) — date-range
transaction picker. Used by reprint, revise and cancel flows.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `TransactionHistory()` | Grabs `ViewManager`, presets `DialogResult = Cancel`, sets both date pickers to now. |
| `SelectedTransactionId` | `string { get; private set; }` | `T_TRANSACTIONS.Id` of the chosen row. |
| `SelectedTransactionFactur` | `string { get; private set; }` | Faktur of the chosen row — what callers actually use. |
| `buttonSearch_Click` | `private void (object, EventArgs)` | `ViewManager.GetTransaction(from, to)` → `DataTableUtil.GetDataTable` → grid. |
| `UpdateDataGridView` | `void UpdateDataGridView(DataTable dataTable)` | Binds the grid; thread-marshalled. |
| `buttonNext_Click` | `private void (object, EventArgs)` | Requires exactly one selected row (*"Silahkan pilih 1 data"*), reads the `Id` and `Factur` cells, sets `DialogResult.OK`, closes. |
| `buttonCancel_Click` | `private void (object, EventArgs)` | Closes as cancelled. |

The grid is bound to a `DataTable` whose columns come from `CustomDao.QUERY_VIEW_TRANSACTION`, so
`Cells["Id"]` and `Cells["Factur"]` depend on that `SELECT` keeping those alias names.

---

## `TransactionUpdateForm.cs`

`public partial class TransactionUpdateForm : Form` — window wrapper hosting
`TransactionUpdatePage`.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `TransactionUpdateForm(string facturNumber, User supervisor)` | Forwards both to `transactionUpdatePage1.Init(...)`. |
| `transactionUpdatePage1_BackClick` | `private void (object, EventArgs)` | Closes the window. |
| `transactionUpdatePage1_CheckoutSucceed` | `private void (object, EventArgs)` | Closes the window. |
| `TransactionUpdateForm_FormClosing` | `private void (object, FormClosingEventArgs)` | Nothing to release — the correction screen owns its own cart and subscribes only to that. |

The empty `FormClosing` used to matter: closing via the title-bar **X** skipped
`controller.Unload()` and leaked a subscription to the shared cart's `CartChange`. With a per-screen
cart there is nothing shared left to leak.

---

## `SettingForm.cs`

`public partial class SettingForm : Form` — settings shell. A `ListBox` of setting pages on one
side, a host panel on the other. Opened non-modally from the *Pengaturan* menu.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `SettingForm()` | `InitializeComponent`, creates `SettingPageController`, sets the list box `DisplayMember = "Tag"`, then `Initialize()`. |
| `Initialize` | `private void Initialize()` | Registers the pages — *Toko*, *Nota*, *Laporan*, *Pembayaran*, *Printer*, *Keamanan*. **This is the extension point: one line per page.** |
| `SettingPageEntry` | nested `class` | A `Tag` plus a factory, and the control once built. |
| `listBoxSettingSelection_SelectedIndexChanged` | `private void (object, EventArgs)` | Builds the page on first selection, then **replaces** the hosted control and docks it fill. |

Pages are built lazily, so opening Settings no longer runs every page's controller and its database
reads. Selecting a page previously **added** its control without removing the last one, leaving every
page ever opened alive underneath the visible one.

---

## `ReprintReceipt.cs`

`public partial class ReprintReceipt : Form` (namespace `InventoryAndSales.GUI`) — a faktur-number
prompt.

| Member | Type | Purpose |
|---|---|---|
| `FacturNumber` | `public string` field | Entered faktur. |
| `IsPrintPressed` | `public bool` field | Whether Print was clicked. |
| `buttonPrint_Click` | `private void (object, EventArgs)` | Captures the text, sets the flag, closes. |

⚠ Note: **unused.** The reprint flow uses `TransactionHistory` instead. Uses public fields and no
`DialogResult`, unlike the other dialogs.

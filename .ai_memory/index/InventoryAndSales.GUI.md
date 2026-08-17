# Directory: `InventoryAndSales/GUI/`

Namespace `InventoryAndSales.GUI`. The application shell.

The UI follows a loose **Model-View-Controller** split: a `Form`/`UserControl` (view) owns a
controller instance, and the controller talks to `BusinessFactory` managers. Views never touch
`Database.*` directly; controllers never touch DAOs.

---

## `MainForm.cs`

`public partial class MainForm : Form` — the shell window. Holds a `TabControl` whose headers are
hidden by `ControlUtility.HideTabHeader`, so tabs act as swappable pages.

Fields: `MainFormController controller`, `DisplayPage currentPage`.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `MainForm()` | Re-pins culture to `en-US`, hides tab headers, creates the controller, titles the window, sets `KeyPreview = true` so the form sees key events before the focused control. |
| `RefreshWindowTitle` | `void RefreshWindowTitle()` | `{shop name} [version: {assembly version}]`. The name comes from `MainFormController.GetShopName()` → `ShopService`, read on demand; called again from the settings dialog's `FormClosed` so a rename shows without restarting the till. Thread-marshalled. |
| `EnableMenu` | `void EnableMenu(int role)` | Shows/hides top-level menus by role bit: *Transaksi* ← `Cashier`, *Edit* ← `Master`, *Laporan* ← **`Laporan`**, *check kasir* ← `Cashier`. Marshals to the UI thread. The reports menu previously tested the `Admin` bit, so a Supervisor held the `Laporan` permission but could not open reports. |
| `LoadCashierPage` | `void LoadCashierPage()` | Selects the cashier tab, sets `currentPage`, calls `cashierPage1.Reset()`. |
| `LoadLoginPage` | `void LoadLoginPage()` | Selects the login tab, **calls `controller.Logout()`**, resets the login page. |
| `LoadProductMasterPage` | `void LoadProductMasterPage()` | Selects and resets the product master page. |
| `LoadUserMasterPage` | `void LoadUserMasterPage()` | Selects and resets the user master page. |
| `UpdateActiveUser` | `void UpdateActiveUser(string name)` | Status bar text `ActiveUser=<name>` or `<None>`. |
| `MainForm_Load` | `private void (object, EventArgs)` | Starts on the login page. |
| `MainForm_KeyUp` | `private void (object, KeyEventArgs)` | Only while `currentPage == Cashier`: **F5** focus the product filter, **F6** focus the payment box, **F7** trigger checkout. |
| `timerDisplayDate_Tick` | `private void (object, EventArgs)` | Clock in the status bar, `dd MMM yyyy HH:mm:ss`. |

### Menu handlers

| Handler | Action |
|---|---|
| `penjualanToolStripMenuItem_Click` | Go to the cashier page. |
| `loginToolStripMenuItem_Click` | Go to the login page (logs out). |
| `exitToolStripMenuItem_Click` | Close the app. |
| `daftarBarangToolStripMenuItem_Click` | Product master. |
| `daftarUserToolStripMenuItem_Click` | User master. |
| `laporanTransaksiToolStripMenuItem_Click` | Switch to the report tab and reset its date pickers to today. |
| `printLastReceiptToolStripMenuItem_Click` | Reprint the last receipt of this session; message box if none. |
| `printUlangTransaksiToolStripMenuItem_Click` | Open the transaction picker and reprint the chosen receipt. |
| `ubahTransaksiToolStripMenuItem_Click` | Revise a transaction (supervisor step-up), then return to the cashier page. |
| `hapusTransaksiToolStripMenuItem_Click` | Cancel a transaction (supervisor step-up); catches and logs exceptions, shows a generic Indonesian error. |
| `jumlahSetoranToolStripMenuItem_Click` | Message box with today's date, time and the cashier's running total, plus a caveat that revisions may skew it. |
| `pengaturanToolStripMenuItem_Click` | Opens `SettingForm` non-modally (`Show()`). |

All `Load*` and `UpdateActiveUser` methods start with the `InvokeRequired` → `BeginInvoke` pattern
using `SimpleCommon.Utility.DelegateUtility` handlers, because `LoginManager.OnActiveUserChanged`
can fire from a non-UI thread.

---

## `MainFormController.cs`

`public class MainFormController` — logic behind the shell menus. Depends on `LoginManager`,
`CashierManager`, `ReportManager` from `BusinessFactory`.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `MainFormController(MainForm mainForm)` | Subscribes to `LoginManager.OnActiveUserChanged`. |
| `OnActiveUserChanged` | `void (object sender, User activeUser)` | `null` → clear the status bar, disable all menus, show login. Otherwise → show the name, enable menus for the role, go to the cashier page. |
| `Logout` | `void Logout()` | Disables menus, clears the status bar, calls `LoginManager.Logout()`. |
| `PrintLastReceipt` | `bool PrintLastReceipt()` | Uses `CashierManager.GetLastFactur()`; reloads the transaction and reprints. `false` when there is no last faktur (e.g. after a restart). |
| `PrintReceipt` | `bool PrintReceipt()` | Opens `TransactionHistory`; on OK reloads by the selected faktur and reprints. Returns `true` when the dialog was cancelled. |
| `RequirePermission` | `private User RequirePermission(AccessOption required)` | Returns the active user when they already hold the permission, otherwise runs the supervisor approval dialog and returns whoever approved — or `null` if refused. **The single place the step-up flow lives**; it was previously duplicated in the two callers below. |
| `RequestUpdateTransaction` | `void RequestUpdateTransaction()` | `RequirePermission(Master)`, then `TransactionHistory`, then `TransactionUpdateForm` modally. |
| `RequestDeleteTransaction` | `bool RequestDeleteTransaction()` | Same step-up, then `CashierManager.CancelTransaction(factur, supervisor.Id)` — recording who authorised the void. |
| `ReprintByFactur` | `private bool (string facturNumber)` | Shared reload-and-print used by both reprint paths. |
| `GetCurrentDayTotalTransaction` | `string GetCurrentDayTotalTransaction()` | `ReportManager.GetTodaySummaryByCashier(activeUser, DateTime.Today)`; returns `"Rp. 0"` when nobody is signed in. |

Dialogs are created inside `using` blocks so their window handles are released.

⚠ Note: `PrintReceipt` returns `true` on cancel and `false` only when the reload fails, so
`MainForm` shows "No Faktur tidak ditemukan" for a genuine miss but stays silent on cancel — which
is the intent, though the return value reads inverted.

---

## `SplashForm.cs`

`public partial class SplashForm : Form` — startup progress window. Runs the database check on a
background thread so the UI stays responsive.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `SplashForm()` | Starts a background `Thread` running `Start()`. |
| `InitializationCheckSuccess` | `public bool` field | Read by `Program.Main` to decide whether to launch `MainForm`. |
| `Start` | `void Start()` | 10 % *Initializing* → 40 % *Checking Database* → `DBUtility.CheckForDatabaseTable()` → 90 % *Inserting Important Row* → `DBUtility.CheckForDatabaseRow()` → 100 % *Starting*. Wrapped in `try`/`catch`. |
| `SetProgressBar` | `private void (int progress, string status)` | Marshals to the UI thread, logs, updates the bar and label. At 100 it sets `InitializationCheckSuccess = true` and closes the form. |
| `ReportStartupFailure` | `private void ()` | Marshals to the UI thread, tells the operator the database could not be reached and to check `Log\log.txt`, then closes with the success flag clear so `Program` exits cleanly. |

A failure in `Start()` used to leave the splash screen hanging: the success flag was never set and
the form never closed, so the application simply appeared frozen with no explanation. Now that
`DBUtility`'s helpers rethrow rather than swallow, this path matters.

---

## `MainForm.Designer.cs`, `SplashForm.Designer.cs`, `MainForm.resx`, `SplashForm.resx`

Visual Studio generated layout and resources. No business logic. `MainForm.Designer.cs` declares the
tab pages (`tabPageLogin`, `tabPageCashier`, `tabPageProductMaster`, `tabPageUserMaster`,
`tabPageReport`), the embedded page controls (`loginPage1`, `cashierPage1`, `masterProductPage1`,
`masterUserPage1`, `reportDisplayPage1`), the menu strip and the status strip.

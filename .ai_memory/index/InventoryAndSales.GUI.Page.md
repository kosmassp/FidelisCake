# Directory: `InventoryAndSales/GUI/Page/`

Namespace `InventoryAndSales.GUI.Page`. Full-screen `UserControl`s hosted in `MainForm`'s tab
control. Each owns a controller from `GUI/Controller/` and exposes a `Reset()` the shell calls when
the page is shown.

Every `*.Designer.cs` / `*.resx` here is Visual Studio generated layout — no logic.

---

## `CashierPage.cs`

`public partial class CashierPage : UserControl` — the sale screen and the most-used page.

Layout: a filter text box + product grid on one side, a cart grid + payment fields on the other.

State: `_itemDictRowIdToItem` (grid row → product), `_cartDictItemToRow` /`_cartDictRowToItem`
(cart row ↔ product), `_isUpdatingItemQuantity` re-entrancy guard.

| Member | Signature | Purpose |
|---|---|---|
| `Reset` | `void Reset()` | Clears the filter, reloads the catalogue, shows all rows, starts a new cart, focuses the filter. |
| `ReloadItemList` | `private void ReloadItemList()` | Fills the product grid with `Code`, `Name`, `NetPrice`, and a `+` button column. |
| `FilterItemView` | `private Product FilterItemView(string filter, out bool byBarcode)` | Row-visibility filter. **Exact** barcode match, or case-insensitive `Name` contains / `Code` starts-with. Selects the first visible row when visibility changed. **Returns the product when exactly one row matched**, otherwise `null`. |
| `SelectNextVisibleRow` / `SelectPrevVisibleRow` | `private void ()` | Wrap-around arrow-key navigation that skips hidden rows. |
| `textBoxFilter_KeyUp` | `private void (object, KeyEventArgs)` | Strips stray `+`/`-` characters; Down/Up move the selection; Left/Right are swallowed; anything else re-filters, and **Enter on a unique match adds it to the cart** — clearing the filter afterwards if the match was by barcode (scanner flow). |
| `textBoxFilter_KeyPress` / `dataGridViewItemList_KeyPress` | `private void (object, KeyPressEventArgs)` | `+` adds the selected product, `-` removes one, then strips the character from the filter box. |
| `dataGridViewItemList_CellContentClick` | `private void (object, DataGridViewCellEventArgs)` | Clicking the `+` button column adds the row's product. |
| `AddToCart` / `RemoveFromCart` | `private void (Product)` | Delegate to the controller. |
| `dataGridViewCart_CellContentClick` / `_CellValueChanged` | `private void (object, DataGridViewCellEventArgs)` | Editing the `CartItemQuantity` cell pushes an absolute quantity to the controller; guarded by `_isUpdatingItemQuantity`. |
| `buttonClearCart_Click` | `private void (object, EventArgs)` | Confirms in Indonesian, then starts a new cart. |
| `buttonCheckout_Click` | `private void (object, EventArgs)` | Validates the payment box parses as `decimal`, calls `controller.Checkout`, shows the error or the success message. |
| `ValidateInput` | `private string (TextBox, string errorMessage)` | `decimal.TryParse` guard. |
| `RecalculateChanges` | `private void RecalculateChanges()` | Blank payment becomes `"0"`; recomputes `change = payment - total` into the read-only box. |
| `UpdateDataGridViewCart` | `void UpdateDataGridViewCart(Product product, int quantity)` | **Called from the controller's `CartChange` handler.** Updates an existing row (hiding it at quantity 0) or appends a new one with code, name, quantity, price, discount, subtotal. Thread-marshalled. |
| `UpdateTotal` | `void UpdateTotal(decimal total)` | Writes the total and recomputes change. Thread-marshalled. |
| `ResetCart` | `void ResetCart()` | Clears the cart grid, both lookup dictionaries and all input boxes. Thread-marshalled. |
| `FocusFilter` / `FocusPayment` / `FocusCheckout` | `void ()` | Targets of the F5 / F6 / F7 hotkeys in `MainForm`. |

The barcode-scanner flow relies on `FilterItemView` returning non-null only for a unique match: a
scanner types the whole barcode then sends Enter, which adds the item and clears the box for the
next scan.

⚠ Note: cart rows are hidden rather than removed at quantity 0, so `_cartDictItemToRow` indices stay
valid. Re-adding the product un-hides the same row.

---

## `TransactionUpdatePage.cs`

`public partial class TransactionUpdatePage : UserControl` — near-duplicate of `CashierPage`, used
inside `TransactionUpdateForm` to revise a past transaction.

Events: `CheckoutSucceed`, `BackClick` — the hosting form closes on either.

| Member | Signature | Purpose |
|---|---|---|
| `Init` | `void Init(string facturNumber, User user)` | Initialises the controller with the faktur and supervisor, shows the original transaction id, then `Reset()`. |
| `Reset` | `void Reset()` | Reloads the catalogue, starts a new cart, calls `controller.ResetByTransaction()` to repopulate it from the original lines, and prefills the payment box with the original payment. |
| `buttonCheckout_Click` | `private void (object, EventArgs)` | As `CashierPage`, but also raises `CheckoutSucceed` on success. |
| `buttonBack_Click` | `private void (object, EventArgs)` | Raises `BackClick`. No unsubscribe needed any more — the screen owns its own `Cart`. |
| `buttonReset_Click` | `private void (object, EventArgs)` | Re-runs `Reset()`. |
| *(all other members)* | | `ReloadItemList`, `FilterItemView`, `SelectNextVisibleRow`, `SelectPrevVisibleRow`, `AddToCart`, cart-grid handlers, `ValidateInput`, `RecalculateChanges`, `UpdateDataGridViewCart`, `UpdateTotal`, `ResetCart`, `FocusFilter`, `FocusPayment`, `FocusCheckout` — same behaviour as `CashierPage`. |

⚠ Note: this page is a copy-paste of `CashierPage` with small divergences — its `textBoxFilter_KeyUp`
tests `Keys.Add` instead of the `+` character, has no `-` handling, and `FilterItemView` lacks the
`flagChange` guard so it re-selects on every keystroke. A bug fixed in one page must be fixed in the
other. Extracting the shared grid/cart behaviour into one control would remove the duplication —
see [../rules-csharp.md](../rules-csharp.md).

---

## `MasterProductPage.cs`

`public partial class MasterProductPage : UserControl` — product master with an edit panel, a sort
combo box populated from the column map, search, and CSV import/export.

Modes: `isOnProductAddEditMode`, `isAddingProduct`, `isUpdatingProduct`.

| Member | Signature | Purpose |
|---|---|---|
| `Reset` | `void Reset()` | Leaves edit mode and reloads the grid. |
| `OnEditMasterItem` | `private void OnEditMasterItem(bool edit)` | The mode switch: toggles Add/Update/Delete vs OK/Cancel buttons, enables the detail fields, greys and disables the grid. **On leaving edit mode it re-queries and rebinds the grid** using the current search text and sort column. |
| `GenerateCode` | `private string GenerateCode(string name)` | Builds a unique product code: prefix from the name, then a zero-padded counter, checked against existing codes (max 5 characters total). |
| `GeneratePrefix` | `private string GeneratePrefix(string name)` | Uppercase initials of each word that starts with a letter. |
| `IsCodeFromName` | `private bool (string name, string code)` | Whether a code looks auto-generated from a name. Only used by commented-out code. |
| `GetItemDetail` | `private void (out code, out barcode, out name, out price, out discount)` | Reads the fields. **Percentage discounts are stored negated** (`discount = -value`), amount discounts positive. |
| `ValidateDetailItemInput` | `private string ()` | Requires code and name, validates price and the active discount box as decimals, then appends `ValidateUniqueItem()`. |
| `ValidateUniqueItem` | `private string ()` | Scans all products for a duplicate code, name or barcode, skipping the row being edited **by `Id`**, and reports every clash. It previously skipped by comparing code, name and barcode together — letting a genuine duplicate through when all three matched — and stopped at the first hit. |
| `ValidateInput` | `private string (TextBox, string)` | `decimal.TryParse` guard. |
| `UpdateDetailBarang` | `private void UpdateDetailBarang(Product product)` | Fills the detail fields from a product, decoding the sign convention into the amount or percent box. |
| `UpdateSelectedProduct` | `private void ()` | Reads the grid selection and calls `UpdateDetailBarang`. |
| `ClearFieldItemDetail` | `private void ()` | Blanks the detail panel. |
| `buttonAddProduct_Click` / `buttonEditProduct_Click` | `private void (object, EventArgs)` | Enter add / edit mode. |
| `buttonOkEdit_Click` | `private void (object, EventArgs)` | Validates, then `controller.UpdateItem` or `controller.AddItem`, then leaves edit mode. |
| `buttonCancelEdit_Click` | `private void (object, EventArgs)` | Leaves edit mode and restores the panel from the selection. |
| `buttonDeleteProduct_Click` | `private void (object, EventArgs)` | Confirms, then soft-deletes. |
| `buttonGenerateCode_Click` | `private void (object, EventArgs)` | Fills the code box from the name. |
| `buttonSearch_Click` | `private void (object, EventArgs)` | Stores `_searchedText` and reloads. |
| `radioButtonDiscount_CheckedChanged` | `private void (object, EventArgs)` | `_isChanging`-guarded toggle enabling exactly one discount box. |
| `buttonExportItems_Click` | `private void (object, EventArgs)` | `SaveFileDialog` → CSV with header `Id,Code,Barcode,Name,Price,Discount`, UTF-8. Catches write errors with an Indonesian message. |
| `QuoteField` | `private string QuoteField(string field)` | Trims and RFC-4180 quotes when the value contains `,`, `"` or a newline. |
| `buttonImport_Click` | `private void (object, EventArgs)` | `OpenFileDialog` → `ImportProductsFromCsv` → `controller.SetItemForImport` → `Reset()`. |
| `ImportProductsFromCsv` | `private List<Product> (string filePath)` | Skips the header, requires ≥ 6 fields per line, parses each row, title-cases the name. Per-row parse errors show a message box and skip the row. |
| `ParseCsvLine` | `private string[] (string line)` | Hand-written CSV parser handling quotes and `""` escapes. |
| `ConvertToTitleCase` | `public static string (string text)` | `en-US` `TextInfo.ToTitleCase`. |

Import semantics: `Id == 0` (or empty) inserts, any other `Id` updates that row — so an
export/edit/import round trip updates in place. See
[../business-product-master.md](../business-product-master.md).

Import now confirms the row count first, runs in **one transaction** (so a failure leaves the
catalogue untouched), and reports how many rows were added and updated instead of an unconditional
*"File imported successfully!"*. `ValidateUniqueItem` no longer reads
`comboBoxSort.SelectedItem.ToString()` without a null guard.

Saving and deleting are wrapped in `try`/`catch` reporting an Indonesian message, because the data
layer now throws on a failed write rather than silently returning `false`.

---

## `MasterUserPage.cs`

`public partial class MasterUserPage : UserControl` — user master, same add/edit-mode shape as the
product page. The role combo box is bound to `Enum.GetValues(typeof(RoleOptions))`.

| Member | Signature | Purpose |
|---|---|---|
| `Reset` | `void Reset()` | Leaves edit mode, which rebinds the grid. |
| `OnEditMasterUser` | `private void OnEditMasterUser(bool edit)` | Mode switch; on leaving edit mode reloads `controller.GetUsers()` into the grid. |
| `GetUserDetail` | `private void (out username, out name, out password, out role)` | Reads the fields; `role` comes from the combo's `RoleOptions` value. |
| `ValidateDetailUser` | `private string ()` | Requires username and name always; the password pair only when adding a user or actually changing it. Also rejects a duplicate username. |
| `UpdateDetailUser` | `private void UpdateDetailUser(User user)` | Fills the panel behind a `_loadingDetail` guard and shows a fixed `********` placeholder in both password boxes. It used to show the **first 8 characters of the stored hash**, which the controller compared against to decide whether to re-hash. |
| `PasswordField_TextChanged` | `private void (object, EventArgs)` | Sets `_passwordChanged` — the flag the controller now uses. |
| `UpdateSelectedUser` | `private void ()` | Reads the grid selection into the panel. |
| `ClearFieldUser` | `private void ()` | Blanks the panel. |
| `buttonAddUserMaster_Click` / `buttonEditUserMaster_Click` | `private void (object, EventArgs)` | Enter add / edit mode. |
| `buttonOkUserMaster_Click` | `private void (object, EventArgs)` | Validates then adds or updates. |
| `buttonCancelUserMaster_Click` | `private void (object, EventArgs)` | Leaves edit mode, restores from selection. |
| `buttonDeleteUserMaster_Click` | `private void (object, EventArgs)` | Confirms, then soft-deletes. |
| `dataGridViewUserMaster_SelectionChanged` / `_Click` | `private void (object, EventArgs)` | Keep the detail panel in sync with the selection. |

Saving and deleting are wrapped in `try`/`catch` reporting an Indonesian message, because the data
layer now throws on a failed write rather than silently returning `false`.

---

## `LoginPage.cs`

`public partial class LoginPage : UserControl` — username and password entry.

| Member | Signature | Purpose |
|---|---|---|
| `Reset` | `void Reset()` | Clears both boxes and the error label, focuses the username box. |
| `buttonLogin_Click` | `private void (object, EventArgs)` | Calls `controller.Login` behind a wait cursor with the button disabled — password verification is deliberately slow. On failure shows *"Username atau password tidak benar"* and clears the password box; a database failure is logged and reported separately. On success navigation happens via `LoginManager.OnActiveUserChanged`. |
| `textBoxUsername_KeyPress` | `private void (object, KeyPressEventArgs)` | Enter moves focus to the password box. |
| `textBoxPassword_KeyPress` | `private void (object, KeyPressEventArgs)` | Enter submits. |
| `textBoxUsername_Enter` / `textBoxPassword_Enter` | `private void (object, EventArgs)` | Select-all on focus. |

---

## `ReportDisplayPage.cs`

`public partial class ReportDisplayPage : UserControl` — date range + report tabs + HTML export
buttons.

| Member | Signature | Purpose |
|---|---|---|
| `RefreshOnDisplay` | `void RefreshOnDisplay()` | Resets both date pickers to today. Called when the shell switches to this tab. Thread-marshalled. |
| `buttonShowReportSummary_Click` | `private void (object, EventArgs)` | Rebuilds the tab set (cashier, product, transaction), runs the summary reports, and adds the detail tab + report when the checkbox is ticked. |
| `RunReport` | `private void RunReport(Action action)` | Wait cursor, plus a single place that logs a report failure and shows an Indonesian message instead of letting it escape into the UI. |
| `UpdateReportDataGridView` | `void UpdateReportDataGridView(DataTable byProduct, DataTable byTransaction, DataTable byCashier)` | Binds the three summary grids. Named parameters replace the earlier `DataTable[]`, whose positional contract depended on the controller assembling the array correctly. Thread-marshalled. |
| `UpdateReportDetailDataGridView` | `void UpdateReportDetailDataGridView(DataTable dataTable)` | Binds the detail grid. Its `InvokeRequired` branch now marshals to **itself**; it previously handed off to the three-grid overload, which would have bound the wrong grids and thrown had it ever been called from another thread. |
| `buttonReportPerKasir_Click` / `PerTransaksi` / `PerProduct` / `PerItem` | `private void (object, EventArgs)` | Generate and open the corresponding HTML report, through `RunReport`. |

---

## `PromoPage.cs`

`public partial class PromoPage : UserControl` — an empty draft screen (constructor plus a no-op
`panel1_Paint`). Compiled into the assembly but **not placed on any tab or form**, so it is
unreachable at runtime. Introduced by the `draft promo` commit.

# Directory: `InventoryAndSales/GUI/Popup/SettingPage/`

Namespace `InventoryAndSales.GUI.Popup.SettingPage`. Individual pages hosted inside `SettingForm`.
Each is a `UserControl` paired with a controller in `GUI/Controller/SettingPage/`.

---

## `ShopSetting.cs`

`public partial class ShopSettingForm : UserControl` — names the shop. Listed as **Toko**, requires
`AccessOption.Master`.

| Member | Signature | Purpose |
|---|---|---|
| `ShopSettingForm_Load` | `private void (object, EventArgs)` | `DesignMode` guard, creates the controller, fills the box behind a `_loading` guard, writes the explanatory text. |
| `RefreshInheritedNote` | `private void ()` | Says so when the box was filled from the receipt header rather than from a saved name — otherwise a name nobody typed here looks like a bug. |
| `buttonSave_Click` | `private void (object, EventArgs)` | Validates, saves, then **reads the value back**: `SettingsService.SetString` only logs when the row is missing, and reporting success without checking would be a lie. |

The name reaches the window title through `MainForm.RefreshWindowTitle`, which runs when this dialog
closes — the dialog is modeless, so the refresh hangs off `FormClosed`.

---

## `HeaderAndFooter.cs`

`public partial class HeaderAndFooterForm : UserControl` — edit the receipt header and footer with a
live preview.

⚠ Note: file name (`HeaderAndFooter.cs`) and type name (`HeaderAndFooterForm`) differ, and the type
is a `UserControl` despite the `Form` suffix.

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `HeaderAndFooterForm()` | Lays out the control and disables Save. **No database access** — the controller is created in `Load`. |
| `HeaderAndFooterForm_Load` | `private void (object, EventArgs)` | **Returns early when `DesignMode`**, then creates the controller and loads the stored header/footer behind a `_loading` guard so filling the boxes does not mark the form dirty. |
| `SetPaymentNoteFont` | `internal void SetPaymentNoteFont(Font font)` | Applies the real receipt font (`Courier New 9pt`) to the preview box so alignment matches the printer. |
| `BuildExample` | `private void BuildExample()` | Calls `_controller.GetExample(header, footer)` and joins the resulting `StringPrint` lines into the preview text box. |
| `textBoxHeader_TextChanged` / `textBoxFooter_TextChanged` | `private void (object, EventArgs)` | Rebuild the preview and enable Save. |
| `buttonSave_Click` | `private void (object, EventArgs)` | Persists both values, disables Save, shows *"Tampilan Nota Berhasil Terubah."*; on exception logs and shows *"Tampilan Nota Belum Berubah."*. |

The preview renders through the same static `CashierManager.GeneratePaymentNote` the printer uses,
so what is previewed is what prints. Alignment (`StringFormat`) is lost in the text box — only the
line content is shown.

Stored as `M_SETTINGS` rows `HEADER` and `FOOTER` in group `GENERAL`, with newlines encoded as
`%NEW_LINE%`. See [../business-settings.md](../business-settings.md).

---

## `ReportSetting.cs`

`public partial class ReportSettingForm : UserControl` — choose where reports are written.

| Member | Signature | Purpose |
|---|---|---|
| `ReportSettingForm_Load` | `private void (object, EventArgs)` | `DesignMode` guard, creates the controller, fills the path box, refreshes the asset status. |
| `RefreshAssetStatus` | `private void ()` | Green when the assets are ready; red when the bundle is missing from the application folder, or cannot be unpacked into the chosen folder. |
| `buttonBrowse_Click` | `private void (object, EventArgs)` | `FolderBrowserDialog`, preselecting the current folder. |
| `buttonDefault_Click` | `private void (object, EventArgs)` | Resets the box to `<Documents>\FidelisCake\Laporan`. |
| `buttonOpenFolder_Click` | `private void (object, EventArgs)` | Opens the folder in Explorer. |
| `buttonSave_Click` | `private void (object, EventArgs)` | Validates, saves and provisions the assets behind a wait cursor. |

---

## `SecuritySetting.cs`

`public partial class SecuritySettingForm : UserControl` — allow or forbid the built-in recovery
account.

| Member | Signature | Purpose |
|---|---|---|
| `SecuritySettingForm_Load` | `private void (object, EventArgs)` | `DesignMode` guard, creates the controller, reads the current value, and writes the explanatory text naming the account. |
| `RefreshWarning` | `private void ()` | Explains, and disables the checkbox, when no other account can administer the system. |
| `buttonSave_Click` | `private void (object, EventArgs)` | Saves; on refusal shows the reason and puts the checkbox back. |

The page states plainly *why* the account exists — administrators have deleted their own account —
so whoever is deciding whether to turn it off understands what they are giving up.

---

### Adding a new settings page

1. Add a `UserControl` here, plus a controller in `GUI/Controller/SettingPage/`.
2. Create the controller in `Load`, not the constructor, and guard with `if (DesignMode) return;` —
   otherwise the Visual Studio designer tries to reach SQL Server.
3. Register it with one line in `SettingForm.Initialize()`; it is built lazily on first selection.
4. Set its `Tag` — the list box shows `Tag` as the page label.

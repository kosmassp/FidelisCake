# Directory: `InventoryAndSales/GUI/Controller/SettingPage/`

Namespace `InventoryAndSales.GUI.Controller.SettingPage`. Controllers for individual pages inside
the settings dialog. Mirrors the view folder `GUI/Popup/SettingPage/`.

---

## `ShopSettingController.cs`

`class ShopSettingController` (internal) — backs `ShopSettingForm`. A thin pass-through to
`ShopService`: `GetName`, `IsNameInherited`, `ValidateName`, `Save`. The resolution and validation
rules live in the service, not here, because the reports read the same name.

---

## `HeaderAndFooterController.cs`

`class HeaderAndFooterController` (internal) — backs `HeaderAndFooterForm`, the receipt
header/footer editor. Depends on `CashierManager`.

The constructor immediately pushes the receipt font into the view
(`headerAndFooter.SetPaymentNoteFont(_cashierManager.GetPrintFont())`) so the preview is rendered in
the same `Courier New 9pt` the printer uses.

| Member | Signature | Purpose |
|---|---|---|
| `GetHeader` | `internal string GetHeader()` | Stored `HEADER` setting with `%NEW_LINE%` decoded. |
| `GetFooter` | `internal string GetFooter()` | Stored `FOOTER` setting, decoded. |
| `SetHeader` | `void SetHeader(string text)` | Encodes and persists to `M_SETTINGS`. |
| `SetFooter` | `void SetFooter(string text)` | Encodes and persists. |
| `GetExample` | `internal List<StringPrint> GetExample(string headers, string footers)` | Builds a **fake** transaction entirely in memory and renders it through `ReceiptBuilder.Build`, so the preview uses the real receipt layout code with no database access and no printing. |

### The sample transaction built by `GetExample`

- Header fields: `Notes = "CONTOH"`, `Time = DateTime.Now`, `Factur = "KODE_UNIK_FACTUR"`,
  `Payment = 0`, `UserId = 0`, `CustomerId = 0`.
- Three lines, `i = 1..3`: `ProductName = "NAMA_PRODUK {i}"`, `ProductPrice = i * 5000`,
  `Quantity = i * 2`, `ProductDiscount = (i / 3) * 500` — **integer division**, so only line 3 gets a
  500 discount; lines 1 and 2 get 0.
- Totals accumulated the same way `CashierManager.GenerateTransactionAndDetails` does;
  `Exchange = Payment - Total` (negative, since payment is 0).
- Cashier: `Name = "NAMA_KASIR"`, `Id = 0`, `Role = 1`.

This is a good example of the pattern worth preserving: `ReceiptBuilder.Build` is a **static pure
function**, so the preview and the real print path can never diverge.

---

## `ReportSettingController.cs`

`internal class ReportSettingController` — backs `ReportSettingForm`. Depends on `ReportService`.

| Member | Signature | Purpose |
|---|---|---|
| `GetReportDirectory` | `string ()` | Currently configured folder. |
| `GetDefaultReportDirectory` | `string ()` | `<Documents>\FidelisCake\Laporan`. |
| `Validate` | `string (string directory)` | Empty when the folder can be used, otherwise an Indonesian message. |
| `Save` | `string (string directory)` | Validates, persists, and unpacks the assets immediately — so a problem shows up here rather than the first time somebody prints a report. |
| `AreAssetsReady` | `bool ()` | Whether the stylesheet and script are in place. |
| `IsAssetBundlePresent` | `bool ()` | Whether `reportassets.zip` is next to the executable. |
| `OpenReportFolder` | `void ()` | Opens the folder in Explorer. |

---

## `SecuritySettingController.cs`

`internal class SecuritySettingController` — backs `SecuritySettingForm`. Depends on `LoginManager`.

| Member | Signature | Purpose |
|---|---|---|
| `IsBuiltInAdminAllowed` | `bool ()` | Current value of `ALLOW_BUILTIN_ADMIN`. |
| `BuiltInAdminUsername` | `string { get; }` | Shown in the explanatory text. |
| `HasRealAdministrator` | `bool ()` | Whether any `M_USERS` account holds `AccessOption.Master`. |
| `Save` | `string (bool allowBuiltInAdmin)` | Persists, but **refuses to disable the recovery account while no real administrator exists** — that is precisely the lockout it guards against. Returns an Indonesian message on refusal. |

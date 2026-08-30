# Settings

The application has two separate configuration mechanisms. Know which one you need.

| | `App.config` | `M_SETTINGS` table |
|---|---|---|
| Changed by | An installer or an administrator editing the file | The operator, in the app |
| Applies from | Application restart | Immediately |
| Read via | `ConfigurationManager.AppSettings[...]` | `SettingsService` |
| Holds | `DatabaseProvider`, `ConnectionString` | receipt notes, report folder, security, printer, EDC terminals |

**Rule of thumb:** infrastructure the operator must not touch goes in `App.config`; anything a shop
owner would reasonably want to change goes in `M_SETTINGS`.

## `M_SETTINGS`

| Column | Purpose |
|---|---|
| `Id` | Identity. |
| `Key` | Setting name. **Reserved SQL word — always bracket as `[Key]`.** |
| `Group` | Category, currently always `GENERAL`. **Also reserved.** |
| `Value` | Current value (`text`). |
| `Default` | Seeded default, for a future reset feature. Never read today. |

Seeded on every startup by `DBUtility.UpsertSettingRow()`, which inserts only when the row is
absent, so operator edits survive restarts:

| Key | Group | Seeded value |
|---|---|---|
| `HEADER` | `GENERAL` | `FIDELIS CAKE AND BAKERY` / `JL MAYJEND SUTOYO NO 1` / `BANJARNEGARA` / `(0286) 594573` |
| `FOOTER` | `GENERAL` | `TERIMA KASIH` / `SELAMAT MENIKMATI` |
| `REPORT_DIRECTORY` | `REPORT` | `<Documents>\FidelisCake\Laporan` |
| `ALLOW_BUILTIN_ADMIN` | `SECURITY` | `true` |
| `EDC_TERMINALS` | `GENERAL` | empty — a shop with no card terminals is offered none |
| `QRIS_PROVIDERS` | `GENERAL` | empty — same for QRIS |
| `PRINTER_NAME` | `PRINTER` | seeded from the `PrinterName` entry in `App.config`; empty means the Windows default printer |
| `PRINTER_PAPER_WIDTH_MM` | `PRINTER` | `67` |
| `UPDATE_MANIFEST_URL` | `UPDATE` | seeded from the `UpdateManifestUrl` entry in `App.config` when present (empty = checking off); with no entry at all, the built-in GitHub `version.txt` address (`SettingKeys.DefaultUpdateManifestUrl`) |

One value migration exists on top of insert-if-missing seeding: `DBUtility.RetireSupersededManifestUrl`
rewrites a stored `UPDATE_MANIFEST_URL` that exactly matches a retired default
(`SettingKeys.RetiredUpdateManifestUrls`, the pre-GitHub Google Doc) to the current built-in address.
Anything else stored there is treated as deliberate configuration and never touched.

**The seed list is data, not code:** `Business/SettingKeys.cs → Seed()` returns the rows a database
is expected to have, and `UpsertSettingRow` inserts whichever are missing. Because installations sit
on many different versions with no migration history, adding a key there is all that is needed for
an old site to pick it up on its next launch.

Reads go through `Business/SettingsService.cs`, which falls back to a caller-supplied default rather
than throwing when the **row is missing**. A missing row can therefore no longer break the feature
that reads it — `CashierManager` previously called `.First()` and would throw if `HEADER` had been
deleted.

⚠ An existing row's value is returned **as-is, empty included**. Empty is a real answer for some
settings — it is how "use the Windows default printer" is expressed — and an earlier version fell
back to the seeded `Default` for an empty value, which made such a value impossible to save: it was
written, then read back as whatever the row was first seeded with. The `Default` column is now only
a record of the original seed.

### Newline encoding

`Value` is a single-line string, so line breaks are stored as the literal token `%NEW_LINE%`.
`SettingsService.EncodeNewLines` / `DecodeNewLines` handle it, and `GetMultiLine` / `SetMultiLine`
wrap the pair. Both are null-safe.

Encoding normalises `\r\n`, `\n` **and** `\r`, so a value edited outside the application — in SQL
Server Management Studio, say — still round-trips. (It previously replaced only
`Environment.NewLine`, so a bare `\n` was left in the stored value and came back as one long line.)

## Editing the receipt header and footer

*Pengaturan* → `SettingForm` → **Header and Footer** page.

`HeaderAndFooterForm` shows two multi-line text boxes and a live preview:

1. `Load` reads the stored values (decoded) — guarded by `if (DesignMode) return;` so the Visual
   Studio designer never hits the database.
2. The controller pushes `CashierManager.GetPrintFont()` into the preview box, so the preview is
   rendered in the same `Courier New 9pt` the printer uses.
3. Every keystroke calls `BuildExample()`, which asks the controller for a sample receipt and joins
   the lines into the preview.
4. Save persists both values (encoded), disables the Save button, and reports
   *"Tampilan Nota Berhasil Terubah."* — or *"Tampilan Nota Belum Berubah."* on failure.

### Why the preview is trustworthy

`HeaderAndFooterController.GetExample` builds a **fake** transaction in memory — three lines
`NAMA_PRODUK 1..3`, faktur `KODE_UNIK_FACTUR`, cashier `NAMA_KASIR` — and renders it through
`ReceiptBuilder.Build`, the very function the printer uses. No database, no printer, no divergence.
This is the pattern to preserve when changing receipt layout.

⚠ The sample's discount is `(i / 3) * 500` with **integer division**, so only the third line shows a
discount. Fine as a sample; just do not read it as a formula.

⚠ Alignment (`StringFormat`) is not reproduced — a plain text box shows content only, not centring.

## The settings dialog

*Pengaturan* opens `SettingForm`: a list of pages on the left, the selected page on the right.
Five pages are registered today.

| Page (`Tag`) | Requires | Control | Controller |
|---|---|---|---|
| Nota | `Master` | `HeaderAndFooterForm` | `HeaderAndFooterController` |
| Pembayaran | `Master` | `PaymentOptionSettingForm` | `PaymentOptionSettingController` |
| Laporan | `Master` | `ReportSettingForm` | `ReportSettingController` |
| Printer | **`Admin`** | `PrinterSettingForm` | `PrinterSettingController` |
| Keamanan | **`Admin`** | `SecuritySettingForm` | `SecuritySettingController` |

**Pages are gated by permission.** Each declares an `AccessOption`, and `SettingForm.Initialize`
only lists the ones the signed-in user holds — a cashier who reaches the dialog sees nothing and it
closes. Printer and security are administrator-only: one decides where every receipt goes, the other
whether the recovery account still works. Only `RoleOptions.Admin` (1023) carries the `Admin` bit, so
a Supervisor (14) sees Nota and Laporan but not those two.

Pages are **built lazily** — `SettingForm.Initialize()` registers a `Tag` plus a factory, and the
control is created the first time its row is selected. Opening Settings therefore no longer runs
every page's database reads. Selecting a page **replaces** the hosted control; it previously added
each one on top of the last, leaving every page ever opened alive underneath the visible one.

⚠ `SettingForm` is still opened with `Show()` (modeless), so several settings windows can be open at
once, each with its own copy of the pages.

⚠ `SettingPageController` remains an empty placeholder.

## Adding a new setting

1. Add the key, group and default to `Business/SettingKeys.cs` — both the constant and an entry in
   `Seed()`. Startup seeding then reaches every installation, however old.
2. Read and write it through `SettingsService` (`GetString`/`GetBool`/`GetMultiLine` and their
   setters), always passing a sensible fallback.
3. Add a `UserControl` under `GUI/Popup/SettingPage/` with a controller in
   `GUI/Controller/SettingPage/`.
4. Create the controller in the control's `Load`, not its constructor, and guard with
   `if (DesignMode) return;` — otherwise the Visual Studio designer tries to reach SQL Server.
5. Register it with one line in `SettingForm.Initialize()`, and set the control's `Tag` — that is
   the label shown in the list.

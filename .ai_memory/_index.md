# Master File Index — FidelisCake / InventoryAndSales

Fast lookup for every source file in the solution. Find the file here, then open the linked
directory page in [`index/`](index/) for its types, methods and purpose.

Before writing code, read [`rules-csharp.md`](rules-csharp.md) — namespaces, layering, SOLID/KISS,
and the framework-only dependency policy.

**Solution:** `InventoryAndSalesProject.sln` — 2 projects, .NET Framework 4.6, C# WinForms.

- `InventoryAndSales` — `WinExe`, the POS application. References `SimpleCommon`, `log4net`.
- `SimpleCommon` — `Library`, reusable WinForms controls and utilities. No project dependencies.

`*.Designer.cs` files are Visual Studio generated form layout. They contain no business logic;
edit them through the WinForms designer, not by hand.

---

## Quick answers — "where do I go to change…?"

| I want to change… | Go to |
|---|---|
| Cart mechanics and totals | `InventoryAndSales/Business/Cart.cs` |
| Discount and price math | `InventoryAndSales/Database/Model/Product.cs`, `TransactionDetail.cs` |
| Receipt layout | `InventoryAndSales/Business/ReceiptBuilder.cs` |
| Checkout, revision, cancellation | `InventoryAndSales/Business/CashierManager.cs` |
| Checkout validation & error messages | `InventoryAndSales/GUI/Controller/CashierController.cs` |
| Payment methods (cash / EDC) | `InventoryAndSales/Business/PaymentDetail.cs` |
| EDC terminal / QRIS provider lists | *Pengaturan → Pembayaran*, stored in `M_SETTINGS` |
| The shop's name (window title, report heading) | *Pengaturan → Toko*, resolved by `InventoryAndSales/Business/ShopService.cs` |
| Who changed what (the audit trail) | `InventoryAndSales/Business/AuditService.cs`, table `T_AUDIT_LOG` |
| Update checking / the version file in the cloud | `InventoryAndSales/Business/UpdateService.cs`, `UpdateManifest.cs` |
| How an update is actually installed | `InventoryAndSales/Utility/UpdateInstaller.cs` |
| Login / password check / recovery account | `InventoryAndSales/Business/LoginManager.cs` |
| Password hashing | `SimpleCommon/Utility/PasswordHasher.cs` |
| Who sees which menu | `InventoryAndSales/GUI/MainForm.cs` → `EnableMenu`, `Enumeration/AccessOption.cs` |
| SQL for a report | `InventoryAndSales/Database/DataAccess/CustomDao.cs` |
| How a report column is formatted, aligned or totalled | `InventoryAndSales/Business/ReportTable.cs` |
| What a generated report looks like | `InventoryAndSales/Utility/HtmlReportGenerator.cs` |
| Where reports are written / their assets | `InventoryAndSales/Business/ReportService.cs` |
| A new configurable setting | `InventoryAndSales/Business/SettingKeys.cs` → `Seed()` |
| A table's columns as the app sees them | `InventoryAndSales/Database/DataTable/DataTableList.cs` |
| The schema itself (tables, indexes, migrations) | `InventoryAndSales/Database/Schema/DatabaseSchema.cs` |
| Anything database-product specific | `InventoryAndSales/Database/Dialect/` |
| Support for another database | Add an `ISqlDialect`; nothing else changes |
| Auto table creation / migration on startup | `InventoryAndSales/Database/DBUtility.cs` |
| Which database, connection string | `InventoryAndSales/App.config` |
| Printer and paper width | *Pengaturan → Printer* (Admin), stored in `M_SETTINGS` |
| Product CSV import/export | `InventoryAndSales/GUI/Page/MasterProductPage.cs` |
| The physical print routine | `SimpleCommon/Utility/PrinterUtility.cs` |

---

## InventoryAndSales (application)

### Root — [index/InventoryAndSales.md](index/InventoryAndSales.md)

| File | Purpose |
|---|---|
| `InventoryAndSales/Program.cs` | Entry point. Dispatches `--apply-update` to `UpdateInstaller` **first**, then forces `en-US` culture, runs `SplashForm` then `MainForm`, logs unhandled exceptions and the environment. |
| `InventoryAndSales/App.config` | `DatabaseProvider` and `ConnectionString`. Also `PrinterName` and `UpdateManifestUrl`, each read once to seed its setting. |
| `InventoryAndSales/log4net.config` | Logging to console + `Log\log.txt`, **appending**, rolled by date and size, 30 backups. |
| `InventoryAndSales/InventoryAndSales.csproj` | Project file, compile list, references. |

### Business — [index/InventoryAndSales.Business.md](index/InventoryAndSales.Business.md)

| File | Purpose |
|---|---|
| `Business/BusinessFactory.cs` | Singleton composition root for all business managers. |
| `Business/PaymentDetail.cs` | How a sale was paid: method, amount tendered, terminal or provider, QRIS code type. Owns the change rule. |
| `Business/PaymentOptionService.cs` | The configured EDC terminals and QRIS providers. |
| `Business/Cart.cs` | The basket for one screen: lines, quantities, total, `CartChange` event. One instance per controller. |
| `Business/CashierManager.cs` | Checkout, revision checkout, cancel, receipt printing, header/footer notes. |
| `Business/ReceiptBuilder.cs` | **Pure function** turning a sale into printable lines. Shared by the printer and the settings preview. |
| `Business/LoginManager.cs` | Authentication policy: credentials, the built-in recovery account, transparent password re-hashing. |
| `Business/MasterManager.cs` | Product and user master CRUD (soft delete). |
| `Business/ReportManager.cs` | Facade over report queries. |
| `Business/ReportTable.cs` | A report's rows worked out for display: column kinds, formatting, totals. |
| `Business/ReportDocument.cs` | A report with its title, shop, period and provenance attached. |
| `Business/ReportColumnKind.cs` | `Text` / `Number` / `Date` — what a report column holds. |
| `Business/ReportService.cs` | Report folder resolution and copying the DataTables assets out of `Report`. |
| `Business/SettingKeys.cs` | Every `M_SETTINGS` key, its group and its seeded default. |
| `Business/ShopService.cs` | What this shop is called: the setting, its receipt-header fallback, and validation. |
| `Business/AuditService.cs` | Records who changed what into `T_AUDIT_LOG`. Never breaks the operation it audits. |
| `Business/UpdateService.cs` | Reads the version file in the cloud; downloads and unpacks a release. |
| `Business/UpdateManifest.cs` | The hand-edited `Version:` / `Drive:` / `File:` text file, and Google Drive link handling. |
| `Business/SettingsService.cs` | Typed reads/writes over `M_SETTINGS`, with fallbacks instead of throws. |
| `Business/ViewManager.cs` | Transaction browsing (used by the history picker). |

### Business/Enum — [index/InventoryAndSales.Business.Enum.md](index/InventoryAndSales.Business.Enum.md)

| File | Purpose |
|---|---|
| `Business/Enum/TransactionStatus.cs` | `INITIATE` / `SUCCESS` / `FAILED` checkout outcome. |

### Database — [index/InventoryAndSales.Database.md](index/InventoryAndSales.Database.md)

| File | Purpose |
|---|---|
| `Database/DBFactory.cs` | Singleton. Resolves the provider, owns DAOs, managers, and the single ambient transaction. Also `DbParam`, the parameter helper. |
| `Database/DbScope.cs` | The connection + transaction a command runs on, taken as one snapshot. Always used with `using`. |
| `Database/DBUtility.cs` | Boot-time schema reconciliation, driven by the dialect; `ExecuteNonQuery` / `ExecuteScalar` and their best-effort `Try*` variants. |

### Database/Schema — [index/InventoryAndSales.Database.Schema.md](index/InventoryAndSales.Database.Schema.md)

| File | Purpose |
|---|---|
| `Database/Schema/DatabaseSchema.cs` | **The schema declared once, product-independent** — tables, columns, indexes, and the columns older installations may be missing. |

### Database/Dialect — [index/InventoryAndSales.Database.Dialect.md](index/InventoryAndSales.Database.Dialect.md)

| File | Purpose |
|---|---|
| `Database/Dialect/ISqlDialect.cs` | Everything that differs between database products. |
| `Database/Dialect/SqlDialectBase.cs` | The parts that are the same everywhere. |
| `Database/Dialect/SqlServerDialect.cs` | Microsoft SQL Server — the default. |
| `Database/Dialect/PostgreSqlDialect.cs` | PostgreSQL, via Npgsql. |
| `Database/Dialect/SqliteDialect.cs` | SQLite, via System.Data.SQLite. |
| `Database/Dialect/SqlDialectFactory.cs` | Picks the dialect from the `DatabaseProvider` setting. |
| `Database/Dialect/ProviderLoader.cs` | Loads the provider assembly with **no config** — replaces both the `DbProviderFactories` registration and the binding redirects. |

### Database/DataAccess — [index/InventoryAndSales.Database.DataAccess.md](index/InventoryAndSales.Database.DataAccess.md)

| File | Purpose |
|---|---|
| `Database/DataAccess/BaseDao.cs` | Generic CRUD over `IDataTable` metadata; parameterised, dialect-quoted SQL. |
| `Database/DataAccess/CustomDao.cs` | All hand-written report/view SQL. CRUD methods throw. |
| `Database/DataAccess/CustomerDao.cs` | `BaseDao<Customer>`, no extras. |
| `Database/DataAccess/ProductDao.cs` | `BaseDao<Product>`, no extras. |
| `Database/DataAccess/SettingDao.cs` | `SettingConfigurationDao : BaseDao<SettingConfiguration>`. |
| `Database/DataAccess/TransactionDao.cs` | Adds `FindByFactur`. |
| `Database/DataAccess/TransactionDetailDao.cs` | Adds `FindByTransactionId`. |
| `Database/DataAccess/UserDao.cs` | `BaseDao<User>`, no extras. |
| `Database/DataAccess/AuditLogDao.cs` | `BaseDao<AuditLog>`, no extras. |

### Database/DataTable — [index/InventoryAndSales.Database.DataTable.md](index/InventoryAndSales.Database.DataTable.md)

| File | Purpose |
|---|---|
| `Database/DataTable/IDataTable.cs` | Table name / primary key / column list contract. |
| `Database/DataTable/DataTable.cs` | Immutable `IDataTable` implementation. |
| `Database/DataTable/DataTableList.cs` | **Single source of truth** mapping each model type to its table and columns. |

### Database/Manager — [index/InventoryAndSales.Database.Manager.md](index/InventoryAndSales.Database.Manager.md)

| File | Purpose |
|---|---|
| `Database/Manager/BaseManager.cs` | Generic manager; wraps `Save` in a DB transaction. |
| `Database/Manager/CustomManager.cs` | Runs report DAO calls, converts rows to `Dictionary<string,string>`. |
| `Database/Manager/CustomerManager.cs` | Pass-through for `Customer`. |
| `Database/Manager/ProductManager.cs` | `GetAllAvailable` — name search excluding soft-deleted rows. |
| `Database/Manager/SettingManager.cs` | `SettingConfigurationManager.FindByKey`. |
| `Database/Manager/TransactionDetailManager.cs` | `FindByTransactionId`, hydrates `ProductName`. |
| `Database/Manager/TransactionManager.cs` | Atomic save / revise / cancel of header + details. |
| `Database/Manager/UserManager.cs` | Credential lookup (incl. hardcoded fallback account), non-deleted list. |
| `Database/Manager/AuditLogManager.cs` | `BaseManager<AuditLog>`, write only in practice. |

### Database/Model — [index/InventoryAndSales.Database.Model.md](index/InventoryAndSales.Database.Model.md)

| File | Purpose |
|---|---|
| `Database/Model/BaseObject.cs` | Abstract `this[columnName]` indexer contract used by the DAO layer. |
| `Database/Model/CustomQuery.cs` | Dynamic row bag for report queries; formats `DateTime` on write. |
| `Database/Model/Customer.cs` | `M_CUSTOMERS` row. |
| `Database/Model/Product.cs` | `M_PRODUCTS` row + `DiscountAmount` / `NetPrice` / `DisplayDiscount` pricing logic. |
| `Database/Model/SettingConfiguration.cs` | `M_SETTINGS` row. |
| `Database/Model/Transaction.cs` | `T_TRANSACTIONS` header row, incl. `Revision`. |
| `Database/Model/TransactionDetail.cs` | `T_TRANSACTION_DETAILS` line + `UpdateQuantity` subtotal recalculation. |
| `Database/Model/User.cs` | `M_USERS` row + `RoleOption`. |
| `Database/Model/AuditLog.cs` | `T_AUDIT_LOG` row: actor, time, workstation, what was touched, before/after. |

### Enumeration — [index/InventoryAndSales.Enumeration.md](index/InventoryAndSales.Enumeration.md)

| File | Purpose |
|---|---|
| `Enumeration/AccessOption.cs` | `[Flags] AccessOption` permission bits and `RoleOptions` presets. |
| `Enumeration/DisplayPage.cs` | Which main tab is showing. |

### GUI — [index/InventoryAndSales.GUI.md](index/InventoryAndSales.GUI.md)

| File | Purpose |
|---|---|
| `GUI/MainForm.cs` | Shell window: menus, tab switching, F5/F6/F7 hotkeys, status bar. |
| `GUI/MainForm.Designer.cs` | Generated layout. |
| `GUI/MainFormController.cs` | Menu actions: logout, reprint, revise/delete transaction (with supervisor step-up), daily total. |
| `GUI/SplashForm.cs` | Runs the DB schema check on a background thread with a progress bar. |
| `GUI/SplashForm.Designer.cs` | Generated layout. |

### GUI/Controller — [index/InventoryAndSales.GUI.Controller.md](index/InventoryAndSales.GUI.Controller.md)

| File | Purpose |
|---|---|
| `GUI/Controller/CashierController.cs` | Sale screen logic: checkout validation, cart mutation, cart-change → view push. |
| `GUI/Controller/LoginController.cs` | Thin login delegate. |
| `GUI/Controller/MasterProductController.cs` | Product add/update/delete/search, bulk import dispatch. |
| `GUI/Controller/MasterUserController.cs` | User add/update/delete, password re-hash rule. |
| `GUI/Controller/ReportDisplayController.cs` | Builds report `DataTable`s and HTML report files. |
| `GUI/Controller/UpdateController.cs` | Check, confirm, stage and hand over to the installer. |
| `GUI/Controller/SettingPageController.cs` | Placeholder for the settings dialog. |
| `GUI/Controller/TransactionUpdateController.cs` | Revision screen: reload old cart, re-checkout as a revision. |

### GUI/Controller/SettingPage — [index/InventoryAndSales.GUI.Controller.SettingPage.md](index/InventoryAndSales.GUI.Controller.SettingPage.md)

| File | Purpose |
|---|---|
| `GUI/Controller/SettingPage/HeaderAndFooterController.cs` | Read/write receipt header & footer; build a live sample receipt. |
| `GUI/Controller/SettingPage/PrinterSettingController.cs` | Choose the printer, set paper width, run a test print. |
| `GUI/Controller/SettingPage/ReportSettingController.cs` | Validate, save and provision the report folder. |
| `GUI/Controller/SettingPage/SecuritySettingController.cs` | Toggle the built-in recovery account; refuse if it would lock everyone out. |
| `GUI/Controller/SettingPage/ShopSettingController.cs` | Read, validate and save the shop's name. |

### GUI/Model — [index/InventoryAndSales.GUI.Model.md](index/InventoryAndSales.GUI.Model.md)

| File | Purpose |
|---|---|
| `GUI/Model/ViewCartModel.cs` | `ViewItemMaster` view model. **Currently unused / dead code.** |

### GUI/Page — [index/InventoryAndSales.GUI.Page.md](index/InventoryAndSales.GUI.Page.md)

| File | Purpose |
|---|---|
| `GUI/Page/CashierPage.cs` | Sale screen: product filter/barcode scan, cart grid, payment, checkout. |
| `GUI/Page/LoginPage.cs` | Username/password entry. |
| `GUI/Page/MasterProductPage.cs` | Product master grid, edit panel, code generator, CSV import/export. |
| `GUI/Page/MasterUserPage.cs` | User master grid and edit panel. |
| `GUI/Page/PromoPage.cs` | Empty draft screen. **Not reachable from the UI.** |
| `GUI/Page/ReportDisplayPage.cs` | Date range pickers, report tabs, HTML report buttons. |
| `GUI/Page/TransactionUpdatePage.cs` | Revision variant of the cashier screen. |
| `GUI/Page/*.Designer.cs` | Generated layouts. |

### GUI/Popup — [index/InventoryAndSales.GUI.Popup.md](index/InventoryAndSales.GUI.Popup.md)

| File | Purpose |
|---|---|
| `GUI/Popup/AuthenticationForm.cs` | Supervisor step-up dialog for a required `AccessOption`. |
| `GUI/Popup/ReprintReceipt.cs` | Faktur-number prompt. **Currently unused.** |
| `GUI/Popup/SettingForm.cs` | Settings shell hosting setting sub-pages in a list. |
| `GUI/Popup/TransactionHistory.cs` | Date-range transaction picker; returns selected faktur. |
| `GUI/Popup/TransactionUpdateForm.cs` | Window wrapper around `TransactionUpdatePage`. |
| `GUI/Popup/*.Designer.cs` | Generated layouts. |

### GUI/Popup/SettingPage — [index/InventoryAndSales.GUI.Popup.SettingPage.md](index/InventoryAndSales.GUI.Popup.SettingPage.md)

| File | Purpose |
|---|---|
| `GUI/Popup/SettingPage/HeaderAndFooter.cs` | `HeaderAndFooterForm` — edit receipt header/footer with live preview. |
| `GUI/Popup/SettingPage/PrinterSetting.cs` | `PrinterSettingForm` — printer, paper width, test print. **Admin only.** |
| `GUI/Popup/SettingPage/ReportSetting.cs` | `ReportSettingForm` — choose the report folder, check asset status. |
| `GUI/Popup/SettingPage/SecuritySetting.cs` | `SecuritySettingForm` — allow or forbid the built-in recovery account. |
| `GUI/Popup/SettingPage/ShopSetting.cs` | `ShopSettingForm` — name the shop, shown as *Toko*. |

### GUI/Util — [index/InventoryAndSales.GUI.Util.md](index/InventoryAndSales.GUI.Util.md)

| File | Purpose |
|---|---|
| `GUI/Util/BusinessUtil.cs` | `AllowedRole` — role bit-flag check. |
| `GUI/Util/DataTableUtil.cs` | `List<Dictionary<string,string>>` → `System.Data.DataTable` for grid binding. |

### Utility — [index/InventoryAndSales.Utility.md](index/InventoryAndSales.Utility.md)

| File | Purpose |
|---|---|
| `Utility/Constant.cs` | `DISPLAY_CURRENCY = "#,##0.00"`. Namespace is `InventoryAndSales.GUI.Utility`. |
| `Utility/HtmlReportGenerator.cs` | Wraps a table fragment in an HTML page referencing DataTables assets. |
| `Utility/UpdateInstaller.cs` | Second-process file swap with backup and rollback, then restart. |

### Properties — [index/InventoryAndSales.Properties.md](index/InventoryAndSales.Properties.md)

| File | Purpose |
|---|---|
| `Properties/AssemblyInfo.cs` | Assembly metadata and version. |
| `Properties/Resources.Designer.cs` / `.resx` | Generated resource accessors. |
| `Properties/Settings.Designer.cs` / `.settings` | Generated settings accessors (unused). |

---

## SimpleCommon (library)

### Root — [index/SimpleCommon.md](index/SimpleCommon.md)

| File | Purpose |
|---|---|
| `SimpleCommon/SimpleCommon.csproj` | Library project file. |

### Model — [index/SimpleCommon.Model.md](index/SimpleCommon.Model.md)

| File | Purpose |
|---|---|
| `SimpleCommon/Model/ISearchable.cs` | Contract for rows shown in `SearchableDataView`. |

### UI — [index/SimpleCommon.UI.md](index/SimpleCommon.UI.md)

| File | Purpose |
|---|---|
| `SimpleCommon/UI/HtmlTableGenerator.cs` | Renders a DataTables-enabled HTML `<table>` from headers + rows. |

### UI/ComponentWinForm — [index/SimpleCommon.UI.ComponentWinForm.md](index/SimpleCommon.UI.ComponentWinForm.md)

| File | Purpose |
|---|---|
| `SimpleCommon/UI/ComponentWinForm/SearcheableDataView.cs` | Generic filterable grid control. **Not used by the app yet.** |
| `SimpleCommon/UI/ComponentWinForm/SpecialKeyEventArgs.cs` | Event args carrying a key char + selected items. |
| `SimpleCommon/UI/ComponentWinForm/SplitButton.cs` | Button with a dropdown menu region. |

### Utility — [index/SimpleCommon.Utility.md](index/SimpleCommon.Utility.md)

| File | Purpose |
|---|---|
| `SimpleCommon/Utility/ControlUtility.cs` | `HideTabHeader` — turn a `TabControl` into a page container. |
| `SimpleCommon/Utility/DelegateUtility.cs` | Reusable delegate shapes for `BeginInvoke` marshalling. |
| `SimpleCommon/Utility/HashUtility.cs` | `GetEncryptedPass` — the legacy unsalted SHA-512 hash. Kept only so old passwords still verify. |
| `SimpleCommon/Utility/PasswordHasher.cs` | PBKDF2 hashing, verification of both formats, and the upgrade flag. |
| `SimpleCommon/Utility/PrinterUtility.cs` | `StringPrint`, `PrintObject`, receipt printing. |

### Properties — [index/SimpleCommon.Properties.md](index/SimpleCommon.Properties.md)

| File | Purpose |
|---|---|
| `SimpleCommon/Properties/AssemblyInfo.cs` | Assembly metadata. |
| `SimpleCommon/Properties/Resources.Designer.cs` / `.resx` | Generated resource accessors. |

---

## Non-C# files — [index/root.md](index/root.md)

| File | Purpose |
|---|---|
| `DDL.sql` | Reference schema dump (UTF-16). Not executed by the app. |
| `InventoryAndSalesProject.sln` | Solution file. |
| `.gitignore` | Inherited from an unrelated project; contains stale rules. |
| `temp.zip` | Untracked 5 MB archive in the working tree. |

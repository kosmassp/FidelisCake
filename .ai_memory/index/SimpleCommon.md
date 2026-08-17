# Directory: `SimpleCommon/`

Project root of the shared library. `OutputType Library`, .NET Framework 4.6, assembly name
`SimpleCommon`. Referenced by `InventoryAndSales`; references nothing of its own beyond framework
assemblies plus `log4net`.

**Design intent:** everything here is application-agnostic and reusable. It must not reference
`InventoryAndSales` types, know about products, transactions or the SalesInventory database. That
constraint is what makes it a library rather than a second copy of the app.

| File | Purpose |
|---|---|
| `SimpleCommon.csproj` | Project file. Framework references: `System`, `System.configuration`, `System.Core`, `System.Drawing`, `System.Web`, `System.Windows.Forms`, `System.Data`, `System.Xml`, `System.Xml.Linq`, `System.Data.DataSetExtensions`, plus `log4net`. |

## Contents

| Folder | Page |
|---|---|
| `Model/` | [SimpleCommon.Model.md](SimpleCommon.Model.md) |
| `UI/` | [SimpleCommon.UI.md](SimpleCommon.UI.md) |
| `UI/ComponentWinForm/` | [SimpleCommon.UI.ComponentWinForm.md](SimpleCommon.UI.ComponentWinForm.md) |
| `Utility/` | [SimpleCommon.Utility.md](SimpleCommon.Utility.md) |
| `Properties/` | [SimpleCommon.Properties.md](SimpleCommon.Properties.md) |

## What the application actually uses

| Type | Used by |
|---|---|
| `HashUtility` | `LoginManager`, `UserManager`, `MasterUserController` |
| `PrinterUtility`, `StringPrint` | `CashierManager`, `HeaderAndFooterController` |
| `DelegateUtility` | Every thread-marshalled UI method |
| `ControlUtility` | `MainForm` (hiding tab headers) |
| `HtmlTableGenerator` | `ReportDisplayController` |
| `SplitButton` | Available to the designer |
| `SearchableDataView<T>`, `ISearchable`, `SpecialKeyEventArgs<T>` | **Nothing yet** — built to replace the duplicated grid code in `CashierPage` / `TransactionUpdatePage` |

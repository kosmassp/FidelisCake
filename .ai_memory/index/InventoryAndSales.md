# Directory: `InventoryAndSales/`

Project root of the WinForms POS application. `WinExe`, .NET Framework 4.6, assembly name
`InventoryAndSales`. References `SimpleCommon` and `log4net`.

---

## `Program.cs`

`static class Program` — application entry point.

| Member | Signature | Purpose |
|---|---|---|
| `Main` | `static void Main()` | `[STAThread]`. Pins culture to `en-US` (see note), hooks both exception handlers, enables visual styles, runs `SplashForm` to completion, then runs `MainForm` only if `splashForm.InitializationCheckSuccess` is true. On failure logs and `Environment.Exit(1)`. |
| `Application_ThreadException` | `static void (object, ThreadExceptionEventArgs)` | Anything escaping a UI event handler: logged in full, reported to the operator in Indonesian pointing at `Log\log.txt`, and the application keeps running. Registered with `Application.SetUnhandledExceptionMode(CatchException)`. Without it an unexpected error in a button handler showed the raw .NET crash dialog. |
| `CurrentDomain_UnhandledException` | `static void (object, UnhandledExceptionEventArgs)` | Logs the exception with a `Terminating` / `Non-Terminating` marker. |

⚠ Note: culture is forced to `en-US` deliberately. Amounts are formatted and parsed with `.` as the
decimal separator throughout (`decimal.Parse(textBoxPayment.Text)`), so removing the pin changes how
money is read from the payment box. `MainForm`'s constructor re-applies the same pin.

Report SQL no longer depends on it — dates are passed as parameters rather than formatted into the
statement.

All four culture properties are now set (`DefaultThreadCurrentCulture`,
`DefaultThreadCurrentUICulture`, and both on the current thread); `CurrentUICulture` used to be
assigned twice and `CurrentCulture` not at all.

---

## `App.config`

| Key | Value | Used by |
|---|---|---|
| `ConnectionString` | `Server=localhost\SQLEXPRESS;Database=SalesInventory;User Id=sa;Password=…` | `Database/DBFactory.cs` |
| `PrinterName` | `Microsoft Print to PDF` (commented alternatives: a PDF printer, `EPSON TM-U220 Receipt`) | `SimpleCommon/Utility/PrinterUtility.cs` |

`<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6" />`.

⚠ Note: the config ships with a plaintext `sa` password. Treat it as a local-development default
and override per deployment.

---

## `log4net.config`

Root logger at level `ALL`, two appenders:

- `console` — `%date %level %logger - %message`
- `file` — `RollingFileAppender` to `Log\log.txt`, size-rolled at 10 MB, 10 backups,
  `appendToFile=false` (**the log is truncated on every application start**).

---

## `reportassets.zip`

DataTables stylesheet and combined script, marked `Content` with `CopyToOutputDirectory` so it lands
next to the executable. `Business/ReportService.cs` unpacks it into the report folder on demand.

1.0 MB, trimmed from the original 5.4 MB download to the two files reports actually use
(`datatables.min.css`, `datatables.min.js`). The dropped content was unminified duplicates, Chart.js
which nothing references, and a 1 MB Office uninstall log.

## `InventoryAndSales.csproj`

Explicit `<Compile Include>` list — new `.cs` files must be added here to be built.
Framework references: `System`, `System.configuration`, `System.Core`, `System.Web` (for
`HtmlTextWriter`), `System.Data`, `System.Deployment`, `System.Drawing`, `System.Windows.Forms`,
`System.Xml`, `System.IO.Compression` and `System.IO.Compression.FileSystem` (for the asset bundle),
plus `log4net`.

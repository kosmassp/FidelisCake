# Reporting

Menu *Laporan → Laporan Transaksi*, requires `AccessOption.Admin` — see the dead `Laporan` bit note
in [business-auth-and-roles.md](business-auth-and-roles.md).

All report SQL lives in one file: `Database/DataAccess/CustomDao.cs`.

## The five reports

| Report | Grain | Columns (Indonesian aliases become the headers) |
|---|---|---|
| **By product** | product × date | ProductName, TransactionDate, Jumlah Transaksi, Jumlah Barang Terjual, Total Sebelum Diskon, Total Diskon, Total |
| **By transaction** | one row per sale | Kasir, Factur, Tanggal Transaksi, Total, Catatan, Pembayaran, Kembalian |
| **By cashier** | cashier × date | Kasir, Tanggal Transaksi, Jumlah Transaksi, Jumlah Barang Terjual, Total Sebelum Diskon, Total Diskon, Total |
| **Detail** | one row per sale line | Cashier, Factur, TransactionTime, ProductName, Jumlah, Harga, Diskon, Total Sebelum Diskon, Total Diskon, SubTotal, Total |
| **Daily cashier total** | one number | `SUM(Total)` for one user on one date |

The first four take a start/stop date range. The detail report is the expensive one — it is only run
when the *detail* checkbox is ticked.

## Rules every report shares

1. **`WHERE t.Revision = 0`** — only active sales. Revised and cancelled transactions are invisible
   to every total. See [business-transaction-revision.md](business-transaction-revision.md).
2. **`t.TransactionTime >= @start AND t.TransactionTime < @stop`** — a half-open interval built by
   `CustomDao.DateRange`: `@start` is `start.Date`, `@stop` is `stop.Date.AddDays(1)`. Both pickers
   remain inclusive dates to the operator. Picking the dates the wrong way round is treated as a
   single day rather than returning nothing.
3. **Dates are passed as parameters**, not formatted into the SQL. This has two consequences:
   the predicate is **sargable**, so `IDX_T_TRANS_TRXTIME` can be used for a range seek; and the
   report no longer depends on the thread culture. (It previously used
   `CAST(TransactionTime AS date) BETWEEN '{ToShortDateString()}' AND …`, which both defeated the
   index and would have selected the wrong rows under any non-`en-US` culture.)
4. **Missing references degrade gracefully** — `COALESCE(p.Name, 'Telah Dihapus')` for a product row
   that is physically gone, `COALESCE(u.Name, 'ADMIN')` for the hardcoded account (`UserId = -1`).
5. **The by-cashier report counts `COUNT(DISTINCT t.Id)`** because it joins through the detail
   table; the by-product report uses `COUNT(t.Id)`, which counts lines, not sales.

## Data pipeline

```
CustomDao (SQL)
  → List<CustomQuery>            dynamic row bag, projected by reader ordinal
  → CustomManager.ConvertToList  → List<Dictionary<string,string>>
  → ReportManager                 façade
  → ReportDisplayController
       ├─ DataTableUtil.GetDataTable → System.Data.DataTable → DataGridView
       └─ HtmlTableGenerator + HtmlReportGenerator → .html file → Process.Start
```

`CustomQuery` is deliberately schema-less: `CustomDao.ExecuteReader` is overridden to project by
`reader.GetName(i)` rather than a fixed column list, because every report returns a different shape.
That is what lets one class serve five queries.

`CustomQuery`'s indexer **formats as it stores**: a `DateTime` with zero time-of-day becomes
`"dd MMM yyyy"`, otherwise `"dd MMM yyyy HH:mm:ss"`. Values arrive at the grid already display-ready.

⚠ Everything downstream is `string`. Grids therefore sort dates and amounts **lexically**, not
chronologically or numerically. Column order comes from the SQL `SELECT` order, and headers are the
SQL aliases — so renaming an alias renames a report column.

## On-screen output

`ReportDisplayPage.buttonShowReportSummary_Click` rebuilds the tab set (cashier, product,
transaction), runs `ShowSummaryReport`, and adds the detail tab and report when the checkbox is
ticked.

`UpdateReportDataGridView(DataTable byProduct, DataTable byTransaction, DataTable byCashier)` takes
three named parameters. It previously took a `DataTable[]` and bound by array position, which was
correct but relied on the controller assembling it in exactly the right order.

## HTML output

Four buttons export a standalone HTML file and open it with the default browser:

| Button | Filename pattern | Title |
|---|---|---|
| Per Kasir | `SBC{yyyyMMdd}_{yyyyMMdd}.html` | Cashier Report |
| Per Transaksi | `SBT…` | Transaction Report |
| Per Product | `SRP…` | Product Sales Report |
| Per Item (detail) | `RDP…` | Detail Report |

`ShowSummaryReportInHtml` takes headers from the first row's keys and rows from its values, renders
with `SimpleCommon.UI.HtmlTableGenerator`, wraps with `Utility.HtmlReportGenerator`, writes into the
**configured report folder**, and launches it.

### Where reports go

The folder is a setting (`REPORT_DIRECTORY`, group `REPORT`), editable under
*Pengaturan → Laporan* with a folder picker. It defaults to
`<Documents>\FidelisCake\Laporan` — per-user, no administrator rights needed. Environment variables
in the value are expanded, so `%USERPROFILE%\Laporan` works.

It was previously hardcoded to `c:\temp\Report\`.

### Where the JavaScript comes from

`ReportService.EnsureAssets` unpacks `datatables.min.css` and `datatables.min.js` into an
`assets\` sub-folder of the report directory, from **`reportassets.zip` shipped next to the
executable**. It is a no-op once they are there, so the cost is paid once per folder.

The bundle is the DataTables combined build — jQuery 3.3.1, DataTables 1.10.18, Buttons, JSZip and
pdfmake in one self-contained file — trimmed from the original 5.4 MB download to the two files
actually referenced (1.0 MB compressed).

Previously the page linked `../datatables.min.css` / `../datatables.min.js`, i.e. files somebody had
to unpack into `c:\temp\` by hand on every machine.

Degradation is now explicit rather than silent:

- assets present → full page, sorting/searching/Excel/PDF export;
- bundle missing → the report is still written and opened, the asset links are omitted entirely, and
  the operator is told which file is missing;
- the DataTables initialiser is wrapped in `if (window.jQuery && jQuery.fn.DataTable)`, so a page
  opened without its assets shows a plain readable table instead of a script error.

Empty reports now say so (*"Tidak ada data pada rentang tanggal tersebut."*) instead of silently
doing nothing, and a failure to write or open the file is reported with the path.

Report values are HTML-encoded (`WriteEncodedText`), so a product name containing `<` or `&` no
longer corrupts the page.

## Daily cashier total

Menu *check kasir → Jumlah Setoran*, available to anyone with `AccessOption.Cashier`.

`ReportManager.GetTodaySummaryByCashier(activeUser, DateTime.Today)` runs:

```sql
SELECT COALESCE(SUM([Total]), 0) FROM T_TRANSACTIONS
WHERE UserId = {id} AND Revision = 0 AND CAST(TransactionTime AS date) = '{date}'
```

and returns a pre-formatted `"Rp. {n}"`. The message box shows the date, the current time, the
total, and this caveat:

> *"Jika terdapat perubahan transaksi, Jumlah kemungkinan tidak sesuai."*
> (If transactions have been changed, the amount may not match.)

That is honest: because revisions reattribute a sale to the approving supervisor and cancellations
remove it entirely, a cashier's total for a day can change after the fact.

## Performance

`IDX_T_TRANS_TRXTIME` (`TransactionTime DESC`) and `IDX_T_TRDETAIL_TRX_ID` back these queries.
`CommandTimeout` is 600 seconds throughout. The detail report joins the full detail table and is the
most expensive — hence the opt-in checkbox.

The date predicate compares the raw `TransactionTime` column against parameters, so
`IDX_T_TRANS_TRXTIME` is usable for a range seek. Reports now run behind a wait cursor and report
failure in Indonesian rather than throwing into the UI.

⚠ `CAST(t.TransactionTime AS date)` still appears in the `SELECT` and `GROUP BY` of the summary
reports. That is projection, not filtering, and does not affect index use.

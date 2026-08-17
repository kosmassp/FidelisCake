# Directory: `SimpleCommon/UI/`

Namespace `SimpleCommon.UI`.

---

## `HtmlTableGenerator.cs`

`public class HtmlTableGenerator` — renders a table fragment as HTML, wired for the
[DataTables](https://datatables.net) jQuery plugin. Uses `System.Web.UI.HtmlTextWriter`, so the only
dependency is the `System.Web` framework assembly — no templating library.

| Member | Signature | Purpose |
|---|---|---|
| `GenerateTable` | `static string GenerateTable(string id, string[] headers, List<string[]> dataRows, string[] columnClasses, string[] footerTotals)` | Writes the table markup, then an inline `<script>` initialising DataTables. Returns the fragment as a string. |
| `GenerateTable` | `private static void GenerateTable(HtmlTextWriter writer, …)` | Writes `<div class="table-wrap">` → `<table id=… class="table table-striped table-bordered" style="width:100%">` → `<thead>` → `<tbody>` → optional `<tfoot>`. |

`columnClasses` puts one caller-chosen CSS class on every cell of a column, head, body and foot —
the control kept **generic on purpose**: this class knows nothing about money or dates, only about
stamping a class its caller's stylesheet understands. `footerTotals` draws a footer row; an empty
entry leaves that cell blank. Both may be null.

The initialiser is emitted **after** the markup and wrapped in
`if (window.jQuery && jQuery.fn.DataTable)`, so a page opened without its assets shows a plain
readable table rather than a script error. It asks for the export buttons only when
`jQuery.fn.dataTable.Buttons` is actually loaded — naming a button that is not there stops DataTables
initialising at all and leaves the operator with no table. The chrome is in Indonesian, ordering is
left as the query returned it (`order: []`), and the rows-per-page menu offers *Semua* so the whole
report can be printed.

The output is a **fragment**, not a document. `InventoryAndSales/Utility/HtmlReportGenerator.cs`
wraps it in `<html>`/`<head>`/`<body>` and adds the asset links.

The assets it depends on ship in the `Report` folder beside the executable and are copied into an
`assets` sub-folder of the report folder by `InventoryAndSales.Business.ReportService`. The bundled `datatables.min.js` is the
DataTables combined build and is self-contained: jQuery 3.3.1, DataTables 1.10.18, Buttons, JSZip and
pdfmake are all inside it, so the Excel and PDF export buttons work with no further files.

Header and cell text is written with `WriteEncodedText`, so a value containing `<`, `>` or `&` no
longer corrupts the markup.

The commented-out `StringBuilder` implementation has been removed.

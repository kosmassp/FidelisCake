# Directory: `SimpleCommon/UI/`

Namespace `SimpleCommon.UI`.

---

## `HtmlTableGenerator.cs`

`public class HtmlTableGenerator` — renders a table fragment as HTML, wired for the
[DataTables](https://datatables.net) jQuery plugin. Uses `System.Web.UI.HtmlTextWriter`, so the only
dependency is the `System.Web` framework assembly — no templating library.

| Member | Signature | Purpose |
|---|---|---|
| `GenerateTable` | `static string GenerateTable(string id, string[] headers, List<string[]> dataRows)` | Writes the table markup, then an inline `<script>` initialising DataTables with the Excel and PDF export buttons. Returns the fragment as a string. |
| `GenerateTable` | `private static void GenerateTable(HtmlTextWriter writer, string id, string[] headers, List<string[]> dataRows)` | Writes `<div style="width:80%">` → `<table id=… class="table table-striped table-bordered" style="width:100%">` → `<thead>` with one `<th>` per header → `<tbody>` with one `<tr>`/`<td>` per row. |

The initialiser is emitted **after** the markup and wrapped in
`if (window.jQuery && jQuery.fn.DataTable)`, so a page opened without its assets shows a plain
readable table rather than a script error.

The output is a **fragment**, not a document. `InventoryAndSales/Utility/HtmlReportGenerator.cs`
wraps it in `<html>`/`<head>`/`<body>` and adds the asset links.

The assets it depends on are shipped with the application as `reportassets.zip` and unpacked into the
report folder by `InventoryAndSales.Business.ReportService`. The bundled `datatables.min.js` is the
DataTables combined build and is self-contained: jQuery 3.3.1, DataTables 1.10.18, Buttons, JSZip and
pdfmake are all inside it, so the Excel and PDF export buttons work with no further files.

Header and cell text is written with `WriteEncodedText`, so a value containing `<`, `>` or `&` no
longer corrupts the markup.

The commented-out `StringBuilder` implementation has been removed.

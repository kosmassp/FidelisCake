# Directory: `InventoryAndSales/Utility/`

⚠ Note: the two files here live in **different namespaces**, neither of which matches the folder:
`Constant.cs` is `InventoryAndSales.GUI.Utility`, `HtmlReportGenerator.cs` is
`InventoryAndSales.Utility`. Keep this in mind when adding `using` directives. See
[../rules-csharp.md](../rules-csharp.md) for the namespace rule going forward.

---

## `Constant.cs`

Namespace `InventoryAndSales.GUI.Utility`. `public class Constant`.

| Member | Signature | Value | Purpose |
|---|---|---|---|
| `DISPLAY_CURRENCY` | `public static string` | `"#,##0.00"` | The money format string used everywhere amounts are shown: cart grid, totals, change, product net price, success messages. |

Used by `CashierPage`, `TransactionUpdatePage`, `CashierController`, `TransactionUpdateController`,
`Product.DisplayDiscount`.

⚠ Note: declared `static` but not `const` or `readonly`, so any code could reassign it. Combined
with the `en-US` culture pin, the format yields `1,234.50`. Receipts use `ToString("N")` instead,
which is the same pattern for `en-US`.

---

## `HtmlReportGenerator.cs`

Namespace `InventoryAndSales.Utility`. `public class HtmlReportGenerator`.

| Member | Signature | Purpose |
|---|---|---|
| `Write` | `static void Write(ReportDocument document, string tableId, string fullpath, string styleSheetHref, string scriptSrc)` | Renders a whole report as an HTML page and writes it as UTF-8. |

Emitted document: `<!DOCTYPE html>` → `<head>` with charset, viewport, title, the DataTables
stylesheet link and script tag **when the caller supplies them**, then the report's own `<style>`
→ `<body>` with a `.page` sheet containing the head block (shop, title, period), a row of summary
cards for each totalled column (capped at 8), the table from `HtmlTableGenerator`, and a foot line
with the row count, timestamp and operator. Built with `System.Web.UI.HtmlTextWriter`.

**The stylesheet is embedded, not linked**, so a report that is mailed on or opened without the
assets still looks like a report. The DataTables link only adds sorting, searching, paging and the
export buttons on top; the embedded block is emitted *after* it so the report's own look wins.
It is deliberately plain CSS — no custom properties, no flexbox gaps — because the shop PC's default
browser is unknown. Print rules hide the DataTables controls and repeat the header row per page.

`ReportColumnKind` decides how a column reads: `Number` → class `num` (right aligned, no wrap),
`Date` → class `date` (no wrap), `Text` → unstyled.

The asset paths are **parameters rather than hardcoded**, so the report folder can move; they were
fixed at `../datatables.min.css` / `.js`, which only resolved when the report was written to
`c:\temp\Report\`. Passing `null` for both emits a self-contained page, which is what
`ReportDisplayController` does when the bundle is missing.

All text goes through `WriteEncodedText`; the only raw markup is the table fragment from
`HtmlTableGenerator`, which encodes the data going into it.

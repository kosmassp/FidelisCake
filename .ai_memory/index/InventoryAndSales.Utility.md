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
| `Write` | `static void Write(string title, string body, string fullpath, string styleSheetHref, string scriptSrc)` | Wraps a pre-rendered table fragment in a full HTML document and writes it as UTF-8. |

Emitted document: `<html><head><meta charset="utf-8"><title>…</title>` + the stylesheet link and
script tag **when the caller supplies them** + `</head><body>{body}</body></html>`, built with
`System.Web.UI.HtmlTextWriter`.

The asset paths are **parameters rather than hardcoded**, so the report folder can move; they were
fixed at `../datatables.min.css` / `.js`, which only resolved when the report was written to
`c:\temp\Report\`. Passing `null` for both emits a self-contained page with no asset links, which is
what `ReportDisplayController` does when the bundle is missing.

The title is HTML-encoded. `body` is still written raw — it is markup produced by
`HtmlTableGenerator`, which encodes the data going into it.

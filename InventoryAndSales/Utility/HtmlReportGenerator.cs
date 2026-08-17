using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using InventoryAndSales.Business;
using SimpleCommon.UI;

namespace InventoryAndSales.Utility
{
  /// <summary>
  /// Writes a <see cref="ReportDocument"/> out as a complete, self-explanatory HTML page.
  ///
  /// The page has to stand on its own once it leaves the till: it is opened in a browser, printed,
  /// mailed to the owner and filed. So it carries its own stylesheet rather than depending on the
  /// DataTables assets being reachable — those only add sorting, searching and export on top. What a
  /// reader sees, with or without them, is a titled report with the shop, the period it covers, its
  /// headline figures and a totalled table.
  /// </summary>
  public class HtmlReportGenerator
  {
    /// <summary>Headline figures shown above the table. Past this many the row stops being a summary.</summary>
    private const int MaxSummaryCards = 8;

    private const string NumericCellClass = "num";
    private const string DateCellClass = "date";

    /// <param name="styleSheetHref">
    /// Relative path to the DataTables stylesheet, or null when the assets are not available. The
    /// paths are supplied by the caller rather than hardcoded so the report folder can be moved.
    /// </param>
    public static void Write(ReportDocument document, string tableId, string fullpath,
                             string styleSheetHref, string scriptSrc)
    {
      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        writer.WriteLine("<!DOCTYPE html>");
        writer.AddAttribute("lang", "id");
        writer.RenderBeginTag(HtmlTextWriterTag.Html);

        WriteHead(writer, document, styleSheetHref, scriptSrc);
        WriteBody(writer, document, tableId);

        writer.RenderEndTag(); // html
      }

      File.WriteAllText(fullpath, stringWriter.ToString(), Encoding.UTF8);
    }

    private static void WriteHead(HtmlTextWriter writer, ReportDocument document,
                                  string styleSheetHref, string scriptSrc)
    {
      writer.RenderBeginTag(HtmlTextWriterTag.Head);

      writer.AddAttribute("charset", "utf-8");
      writer.RenderBeginTag(HtmlTextWriterTag.Meta);
      writer.RenderEndTag();

      writer.AddAttribute(HtmlTextWriterAttribute.Name, "viewport");
      writer.AddAttribute(HtmlTextWriterAttribute.Content, "width=device-width, initial-scale=1");
      writer.RenderBeginTag(HtmlTextWriterTag.Meta);
      writer.RenderEndTag();

      writer.RenderBeginTag(HtmlTextWriterTag.Title);
      writer.WriteEncodedText(document.Title + " - " + document.PeriodText);
      writer.RenderEndTag();

      if (!string.IsNullOrEmpty(styleSheetHref))
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
        writer.AddAttribute(HtmlTextWriterAttribute.Href, styleSheetHref);
        writer.RenderBeginTag(HtmlTextWriterTag.Link);
        writer.RenderEndTag();
      }

      if (!string.IsNullOrEmpty(scriptSrc))
      {
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
        writer.AddAttribute(HtmlTextWriterAttribute.Src, scriptSrc);
        writer.RenderBeginTag(HtmlTextWriterTag.Script);
        writer.RenderEndTag();
      }

      // Last, so the report's own look wins wherever it overlaps the DataTables stylesheet.
      writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
      writer.RenderBeginTag(HtmlTextWriterTag.Style);
      writer.Write(STYLESHEET);
      writer.RenderEndTag();

      writer.RenderEndTag(); // head
    }

    private static void WriteBody(HtmlTextWriter writer, ReportDocument document, string tableId)
    {
      ReportTable table = document.Table;

      writer.RenderBeginTag(HtmlTextWriterTag.Body);

      writer.AddAttribute(HtmlTextWriterAttribute.Class, "page");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);

      WriteReportHead(writer, document);

      if (table.RowCount == 0)
      {
        WriteDiv(writer, "empty", "Tidak ada data pada periode ini.");
      }
      else
      {
        WriteSummaryCards(writer, table);
        writer.Write(HtmlTableGenerator.GenerateTable(tableId, table.Headers, table.Rows,
                                                      ColumnClasses(table),
                                                      table.HasTotals ? WithTotalLabel(table) : null));
      }

      WriteReportFoot(writer, document);

      writer.RenderEndTag(); // div.page
      writer.RenderEndTag(); // body
    }

    private static void WriteReportHead(HtmlTextWriter writer, ReportDocument document)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "report-head");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);

      if (!string.IsNullOrEmpty(document.ShopName))
        WriteDiv(writer, "shop", document.ShopName);

      writer.RenderBeginTag(HtmlTextWriterTag.H1);
      writer.WriteEncodedText(document.Title);
      writer.RenderEndTag();

      WriteDiv(writer, "period", "Periode " + document.PeriodText);

      writer.RenderEndTag(); // div.report-head
    }

    /// <summary>
    /// The totals repeated above the table, so the figures that matter are readable without
    /// scrolling to the bottom of a few hundred rows.
    /// </summary>
    private static void WriteSummaryCards(HtmlTextWriter writer, ReportTable table)
    {
      if (!table.HasTotals)
        return;

      writer.AddAttribute(HtmlTextWriterAttribute.Class, "cards");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);

      int shown = 0;
      for (int i = 0; i < table.ColumnCount && shown < MaxSummaryCards; i++)
      {
        if (string.IsNullOrEmpty(table.Totals[i]))
          continue;

        writer.AddAttribute(HtmlTextWriterAttribute.Class, "card");
        writer.RenderBeginTag(HtmlTextWriterTag.Div);
        WriteDiv(writer, "card-label", table.Headers[i]);
        WriteDiv(writer, "card-value", table.Totals[i]);
        writer.RenderEndTag();
        shown++;
      }

      writer.RenderEndTag(); // div.cards
    }

    private static void WriteReportFoot(HtmlTextWriter writer, ReportDocument document)
    {
      StringBuilder text = new StringBuilder();
      text.Append(document.Table.RowCount.ToString("#,##0", CultureInfo.InvariantCulture)).Append(" baris");
      text.Append(" · dibuat ").Append(document.GeneratedAtText);
      if (!string.IsNullOrEmpty(document.GeneratedBy))
        text.Append(" oleh ").Append(document.GeneratedBy);

      WriteDiv(writer, "report-foot", text.ToString());
    }

    /// <summary>
    /// How each kind of column reads: amounts right aligned, dates kept whole on one line, and free
    /// text left alone to wrap.
    /// </summary>
    private static string[] ColumnClasses(ReportTable table)
    {
      string[] classes = new string[table.ColumnCount];
      for (int i = 0; i < classes.Length; i++)
      {
        switch (table.ColumnKinds[i])
        {
          case ReportColumnKind.Number:
            classes[i] = NumericCellClass;
            break;
          case ReportColumnKind.Date:
            classes[i] = DateCellClass;
            break;
          default:
            classes[i] = null;
            break;
        }
      }
      return classes;
    }

    /// <summary>Labels the totals row, using the first column that has no total of its own.</summary>
    private static string[] WithTotalLabel(ReportTable table)
    {
      string[] totals = (string[])table.Totals.Clone();
      for (int i = 0; i < totals.Length; i++)
      {
        if (string.IsNullOrEmpty(totals[i]))
        {
          totals[i] = "TOTAL";
          break;
        }
      }
      return totals;
    }

    private static void WriteDiv(HtmlTextWriter writer, string cssClass, string text)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
      writer.RenderBeginTag(HtmlTextWriterTag.Div);
      writer.WriteEncodedText(text ?? string.Empty);
      writer.RenderEndTag();
    }

    /// <summary>
    /// Deliberately plain CSS - no custom properties, no flexbox gaps - because the report is opened
    /// by whatever browser the shop PC happens to default to, and a stylesheet that half-renders is
    /// worse than one that is simple everywhere.
    /// </summary>
    private const string STYLESHEET =
      "*{box-sizing:border-box}" +
      "body{margin:0;padding:24px 16px;background:#f4f5f7;color:#1f2933;" +
      "font-family:'Segoe UI',Tahoma,Geneva,Verdana,sans-serif;font-size:14px;line-height:1.45}" +
      ".page{max-width:1200px;margin:0 auto;background:#ffffff;border:1px solid #dfe3e8;border-radius:8px;" +
      "padding:28px;box-shadow:0 1px 3px rgba(0,0,0,.08)}" +
      ".report-head{border-bottom:3px solid #8a1538;padding-bottom:14px;margin-bottom:20px}" +
      ".shop{font-size:12px;letter-spacing:.12em;text-transform:uppercase;color:#8a1538;font-weight:700}" +
      ".report-head h1{margin:6px 0 4px;font-size:24px;font-weight:600}" +
      ".period{font-size:15px;color:#3e4c59}" +
      ".cards{margin-bottom:20px}" +
      ".card{display:inline-block;vertical-align:top;min-width:170px;margin:0 10px 10px 0;padding:10px 14px;" +
      "background:#fbfbfc;border:1px solid #dfe3e8;border-left:4px solid #8a1538;border-radius:6px}" +
      ".card-label{font-size:11px;text-transform:uppercase;letter-spacing:.06em;color:#6b7785}" +
      ".card-value{font-size:19px;font-weight:600;margin-top:2px}" +
      ".table-wrap{overflow-x:auto}" +
      ".page table.table{width:100%;border-collapse:collapse;font-size:13px}" +
      ".page table.table th,.page table.table td{border:1px solid #dfe3e8;padding:7px 10px;text-align:left}" +
      ".page table.table thead th{background:#eef0f3;font-weight:600;white-space:nowrap;border-bottom:2px solid #c9cfd6}" +
      ".page table.table tbody tr:nth-child(even) td{background:#fafbfc}" +
      ".page table.table tbody tr:hover td{background:#eef4fa}" +
      ".page table.table ." + NumericCellClass + "{text-align:right;white-space:nowrap}" +
      ".page table.table ." + DateCellClass + "{white-space:nowrap}" +
      ".page table.table tfoot th{background:#eef0f3;font-weight:700;border-top:2px solid #8a1538}" +
      ".report-foot{margin-top:18px;padding-top:10px;border-top:1px solid #dfe3e8;font-size:11px;color:#6b7785}" +
      ".empty{padding:48px;text-align:center;color:#6b7785}" +
      ".dt-buttons{margin-bottom:10px}" +
      "@media print{" +
      "body{background:#ffffff;padding:0;font-size:11px}" +
      ".page{max-width:none;border:0;border-radius:0;box-shadow:none;padding:0}" +
      ".dt-buttons,.dataTables_filter,.dataTables_length,.dataTables_info,.dataTables_paginate{display:none !important}" +
      ".page table.table thead th{background:#eeeeee !important;-webkit-print-color-adjust:exact}" +
      "thead{display:table-header-group}tr{page-break-inside:avoid}" +
      "}";
  }
}

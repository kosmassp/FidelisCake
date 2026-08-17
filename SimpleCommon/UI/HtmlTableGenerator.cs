using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace SimpleCommon.UI
{
  /// <summary>
  /// Renders a table fragment wired up for the DataTables plugin. The caller is responsible for
  /// wrapping it in a document and for making jQuery/DataTables reachable.
  /// </summary>
  public class HtmlTableGenerator
  {
    /// <param name="columnClasses">
    /// One CSS class per column, or null for none. It is put on that column's cells in the head, the
    /// body and the foot; what the class means is the caller's stylesheet's business.
    /// </param>
    /// <param name="footerTotals">
    /// One value per column to draw in a footer row, or null for no footer. An empty entry leaves
    /// that cell blank, which is how a column that cannot be totalled is shown.
    /// </param>
    public static string GenerateTable(string id, string[] headers, List<string[]> dataRows,
                                       string[] columnClasses, string[] footerTotals)
    {
      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        GenerateTable(writer, id, headers, dataRows, columnClasses, footerTotals);

        // Initialised after the markup so the element exists, and guarded so a page opened without
        // the assets still shows a readable table instead of a script error.
        writer.RenderBeginTag(HtmlTextWriterTag.Script);
        writer.Write(InitScript(id));
        writer.RenderEndTag();
      }

      return stringWriter.ToString();
    }

    /// <summary>
    /// Turns the table interactive: sort, search, page and export. The export buttons are only asked
    /// for when the Buttons extension is actually loaded, because requesting them without it stops
    /// DataTables from initialising at all and leaves the operator with no table.
    /// </summary>
    private static string InitScript(string id)
    {
      return
        "if (window.jQuery && jQuery.fn.DataTable) {" +
        "  jQuery(function () {" +
        "    var hasButtons = !!jQuery.fn.dataTable.Buttons;" +
        "    jQuery('#" + id + "').DataTable({" +
        // 'l' is the rows-per-page menu: picking 'Semua' is what makes the whole report printable.
        "      dom: hasButtons ? 'Blfrtip' : 'lfrtip'," +
        "      buttons: hasButtons ? ['copyHtml5', 'excelHtml5', 'pdfHtml5', 'print'] : []," +
        // The report arrives already ordered the way its query meant it to be read.
        "      order: []," +
        "      pageLength: 25," +
        "      lengthMenu: [[25, 50, 100, -1], ['25', '50', '100', 'Semua']]," +
        "      language: {" +
        "        search: 'Cari:'," +
        "        lengthMenu: 'Tampilkan _MENU_ baris'," +
        "        info: 'Baris _START_ sampai _END_ dari _TOTAL_'," +
        "        infoEmpty: 'Tidak ada data'," +
        "        infoFiltered: '(disaring dari _MAX_ baris)'," +
        "        zeroRecords: 'Tidak ada baris yang cocok'," +
        "        emptyTable: 'Tidak ada data'," +
        "        paginate: { first: 'Awal', previous: 'Sebelumnya', next: 'Berikutnya', last: 'Akhir' }" +
        "      }" +
        "    });" +
        "  });" +
        "}";
    }

    private static void GenerateTable(HtmlTextWriter writer, string id, string[] headers,
                                      List<string[]> dataRows, string[] columnClasses, string[] footerTotals)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "table-wrap");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);

      writer.AddAttribute(HtmlTextWriterAttribute.Id, id);
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "table table-striped table-bordered");
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:100%");
      writer.RenderBeginTag(HtmlTextWriterTag.Table);

      writer.RenderBeginTag(HtmlTextWriterTag.Thead);
      writer.RenderBeginTag(HtmlTextWriterTag.Tr);
      for (int i = 0; i < headers.Length; i++)
      {
        WriteCell(writer, HtmlTextWriterTag.Th, headers[i], ClassAt(columnClasses, i));
      }
      writer.RenderEndTag(); //tr
      writer.RenderEndTag(); //thead

      writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
      foreach (string[] dataRow in dataRows)
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        for (int i = 0; i < dataRow.Length; i++)
        {
          WriteCell(writer, HtmlTextWriterTag.Td, dataRow[i], ClassAt(columnClasses, i));
        }
        writer.RenderEndTag(); //tr
      }
      writer.RenderEndTag(); //tbody

      if (footerTotals != null && footerTotals.Length > 0)
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Tfoot);
        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        for (int i = 0; i < footerTotals.Length; i++)
        {
          WriteCell(writer, HtmlTextWriterTag.Th, footerTotals[i], ClassAt(columnClasses, i));
        }
        writer.RenderEndTag(); //tr
        writer.RenderEndTag(); //tfoot
      }

      writer.RenderEndTag(); //table
      writer.RenderEndTag(); //div
    }

    private static string ClassAt(string[] columnClasses, int column)
    {
      if (columnClasses == null || column >= columnClasses.Length)
        return null;
      return columnClasses[column];
    }

    private static void WriteCell(HtmlTextWriter writer, HtmlTextWriterTag tag, string text, string cssClass)
    {
      if (!string.IsNullOrEmpty(cssClass))
        writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
      writer.RenderBeginTag(tag);
      // Encoded: report data is free text, and a product name containing < or & would otherwise
      // corrupt the page.
      writer.WriteEncodedText(text ?? string.Empty);
      writer.RenderEndTag();
    }
  }
}

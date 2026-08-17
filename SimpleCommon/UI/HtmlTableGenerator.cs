using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

namespace SimpleCommon.UI
{
  /// <summary>
  /// Renders a table fragment wired up for the DataTables plugin. The caller is responsible for
  /// wrapping it in a document and for making jQuery/DataTables reachable.
  /// </summary>
  public class HtmlTableGenerator
  {
    public static string GenerateTable(string id, string[] headers, List<string[]> dataRows)
    {
      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        GenerateTable(writer, id, headers, dataRows);

        // Initialised after the markup so the element exists, and guarded so a page opened without
        // the assets still shows a readable table instead of a script error.
        writer.RenderBeginTag(HtmlTextWriterTag.Script);
        writer.Write(
          "if (window.jQuery && jQuery.fn.DataTable) {" +
          "  jQuery(function () {" +
          "    jQuery('#" + id + "').DataTable({ dom: 'Bfrtip', buttons: ['excelHtml5', 'pdfHtml5'] });" +
          "  });" +
          "}");
        writer.RenderEndTag();
      }

      return stringWriter.ToString();
    }

    private static void GenerateTable(HtmlTextWriter writer, string id, string[] headers, List<string[]> dataRows)
    {
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:80%");
      writer.RenderBeginTag(HtmlTextWriterTag.Div);

      writer.AddAttribute(HtmlTextWriterAttribute.Id, id);
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "table table-striped table-bordered");
      writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:100%");
      writer.RenderBeginTag(HtmlTextWriterTag.Table);

      writer.RenderBeginTag(HtmlTextWriterTag.Thead);
      writer.RenderBeginTag(HtmlTextWriterTag.Tr);
      foreach (string header in headers)
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Th);
        // Encoded: report data is free text, and a product name containing < or & would otherwise
        // corrupt the page.
        writer.WriteEncodedText(header ?? string.Empty);
        writer.RenderEndTag(); //th
      }
      writer.RenderEndTag(); //tr
      writer.RenderEndTag(); //thead

      writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
      foreach (var dataRow in dataRows)
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        foreach (string datumRow in dataRow)
        {
          writer.RenderBeginTag(HtmlTextWriterTag.Td);
          writer.WriteEncodedText(datumRow ?? string.Empty);
          writer.RenderEndTag(); //td
        }
        writer.RenderEndTag(); //tr
      }
      writer.RenderEndTag(); //tbody

      writer.RenderEndTag(); //table
      writer.RenderEndTag(); //div
    }
  }
}

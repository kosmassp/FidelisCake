using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

namespace SimpleCommon.UI
{
  public class HtmlTableGenerator
  {
    public static string GenerateTable(string id, string[] headers, List<string[]> dataRows)
    {

      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Script);
        writer.Write(@"$(document).ready(function() {$('#" + id + @"').DataTable(		 {
        dom: 'Bfrtip',
        buttons: [
            'excelHtml5',
            'pdfHtml5'
        ]
        });} );");
        writer.RenderEndTag(); //script

        GenerateTable(writer, id, headers, dataRows);
      }

      //StringBuilder sb = new StringBuilder();
      //sb.AppendLine("<div>");
      //sb.AppendLine("<table id={0} class={1} style={2}>");
      //sb.AppendLine("<tr>");
      //foreach (string header in headers)
      //{
      //  sb.AppendLine("<th>" + header + "</th>");
      //}
      //sb.AppendLine("</tr>");
      //foreach (var dataRow in dataRows)
      //{
      //  sb.AppendLine(GenerateTableRow(dataRow));
      //}
      //sb.AppendLine("</table>");
      //sb.AppendLine("</div>");
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
        writer.Write(header);
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
          writer.Write(datumRow);
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

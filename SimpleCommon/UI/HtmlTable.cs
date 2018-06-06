

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimpleCommon.UI
{
  public class HtmlTable
  {
    private string[] _headers;
    //public HtmlTable(ICollection<string> headers)
    //{
    //  _headers = new string[headers.Count];
    //  int i = 0;
    //  foreach(string header in headers)
    //  {
    //    _headers[i++] = header;
    //  }
    //}

    public HtmlTable( params string[] headers )
    {
      _headers = headers;
    }


    public string GetHeaderTable()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine ("<table>");
      sb.AppendLine( "<tr>" );
      foreach (string header in _headers)
      {
        sb.AppendLine( "<th>" + header + "</th>" );
      }
      sb.AppendLine( "</tr>" );
      return sb.ToString();
    }

    public string GenerateTableRow(params string[] dataRows )
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine( "<tr>" );
      foreach( string dataRow in dataRows )
      {
        sb.AppendLine( "<td>" + dataRow + "</td>" );
      }
      sb.AppendLine( "</tr>" );
      return sb.ToString();
    }

    public string GetFooterTable()
    {
      return "</table>";
    }

  }
}

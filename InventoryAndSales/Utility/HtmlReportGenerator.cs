using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace InventoryAndSales.Utility
{
  public class HtmlReportGenerator
  {
    public static void Write(string title, string body, string fullpath)
    {
      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Html);

        //Head
        writer.RenderBeginTag(HtmlTextWriterTag.Head); //Begin HEAD
        writer.RenderBeginTag(HtmlTextWriterTag.Title);
        writer.Write(title);
        writer.RenderEndTag();

        writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
        writer.AddAttribute(HtmlTextWriterAttribute.Href, "../datatables.min.css");
        writer.RenderBeginTag(HtmlTextWriterTag.Link);
        writer.RenderEndTag();

        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
        writer.AddAttribute(HtmlTextWriterAttribute.Src, "../datatables.min.js");
        writer.RenderBeginTag(HtmlTextWriterTag.Script);
        writer.RenderEndTag();

        writer.RenderEndTag(); //END HEAD

        writer.RenderBeginTag(HtmlTextWriterTag.Body); //Body
        writer.Write(body);
        writer.RenderEndTag(); //End Body

        writer.RenderEndTag();

      }
      File.WriteAllText(fullpath, stringWriter.ToString());
    }
  }
}

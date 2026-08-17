using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace InventoryAndSales.Utility
{
  /// <summary>
  /// Wraps a rendered table in a complete HTML document.
  /// </summary>
  public class HtmlReportGenerator
  {
    /// <summary>
    /// Writes the report file.
    /// </summary>
    /// <param name="styleSheetHref">
    /// Relative path to the stylesheet, or null to emit a self contained page with no assets. The
    /// paths are supplied by the caller rather than hardcoded so the report folder can be moved.
    /// </param>
    public static void Write(string title, string body, string fullpath, string styleSheetHref, string scriptSrc)
    {
      StringWriter stringWriter = new StringWriter();
      using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
      {
        writer.RenderBeginTag(HtmlTextWriterTag.Html);

        writer.RenderBeginTag(HtmlTextWriterTag.Head);

        writer.AddAttribute("charset", "utf-8");
        writer.RenderBeginTag(HtmlTextWriterTag.Meta);
        writer.RenderEndTag();

        writer.RenderBeginTag(HtmlTextWriterTag.Title);
        writer.WriteEncodedText(title ?? string.Empty);
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

        writer.RenderEndTag(); // head

        writer.RenderBeginTag(HtmlTextWriterTag.Body);
        writer.Write(body);
        writer.RenderEndTag(); // body

        writer.RenderEndTag(); // html
      }

      File.WriteAllText(fullpath, stringWriter.ToString(), Encoding.UTF8);
    }
  }
}

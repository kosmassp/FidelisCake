using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace SimpleCommon.Utility
{
  public class PrinterUtility
  {
    public static void Print(List<StringPrint> textToPrint, Font font)
    {
      PrintObject po = new PrintObject(textToPrint, font);
      po.Print();
    }
  }

  public class StringPrint
  {
    public string Text { get; set; }

    private StringFormat _format;
    public StringFormat Format
    {
      get
      {
        if (_format == null)
          return new StringFormat();
        return _format;
      }
      set { _format = value; }
    }

    public StringPrint(string text)
    {
      Text = text;
    }

    public StringPrint(string text, StringFormat format)
      : this(text)
    {
      _format = format;
    }
  }

  internal class PrintObject
  {
    private List<StringPrint> _textToPrint;
    private List<StringPrint>.Enumerator enumerator;
    private Font _printFont;
    public PrintObject(List<StringPrint> textToPrint, Font font)
    {
      _textToPrint = textToPrint;
      _printFont = font;
      enumerator = _textToPrint.GetEnumerator();
    }

    public void Print()
    {
      string printerName = ConfigurationManager.AppSettings["PrinterName"];
      PrintDocument printDoc = new PrintDocument();
      printDoc.PrinterSettings.PrinterName = printerName;
      Margins margins = new Margins(0, 0, 0, 0);
      printDoc.DefaultPageSettings.PaperSize = new PaperSize("Receipt",265,10000);
      printDoc.DefaultPageSettings.Margins = margins;
      printDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
      printDoc.Print();
    }


    // The PrintPage event is raised for each page to be printed.
    private void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
      float linesPerPage = 0;
      float yPos = 0;
      int count = 0;
      float leftMargin = ev.MarginBounds.Left;
      float topMargin = ev.MarginBounds.Top;
      var page= ev.PageSettings;
      String line = null;

      // Calculate the number of lines per page.
      linesPerPage = ev.MarginBounds.Height / _printFont.GetHeight(ev.Graphics);

      // Iterate over the file, printing each line.
      while (count < linesPerPage  )
      {
        if (!enumerator.MoveNext())
        {
          line = null;
          break;
        }
        StringPrint sp = enumerator.Current;
        line = sp.Text;
        yPos = topMargin + (count*_printFont.GetHeight(ev.Graphics));
        RectangleF rect = new RectangleF(leftMargin, yPos, page.PaperSize.Width, page.PaperSize.Height);
        ev.Graphics.DrawString(line, _printFont, Brushes.Black, rect, sp.Format);
        //ev.Graphics.DrawString(line, _printFont, Brushes.Black, leftMargin, yPos, sp.Format);
        count++;
      }

      // If more lines exist, print another page.
      if (line != null)
        ev.HasMorePages = true;
      else
        ev.HasMorePages = false;
    }
  }
}

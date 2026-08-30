using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace SimpleCommon.Utility
{
  /// <summary>
  /// Where and how a receipt is printed.
  ///
  /// Passed in rather than read from configuration, so this library stays usable by anything and the
  /// hosting application decides where the values come from.
  /// </summary>
  public class PrintSettings
  {
    /// <summary>Windows printer name. Empty or null uses the Windows default printer.</summary>
    public string PrinterName { get; set; }

    /// <summary>Printable paper width in millimetres. Governs where receipt lines wrap.</summary>
    public int PaperWidthMm { get; set; }

    public PrintSettings(string printerName, int paperWidthMm)
    {
      PrinterName = printerName;
      PaperWidthMm = paperWidthMm;
    }

    /// <summary>
    /// Paper width in the hundredths of an inch <see cref="PaperSize"/> expects.
    /// </summary>
    public int PaperWidthUnits
    {
      get { return MillimetresToUnits(PaperWidthMm); }
    }

    public static int MillimetresToUnits(int millimetres)
    {
      return (int)Math.Round(millimetres / 25.4 * 100.0);
    }

    public static int UnitsToMillimetres(int units)
    {
      return (int)Math.Round(units * 25.4 / 100.0);
    }
  }

  public class PrinterUtility
  {
    /// <summary>Names of the printers installed on this machine.</summary>
    public static List<string> GetInstalledPrinters()
    {
      List<string> printers = new List<string>();
      foreach (string name in PrinterSettings.InstalledPrinters)
        printers.Add(name);
      return printers;
    }

    /// <summary>Name Windows would print to when none is chosen, or empty if there is none.</summary>
    public static string GetDefaultPrinterName()
    {
      try
      {
        return new PrinterSettings().PrinterName ?? string.Empty;
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Whether a printer exists and reports itself as usable. Checked before printing so the
    /// operator gets a clear message instead of an exception.
    /// </summary>
    public static bool IsPrinterAvailable(string printerName)
    {
      try
      {
        PrinterSettings settings = new PrinterSettings();
        if (!string.IsNullOrEmpty(printerName))
          settings.PrinterName = printerName;
        return settings.IsValid;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public static void Print(List<StringPrint> textToPrint, Font font, PrintSettings settings)
    {
      PrintObject po = new PrintObject(textToPrint, font, settings);
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
    private readonly List<StringPrint> _textToPrint;
    private List<StringPrint>.Enumerator enumerator;
    private readonly Font _printFont;
    private readonly PrintSettings _settings;

    public PrintObject(List<StringPrint> textToPrint, Font font, PrintSettings settings)
    {
      _textToPrint = textToPrint;
      _printFont = font;
      _settings = settings;
      enumerator = _textToPrint.GetEnumerator();
    }

    public void Print()
    {
      using (PrintDocument printDoc = new PrintDocument())
      {
        // Left unset, the document goes to the Windows default printer.
        if (!string.IsNullOrEmpty(_settings.PrinterName))
          printDoc.PrinterSettings.PrinterName = _settings.PrinterName;

        // Height is effectively unbounded: a receipt is one long page.
        printDoc.DefaultPageSettings.PaperSize =
          new PaperSize("Receipt", _settings.PaperWidthUnits, 10000);
        printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        printDoc.PrintPage += pd_PrintPage;
        printDoc.Print();
      }
    }

    // The PrintPage event is raised for each page to be printed.
    private void pd_PrintPage(object sender, PrintPageEventArgs ev)
    {
      float linesPerPage = 0;
      float yPos = 0;
      int count = 0;
      float leftMargin = ev.MarginBounds.Left;
      float topMargin = ev.MarginBounds.Top;
      var page = ev.PageSettings;
      String line = null;

      // Calculate the number of lines per page.
      linesPerPage = ev.MarginBounds.Height / _printFont.GetHeight(ev.Graphics);

      // Iterate over the file, printing each line.
      while (count < linesPerPage)
      {
        if (!enumerator.MoveNext())
        {
          line = null;
          break;
        }
        StringPrint sp = enumerator.Current;
        line = sp.Text;
        yPos = topMargin + (count * _printFont.GetHeight(ev.Graphics));
        RectangleF rect = new RectangleF(leftMargin, yPos, page.PaperSize.Width, page.PaperSize.Height);
        ev.Graphics.DrawString(line, _printFont, Brushes.Black, rect, sp.Format);
        count++;
      }

      // If more lines exist, print another page.
      ev.HasMorePages = line != null;
    }
  }
}

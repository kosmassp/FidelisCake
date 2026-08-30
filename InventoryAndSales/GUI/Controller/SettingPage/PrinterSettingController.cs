using System;
using System.Collections.Generic;
using System.Drawing;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Popup.SettingPage;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the printer settings page: which printer receipts go to, how wide the paper is, and a
  /// test print so the answer can be checked without ringing up a sale.
  /// </summary>
  internal class PrinterSettingController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// Sensible bounds for a receipt roll. Narrower than 40 mm fits almost nothing; wider than
    /// 120 mm is not a receipt printer.
    /// </summary>
    public const int MinPaperWidthMm = 40;
    public const int MaxPaperWidthMm = 120;

    private readonly PrinterSettingForm _view;
    private readonly SettingsService _settings;
    private readonly CashierManager _cashierManager;

    public PrinterSettingController(PrinterSettingForm view)
    {
      _view = view;
      _settings = BusinessFactory.GetInstance().Settings;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
    }

    public List<string> GetInstalledPrinters()
    {
      return PrinterUtility.GetInstalledPrinters();
    }

    public string GetDefaultPrinterName()
    {
      return PrinterUtility.GetDefaultPrinterName();
    }

    /// <summary>Configured printer, or empty meaning the Windows default.</summary>
    public string GetPrinterName()
    {
      return _settings.GetString(SettingKeys.PrinterName, string.Empty);
    }

    public int GetPaperWidthMm()
    {
      return _settings.GetInt(SettingKeys.PrinterPaperWidthMm, SettingKeys.DefaultPaperWidthMm);
    }

    public int GetDefaultPaperWidthMm()
    {
      return SettingKeys.DefaultPaperWidthMm;
    }

    public bool IsPrinterAvailable(string printerName)
    {
      return PrinterUtility.IsPrinterAvailable(printerName);
    }

    /// <summary>Empty when the values can be saved, otherwise a message for the operator.</summary>
    public string Validate(string printerName, int paperWidthMm)
    {
      if (paperWidthMm < MinPaperWidthMm || paperWidthMm > MaxPaperWidthMm)
      {
        return string.Format("Lebar kertas harus antara {0} dan {1} mm.", MinPaperWidthMm, MaxPaperWidthMm);
      }

      // An empty name is allowed and means the Windows default printer.
      if (!string.IsNullOrEmpty(printerName) && !PrinterUtility.IsPrinterAvailable(printerName))
        return "Printer tersebut tidak tersedia. Pilih printer lain.";

      return string.Empty;
    }

    /// <summary>Empty on success, otherwise a message for the operator.</summary>
    public string Save(string printerName, int paperWidthMm)
    {
      string problem = Validate(printerName, paperWidthMm);
      if (!string.IsNullOrEmpty(problem))
        return problem;

      _settings.SetString(SettingKeys.PrinterName, printerName ?? string.Empty);
      _settings.SetInt(SettingKeys.PrinterPaperWidthMm, paperWidthMm);
      _log.InfoFormat("Printer set to '{0}' at {1} mm.",
                      string.IsNullOrEmpty(printerName) ? "<Windows default>" : printerName, paperWidthMm);
      return string.Empty;
    }

    /// <summary>
    /// Prints a sample receipt using the values currently on screen, without saving them, so the
    /// operator can try a width and see the result before committing to it.
    /// </summary>
    /// <returns>Empty on success, otherwise a message for the operator.</returns>
    public string TestPrint(string printerName, int paperWidthMm)
    {
      string problem = Validate(printerName, paperWidthMm);
      if (!string.IsNullOrEmpty(problem))
        return problem;

      try
      {
        PrinterUtility.Print(BuildTestReceipt(paperWidthMm),
                             _cashierManager.GetPrintFont(),
                             new PrintSettings(printerName, paperWidthMm));
        return string.Empty;
      }
      catch (Exception e)
      {
        _log.Error("Test print failed.", e);
        return "Gagal mencetak. Pastikan printer menyala dan terhubung.";
      }
    }

    /// <summary>
    /// A sample receipt in the real layout, with the shop's own header and footer, plus a ruler line
    /// that shows whether the chosen width is actually what the paper takes.
    /// </summary>
    public List<StringPrint> BuildTestReceipt(int paperWidthMm)
    {
      Transaction transaction = new Transaction
      {
        Notes = "TES CETAK",
        Time = DateTime.Now,
        Factur = "TES-" + DateTime.Now.ToString("HHmmss"),
        Payment = 50000,
        UserId = 0,
        CustomerId = 0,
        TotalPrice = 0,
        TotalDiscount = 0,
        Total = 0,
      };

      var details = new List<TransactionDetail>();
      for (int i = 1; i <= 2; i++)
      {
        TransactionDetail td = new TransactionDetail
        {
          ProductName = "CONTOH BARANG " + i,
          ProductId = 0,
          ProductPrice = i * 10000,
          ProductDiscount = i == 2 ? 500 : 0,
          Quantity = i,
        };
        td.SubtotalPrice = td.ProductPrice * td.Quantity;
        td.SubtotalDiscount = td.ProductDiscount * td.Quantity;
        td.Subtotal = td.SubtotalPrice - td.SubtotalDiscount;

        transaction.TotalPrice += td.SubtotalPrice;
        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.Total += td.Subtotal;
        details.Add(td);
      }
      transaction.Exchange = transaction.Payment - transaction.Total;

      List<StringPrint> lines = ReceiptBuilder.Build(
        _cashierManager.GetHeaderNote(), _cashierManager.GetFooterNote(),
        transaction, details, "TES PRINTER");

      // If this line wraps or runs off the paper, the configured width is wrong.
      lines.Add(new StringPrint(string.Empty));
      lines.Add(new StringPrint(string.Format("Lebar kertas: {0} mm", paperWidthMm)));
      lines.Add(new StringPrint(ReceiptBuilder.LineSeparator));
      return lines;
    }
  }
}

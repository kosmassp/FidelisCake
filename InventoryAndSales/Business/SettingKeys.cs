using System;
using System.Collections.Generic;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Every key stored in M_SETTINGS, together with the group it belongs to and the value a fresh
  /// installation starts with.
  ///
  /// Installations are on many different versions and there is no migration history, so seeding is
  /// driven from <see cref="Seed"/> and applied by DBUtility on every startup. Adding a key here is
  /// all that is needed for old installations to pick it up on their next launch - the insert is
  /// guarded, so operator edits are never overwritten.
  /// </summary>
  public static class SettingKeys
  {
    public const string GroupGeneral = "GENERAL";
    public const string GroupReport = "REPORT";
    public const string GroupSecurity = "SECURITY";
    public const string GroupPrinter = "PRINTER";

    /// <summary>Receipt header lines. Multi-line - stored with %NEW_LINE% separators.</summary>
    public const string Header = "HEADER";

    /// <summary>Receipt footer lines. Multi-line - stored with %NEW_LINE% separators.</summary>
    public const string Footer = "FOOTER";

    /// <summary>
    /// Folder the generated HTML reports are written to. May contain environment variables
    /// (for example %USERPROFILE%\Laporan); they are expanded when the value is read.
    /// </summary>
    public const string ReportDirectory = "REPORT_DIRECTORY";

    /// <summary>
    /// Whether the built-in recovery account may sign in. Defaults to enabled so that existing
    /// installations keep working exactly as before after an upgrade.
    /// </summary>
    public const string AllowBuiltInAdmin = "ALLOW_BUILTIN_ADMIN";

    /// <summary>
    /// Windows name of the receipt printer. Empty means the Windows default printer.
    ///
    /// Seeded from the PrinterName entry in App.config so an upgrade keeps printing to whatever the
    /// site was already using; from then on it is edited in the application.
    /// </summary>
    public const string PrinterName = "PRINTER_NAME";

    /// <summary>
    /// Printable width of the receipt paper, in millimetres. Governs where receipt lines wrap.
    /// </summary>
    public const string PrinterPaperWidthMm = "PRINTER_PAPER_WIDTH_MM";

    /// <summary>
    /// Roll width the previous build effectively used: it hardcoded 265 hundredths of an inch,
    /// which is 67.3 mm. Rounding to 67 keeps every existing receipt laid out as before.
    /// </summary>
    public const int DefaultPaperWidthMm = 67;

    /// <summary>
    /// The EDC terminals a cashier can pick from, one per line.
    ///
    /// A list rather than a table: a terminal is only a name, and the multi-line setting machinery
    /// already exists. If a terminal ever needs more than a name - a bank, a merchant id - this
    /// should become a table.
    /// </summary>
    public const string EdcTerminals = "EDC_TERMINALS";

    /// <summary>Marker used inside a setting value to represent a line break.</summary>
    public const string NewLineToken = "%NEW_LINE%";

    public class SettingSeed
    {
      public string Key { get; private set; }
      public string Group { get; private set; }
      public string Value { get; private set; }

      public SettingSeed(string key, string group, string value)
      {
        Key = key;
        Group = group;
        Value = value;
      }
    }

    /// <summary>
    /// Rows a database is expected to contain. Only missing rows are inserted.
    /// </summary>
    public static List<SettingSeed> Seed()
    {
      return new List<SettingSeed>
      {
        new SettingSeed(Header, GroupGeneral,
          "FIDELIS CAKE AND BAKERY" + NewLineToken +
          "JL MAYJEND SUTOYO NO 1" + NewLineToken +
          "BANJARNEGARA" + NewLineToken +
          "(0286) 594573"),

        new SettingSeed(Footer, GroupGeneral,
          "TERIMA KASIH" + NewLineToken +
          "SELAMAT MENIKMATI"),

        new SettingSeed(ReportDirectory, GroupReport, DefaultReportDirectory()),

        new SettingSeed(AllowBuiltInAdmin, GroupSecurity, "true"),

        // Seeded empty: a shop with no card terminals should not be offered imaginary ones.
        new SettingSeed(EdcTerminals, GroupGeneral, string.Empty),

        new SettingSeed(PrinterName, GroupPrinter, LegacyConfiguredPrinterName()),
        new SettingSeed(PrinterPaperWidthMm, GroupPrinter,
                        DefaultPaperWidthMm.ToString(System.Globalization.CultureInfo.InvariantCulture)),
      };
    }

    /// <summary>
    /// The printer this installation was already using, taken from App.config once so that moving
    /// the setting into the database does not silently change which printer receipts go to.
    /// Empty when unset, which means the Windows default printer.
    /// </summary>
    private static string LegacyConfiguredPrinterName()
    {
      string configured = System.Configuration.ConfigurationManager.AppSettings["PrinterName"];
      return configured == null ? string.Empty : configured.Trim();
    }

    /// <summary>
    /// Where reports go when nothing has been configured: a per-user folder that needs no
    /// administrator rights and is easy for an operator to find again.
    /// </summary>
    public static string DefaultReportDirectory()
    {
      string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      if (string.IsNullOrEmpty(documents))
        documents = AppDomain.CurrentDomain.BaseDirectory;
      return System.IO.Path.Combine(documents, "FidelisCake", "Laporan");
    }
  }
}

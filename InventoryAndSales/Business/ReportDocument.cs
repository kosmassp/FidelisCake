using System;
using System.Globalization;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Everything a generated report says about itself: which shop it belongs to, what it covers, who
  /// asked for it and when — alongside the figures.
  ///
  /// A report leaves the application as a file that gets mailed on, printed and filed, so it has to
  /// answer those questions on its own. A page of numbers with no period on it cannot be checked by
  /// anyone who was not standing at the till when it was made.
  /// </summary>
  public class ReportDocument
  {
    public ReportDocument(string title, string shopName, DateTime start, DateTime stop,
                          string generatedBy, DateTime generatedAt, ReportTable table)
    {
      Title = title ?? string.Empty;
      ShopName = shopName ?? string.Empty;
      Start = start;
      Stop = stop;
      GeneratedBy = generatedBy ?? string.Empty;
      GeneratedAt = generatedAt;
      Table = table;
    }

    public string Title { get; private set; }

    /// <summary>Taken from the receipt header, so the report and the receipt name the same shop.</summary>
    public string ShopName { get; private set; }

    public DateTime Start { get; private set; }
    public DateTime Stop { get; private set; }
    public string GeneratedBy { get; private set; }
    public DateTime GeneratedAt { get; private set; }
    public ReportTable Table { get; private set; }

    /// <summary>The covered period, collapsed to a single date when both ends are the same day.</summary>
    public string PeriodText
    {
      get
      {
        string from = Start.ToString(DateFormat, CultureInfo.InvariantCulture);
        if (Start.Date == Stop.Date)
          return from;
        return from + " - " + Stop.ToString(DateFormat, CultureInfo.InvariantCulture);
      }
    }

    public string GeneratedAtText
    {
      get { return GeneratedAt.ToString(DateTimeFormat, CultureInfo.InvariantCulture); }
    }

    // Explicit, and invariant, for the same reason every other date in this application is: the
    // machine's own culture must never decide what a saved report says.
    private const string DateFormat = "dd MMM yyyy";
    private const string DateTimeFormat = "dd MMM yyyy HH:mm";
  }
}

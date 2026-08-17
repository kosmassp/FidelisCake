using System;

namespace InventoryAndSales.Database.Model
{
  /// <summary>
  /// One cashier's takings for a day, split by how they were paid.
  ///
  /// Kept apart because only <see cref="Cash"/> is money that has to be counted out of the drawer at
  /// the end of the day; card takings settle through the bank.
  ///
  /// Values are already formatted for display, as everything from the report layer is.
  /// </summary>
  public class CashierDayTotals
  {
    public string Cash { get; private set; }
    public string Edc { get; private set; }

    public CashierDayTotals(string cash, string edc)
    {
      Cash = cash ?? "0";
      Edc = edc ?? "0";
    }

    /// <summary>True when no card payments were taken, so the split is not worth showing.</summary>
    public bool EdcIsZero
    {
      get
      {
        decimal parsed;
        return decimal.TryParse(Edc, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.CurrentCulture, out parsed) && parsed == 0m;
      }
    }
  }
}

using System;

namespace InventoryAndSales.Database.Model
{
  /// <summary>
  /// One cashier's takings for a day, split by how they were paid.
  ///
  /// Kept apart because only <see cref="Cash"/> is money that has to be counted out of the drawer at
  /// the end of the day; the electronic takings settle through the bank.
  ///
  /// Values are already formatted for display, as everything from the report layer is.
  /// </summary>
  public class CashierDayTotals
  {
    public string Cash { get; private set; }
    public string Edc { get; private set; }
    public string Qris { get; private set; }
    public string Transfer { get; private set; }

    public CashierDayTotals(string cash, string edc, string qris, string transfer)
    {
      Cash = cash ?? "0";
      Edc = edc ?? "0";
      Qris = qris ?? "0";
      Transfer = transfer ?? "0";
    }

    /// <summary>
    /// True when everything was cash, so there is nothing to split and the old single figure is
    /// still the clearest thing to show.
    /// </summary>
    public bool CashOnly
    {
      get { return IsZero(Edc) && IsZero(Qris) && IsZero(Transfer); }
    }

    public bool EdcIsZero
    {
      get { return IsZero(Edc); }
    }

    public bool QrisIsZero
    {
      get { return IsZero(Qris); }
    }

    public bool TransferIsZero
    {
      get { return IsZero(Transfer); }
    }

    private static bool IsZero(string amount)
    {
      decimal parsed;
      return decimal.TryParse(amount, System.Globalization.NumberStyles.Any,
                              System.Globalization.CultureInfo.CurrentCulture, out parsed) && parsed == 0m;
    }
  }
}

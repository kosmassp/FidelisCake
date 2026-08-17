using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// The list of EDC terminals a cashier can pick from, kept as one setting holding a name per line.
  /// </summary>
  public class EdcTerminalService
  {
    private readonly SettingsService _settings;

    public EdcTerminalService(SettingsService settings)
    {
      _settings = settings;
    }

    /// <summary>
    /// Configured terminals, in order, with blanks and duplicates removed. Empty when the shop takes
    /// no card payments, which is what disables the EDC option on the sale screen.
    /// </summary>
    public List<string> GetTerminals()
    {
      string raw = _settings.GetMultiLine(SettingKeys.EdcTerminals, string.Empty);
      return Parse(raw);
    }

    public void SetTerminals(IEnumerable<string> terminals)
    {
      List<string> cleaned = Clean(terminals);
      _settings.SetMultiLine(SettingKeys.EdcTerminals, string.Join(Environment.NewLine, cleaned.ToArray()));
    }

    public bool HasTerminals()
    {
      return GetTerminals().Count > 0;
    }

    /// <summary>
    /// Whether a terminal is one this shop actually has. Checked at checkout so a terminal removed
    /// while a sale was being rung up cannot be recorded against it.
    /// </summary>
    public bool IsKnown(string terminal)
    {
      if (string.IsNullOrWhiteSpace(terminal))
        return false;
      foreach (string known in GetTerminals())
      {
        if (string.Equals(known, terminal.Trim(), StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    private static List<string> Parse(string raw)
    {
      if (string.IsNullOrEmpty(raw))
        return new List<string>();
      return Clean(raw.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None));
    }

    private static List<string> Clean(IEnumerable<string> terminals)
    {
      List<string> cleaned = new List<string>();
      if (terminals == null)
        return cleaned;

      foreach (string terminal in terminals)
      {
        if (string.IsNullOrWhiteSpace(terminal))
          continue;
        string trimmed = terminal.Trim();
        bool duplicate = cleaned.Any(existing => string.Equals(existing, trimmed, StringComparison.OrdinalIgnoreCase));
        if (!duplicate)
          cleaned.Add(trimmed);
      }
      return cleaned;
    }
  }
}

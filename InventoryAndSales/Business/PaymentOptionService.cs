using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// The lists a cashier picks from when the payment is not cash: EDC terminals and QRIS providers.
  ///
  /// Each is one setting holding a name per line. Lists rather than tables because a terminal or a
  /// provider is only a name, and the multi-line setting machinery already exists. If one ever needs
  /// more than a name - a bank, a merchant id - it should become a table.
  /// </summary>
  public class PaymentOptionService
  {
    private readonly SettingsService _settings;

    public PaymentOptionService(SettingsService settings)
    {
      _settings = settings;
    }

    /// <summary>
    /// Configured EDC terminals. Empty means the shop takes no cards, which is what hides the EDC
    /// option on the sale screen.
    /// </summary>
    public List<string> GetEdcTerminals()
    {
      return GetList(SettingKeys.EdcTerminals);
    }

    public void SetEdcTerminals(IEnumerable<string> terminals)
    {
      SetList(SettingKeys.EdcTerminals, terminals);
    }

    /// <summary>Configured QRIS providers. Empty means the shop takes no QRIS.</summary>
    public List<string> GetQrisProviders()
    {
      return GetList(SettingKeys.QrisProviders);
    }

    public void SetQrisProviders(IEnumerable<string> providers)
    {
      SetList(SettingKeys.QrisProviders, providers);
    }

    public bool HasEdcTerminals()
    {
      return GetEdcTerminals().Count > 0;
    }

    public bool HasQrisProviders()
    {
      return GetQrisProviders().Count > 0;
    }

    /// <summary>
    /// Whether a name is one the shop actually has. Checked at checkout so a terminal or provider
    /// removed while a sale was being rung up cannot be recorded against it.
    /// </summary>
    public bool IsKnownEdcTerminal(string terminal)
    {
      return Contains(GetEdcTerminals(), terminal);
    }

    public bool IsKnownQrisProvider(string provider)
    {
      return Contains(GetQrisProviders(), provider);
    }

    private List<string> GetList(string key)
    {
      return Parse(_settings.GetMultiLine(key, string.Empty));
    }

    private void SetList(string key, IEnumerable<string> values)
    {
      List<string> cleaned = Clean(values);
      _settings.SetMultiLine(key, string.Join(Environment.NewLine, cleaned.ToArray()));
    }

    private static bool Contains(List<string> known, string candidate)
    {
      if (string.IsNullOrWhiteSpace(candidate))
        return false;
      return known.Any(k => string.Equals(k, candidate.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private static List<string> Parse(string raw)
    {
      if (string.IsNullOrEmpty(raw))
        return new List<string>();
      return Clean(raw.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None));
    }

    private static List<string> Clean(IEnumerable<string> values)
    {
      List<string> cleaned = new List<string>();
      if (values == null)
        return cleaned;

      foreach (string value in values)
      {
        if (string.IsNullOrWhiteSpace(value))
          continue;
        string trimmed = value.Trim();
        if (!cleaned.Any(existing => string.Equals(existing, trimmed, StringComparison.OrdinalIgnoreCase)))
          cleaned.Add(trimmed);
      }
      return cleaned;
    }
  }
}

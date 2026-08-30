using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// A QRIS provider the shop is set up with.
  ///
  /// The code type belongs to the provider, not to the sale: a shop's arrangement with a provider is
  /// either a printed sticker at the till or codes generated per transaction, and that does not
  /// change from one customer to the next. The cashier picks a provider and the type follows.
  /// </summary>
  public class QrisProvider
  {
    public string Name { get; private set; }
    public QrisMode Mode { get; private set; }

    public QrisProvider(string name, QrisMode mode)
    {
      Name = (name ?? string.Empty).Trim();
      Mode = mode;
    }

    /// <summary>Indonesian label for the code type.</summary>
    public string ModeLabel
    {
      get { return Mode == QrisMode.Dynamic ? "Dinamis" : "Statis"; }
    }

    /// <summary>What the cashier sees in the dropdown, so the type is visible at the till.</summary>
    public override string ToString()
    {
      return string.Format("{0} ({1})", Name, ModeLabel);
    }
  }

  /// <summary>
  /// The lists a cashier picks from when the payment is not cash: EDC terminals, QRIS providers and
  /// transfer destination accounts.
  ///
  /// Each is one setting holding an entry per line. Lists rather than tables because an entry is
  /// little more than a name; if one ever needs a bank or a merchant id it should become a table.
  /// </summary>
  public class PaymentOptionService
  {
    /// <summary>Separates a QRIS provider's name from its code type in the stored line.</summary>
    private const char FieldSeparator = '|';

    private readonly SettingsService _settings;

    public PaymentOptionService(SettingsService settings)
    {
      _settings = settings;
    }

    #region EDC terminals

    /// <summary>
    /// Configured EDC terminals. Empty means the shop takes no cards, which is what keeps EDC off
    /// the sale screen.
    /// </summary>
    public List<string> GetEdcTerminals()
    {
      return CleanNames(SplitLines(_settings.GetMultiLine(SettingKeys.EdcTerminals, string.Empty)));
    }

    public void SetEdcTerminals(IEnumerable<string> terminals)
    {
      List<string> cleaned = CleanNames(terminals);
      _settings.SetMultiLine(SettingKeys.EdcTerminals, string.Join(Environment.NewLine, cleaned.ToArray()));
    }

    public bool HasEdcTerminals()
    {
      return GetEdcTerminals().Count > 0;
    }

    /// <summary>
    /// Whether a terminal is one the shop actually has. Checked at checkout so a terminal removed
    /// while a sale was being rung up cannot be recorded against it.
    /// </summary>
    public bool IsKnownEdcTerminal(string terminal)
    {
      if (string.IsNullOrWhiteSpace(terminal))
        return false;
      return GetEdcTerminals().Any(t => string.Equals(t, terminal.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region Transfer banks

    /// <summary>
    /// The accounts a customer can transfer to, as the cashier reads them out - typically bank,
    /// number and holder in one line. Empty means the shop takes no transfers, which is what keeps
    /// the method off the sale screen.
    /// </summary>
    public List<string> GetTransferBanks()
    {
      return CleanNames(SplitLines(_settings.GetMultiLine(SettingKeys.TransferBanks, string.Empty)));
    }

    public void SetTransferBanks(IEnumerable<string> banks)
    {
      List<string> cleaned = CleanNames(banks);
      _settings.SetMultiLine(SettingKeys.TransferBanks, string.Join(Environment.NewLine, cleaned.ToArray()));
    }

    public bool HasTransferBanks()
    {
      return GetTransferBanks().Count > 0;
    }

    /// <summary>
    /// Whether an account is one the shop actually holds. Checked at checkout so an account removed
    /// while a sale was being rung up cannot be recorded against it.
    /// </summary>
    public bool IsKnownTransferBank(string bank)
    {
      if (string.IsNullOrWhiteSpace(bank))
        return false;
      return GetTransferBanks().Any(b => string.Equals(b, bank.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region QRIS providers

    /// <summary>Configured QRIS providers, each with its code type. Empty means the shop takes no QRIS.</summary>
    public List<QrisProvider> GetQrisProviders()
    {
      List<QrisProvider> providers = new List<QrisProvider>();
      foreach (string line in SplitLines(_settings.GetMultiLine(SettingKeys.QrisProviders, string.Empty)))
      {
        if (string.IsNullOrWhiteSpace(line))
          continue;

        // Lines written before the code type existed are just a name, and a shop's first QRIS
        // arrangement is normally the printed sticker - so an unqualified line reads as static.
        string[] parts = line.Split(FieldSeparator);
        string name = parts[0].Trim();
        if (name.Length == 0)
          continue;
        QrisMode mode = parts.Length > 1 && string.Equals(parts[1].Trim(), PaymentDetail.DynamicCode,
                                                          StringComparison.OrdinalIgnoreCase)
          ? QrisMode.Dynamic
          : QrisMode.Static;

        if (!providers.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
          providers.Add(new QrisProvider(name, mode));
      }
      return providers;
    }

    public void SetQrisProviders(IEnumerable<QrisProvider> providers)
    {
      List<string> lines = new List<string>();
      if (providers != null)
      {
        foreach (QrisProvider provider in providers)
        {
          if (provider == null || string.IsNullOrWhiteSpace(provider.Name))
            continue;
          if (lines.Any(l => string.Equals(l.Split(FieldSeparator)[0], provider.Name, StringComparison.OrdinalIgnoreCase)))
            continue;
          lines.Add(provider.Name + FieldSeparator +
                    (provider.Mode == QrisMode.Dynamic ? PaymentDetail.DynamicCode : PaymentDetail.StaticCode));
        }
      }
      _settings.SetMultiLine(SettingKeys.QrisProviders, string.Join(Environment.NewLine, lines.ToArray()));
    }

    public bool HasQrisProviders()
    {
      return GetQrisProviders().Count > 0;
    }

    /// <summary>
    /// The configured provider of that name, or null. Used at checkout both to confirm the provider
    /// still exists and to take its code type, which the till does not choose.
    /// </summary>
    public QrisProvider FindQrisProvider(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return null;
      return GetQrisProviders()
        .FirstOrDefault(p => string.Equals(p.Name, name.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>A provider name may not contain the character that separates it from its type.</summary>
    public static bool IsValidProviderName(string name)
    {
      return name != null && name.IndexOf(FieldSeparator) < 0;
    }

    #endregion

    private static string[] SplitLines(string raw)
    {
      if (string.IsNullOrEmpty(raw))
        return new string[0];
      return raw.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
    }

    private static List<string> CleanNames(IEnumerable<string> values)
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

using System;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// The shop's own identity — for now, what it is called.
  ///
  /// The name titles the main window and heads every generated report, so it cannot stay compiled
  /// in: the same build runs in more than one shop.
  ///
  /// Where the name comes from, in order:
  ///  1. the <see cref="SettingKeys.ShopName"/> setting, when somebody has set one;
  ///  2. otherwise the first line of the receipt header, which is where installations have been
  ///     writing their name for years — so an upgrade never renames a shop behind its back;
  ///  3. otherwise <see cref="SettingKeys.DefaultShopName"/>, so there is always something to show.
  ///
  /// Step 2 is why this is a resolution rule rather than a plain setting read, and why it lives in
  /// one place: a shop that reads its own name off two screens must see the same answer on both.
  /// </summary>
  public class ShopService
  {
    /// <summary>
    /// Long enough for a real shop name, short enough to stay inside a window title and a report
    /// heading without wrapping.
    /// </summary>
    public const int MaxNameLength = 60;

    private readonly SettingsService _settings;

    public ShopService(SettingsService settings)
    {
      _settings = settings;
    }

    public string GetName()
    {
      string configured = ConfiguredName();
      if (configured.Length > 0)
        return configured;

      string fromReceipt = FirstHeaderLine();
      return fromReceipt.Length > 0 ? fromReceipt : SettingKeys.DefaultShopName;
    }

    /// <summary>
    /// True when <see cref="GetName"/> is only echoing the receipt header because no name of its own
    /// has been saved. The settings page says so rather than pretending the box was filled in.
    /// </summary>
    public bool IsNameInherited()
    {
      return ConfiguredName().Length == 0;
    }

    public void SetName(string name)
    {
      _settings.SetString(SettingKeys.ShopName, (name ?? string.Empty).Trim());
    }

    /// <summary>Empty when the name can be saved, otherwise a message for the operator.</summary>
    public string ValidateName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return "Nama toko belum diisi.";

      if (name.Trim().Length > MaxNameLength)
        return string.Format("Nama toko terlalu panjang (maksimal {0} karakter).", MaxNameLength);

      return string.Empty;
    }

    private string ConfiguredName()
    {
      return (_settings.GetString(SettingKeys.ShopName, string.Empty) ?? string.Empty).Trim();
    }

    private string FirstHeaderLine()
    {
      string header = _settings.GetMultiLine(SettingKeys.Header, string.Empty) ?? string.Empty;
      foreach (string line in header.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
      {
        string trimmed = line.Trim();
        if (trimmed.Length > 0)
          return trimmed;
      }
      return string.Empty;
    }
  }
}

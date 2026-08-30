using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Typed access to the M_SETTINGS key/value store.
  ///
  /// Every read falls back to a caller-supplied default rather than throwing, so a row that is
  /// missing - because the database predates the setting, or because somebody deleted it - degrades
  /// to the default instead of breaking the feature that reads it.
  /// </summary>
  public class SettingsService
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly SettingConfigurationManager _settingManager;
    private readonly AuditService _audit;

    public SettingsService(SettingConfigurationManager settingManager, AuditService audit)
    {
      _settingManager = settingManager;
      _audit = audit;
    }

    public string GetString(string key, string fallback)
    {
      try
      {
        SettingConfiguration setting = _settingManager.FindByKey(key).FirstOrDefault();
        if (setting == null)
        {
          _log.WarnFormat("Setting '{0}' not found, using default.", key);
          return fallback;
        }

        // An existing row's value is returned as-is, empty included. Empty is a real answer for
        // some settings - it is how "use the Windows default printer" is expressed - and falling
        // back to the seeded Default here made such a value impossible to save: it was written,
        // then read back as whatever the row was first seeded with.
        return setting.Value;
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Failed reading setting '{0}', using default.", key), e);
        return fallback;
      }
    }

    /// <summary>
    /// Writes a value and records who changed it. Every configurable thing in the application ends
    /// up here, so this one place is what makes a settings change auditable at all.
    /// </summary>
    public void SetString(string key, string value)
    {
      SettingConfiguration setting = _settingManager.FindByKey(key).FirstOrDefault();
      if (setting == null)
      {
        _log.WarnFormat("Setting '{0}' does not exist and cannot be saved.", key);
        return;
      }

      string previous = setting.Value;
      if (string.Equals(previous, value, StringComparison.Ordinal))
        return;

      setting.Value = value;
      _settingManager.Update(setting);
      _log.InfoFormat("Setting '{0}' changed.", key);

      if (_audit != null)
      {
        _audit.Record(AuditService.ActionSettingChange, AuditService.EntitySetting, key,
                      string.Format("'{0}' -> '{1}'", Describe(previous), Describe(value)));
      }
    }

    /// <summary>
    /// Keeps one setting's value readable in an audit row. Multi-line values are collapsed and long
    /// ones cut, because the point is to see *that* it changed and roughly to what.
    /// </summary>
    private static string Describe(string value)
    {
      if (value == null)
        return string.Empty;

      string single = value.Replace(SettingKeys.NewLineToken, " | ").Replace("\r", " ").Replace("\n", " ");
      const int maxLength = 200;
      return single.Length <= maxLength ? single : single.Substring(0, maxLength) + "...";
    }

    public bool GetBool(string key, bool fallback)
    {
      string raw = GetString(key, fallback ? "true" : "false");
      bool parsed;
      if (bool.TryParse(raw == null ? string.Empty : raw.Trim(), out parsed))
        return parsed;

      // Tolerate the shapes an operator or an older build might have written.
      string normalized = (raw ?? string.Empty).Trim().ToLowerInvariant();
      if (normalized == "1" || normalized == "yes" || normalized == "y" || normalized == "on")
        return true;
      if (normalized == "0" || normalized == "no" || normalized == "n" || normalized == "off")
        return false;

      _log.WarnFormat("Setting '{0}' has non-boolean value '{1}', using default.", key, raw);
      return fallback;
    }

    public void SetBool(string key, bool value)
    {
      SetString(key, value ? "true" : "false");
    }

    public int GetInt(string key, int fallback)
    {
      string raw = GetString(key, null);
      int parsed;
      if (int.TryParse((raw ?? string.Empty).Trim(), System.Globalization.NumberStyles.Integer,
                       System.Globalization.CultureInfo.InvariantCulture, out parsed))
        return parsed;

      if (!string.IsNullOrEmpty(raw))
        _log.WarnFormat("Setting '{0}' has non-numeric value '{1}', using default.", key, raw);
      return fallback;
    }

    public void SetInt(string key, int value)
    {
      SetString(key, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Reads a value that holds several lines. Line breaks are stored as %NEW_LINE% because the
    /// column is a single-line string.
    /// </summary>
    public string GetMultiLine(string key, string fallback)
    {
      return DecodeNewLines(GetString(key, fallback));
    }

    public void SetMultiLine(string key, string value)
    {
      SetString(key, EncodeNewLines(value));
    }

    /// <summary>Turns real line breaks into the stored %NEW_LINE% token.</summary>
    public static string EncodeNewLines(string original)
    {
      if (original == null)
        return null;
      return original
        .Replace("\r\n", SettingKeys.NewLineToken)
        .Replace("\n", SettingKeys.NewLineToken)
        .Replace("\r", SettingKeys.NewLineToken);
    }

    /// <summary>Turns the stored %NEW_LINE% token back into real line breaks.</summary>
    public static string DecodeNewLines(string original)
    {
      if (original == null)
        return null;
      return original.Replace(SettingKeys.NewLineToken, Environment.NewLine);
    }
  }
}

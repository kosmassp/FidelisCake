using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Decides where generated reports go and makes sure the JavaScript and stylesheet they need are
  /// sitting next to them.
  ///
  /// Reports used to be written to a hardcoded c:\temp\Report and to link assets out of c:\temp,
  /// which somebody had to unpack by hand on every machine; if they had not, the report opened as a
  /// bare table with no sorting, searching or export and no indication why. The folder is now a
  /// setting, and the assets are unpacked automatically from the bundle shipped beside the
  /// executable.
  /// </summary>
  public class ReportService
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// Folder shipped beside the executable holding the files a generated report links to.
    /// </summary>
    public const string AssetSourceFolderName = "Report";

    /// <summary>Sub-folder of the report directory the assets are copied into.</summary>
    public const string AssetFolderName = "assets";

    public const string StyleSheetFileName = "datatables.min.css";
    public const string ScriptFileName = "datatables.min.js";

    private readonly SettingsService _settings;

    public ReportService(SettingsService settings)
    {
      _settings = settings;
    }

    /// <summary>
    /// Configured report folder, with any environment variables expanded so a value such as
    /// %USERPROFILE%\Laporan works. Falls back to the default when unset or unusable.
    /// </summary>
    public string GetReportDirectory()
    {
      string configured = _settings.GetString(SettingKeys.ReportDirectory, null);
      string resolved = Resolve(configured);
      if (!string.IsNullOrEmpty(resolved))
        return resolved;

      string fallback = SettingKeys.DefaultReportDirectory();
      _log.WarnFormat("Report directory '{0}' is not usable, falling back to '{1}'.", configured, fallback);
      return fallback;
    }

    public void SetReportDirectory(string directory)
    {
      _settings.SetString(SettingKeys.ReportDirectory, directory);
    }

    /// <summary>
    /// Checks a folder the operator typed or picked, without saving it. Returns an Indonesian
    /// message describing the problem, or empty when the folder can be used.
    /// </summary>
    public string ValidateReportDirectory(string directory)
    {
      if (string.IsNullOrWhiteSpace(directory))
        return "Folder laporan belum diisi.";

      string resolved;
      try
      {
        resolved = Path.GetFullPath(Environment.ExpandEnvironmentVariables(directory.Trim()));
      }
      catch (Exception)
      {
        return "Nama folder tidak valid.";
      }

      try
      {
        if (!Directory.Exists(resolved))
          Directory.CreateDirectory(resolved);

        // Prove it is actually writable rather than just present.
        string probe = Path.Combine(resolved, ".write_test");
        File.WriteAllText(probe, string.Empty);
        File.Delete(probe);
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Report directory '{0}' is not writable.", resolved), e);
        return "Folder tidak dapat ditulis. Pilih folder lain.";
      }

      return string.Empty;
    }

    private static string Resolve(string directory)
    {
      if (string.IsNullOrWhiteSpace(directory))
        return null;
      try
      {
        return Path.GetFullPath(Environment.ExpandEnvironmentVariables(directory.Trim()));
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not resolve report directory '{0}'.", directory), e);
        return null;
      }
    }

    /// <summary>
    /// Creates the report folder if needed and returns it. Throws only if it genuinely cannot be
    /// created, in which case the caller should tell the operator to pick another folder.
    /// </summary>
    public string PrepareReportDirectory()
    {
      string directory = GetReportDirectory();
      Directory.CreateDirectory(directory);
      return directory;
    }

    /// <summary>
    /// Makes sure the stylesheet and script sit under the report folder, copying them from the
    /// application's Report folder when they are not there yet.
    /// </summary>
    /// <returns>
    /// True when both assets are in place. False means the report will still open and be readable,
    /// just without sorting, searching and the export buttons.
    /// </returns>
    public bool EnsureAssets(string reportDirectory)
    {
      string assetDirectory = Path.Combine(reportDirectory, AssetFolderName);
      try
      {
        if (HasAssets(assetDirectory))
          return true;

        string source = GetAssetSourceDirectory();
        if (!Directory.Exists(source))
        {
          _log.ErrorFormat("Report asset folder '{0}' is missing; reports will render without DataTables.", source);
          return false;
        }

        Directory.CreateDirectory(assetDirectory);
        foreach (string fileName in AssetFileNames)
        {
          string from = Path.Combine(source, fileName);
          if (!File.Exists(from))
          {
            _log.ErrorFormat("Report asset '{0}' is missing.", from);
            continue;
          }
          File.Copy(from, Path.Combine(assetDirectory, fileName), true);
        }

        bool copied = HasAssets(assetDirectory);
        if (copied)
          _log.InfoFormat("Copied report assets into '{0}'.", assetDirectory);
        return copied;
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not prepare report assets in '{0}'.", assetDirectory), e);
        return false;
      }
    }

    private static string[] AssetFileNames
    {
      get { return new[] { StyleSheetFileName, ScriptFileName }; }
    }

    private static bool HasAssets(string assetDirectory)
    {
      foreach (string fileName in AssetFileNames)
      {
        if (!IsPresent(Path.Combine(assetDirectory, fileName)))
          return false;
      }
      return true;
    }

    private static bool IsPresent(string path)
    {
      var info = new FileInfo(path);
      return info.Exists && info.Length > 0;
    }

    /// <summary>Where the shipped assets live, beside the executable.</summary>
    public static string GetAssetSourceDirectory()
    {
      return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AssetSourceFolderName);
    }

    /// <summary>True when the application has the files it needs to make a report interactive.</summary>
    public static bool IsAssetSourcePresent()
    {
      string source = GetAssetSourceDirectory();
      foreach (string fileName in AssetFileNames)
      {
        if (!IsPresent(Path.Combine(source, fileName)))
          return false;
      }
      return true;
    }

    /// <summary>Relative href a generated report uses to reach the stylesheet.</summary>
    public static string StyleSheetHref
    {
      get { return AssetFolderName + "/" + StyleSheetFileName; }
    }

    /// <summary>Relative src a generated report uses to reach the script.</summary>
    public static string ScriptSrc
    {
      get { return AssetFolderName + "/" + ScriptFileName; }
    }
  }
}

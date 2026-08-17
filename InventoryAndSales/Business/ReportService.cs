using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

    /// <summary>Bundle shipped with the application, next to the executable.</summary>
    public const string AssetBundleFileName = "reportassets.zip";

    /// <summary>Sub-folder of the report directory the bundle is unpacked into.</summary>
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
    /// Makes sure the stylesheet and script are unpacked under the report folder.
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

        string bundle = GetAssetBundlePath();
        if (!File.Exists(bundle))
        {
          _log.ErrorFormat("Report asset bundle '{0}' is missing; reports will render without DataTables.", bundle);
          return false;
        }

        Directory.CreateDirectory(assetDirectory);
        ExtractBundle(bundle, assetDirectory);

        bool extracted = HasAssets(assetDirectory);
        if (extracted)
          _log.InfoFormat("Unpacked report assets into '{0}'.", assetDirectory);
        else
          _log.ErrorFormat("Report asset bundle '{0}' did not contain the expected files.", bundle);
        return extracted;
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not prepare report assets in '{0}'.", assetDirectory), e);
        return false;
      }
    }

    private static void ExtractBundle(string bundlePath, string assetDirectory)
    {
      using (ZipArchive archive = ZipFile.OpenRead(bundlePath))
      {
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
          if (string.IsNullOrEmpty(entry.Name))
            continue;

          // Only the two files are wanted, and taking the entry name alone keeps a crafted archive
          // from writing outside the target folder.
          if (!string.Equals(entry.Name, StyleSheetFileName, StringComparison.OrdinalIgnoreCase)
              && !string.Equals(entry.Name, ScriptFileName, StringComparison.OrdinalIgnoreCase))
            continue;

          string destination = Path.Combine(assetDirectory, entry.Name);
          entry.ExtractToFile(destination, true);
        }
      }
    }

    private static bool HasAssets(string assetDirectory)
    {
      return IsPresent(Path.Combine(assetDirectory, StyleSheetFileName))
             && IsPresent(Path.Combine(assetDirectory, ScriptFileName));
    }

    private static bool IsPresent(string path)
    {
      var info = new FileInfo(path);
      return info.Exists && info.Length > 0;
    }

    public static string GetAssetBundlePath()
    {
      return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AssetBundleFileName);
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

using System;
using System.IO;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Popup.SettingPage;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the report folder settings page.
  /// </summary>
  internal class ReportSettingController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly ReportSettingForm _view;
    private readonly ReportService _reportService;

    public ReportSettingController(ReportSettingForm view)
    {
      _view = view;
      _reportService = BusinessFactory.GetInstance().ReportService;
    }

    public string GetReportDirectory()
    {
      return _reportService.GetReportDirectory();
    }

    public string GetDefaultReportDirectory()
    {
      return SettingKeys.DefaultReportDirectory();
    }

    /// <summary>Empty when the folder can be used, otherwise a message for the operator.</summary>
    public string Validate(string directory)
    {
      return _reportService.ValidateReportDirectory(directory);
    }

    /// <summary>
    /// Saves the folder and unpacks the report assets into it straight away, so a problem shows up
    /// here rather than the first time somebody prints a report.
    /// </summary>
    /// <returns>Empty on success, otherwise a message for the operator.</returns>
    public string Save(string directory)
    {
      string problem = Validate(directory);
      if (!string.IsNullOrEmpty(problem))
        return problem;

      _reportService.SetReportDirectory(directory.Trim());

      string resolved = _reportService.GetReportDirectory();
      if (!_reportService.EnsureAssets(resolved))
      {
        return "Folder tersimpan, namun file pendukung laporan tidak dapat disiapkan. " +
               "Pastikan folder '" + ReportService.AssetSourceFolderName + "' ada di folder aplikasi.";
      }
      return string.Empty;
    }

    public bool AreAssetsReady()
    {
      try
      {
        string directory = _reportService.GetReportDirectory();
        return Directory.Exists(directory) && _reportService.EnsureAssets(directory);
      }
      catch (Exception e)
      {
        _log.Error("Could not check report assets.", e);
        return false;
      }
    }

    /// <summary>Whether the application has the files that make a report interactive.</summary>
    public bool IsAssetSourcePresent()
    {
      return ReportService.IsAssetSourcePresent();
    }

    public void OpenReportFolder()
    {
      string directory = _reportService.PrepareReportDirectory();
      System.Diagnostics.Process.Start(directory);
    }
  }
}

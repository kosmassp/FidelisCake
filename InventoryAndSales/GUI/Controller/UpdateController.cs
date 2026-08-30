using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Utility;

namespace InventoryAndSales.GUI.Controller
{
  /// <summary>
  /// Drives the update from the shell: check, ask, stage, hand over to the installer and close.
  ///
  /// The application is never updated without somebody saying yes. Applying one closes the till and
  /// restarts it, and doing that unannounced to a cashier with a customer at the counter is not a
  /// convenience.
  /// </summary>
  public class UpdateController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>Files the installer copy needs to run on its own, away from the installation.</summary>
    private static readonly string[] RunnerFiles =
    {
      UpdateService.ApplicationExecutable,
      "InventoryAndSales.exe.config",
      "log4net.config",
      "log4net.dll",
      "SimpleCommon.dll",
    };

    private readonly UpdateService _updateService;
    private readonly AuditService _audit;

    public UpdateController()
    {
      BusinessFactory factory = BusinessFactory.GetInstance();
      _updateService = factory.UpdateService;
      _audit = factory.Audit;
    }

    public bool IsConfigured
    {
      get { return _updateService.IsConfigured; }
    }

    /// <summary>
    /// Looks for a newer release and, if the operator agrees, installs it.
    /// </summary>
    /// <param name="announceWhenCurrent">
    /// True when a person asked and is waiting for an answer, so "you are up to date" is worth
    /// saying. False for the check that runs by itself at startup, which should stay silent unless
    /// it has something to report.
    /// </param>
    public void CheckForUpdate(bool announceWhenCurrent)
    {
      UpdateManifest manifest = _updateService.FetchManifest();

      if (manifest == null)
      {
        if (announceWhenCurrent)
        {
          MessageBox.Show(
            _updateService.IsConfigured
              ? "Informasi pembaruan tidak dapat dibaca. Periksa koneksi internet lalu coba lagi."
              : "Alamat berkas pembaruan belum diatur pada aplikasi ini.",
            "Periksa Pembaruan", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        return;
      }

      if (!UpdateService.IsNewer(manifest))
      {
        if (announceWhenCurrent)
        {
          MessageBox.Show(
            string.Format("Aplikasi sudah versi terbaru ({0}).", UpdateService.CurrentVersion),
            "Periksa Pembaruan", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        return;
      }

      Offer(manifest);
    }

    private void Offer(UpdateManifest manifest)
    {
      string notes = string.IsNullOrEmpty(manifest.Notes)
        ? string.Empty
        : Environment.NewLine + Environment.NewLine + manifest.Notes;

      // Without a release archive there is nothing to install, so the operator is pointed at the
      // folder instead of being offered something that cannot happen.
      if (!manifest.CanInstall)
      {
        DialogResult open = MessageBox.Show(
          string.Format("Tersedia versi {0} (saat ini {1}).{2}{3}{3}Buka folder pembaruan sekarang?",
                        manifest.Version, UpdateService.CurrentVersion, notes, Environment.NewLine),
          "Pembaruan Tersedia", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (open == DialogResult.Yes)
          OpenFolder(manifest.DriveUrl);
        return;
      }

      DialogResult install = MessageBox.Show(
        string.Format(
          "Tersedia versi {0} (saat ini {1}).{2}{3}{3}Aplikasi akan ditutup, diperbarui, lalu dibuka " +
          "kembali. Pastikan tidak ada transaksi yang sedang berjalan.{3}{3}Pasang sekarang?",
          manifest.Version, UpdateService.CurrentVersion, notes, Environment.NewLine),
        "Pembaruan Tersedia", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2);
      if (install != DialogResult.Yes)
      {
        _log.InfoFormat("Update to {0} was offered and declined.", manifest.Version);
        return;
      }

      Apply(manifest);
    }

    private void Apply(UpdateManifest manifest)
    {
      Cursor previous = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      string staging;
      string problem;
      try
      {
        staging = _updateService.StageUpdate(manifest, out problem);
      }
      finally
      {
        Cursor.Current = previous;
      }

      if (staging.Length == 0)
      {
        MessageBox.Show(problem, "Pembaruan Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      string runner;
      try
      {
        runner = PrepareRunner(manifest);
      }
      catch (Exception e)
      {
        _log.Error("Could not prepare the updater.", e);
        MessageBox.Show("Pembaruan gagal disiapkan.", "Pembaruan Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      // Recorded before handing over, because after this the process is on its way out and there
      // will be no opportunity to write anything.
      _audit.Record(AuditService.ActionUpdateApplied, AuditService.EntityApplication,
                    manifest.Version.ToString(),
                    string.Format("Pembaruan dari {0} ke {1}.", UpdateService.CurrentVersion, manifest.Version));

      try
      {
        Process.Start(new ProcessStartInfo(runner,
          string.Format("{0} \"{1}\" \"{2}\" {3}", UpdateInstaller.Switch, staging,
                        UpdateService.InstallDirectory, Process.GetCurrentProcess().Id))
        {
          WorkingDirectory = Path.GetDirectoryName(runner),
        });
      }
      catch (Exception e)
      {
        _log.Error("Could not start the updater.", e);
        MessageBox.Show("Pembaruan gagal dijalankan.", "Pembaruan Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      _log.InfoFormat("Handing over to the updater for version {0}; closing.", manifest.Version);
      Application.Exit();
    }

    /// <summary>
    /// Copies the application somewhere else so it can overwrite its own installation from there.
    /// Only the few files the installer path actually touches are copied — it does not open the
    /// database or show a window.
    /// </summary>
    private static string PrepareRunner(UpdateManifest manifest)
    {
      string runnerDirectory = Path.Combine(UpdateService.WorkingRoot, manifest.Version.ToString(), "runner");
      if (Directory.Exists(runnerDirectory))
        Directory.Delete(runnerDirectory, true);
      Directory.CreateDirectory(runnerDirectory);

      foreach (string name in RunnerFiles)
      {
        string source = Path.Combine(UpdateService.InstallDirectory, name);
        if (File.Exists(source))
          File.Copy(source, Path.Combine(runnerDirectory, name), true);
        else
          _log.WarnFormat("Updater file '{0}' was not found beside the application.", name);
      }

      return Path.Combine(runnerDirectory, UpdateService.ApplicationExecutable);
    }

    private static void OpenFolder(string url)
    {
      if (string.IsNullOrEmpty(url))
        return;
      try
      {
        Process.Start(url);
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not open '{0}'.", url), e);
        MessageBox.Show("Folder pembaruan tidak dapat dibuka:" + Environment.NewLine + url,
                        "Pembaruan", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }
  }
}

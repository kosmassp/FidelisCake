using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace InventoryAndSales.Utility
{
  /// <summary>
  /// Replaces the installed files with a staged release, then starts the application again.
  ///
  /// This runs in a **second copy** of the application, started from a temporary folder with
  /// <c>--apply-update</c> before the first copy exits — a running executable cannot overwrite
  /// itself. It touches no database, shows no forms and creates nothing from the business layer; it
  /// is file copying and nothing else.
  ///
  /// The order is chosen so that a failure is survivable, because the failure mode here is a shop
  /// that cannot take money:
  ///
  ///  1. wait for the old process to actually exit;
  ///  2. copy every file that is about to be overwritten into a timestamped backup folder;
  ///  3. copy the new files over the installation;
  ///  4. if anything in step 3 fails, put the backup back.
  ///
  /// Nothing is ever deleted from the installation. A release that drops a file leaves the old one
  /// behind, which is untidy and harmless — far better than removing something the new version turns
  /// out to need.
  /// </summary>
  public static class UpdateInstaller
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UpdateInstaller));

    /// <summary>The argument that puts the application into installer mode.</summary>
    public const string Switch = "--apply-update";

    /// <summary>How long to wait for the old process to close before giving up.</summary>
    private const int ProcessExitTimeoutMs = 60000;

    /// <summary>
    /// True when these are installer-mode arguments, in which case
    /// <see cref="Run"/> owns the whole process.
    /// </summary>
    public static bool IsInstallerMode(string[] args)
    {
      return args != null && args.Length > 0 &&
             string.Equals(args[0], Switch, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Applies the update. Arguments: <c>--apply-update &lt;stagingDir&gt; &lt;installDir&gt; &lt;pid&gt;</c>.
    /// </summary>
    /// <returns>Process exit code: 0 applied, 1 refused or failed.</returns>
    public static int Run(string[] args)
    {
      if (args.Length < 4)
      {
        _log.ErrorFormat("Installer mode needs {0} <staging> <install> <pid>.", Switch);
        return 1;
      }

      string staging = args[1];
      string install = args[2];
      int pid;
      if (!int.TryParse(args[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out pid))
        pid = 0;

      _log.InfoFormat("Installer starting: '{0}' -> '{1}', waiting for process {2}.", staging, install, pid);

      if (!Directory.Exists(staging) || !Directory.Exists(install))
      {
        _log.Error("Installer refused: staging or install folder is missing.");
        return 1;
      }

      if (!WaitForExit(pid))
      {
        // Copying over a running executable fails per file and would leave a half-updated
        // installation, which is the one outcome worth refusing outright.
        _log.Error("Installer refused: the application is still running.");
        Restart(install);
        return 1;
      }

      string backup = Path.Combine(install, "Backup",
                                   DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture));
      List<string> files = RelativeFiles(staging);
      _log.InfoFormat("Applying {0} file(s); backup in '{1}'.", files.Count, backup);

      try
      {
        BackUp(files, install, backup);
      }
      catch (Exception e)
      {
        _log.Error("Could not back up the current installation; update abandoned.", e);
        Restart(install);
        return 1;
      }

      try
      {
        Copy(files, staging, install);
        _log.Info("Update applied.");
      }
      catch (Exception e)
      {
        _log.Error("Copying the update failed; restoring the previous version.", e);
        try
        {
          Restore(backup, install);
          _log.Info("Previous version restored.");
        }
        catch (Exception restoreFailure)
        {
          // Nothing left to try automatically. The backup folder is named in the log and still on
          // disk, so the installation can be put back by hand.
          _log.Fatal(string.Format(
            "Restore failed. The installation may be incomplete; the previous files are in '{0}'.", backup),
            restoreFailure);
        }
        Restart(install);
        return 1;
      }

      Restart(install);
      return 0;
    }

    private static bool WaitForExit(int pid)
    {
      if (pid <= 0)
        return true;

      try
      {
        using (Process process = Process.GetProcessById(pid))
        {
          return process.WaitForExit(ProcessExitTimeoutMs);
        }
      }
      catch (ArgumentException)
      {
        // Already gone, which is what we were waiting for.
        return true;
      }
      catch (Exception e)
      {
        _log.Warn("Could not wait for the application to exit.", e);
        // A short pause still gives file handles time to be released.
        Thread.Sleep(2000);
        return true;
      }
    }

    private static List<string> RelativeFiles(string root)
    {
      List<string> relative = new List<string>();
      foreach (string full in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
        relative.Add(full.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar));
      return relative;
    }

    /// <summary>Copies aside only what is about to be overwritten; a new file has nothing to back up.</summary>
    private static void BackUp(List<string> files, string install, string backup)
    {
      foreach (string relative in files)
      {
        string existing = Path.Combine(install, relative);
        if (!File.Exists(existing))
          continue;

        string target = Path.Combine(backup, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(target));
        File.Copy(existing, target, true);
      }
    }

    private static void Copy(List<string> files, string from, string to)
    {
      foreach (string relative in files)
      {
        string target = Path.Combine(to, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(target));
        File.Copy(Path.Combine(from, relative), target, true);
      }
    }

    private static void Restore(string backup, string install)
    {
      if (!Directory.Exists(backup))
        return;
      foreach (string relative in RelativeFiles(backup))
      {
        string target = Path.Combine(install, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(target));
        File.Copy(Path.Combine(backup, relative), target, true);
      }
    }

    /// <summary>
    /// Starts the till again, whether or not the update worked. A shop left staring at a closed
    /// application is worse off than one still running the old version.
    /// </summary>
    private static void Restart(string install)
    {
      string executable = Path.Combine(install, Business.UpdateService.ApplicationExecutable);
      try
      {
        if (!File.Exists(executable))
        {
          _log.ErrorFormat("Cannot restart: '{0}' does not exist.", executable);
          return;
        }
        Process.Start(new ProcessStartInfo(executable) { WorkingDirectory = install });
        _log.Info("Application restarted.");
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not restart '{0}'.", executable), e);
      }
    }
  }
}

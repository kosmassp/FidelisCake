using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using InventoryAndSales.Database;
using InventoryAndSales.GUI;
using InventoryAndSales.Utility;

namespace InventoryAndSales
{
  static class Program
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main(string[] args)
    {
      // Installer mode first, and before anything else is touched. This copy of the application was
      // started from a temporary folder purely to overwrite the installation the other copy is
      // holding open; it must not open a database, a window or a settings row.
      if (UpdateInstaller.IsInstallerMode(args))
        return UpdateInstaller.Run(args);

      // Culture is pinned deliberately. Amounts are parsed and formatted with '.' as the decimal
      // separator throughout, and dates are compared as parameters against SQL Server. Changing this
      // silently changes how money is read from the payment box.
      CultureInfo culture = new CultureInfo("en-US");
      CultureInfo.DefaultThreadCurrentCulture = culture;
      CultureInfo.DefaultThreadCurrentUICulture = culture;
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;

      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

      // Without this an unexpected error in a button handler shows the raw .NET crash dialog.
      Application.ThreadException += Application_ThreadException;
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      LogEnvironment();

      var splashForm = new SplashForm();
      Application.Run(splashForm);
      if (splashForm.InitializationCheckSuccess)
      {
        _log.Info("Application started");
        try
        {
          Application.Run(new MainForm());
          _log.Info("Application closed normally");
        }
        catch (Exception e)
        {
          _log.Error(e);
        }
      }
      else
      {
        _log.Info("Application failed to start");
        return 1;
      }
      return 0;
    }

    /// <summary>
    /// Stamps every run with what it is and where it is running. A log that does not say which
    /// version and which machine produced it cannot be matched to a report from the shop.
    /// </summary>
    private static void LogEnvironment()
    {
      try
      {
        _log.InfoFormat("=== FidelisCake {0} starting on {1} as {2}, {3}, CLR {4} ===",
                        Assembly.GetExecutingAssembly().GetName().Version,
                        Environment.MachineName, Environment.UserName,
                        Environment.OSVersion, Environment.Version);
        _log.InfoFormat("Working folder: {0}", AppDomain.CurrentDomain.BaseDirectory);
      }
      catch (Exception e)
      {
        _log.Warn("Could not describe the environment.", e);
      }
    }

    /// <summary>
    /// Anything that escapes a UI event handler. Logged in full, reported to the operator in terms
    /// they can act on, and the application is left running - a single failed screen should not cost
    /// them the till.
    /// </summary>
    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      _log.Error("Unhandled UI exception", e.Exception);
      try
      {
        MessageBox.Show(
          "Terjadi kesalahan yang tidak terduga." + Environment.NewLine + Environment.NewLine +
          "Silahkan coba lagi. Jika berulang, hubungi teknisi dan sertakan file Log\\log.txt.",
          "Kesalahan Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception)
      {
        // Nothing useful left to do if even the message box fails.
      }
    }

    private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
    {
      _log.Error(string.Format("*** UNHANDLED APPDOMAIN EXCEPTION ({0}) *****", e.IsTerminating ? "Terminating" : "Non-Terminating"), e.ExceptionObject as Exception);
    }
  }
}

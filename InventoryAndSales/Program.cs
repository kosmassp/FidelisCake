using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using InventoryAndSales.Database;
using InventoryAndSales.GUI;

namespace InventoryAndSales
{
  static class Program
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
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

      var splashForm = new SplashForm();
      Application.Run(splashForm);
      if (splashForm.InitializationCheckSuccess)
      {
        _log.Info("Application started");
        try
        {
          Application.Run(new MainForm());
        }
        catch (Exception e)
        {
          _log.Error(e);
        }
      }
      else
      {
        _log.Info("Application failed to start");
        Environment.Exit(1);
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

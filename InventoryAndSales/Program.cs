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
      CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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
        catch(Exception e)
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

    private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
    {
      _log.Error(string.Format("*** UNHANDLED APPDOMAIN EXCEPTION ({0}) *****", e.IsTerminating ? "Terminating" : "Non-Terminating"), e.ExceptionObject as Exception);
    }

  }
}

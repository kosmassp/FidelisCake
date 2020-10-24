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
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");


      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      var splashForm = new SplashForm();
      Application.Run(splashForm);
      if (splashForm.InitializationCheckSuccess)
      {
        log.Info("Application started");
        Application.Run(new MainForm());
      }
      else
      {
        log.Info("Application failed to start");
        Environment.Exit(1);
      }
          
    }
  }
}

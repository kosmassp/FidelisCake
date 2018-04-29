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
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

      DBUtility.CheckForUpdate();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}

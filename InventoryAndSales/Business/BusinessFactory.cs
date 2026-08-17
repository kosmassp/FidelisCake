using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Composition root of the business layer. Everything below this point receives its dependencies
  /// through its constructor; this and <see cref="DBFactory"/> are the only two lookups.
  /// </summary>
  public class BusinessFactory
  {
    private static readonly object InstanceLock = new object();
    private static BusinessFactory _instance;

    public static BusinessFactory GetInstance()
    {
      if (_instance != null)
        return _instance;
      lock (InstanceLock)
      {
        if (_instance == null)
          _instance = new BusinessFactory();
      }
      return _instance;
    }

    public SettingsService Settings { get; private set; }
    public ReportService ReportService { get; private set; }
    public EdcTerminalService EdcTerminals { get; private set; }
    public CashierManager CashierManager { get; private set; }
    public LoginManager LoginManager { get; private set; }
    public MasterManager MasterManager { get; private set; }
    public ReportManager ReportManager { get; private set; }
    public ViewManager ViewManager { get; private set; }

    private BusinessFactory()
    {
      DBFactory dbFactory = DBFactory.GetInstance();

      Settings = new SettingsService(dbFactory.SettingManager);
      ReportService = new ReportService(Settings);
      EdcTerminals = new EdcTerminalService(Settings);
      CashierManager = new CashierManager(dbFactory.TransactionManager, dbFactory.UserManager, Settings);
      LoginManager = new LoginManager(dbFactory.UserManager, Settings);
      MasterManager = new MasterManager(dbFactory.ProductManager, dbFactory.UserManager);
      ReportManager = new ReportManager(dbFactory.CustomManager);
      ViewManager = new ViewManager(dbFactory.CustomManager);
    }
  }
}

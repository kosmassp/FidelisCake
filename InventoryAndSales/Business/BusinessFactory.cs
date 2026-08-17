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

    /// <summary>Who changed what. Built first, because most of the rest reports into it.</summary>
    public AuditService Audit { get; private set; }

    public SettingsService Settings { get; private set; }
    public ReportService ReportService { get; private set; }
    public PaymentOptionService PaymentOptions { get; private set; }

    /// <summary>What this shop is called, wherever the name has to be shown.</summary>
    public ShopService Shop { get; private set; }

    /// <summary>Whether a newer release exists, and getting it ready to install.</summary>
    public UpdateService UpdateService { get; private set; }

    /// <summary>Baskets set aside mid-sale. In memory, and only for the current session.</summary>
    public HeldCartService HeldCarts { get; private set; }
    public CashierManager CashierManager { get; private set; }
    public LoginManager LoginManager { get; private set; }
    public MasterManager MasterManager { get; private set; }
    public ReportManager ReportManager { get; private set; }
    public ViewManager ViewManager { get; private set; }

    private BusinessFactory()
    {
      DBFactory dbFactory = DBFactory.GetInstance();

      Audit = new AuditService(dbFactory.AuditLogManager);
      Settings = new SettingsService(dbFactory.SettingManager, Audit);
      ReportService = new ReportService(Settings);
      PaymentOptions = new PaymentOptionService(Settings);
      Shop = new ShopService(Settings);
      UpdateService = new UpdateService(Settings);
      HeldCarts = new HeldCartService();
      CashierManager = new CashierManager(dbFactory.TransactionManager, dbFactory.UserManager, Settings, Audit);
      LoginManager = new LoginManager(dbFactory.UserManager, Settings, Audit);
      MasterManager = new MasterManager(dbFactory.ProductManager, dbFactory.UserManager, Audit);
      ReportManager = new ReportManager(dbFactory.CustomManager);
      ViewManager = new ViewManager(dbFactory.CustomManager);

      // Done last, and by subscription rather than a reference, because the login manager already
      // depends on the audit service; this is what lets every later entry know who is at the till.
      Audit.Follow(LoginManager);
    }
  }
}

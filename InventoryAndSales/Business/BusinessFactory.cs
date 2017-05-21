using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database;

namespace InventoryAndSales.Business
{
  public class BusinessFactory
  {
    private static readonly object InstanceLock = new object();
    private static BusinessFactory _instance;

    public static BusinessFactory GetInstance()
    {
      lock (InstanceLock)
      {
        if (_instance == null)
          _instance = new BusinessFactory();
      }
      return _instance;
    }

    public CashierManager CashierManager { get; private set; }
    public LoginManager LoginManager { get; private set; }
    public MasterManager MasterManager { get; private set; }
    public ReportManager ReportManager { get; private set; }
    private BusinessFactory()
    {
      DBFactory dbFactory = DBFactory.GetInstance();
      CashierManager = new CashierManager(dbFactory.TransactionManager);
      LoginManager = new LoginManager(dbFactory.UserManager);
      MasterManager = new MasterManager(dbFactory.ProductManager, dbFactory.UserManager);
      ReportManager = new ReportManager(dbFactory.CustomManager);
    }
  }
}

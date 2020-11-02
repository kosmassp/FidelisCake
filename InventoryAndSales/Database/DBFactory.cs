using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Manager;

namespace InventoryAndSales.Database
{
  public class DBFactory
  {
    private static readonly object InstanceLock = new object();
    private static DBFactory _instance;
    public static DBFactory GetInstance()
    {
      if (_instance != null)
        return _instance;
      lock (InstanceLock)
      {
        if (_instance == null)
          _instance = new DBFactory();
      }
      return _instance;
    }


    private ProductDao ProductDao { get; set; }
    private UserDao UserDao { get; set; }
    private TransactionDao TransactionDao { get; set; }
    private TransactionDetailDao TransactionDetailDao { get; set; }
    private CustomerDao CustomerDao { get; set; }
    private SettingConfigurationDao SettingDao { get; set; }
    private CustomDao CustomDao { get; set; }

    public ProductManager ProductManager { get; private set; }
    public UserManager UserManager { get; private set; }
    public SettingConfigurationManager SettingManager { get; private set; }
    public TransactionDetailManager TransactionDetailManager { get; private set; }
    public TransactionManager TransactionManager { get; private set; }
    public CustomerManager CustomerManager { get; private set; }
    public CustomManager CustomManager { get; private set; }

    private string ConnectionString { get; set; }
    
    private DBFactory()
    {
      string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
      ConnectionString = connectionString;

      SettingDao = new SettingConfigurationDao();
      ProductDao = new ProductDao();
      UserDao = new UserDao();
      TransactionDao = new TransactionDao();
      TransactionDetailDao = new TransactionDetailDao();
      CustomerDao = new CustomerDao();
      CustomDao = new CustomDao();

      SettingManager = new SettingConfigurationManager(SettingDao);
      ProductManager = new ProductManager(ProductDao);
      UserManager = new UserManager(UserDao);
      TransactionDetailManager = new TransactionDetailManager( TransactionDetailDao, ProductManager);
      TransactionManager = new TransactionManager(TransactionDao, TransactionDetailManager);
      CustomerManager = new CustomerManager(CustomerDao);
      CustomManager = new CustomManager(CustomDao);
    }

    private SqlTransaction _activeTransaction;
    private SqlConnection _activeConnection;
    private readonly object _lockTransaction = new object();
    public bool BeginTransaction()
    {
      if (_activeTransaction != null)
        return false;
      //while (_activeTransaction != null ) //TODO What if a transaction want to be inside another transaction.
      //{
      //  //Todo look after this
      //  Thread.Sleep(50);
      //  //wait until transaction become inactive;
      //}
      lock (_lockTransaction)
      {
        _activeConnection = GetConnection(); // in here should be new connection
        _activeConnection.Open();
        _activeTransaction = _activeConnection.BeginTransaction();
        return true;
      }
    }

    public void CommitTransaction()
    {
      if (_activeTransaction == null )
        return;
      lock (_lockTransaction)
      {
        _activeTransaction.Commit();
        _activeTransaction.Dispose();
        _activeTransaction = null;
        _activeConnection.Close();
        _activeConnection.Dispose();
        _activeConnection = null;
      }
    }


    public void RollbackTransaction()
    {
      if (_activeTransaction == null)
        return;
      lock (_lockTransaction)
      {
        _activeTransaction.Rollback();
        _activeTransaction.Dispose();
        _activeTransaction = null;
        _activeConnection.Close();
        _activeConnection.Dispose();
        _activeConnection = null;
      }
    }

    public SqlConnection GetConnection()
    {
      if (_activeConnection == null)
        return new SqlConnection(ConnectionString);
      else
        return _activeConnection;
    }

    public SqlTransaction GetActiveTransaction()
    {
      return _activeTransaction;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Dialect;
using InventoryAndSales.Database.Manager;

namespace InventoryAndSales.Database
{
  /// <summary>
  /// Composition root of the data layer, and the holder of the single ambient transaction.
  ///
  /// Connections and commands are created through a <see cref="DbProviderFactory"/> chosen by
  /// configuration, so nothing below this class names a specific database product. What differs
  /// between products lives in <see cref="ISqlDialect"/>.
  /// </summary>
  public class DBFactory
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

    /// <summary>What this installation's database understands.</summary>
    public ISqlDialect Dialect { get; private set; }

    private DbProviderFactory ProviderFactory { get; set; }
    private string ConnectionString { get; set; }

    private DBFactory()
    {
      Dialect = SqlDialectFactory.Create();
      ProviderFactory = SqlDialectFactory.ResolveProviderFactory(Dialect);
      ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
      _log.InfoFormat("Data layer using {0} via {1}.", Dialect.Name, Dialect.ProviderInvariantName);

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
      TransactionDetailManager = new TransactionDetailManager(TransactionDetailDao, ProductManager);
      TransactionManager = new TransactionManager(TransactionDao, TransactionDetailManager);
      CustomerManager = new CustomerManager(CustomerDao);
      CustomManager = new CustomManager(CustomDao);
    }

    #region Ambient transaction

    private DbTransaction _activeTransaction;
    private DbConnection _activeConnection;
    private readonly object _lockTransaction = new object();

    /// <summary>
    /// Starts a transaction unless one is already running.
    /// </summary>
    /// <returns>
    /// True when this call opened it, meaning the caller owns the commit. False when it joined an
    /// existing one, so an inner save takes part in the outer unit of work.
    /// </returns>
    public bool BeginTransaction()
    {
      if (_activeTransaction != null)
        return false;
      lock (_lockTransaction)
      {
        _activeConnection = CreateConnection();
        _activeConnection.Open();
        _activeTransaction = _activeConnection.BeginTransaction();
        return true;
      }
    }

    public void CommitTransaction()
    {
      if (_activeTransaction == null)
        return;
      lock (_lockTransaction)
      {
        _activeTransaction.Commit();
        DisposeAmbient();
      }
    }

    public void RollbackTransaction()
    {
      if (_activeTransaction == null)
        return;
      lock (_lockTransaction)
      {
        _activeTransaction.Rollback();
        DisposeAmbient();
      }
    }

    private void DisposeAmbient()
    {
      _activeTransaction.Dispose();
      _activeTransaction = null;
      _activeConnection.Close();
      _activeConnection.Dispose();
      _activeConnection = null;
    }

    /// <summary>
    /// The ambient connection when a transaction is running, otherwise a new one the caller must
    /// open and close.
    /// </summary>
    public DbConnection GetConnection()
    {
      if (_activeConnection == null)
        return CreateConnection();
      return _activeConnection;
    }

    public DbTransaction GetActiveTransaction()
    {
      return _activeTransaction;
    }

    #endregion

    private DbConnection CreateConnection()
    {
      DbConnection connection = ProviderFactory.CreateConnection();
      if (connection == null)
        throw new InvalidOperationException(
          string.Format("Provider '{0}' did not return a connection.", Dialect.ProviderInvariantName));
      connection.ConnectionString = ConnectionString;
      return connection;
    }

    /// <summary>
    /// Builds a parameter for the configured provider. Callers use <see cref="DbParam"/> rather than
    /// this directly.
    /// </summary>
    internal DbParameter CreateParameter(string name, object value)
    {
      DbParameter parameter = ProviderFactory.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value ?? DBNull.Value;
      return parameter;
    }

    internal DbParameter CreateParameter(string name, DbType type, int size, object value)
    {
      DbParameter parameter = CreateParameter(name, value);
      parameter.DbType = type;
      if (size > 0)
        parameter.Size = size;
      return parameter;
    }
  }

  /// <summary>
  /// Short-hand for building parameters without naming a provider type.
  ///
  /// All three supported databases accept the @name prefix, so query text needs no adjustment.
  /// </summary>
  public static class DbParam
  {
    public static DbParameter Of(string name, object value)
    {
      return DBFactory.GetInstance().CreateParameter(name, value);
    }

    /// <summary>
    /// A parameter typed to match a non-Unicode text column.
    ///
    /// Worth being explicit about on an indexed column: left to infer, a string parameter becomes
    /// Unicode, and comparing it against a non-Unicode column makes the server convert the column
    /// rather than the value - which gives up the index seek.
    /// </summary>
    public static DbParameter AnsiText(string name, int size, string value)
    {
      return DBFactory.GetInstance().CreateParameter(name, DbType.AnsiString, size, value);
    }
  }
}

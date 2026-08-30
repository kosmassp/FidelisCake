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
    private AuditLogDao AuditLogDao { get; set; }

    public ProductManager ProductManager { get; private set; }
    public UserManager UserManager { get; private set; }
    public SettingConfigurationManager SettingManager { get; private set; }
    public TransactionDetailManager TransactionDetailManager { get; private set; }
    public TransactionManager TransactionManager { get; private set; }
    public CustomerManager CustomerManager { get; private set; }
    public CustomManager CustomManager { get; private set; }
    public AuditLogManager AuditLogManager { get; private set; }

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
      AuditLogDao = new AuditLogDao();

      SettingManager = new SettingConfigurationManager(SettingDao);
      ProductManager = new ProductManager(ProductDao);
      UserManager = new UserManager(UserDao);
      TransactionDetailManager = new TransactionDetailManager(TransactionDetailDao, ProductManager);
      TransactionManager = new TransactionManager(TransactionDao, TransactionDetailManager);
      CustomerManager = new CustomerManager(CustomerDao);
      CustomManager = new CustomManager(CustomDao);
      AuditLogManager = new AuditLogManager(AuditLogDao);
    }

    #region Ambient transaction

    private DbTransaction _activeTransaction;
    private DbConnection _activeConnection;
    private bool _transactionFailed;
    private readonly object _lockTransaction = new object();

    /// <summary>
    /// Starts a transaction unless one is already running.
    ///
    /// The check happens inside the lock. Tested outside it, two threads could both find no
    /// transaction and both open one - the second overwriting the first, which then holds its row
    /// locks with nothing left able to commit or roll it back.
    /// </summary>
    /// <returns>
    /// True when this call opened it, meaning the caller owns the commit. False when it joined an
    /// existing one, so an inner save takes part in the outer unit of work.
    /// </returns>
    public bool BeginTransaction()
    {
      lock (_lockTransaction)
      {
        if (_activeTransaction != null)
          return false;

        DbConnection connection = CreateConnection();
        try
        {
          connection.Open();
          _activeTransaction = connection.BeginTransaction();
          _activeConnection = connection;
        }
        catch (Exception)
        {
          connection.Dispose();
          throw;
        }
        return true;
      }
    }

    /// <summary>
    /// Commits and clears the ambient transaction. A commit failure is rethrown - the caller has to
    /// know the write did not land - but the ambient is cleared either way.
    /// </summary>
    public void CommitTransaction()
    {
      lock (_lockTransaction)
      {
        if (_activeTransaction == null)
          return;

        if (_transactionFailed)
        {
          _log.Error("Not committing: an operation taking part in this transaction failed. Rolling back.");
          RollbackActive();
          throw new InvalidOperationException(
            "The transaction was rolled back because an operation taking part in it failed.");
        }

        try
        {
          _activeTransaction.Commit();
        }
        finally
        {
          // Without the finally a failed commit left the ambient transaction in place for the rest
          // of the session: every later BeginTransaction saw it and returned false, so no caller
          // ever owned a commit again and no sale after the first failure could be saved.
          DisposeAmbient();
        }
      }
    }

    public void RollbackTransaction()
    {
      lock (_lockTransaction)
      {
        if (_activeTransaction == null)
          return;
        RollbackActive();
      }
    }

    /// <summary>Rolls back and clears the ambient transaction. Call with the lock held.</summary>
    private void RollbackActive()
    {
      try
      {
        _activeTransaction.Rollback();
      }
      catch (Exception e)
      {
        // The server may have aborted it already - a deadlock victim or a lock timeout leaves
        // nothing to undo and Rollback throws. This runs from a catch block, so rethrowing would
        // replace the exception being handled and bury the real cause in the log.
        _log.Warn("Rollback failed; the transaction had probably already been aborted by the server.", e);
      }
      finally
      {
        DisposeAmbient();
      }
    }

    /// <summary>
    /// Clears the ambient fields first and releases afterwards, so that however badly the release
    /// goes the next transaction starts from a clean slate.
    /// </summary>
    private void DisposeAmbient()
    {
      DbTransaction transaction = _activeTransaction;
      DbConnection connection = _activeConnection;
      _activeTransaction = null;
      _activeConnection = null;
      _transactionFailed = false;

      try
      {
        if (transaction != null)
          transaction.Dispose();
      }
      catch (Exception e)
      {
        _log.Warn("Disposing the transaction failed.", e);
      }

      try
      {
        if (connection != null)
        {
          connection.Close();
          connection.Dispose();
        }
      }
      catch (Exception e)
      {
        _log.Warn("Closing the transaction's connection failed.", e);
      }
    }

    /// <summary>
    /// Records that an operation taking part in the ambient transaction failed, so that whoever owns
    /// the commit is refused it.
    ///
    /// A caller that joined an existing transaction must not roll back itself - the scope that
    /// opened it may still have work to do and owns that decision. But if that outer scope catches
    /// the failure and carries on, its commit would write a half-finished unit of work. This is how
    /// a joined caller says so; the commit then turns into a rollback and throws.
    /// </summary>
    public void MarkTransactionFailed()
    {
      lock (_lockTransaction)
      {
        if (_activeTransaction != null)
          _transactionFailed = true;
      }
    }

    /// <summary>
    /// The connection and transaction to run a command on, read as one snapshot. See
    /// <see cref="DbScope"/> for why they are never fetched separately. Always wrap the result in a
    /// <c>using</c>.
    /// </summary>
    public DbScope AcquireScope()
    {
      DbConnection ambientConnection;
      DbTransaction ambientTransaction;
      lock (_lockTransaction)
      {
        ambientConnection = _activeConnection;
        ambientTransaction = _activeTransaction;
      }

      if (ambientTransaction != null)
        return DbScope.Joined(ambientConnection, ambientTransaction);

      // Opened outside the lock: it can block on the network, and a read that takes no part in the
      // transaction has no business holding up the one that does.
      DbConnection connection = CreateConnection();
      try
      {
        connection.Open();
      }
      catch (Exception)
      {
        connection.Dispose();
        throw;
      }
      return DbScope.Owned(connection);
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

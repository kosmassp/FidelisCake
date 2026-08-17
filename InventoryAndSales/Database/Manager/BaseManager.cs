using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public abstract class BaseManager<T> where T : BaseObject, new()
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected BaseDao<T> BaseDao;

    public BaseManager(BaseDao<T> baseDao)
    {
      BaseDao = baseDao;
    }

    /// <summary>
    /// What this installation's database understands. Any identifier a manager writes into a where
    /// clause has to go through <c>Dialect.Quote</c> - unquoted names are folded to lower case on
    /// PostgreSQL and would not match the schema.
    /// </summary>
    protected static Dialect.ISqlDialect Dialect
    {
      get { return DBFactory.GetInstance().Dialect; }
    }

    public virtual T FindById(int id)
    {
      return BaseDao.FindById(id);
    }

    public virtual bool Save(T t)
    {
      // BeginTransaction returns false when a transaction is already running, so a caller that has
      // opened its own scope keeps ownership of the commit and this save simply joins it.
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        bool success = BaseDao.Save(t);
        if (newTransaction)
          DBFactory.GetInstance().CommitTransaction();
        return success;
      }
      catch (Exception e)
      {
        _log.Error(e);
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }

    public virtual int Update(T t)
    {
      return BaseDao.Update(t);
    }

    public virtual bool Delete(T t)
    {
      return BaseDao.Delete(t);
    }

    public virtual bool DeleteById(int id)
    {
      return BaseDao.DeleteById(id);
    }

    public virtual List<T> GetAll()
    {
      return BaseDao.FindByQuery(string.Empty);
    }
  }
}

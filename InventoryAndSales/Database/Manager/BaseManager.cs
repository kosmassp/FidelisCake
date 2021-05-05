using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public abstract class BaseManager<T> where T:BaseObject, new()
  //public abstract class BaseManager<T,V> when we start to use Int65 as Id
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //public abstract T FindById(V id);
    protected BaseDao<T> BaseDao;
    public BaseManager(BaseDao<T> baseDao)
    {
      BaseDao = baseDao;
    }

    public virtual T FindById(int id)
    {
      return BaseDao.FindById(id);
    }
    public virtual bool Save(T t)
    {
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      bool success = false;
      try
      {
        success = BaseDao.Save(t);
        if(newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch (Exception e)
      {
        _log.Error(e);
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        success = false;
        throw e;
      }
      return success;
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

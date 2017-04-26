using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public abstract class BaseManager<T> where T:BaseObject
  //public abstract class BaseManager<T,V> when we start to use Int65 as Id
  {
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
    public virtual bool SaveOrUpdate(T t)
    {
      return BaseDao.SaveOrUpdate(t);
    }
    public virtual bool Delete(T t)
    {
      return BaseDao.Delete(t);
    }
    public virtual bool DeleteById(int id)
    {
      return BaseDao.DeleteById(id);
    }

  }
}

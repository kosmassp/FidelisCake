using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class TransactionManager : BaseManager<Transaction>
  {
    private TransactionDetailDao tdDao;
    //private TransactionDao trxDao;
    public TransactionManager(BaseDao<Transaction> baseDao) : base(baseDao)
    {
      //tdDao = ;
    }

    public void SaveCompleteTransaction(Transaction transaction)
    {
      //using (var tran = BeginTransaction())
      {
        BaseDao.SaveOrUpdate(transaction);
        foreach (var tDetail in transaction.TransactionDetails)
        {
          tdDao.SaveOrUpdate(tDetail);
        }
      }
    }
  }
}

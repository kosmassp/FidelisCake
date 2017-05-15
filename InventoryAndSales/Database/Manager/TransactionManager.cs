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
    private TransactionDetailDao _tdDao;
    private TransactionDao _trxDao;
    public TransactionManager(TransactionDao dao, TransactionDetailDao tdDao)
      : base(dao)
    {
      _tdDao = tdDao;
      _trxDao = dao;
    }

    public void SaveCompleteTransaction(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      DBFactory.GetInstance().BeginTransaction();
      try
      {
        _trxDao.Save(transaction);
        foreach (TransactionDetail tDetail in transactionDetails)
        {
          tDetail.TransactionId = transaction.Id;
          _tdDao.Save(tDetail);
        }
        DBFactory.GetInstance().CommitTransaction();
      }
      catch(Exception e)
      {
        DBFactory.GetInstance().RollbackTransaction();
      }
    }
  }
}

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
    public Transaction GetTransaction(string factur, out List<TransactionDetail> transactionDetails)
    {
      Transaction t = _trxDao.FindByFactur(factur);
      if(t!= null)
      {
        transactionDetails = _tdDao.FindByTransactionId(t.Id);
      } 
      else
      {
        transactionDetails = new List<TransactionDetail>();
      }
      return t;
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
        throw;
      }
    }
    public void UpdateCompleteTransaction(
      Transaction originalTransaction, 
      Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      DBFactory.GetInstance().BeginTransaction();
      try
      {
        _trxDao.Save(transaction);
        originalTransaction.Revision = transaction.Id;
        _trxDao.Update(originalTransaction);
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
        throw;
      }
    }
  }
}

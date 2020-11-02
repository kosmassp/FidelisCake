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
    private TransactionDetailManager _tdManager;
    private TransactionDao _trxDao;
    public TransactionManager(TransactionDao dao, TransactionDetailManager tdManager)
      : base(dao)
    {
      _tdManager = tdManager;
      _trxDao = dao;
    }
    public Transaction GetTransaction(string factur, out List<TransactionDetail> transactionDetails)
    {
      Transaction t = _trxDao.FindByFactur(factur);
      if(t!= null)
      {
        transactionDetails = _tdManager.FindByTransactionId(t.Id);
      } 
      else
      {
        transactionDetails = new List<TransactionDetail>();
      }
      return t;
    }

    public void SaveCompleteTransaction(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        _trxDao.Save(transaction);
        foreach (TransactionDetail tDetail in transactionDetails)
        {
          tDetail.TransactionId = transaction.Id;
          _tdManager.Save(tDetail);
        }
        if(newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch(Exception e)
      {
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }
    public void UpdateCompleteTransaction(
      Transaction originalTransaction, 
      Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        _trxDao.Save(transaction);
        originalTransaction.Revision = transaction.Id;
        _trxDao.Update(originalTransaction);
        foreach (TransactionDetail tDetail in transactionDetails)
        {
          tDetail.TransactionId = transaction.Id;
          _tdManager.Save(tDetail);
        }
        if(newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch(Exception e)
      {
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }

    public void CancelTransaction(Transaction originalTransaction)
    {
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        originalTransaction.Revision = -1;
        _trxDao.Update(originalTransaction);
        if(newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch(Exception e)
      {
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }
  }
}

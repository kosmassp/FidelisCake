using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  /// <summary>
  /// Writes sales. A header and its detail rows always land together or not at all.
  ///
  /// Sales are never edited in place. A correction writes a new transaction and points the old one
  /// at it through Revision; a void sets Revision to -1. Reports only ever look at Revision = 0.
  /// </summary>
  public class TransactionManager : BaseManager<Transaction>
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
      if (t != null)
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
        if (newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch (Exception e)
      {
        _log.Error("Rolling Back Transaction", e);
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
        if (newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch (Exception e)
      {
        _log.Error("Rolling Back Transaction", e);
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }

    /// <summary>
    /// Voids a sale. Nothing is deleted - the row simply stops counting.
    /// </summary>
    /// <param name="cancelledByUserId">Who authorised it, recorded for audit.</param>
    public void CancelTransaction(Transaction originalTransaction, int cancelledByUserId)
    {
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        originalTransaction.Revision = -1;
        _trxDao.Update(originalTransaction);
        RecordCancellationAudit(originalTransaction.Id, cancelledByUserId);
        if (newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch (Exception e)
      {
        _log.Error("Rolling Back Transaction", e);
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        throw;
      }
    }

    /// <summary>
    /// Stamps who voided the sale and when.
    ///
    /// Written with a targeted statement rather than through the column map, and tolerated if it
    /// fails: installations that have not picked up the CancelledBy/CancelledAt columns yet must
    /// still be able to void a sale. The void itself is what matters; the audit stamp is extra.
    /// </summary>
    private void RecordCancellationAudit(long transactionId, int cancelledByUserId)
    {
      string sql = string.Format(
        "UPDATE {0} SET {1} = @cancelledBy, {2} = @cancelledAt WHERE {3} = @id",
        Dialect.Quote("T_TRANSACTIONS"), Dialect.Quote("CancelledBy"),
        Dialect.Quote("CancelledAt"), Dialect.Quote("Id"));

      int affected = DBUtility.TryExecuteNonQuery(sql,
        DbParam.Of("@cancelledBy", cancelledByUserId),
        DbParam.Of("@cancelledAt", DateTime.Now),
        DbParam.Of("@id", transactionId));

      if (affected < 0)
        _log.WarnFormat(
          "Could not record who cancelled transaction {0}. The cancellation itself succeeded; " +
          "the CancelledBy/CancelledAt columns are probably missing on this database.",
          transactionId);
    }
  }
}

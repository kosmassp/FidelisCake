using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class TransactionDetailManager : BaseManager<TransactionDetail>
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly TransactionDetailDao _transactionDetailDao;
    private readonly ProductManager _productManager;

    public TransactionDetailManager(TransactionDetailDao dao, ProductManager productManager)
      : base(dao)
    {
      _transactionDetailDao = dao;
      _productManager = productManager;
    }

    /// <summary>
    /// Loads the lines of a sale and fills in the product name for display. ProductName is not a
    /// stored column - the line keeps the price it was sold at, but the name is looked up.
    /// </summary>
    internal List<TransactionDetail> FindByTransactionId(long id)
    {
      List<TransactionDetail> transactionDetails = _transactionDetailDao.FindByTransactionId(id);
      foreach (TransactionDetail td in transactionDetails)
      {
        Product product = _productManager.FindById(td.ProductId);
        if (product != null)
        {
          td.ProductName = product.Name;
        }
        else
        {
          // Products are only ever soft deleted, so this means the row was removed outside the
          // application. Reprinting an old receipt must still work.
          _log.WarnFormat("Product {0} referenced by transaction {1} no longer exists.", td.ProductId, id);
          td.ProductName = "Telah Dihapus";
        }
      }
      return transactionDetails;
    }
  }
}

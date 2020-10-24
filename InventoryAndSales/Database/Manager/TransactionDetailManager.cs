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
    TransactionDetailDao _transactionDetailDao;
    ProductManager _productManager;
    public TransactionDetailManager(TransactionDetailDao dao, ProductManager productManager)
      : base(dao)
    {
      _transactionDetailDao = dao;
      _productManager = productManager;
    }

    internal List<TransactionDetail> FindByTransactionId(long id)
    {
      var transactionDetails = _transactionDetailDao.FindByTransactionId(id);
      foreach(var td in transactionDetails)
      {
        var product = _productManager.FindById(td.ProductId);
        td.ProductName = product.Name;
      }
      return transactionDetails;

    }
  }
}

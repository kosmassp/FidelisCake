using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;

namespace InventoryAndSales.GUI.Controller
{
  public class MasterProductController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private MasterProductPage control;
    private MasterManager _masterManager;
    public MasterProductController(MasterProductPage masterProductPage)
    {
      control = masterProductPage;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
    }

    public IList<string> GetSortableColumns()
    {
      return DataTableList.Instance.GetDataTable(typeof (Product)).Columns;
    }

    public void AddItem(string code, string barcode, string name, decimal price, decimal discount)
    {
      _masterManager.AddProduct(new Product(code, barcode, name, price, discount, false));
    }

    public void UpdateItem(Product currentProductSelection, string code, string barcode, string name, decimal price, decimal discount)
    {
      currentProductSelection.Code = code;
      currentProductSelection.Barcode = barcode;
      currentProductSelection.Name = name;
      currentProductSelection.Price = price;
      currentProductSelection.Discount = discount;
      _masterManager.UpdateProduct(currentProductSelection);
    }

    public void RemoveItem(Product currentProductSelection)
    {
      _masterManager.DeleteProduct(currentProductSelection);
    }

    public List<Product> GetItems(string nameLike, string orderBy)
    {
      return _masterManager.GetAllAvailable(nameLike, orderBy);
    }
 
    /// <summary>
    /// Applies an imported product list: rows without an Id are inserted, the rest update the
    /// product with that Id.
    ///
    /// Wrapped in a single database transaction, so a failure part way through leaves the catalogue
    /// exactly as it was rather than half updated.
    /// </summary>
    /// <returns>How many rows were inserted and updated.</returns>
    public ImportResult SetItemForImport(List<Product> products)
    {
      ImportResult result = new ImportResult();
      bool newTransaction = DBFactory.GetInstance().BeginTransaction();
      try
      {
        foreach (Product product in products)
        {
          if (product.Id == 0)
          {
            _masterManager.AddProduct(product);
            result.Added++;
          }
          else
          {
            _masterManager.UpdateProduct(product);
            result.Updated++;
          }
        }
        if (newTransaction)
          DBFactory.GetInstance().CommitTransaction();
      }
      catch (Exception e)
      {
        _log.Error("Product import failed; rolling back.", e);
        if (newTransaction)
          DBFactory.GetInstance().RollbackTransaction();
        else
          DBFactory.GetInstance().MarkTransactionFailed();
        throw;
      }
      return result;
    }

    public class ImportResult
    {
      public int Added { get; set; }
      public int Updated { get; set; }
    }


  }
}

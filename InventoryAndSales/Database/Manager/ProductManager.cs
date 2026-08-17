using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class ProductManager : BaseManager<Product>
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public ProductManager(ProductDao dao)
      : base(dao)
    {
    }

    public List<Product> GetAllAvailable(string criteria)
    {
      return GetAllAvailable(criteria, null);
    }

    /// <summary>
    /// Products that have not been soft deleted, optionally filtered by name.
    /// </summary>
    /// <param name="criteria">
    /// Free text from the search box. Spaces become wildcards so "kue coklat" still finds
    /// "kue besar coklat".
    /// </param>
    /// <param name="orderBy">
    /// Column to sort by. A column name cannot be passed as a parameter, so it is checked against
    /// the mapped column list and ignored if it is not one of them.
    /// </param>
    public List<Product> GetAllAvailable(string criteria, string orderBy)
    {
      string pattern = "%" + (criteria ?? string.Empty).Replace(' ', '%') + "%";
      return BaseDao.FindByQuery(
        "WHERE Name LIKE @criteria AND Deleted = @deleted",
        SanitizeOrderBy(orderBy),
        new SqlParameter("@criteria", SqlDbType.VarChar, 200) { Value = pattern },
        new SqlParameter("@deleted", false));
    }

    private static string SanitizeOrderBy(string orderBy)
    {
      if (string.IsNullOrEmpty(orderBy))
        return string.Empty;

      string requested = orderBy.Trim();
      IDataTable table = DataTableList.Instance.GetDataTable(typeof(Product));
      foreach (string column in table.Columns)
      {
        if (string.Equals(column, requested, StringComparison.OrdinalIgnoreCase))
          return "[" + column + "]";
      }

      _log.WarnFormat("Ignoring unrecognised sort column '{0}'.", orderBy);
      return string.Empty;
    }
  }
}

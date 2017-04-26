using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  public class CustomerDao : BaseDao<Customer>
  {
    public CustomerDao(IDataTable dataTable) : base(dataTable)
    {
    }
  }
}

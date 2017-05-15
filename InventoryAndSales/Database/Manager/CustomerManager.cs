using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class CustomerManager : BaseManager<Customer>
  {
    public CustomerManager(CustomerDao dao)
      : base(dao)
    {
    }
  }
}

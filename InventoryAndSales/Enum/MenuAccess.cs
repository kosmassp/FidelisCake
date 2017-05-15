using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Business.Enum
{
  [Flags]
  enum MenuAccess
  {
    Admin = 1,
    Cashier = 2,
    Master = 4
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Enumeration;

namespace InventoryAndSales.GUI.Util
{
  class BusinessUtil
  {
    public static bool AllowedRole(int role, AccessOption accessOption)
    {
      return (((AccessOption)role & accessOption) == accessOption);
    }
  }
}

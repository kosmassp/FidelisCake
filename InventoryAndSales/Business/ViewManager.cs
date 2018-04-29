using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  public class ViewManager
  {
    private readonly CustomManager _customManager;
    public ViewManager(CustomManager customManager)
    {
      _customManager = customManager;
    }

    public List<Dictionary<string, string>> GetTransaction(DateTime start, DateTime stop)
    {
      return _customManager.GetTransaction(start, stop);
    }
  }
}

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
  public class ReportManager
  {
    private readonly CustomManager _customManager;
    public ReportManager(CustomManager customManager)
    {
      _customManager = customManager;
    }

    public List<Dictionary<string, string>> GetSummaryReport(DateTime start, DateTime stop)
    {
      return _customManager.GetSummaryReport(start, stop);
    }

    public List<Dictionary<string, string>> GetDetailReport(DateTime start, DateTime stop)
    {
      return _customManager.GetDetailReport(start, stop);
    }
  }
}

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

    public List<Dictionary<string, string>> GetSummaryReportProduct(DateTime start, DateTime stop)
    {
      return _customManager.GetSummaryReportByProduct(start, stop);
    }

    public List<Dictionary<string, string>> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      return _customManager.GetReportSummaryByTransaction(start, stop);
    }

    public List<Dictionary<string, string>> GetDetailReport(DateTime start, DateTime stop)
    {
      return _customManager.GetDetailReport(start, stop);
    }

    public List<Dictionary<string, string>> GetReportSummaryByCashier(DateTime start, DateTime stop)
    {
      return _customManager.GetReportSummaryByCashier(start, stop);
    }

    public string GetTodaySummaryByCashier(User activeUser, DateTime date)
    {
      return _customManager.GetTodaySummaryByCashier(activeUser, date);
    }
  }
}

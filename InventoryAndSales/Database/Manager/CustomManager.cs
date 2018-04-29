using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class CustomManager : BaseManager<CustomQuery>
  {
    private readonly CustomDao _customDao;
    public CustomManager(CustomDao dao)
      : base(dao)
    {
      _customDao = dao;
    }

    public List<Dictionary<string, string>> GetSummaryReportByProduct(DateTime start, DateTime stop)
    {
      List<CustomQuery> returnList = _customDao.GetReportSummaryByProduct(start, stop);
      return ConvertToList(returnList);
    }

    public List<Dictionary<string, string>> GetReportSummaryByTransaction(DateTime start, DateTime stop)
    {
      List<CustomQuery> returnList = _customDao.GetReportSummaryByTransaction(start, stop);
      return ConvertToList(returnList);
    }

    public List<Dictionary<string, string>> GetTransaction(DateTime start, DateTime stop)
    {
      List<CustomQuery> returnList = _customDao.GetTransaction(start, stop);
      return ConvertToList(returnList);
    }

    public List<Dictionary<string, string>> GetDetailReport(DateTime start, DateTime stop)
    {
      List<CustomQuery> returnList = _customDao.GetReportDetailByTime(start, stop);
      return ConvertToList(returnList);
    }

    public List<Dictionary<string, string>> GetReportSummaryByCashier(DateTime start, DateTime stop)
    {
      List<CustomQuery> returnList = _customDao.GetReportSummaryByUserId(start, stop);
      return ConvertToList(returnList);
    }

    private static List<Dictionary<string, string>> ConvertToList(List<CustomQuery> returnList)
    {
      List<Dictionary<string, string>> returnListDict = new List<Dictionary<string, string>>(returnList.Count);
      foreach (CustomQuery customQuery in returnList)
      {
        returnListDict.Add(customQuery.GetDict());
      }
      return returnListDict;
    }
  }
}

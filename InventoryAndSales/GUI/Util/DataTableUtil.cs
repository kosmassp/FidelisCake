using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace InventoryAndSales.GUI
{
  public class DataTableUtil
  {
    public static DataTable GetDataTable(List<Dictionary<string, string>> summaryReport, string tableName)
    {
      DataTable dataTable = new DataTable();
      if (summaryReport.Count > 0)
      {
        foreach (string columnName in summaryReport[0].Keys)
        {
          dataTable.Columns.Add(columnName);
        }
        foreach (Dictionary<string, string> dictionary in summaryReport)
        {
          dataTable.Rows.Add(dictionary.Values.ToArray());
        }
      }
      return dataTable;
    }
  }
}

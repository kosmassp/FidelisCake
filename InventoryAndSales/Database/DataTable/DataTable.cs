using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database.DataTable
{
  public class DataTable : IDataTable
  {
    public string TableName { get; private set; }
    public string PrimaryKeyColumn { get; private set; }
    public List<string> Columns { get; private set; }

    public DataTable(string tableName, string primaryKey, params string[] columns)
    {
      TableName = tableName;
      PrimaryKeyColumn = primaryKey;
      Columns = new List<string>();
      Columns.Add(primaryKey);
      foreach(string col in columns)
      {
        Columns.Add(col);
      }
    }
  }
}

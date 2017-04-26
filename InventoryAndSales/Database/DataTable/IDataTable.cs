using System.Collections.Generic;

namespace InventoryAndSales.Database.DataTable
{
  public interface IDataTable
  {
    string TableName { get; }
    string PrimaryKeyColumn { get; }
    List<string> Columns { get; }
  }
}
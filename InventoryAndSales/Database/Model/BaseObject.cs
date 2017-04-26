using System.Collections.Generic;

namespace InventoryAndSales.Database.Model
{
  public abstract class BaseObject
  {
    /// <summary>
    /// Return dictionary of table column and it's values
    /// </summary>
    /// <returns></returns>
    public abstract Dictionary<string, object> Data { get; }
  }
}
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InventoryAndSales.Database.Model
{
  public abstract class BaseObject
  {
    /// <summary>
    /// Return dictionary of table column and it's values
    /// </summary>
    /// <returns></returns>
    [IndexerName("DataColumn")]
    public abstract object this[string columnName] { get; set; }
  }
}
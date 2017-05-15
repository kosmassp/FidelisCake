using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database.Model
{
  public class CustomQuery : BaseObject
  {
    private readonly Dictionary<string, string> _columnValues;
    public CustomQuery()
    {
      _columnValues = new Dictionary<string, string>();
    }
    public override object this[string columnName]
    {
      get { return _columnValues[columnName]; }
      set { _columnValues[columnName] = value.ToString(); }
    }

    public Dictionary<string, string> GetDict()
    {
      return _columnValues;
    }
  }
}

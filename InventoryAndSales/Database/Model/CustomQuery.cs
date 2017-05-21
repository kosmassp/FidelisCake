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
      set
      {
        if (value is DateTime)
        {
          DateTime dt = (DateTime) value;
          if (dt.TimeOfDay == TimeSpan.Zero)
            _columnValues[columnName] = dt.ToString("dd MMM yyyy");
          else
            _columnValues[columnName] = dt.ToString("dd MMM yyyy HH:mm:ss");
        }
        else
          _columnValues[columnName] = value.ToString();
      }
    }

    public Dictionary<string, string> GetDict()
    {
      return _columnValues;
    }
  }
}

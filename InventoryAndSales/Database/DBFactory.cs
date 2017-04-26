using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAndSales.Database
{
  public class DBFactory
  {
    private static DBFactory _instance;
    public static DBFactory Instance
    {
      get
      {
        lock (_instance)
        {
          if (_instance == null)
            _instance = new DBFactory();
        }
        return _instance;
      }
    }

    public DBFactory()
    {
      
    }


  }
}

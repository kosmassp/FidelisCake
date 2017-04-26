using System;
using System.Collections.Generic;

namespace InventoryAndSales.Database.Model
{
  public class User : BaseObject
  {
    public int Id { get; set; }
    public int Username { get; set; }
    public int Password { get; set; }
    public int Role { get; set; }

    public override Dictionary<string, object> Data
    {
      get { throw new NotImplementedException(); }
    }
  }
}
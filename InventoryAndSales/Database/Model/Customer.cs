using System;
using System.Collections.Generic;

namespace InventoryAndSales.Database.Model
{
  public class Customer : BaseObject
  {
    public int Id { get; set; }
    public int Name { get; set; }
    public int MemberType { get; set; }

    public override Dictionary<string, object> Data
    {
      get { throw new NotImplementedException(); }
    }
  }
}
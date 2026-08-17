using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InventoryAndSales.Database.Model
{
  public class Customer : BaseObject
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int MemberType { get; set; }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id": return Id;
          case "Name": return Name;
          case "Address": return Address;
          case "Phone": return Phone;
          case "MemberType": return MemberType;
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
      }

      set
      {
        switch (columnName)
        {
          case "Id":
            Id = ToInt(value);
            break;
          case "Name":
            Name = ToText(value);
            break;
          case "Address":
            Address = ToText(value);
            break;
          case "Phone":
            Phone = ToText(value);
            break;
          case "MemberType":
            MemberType = ToInt(value);
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
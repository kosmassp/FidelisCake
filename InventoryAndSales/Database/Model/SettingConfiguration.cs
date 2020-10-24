using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InventoryAndSales.Database.Model
{
  //Name setting is conflicted with extension.
  public class SettingConfiguration : BaseObject
  {
    public int Id { get; set; }
    public string Key { get; set; }
    public string Group { get; set; }
    public string Value { get; set; }
    public string Default { get; set; }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id": return Id;
          case "Key": return Key;
          case "Group": return Group;
          case "Value": return Value;
          case "Default": return Default;
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
      }

      set
      {
        switch (columnName)
        {
          case "Id":
            Id = (int)value;
            break;
          case "Key":
            Key = (string)value;
            break;
          case "Group":
            Group = (string)value;
            break;
          case "Value":
            Value = (string)value;
            break;
          case "Default":
            Default = (string)value;
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
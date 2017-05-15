using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InventoryAndSales.Database.Model
{
  public class User : BaseObject
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }

    public User() : base()
    {
    }

    public User(int id, string username, string password, int role) 
      : this()
    {
      Id = id;
      Username = username;
      Password = password;
      Role = role;
    }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id": return Id;
          case "Username": return Username;
          case "Password": return Password;
          case "Role": return Role;
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
          case "Username":
            Username = (string)value;
            break;
          case "Password":
            Password = (string)value;
            break;
          case "Role":
            Role = (int)value;
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
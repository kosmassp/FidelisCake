using System;
using System.Collections.Generic;
using System.ComponentModel;
using InventoryAndSales.Enumeration;

namespace InventoryAndSales.Database.Model
{
  public class User : BaseObject
  {
    [Browsable(false)]
    public int Id { get; set; }
    
    public string Username { get; set; }

    [Browsable(false)]
    public string Password { get; set; }
    public string Name { get; set; }

    [Browsable(false)]
    public int Role { get; set; }

    public RoleOptions RoleOption
    {
      get { return (RoleOptions)Role; }
    }

    [Browsable(false)]
    public bool Deleted { get; set; }

    public User() : base()
    {
    }

    public User(string username, string password, string name, int role, bool deleted) 
      : this()
    {
      Username = username;
      Password = password;
      Name = name;
      Role = role;
      Deleted = deleted;
    }

    public User(int id, string username, string password, string name, int role, bool deleted)
      : this(username, password, name, role, deleted)
    {
      Id = id;
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
          case "Name": return Name;
          case "Role": return Role;
          case "Deleted": return Deleted;
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
          case "Username":
            Username = ToText(value);
            break;
          case "Password":
            Password = ToText(value);
            break;
          case "Name":
            Name = ToText(value);
            break;
          case "Role":
            Role = ToInt(value);
            break;
          case "Deleted":
            Deleted = ToBool(value);
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}
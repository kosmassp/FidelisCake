using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InventoryAndSales.Database.Model
{
  /// <summary>
  /// One recorded change: who did it, when, to what, and what it looked like afterwards.
  ///
  /// The actor is kept as both an id and a name because the name is the part that still answers the
  /// question years later, after the user row has been renamed or soft-deleted.
  /// </summary>
  public class AuditLog : BaseObject
  {
    public long Id { get; set; }
    public DateTime AuditTime { get; set; }

    /// <summary>The signed-in user, 0 when the application itself acted.</summary>
    public int UserId { get; set; }

    public string UserName { get; set; }

    /// <summary>What was done, from the vocabulary on <see cref="InventoryAndSales.Business.AuditService"/>.</summary>
    public string Action { get; set; }

    /// <summary>What kind of thing was touched — a product, a user, a setting, a sale.</summary>
    public string EntityType { get; set; }

    /// <summary>Which one: a product code, a username, a setting key, a faktur number.</summary>
    public string EntityKey { get; set; }

    /// <summary>Which machine it happened on; shops run more than one till.</summary>
    public string Workstation { get; set; }

    /// <summary>Human-readable detail, typically "was → now" for the fields that changed.</summary>
    public string Detail { get; set; }

    [Browsable(false)]
    public override object this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "Id":
            return Id;
          case "AuditTime":
            return AuditTime;
          case "UserId":
            return UserId;
          case "UserName":
            return UserName;
          case "Action":
            return Action;
          case "EntityType":
            return EntityType;
          case "EntityKey":
            return EntityKey;
          case "Workstation":
            return Workstation;
          case "Detail":
            return Detail;
        }
        throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
      }

      set
      {
        switch (columnName)
        {
          case "Id":
            Id = ToLong(value);
            break;
          case "AuditTime":
            AuditTime = ToDateTime(value);
            break;
          case "UserId":
            UserId = ToInt(value);
            break;
          case "UserName":
            UserName = ToText(value);
            break;
          case "Action":
            Action = ToText(value);
            break;
          case "EntityType":
            EntityType = ToText(value);
            break;
          case "EntityKey":
            EntityKey = ToText(value);
            break;
          case "Workstation":
            Workstation = ToText(value);
            break;
          case "Detail":
            Detail = ToText(value);
            break;
          default:
            throw new KeyNotFoundException(string.Format("Column name {0} not registered on class", columnName));
        }
      }
    }
  }
}

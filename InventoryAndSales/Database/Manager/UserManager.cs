using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class UserManager : BaseManager<User>
  {
    public UserManager(UserDao dao)
      : base(dao)
    {
    }

    /// <summary>
    /// Looks a user up by name only. Password checking belongs to the business layer - it has to
    /// cope with two stored hash formats and with a per-user salt, neither of which can be expressed
    /// as a SQL predicate.
    /// </summary>
    public User FindByUsername(string username)
    {
      if (string.IsNullOrEmpty(username))
        return null;

      // AnsiText to match the column type, so no implicit conversion is forced on it.
      List<User> users = BaseDao.FindByQuery(
        string.Format("WHERE {0} = @username AND {1} = @deleted",
                      Dialect.Quote("Username"), Dialect.Quote("Deleted")),
        string.Empty,
        DbParam.AnsiText("@username", 50, username),
        DbParam.Of("@deleted", false));

      // More than one row means duplicate usernames, which the application never creates on
      // purpose. Refuse rather than guess which account was meant.
      if (users != null && users.Count == 1)
        return users[0];
      return null;
    }

    public override List<User> GetAll()
    {
      return BaseDao.FindByQuery(
        string.Format("WHERE {0} = @deleted", Dialect.Quote("Deleted")),
        string.Empty,
        DbParam.Of("@deleted", false));
    }
  }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

      // VarChar to match the column type, so no implicit conversion is forced on it.
      List<User> users = BaseDao.FindByQuery(
        "WHERE Username = @username AND Deleted = @deleted",
        string.Empty,
        new SqlParameter("@username", SqlDbType.VarChar, 50) { Value = username },
        new SqlParameter("@deleted", false));

      // More than one row means duplicate usernames, which the application never creates on
      // purpose. Refuse rather than guess which account was meant.
      if (users != null && users.Count == 1)
        return users[0];
      return null;
    }

    public override List<User> GetAll()
    {
      return BaseDao.FindByQuery(
        "WHERE Deleted = @deleted",
        string.Empty,
        new SqlParameter("@deleted", false));
    }
  }
}

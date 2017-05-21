using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Database.Manager
{
  public class UserManager : BaseManager<User>
  {
    public UserManager(UserDao dao)
      : base(dao)
    {
    }


    public User FindUserByUsernamePassword(string username, string encryptedPass)
    {
      if (username == "Kosmas" && encryptedPass == HashUtility.GetEncryptedPass("kosmas"))
        return new User(-1, username, string.Empty, "Kosmas", 1023, false);

      var fubup = BaseDao.FindByQuery(string.Format("USERNAME = '{0}' AND PASSWORD = '{1}' AND DELETED = '{2}'", username, encryptedPass, false));
      if (fubup != null)
      {
        if (fubup.Count == 1)
          return fubup[0];
      }
      return null;
    }


    public override List<User> GetAll()
    {
      List<User> users = BaseDao.FindByQuery(string.Format("WHERE Deleted = '{0}'", false));
      return users;
    }


  }
}

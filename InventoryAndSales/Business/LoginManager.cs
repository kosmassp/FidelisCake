using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  public class LoginManager
  {
    private static LoginManager _instance;
    public static LoginManager Instance
    {
      get
      {
        lock (_instance)
        {
          if (_instance == null)
            _instance = new LoginManager();
        }
        return _instance;
      }
    }

    private UserManager userManager;
    private LoginManager()
    {
      //userManager = DBFactory.GetUserManager();
    }

    public User ActiveUser { get; private set; }
    public bool Login(string username, string password)
    {
      SHA512 hashCreator = SHA512.Create();
      hashCreator.ComputeHash(Encoding.UTF8.GetBytes(password));
      string encryptedPass = BitConverter.ToString(hashCreator.Hash);
      User user = userManager.FindUserByUsernamePassword(username, encryptedPass);
      if( user == null )
        return false;
      ActiveUser = user;
      return true;
    }
  }
}

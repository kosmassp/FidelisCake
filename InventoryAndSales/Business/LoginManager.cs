using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  public class LoginManager
  {
    private readonly UserManager _userManager;
    public LoginManager(UserManager userManager)
    {
      _userManager = userManager;
    }

    public User ActiveUser { get; private set; }
    public bool Login(string username, string password)
    {
      string encryptedPass = HashUtility.GetEncryptedPass(password);
      User user = _userManager.FindUserByUsernamePassword(username, encryptedPass);
      if( user == null )
        return false;
      ActiveUser = user;
      return true;
    }

    public void Logout()
    {
      ActiveUser = null;
    }
  }
}

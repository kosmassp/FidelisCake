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
      User user = AuthenticateUsernamePassword(password, username);
      ActiveUser = user;
      if (OnActiveUserChanged != null)
        OnActiveUserChanged(this, user);
      return ActiveUser != null;
    }

    public User AuthenticateUsernamePassword(string password, string username)
    {
      string encryptedPass = HashUtility.GetEncryptedPass(password);
      return _userManager.FindUserByUsernamePassword(username, encryptedPass);
    }

    public delegate void OnActiveUserDelegate (object sender, User args);
    public event OnActiveUserDelegate OnActiveUserChanged;

    public void Logout()
    {
      ActiveUser = null;
    }

} }

 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;

namespace InventoryAndSales.GUI.Controller
{
  public class LoginController
  {
    private LoginPage _control;
    private LoginManager _loginManager;
    public LoginController(LoginPage loginPage)
    {
      _control = loginPage;
      _loginManager = BusinessFactory.GetInstance().LoginManager;
    }

    public bool Login(string username, string password)
    {
      //TestPrint();
      bool loginSuccess = _loginManager.Login(username, password);
      //if (loginSuccess)
      //{
      //  User activeUser = _loginManager.ActiveUser;

      //  _control.EnableMenu(activeUser.Role);
      //  _control.UpdateActiveUser(activeUser.Name);
      //}
      return loginSuccess;
    }
  }
}

using System;
using System.Collections.Generic;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Controller
{
  public class MasterUserController
  {
    private MasterUserPage _control;
    private MasterManager _masterManager;
    public MasterUserController(MasterUserPage masterUserPage)
    {
      _control = masterUserPage;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
    }

    public List<User> GetUsers()
    {
      return _masterManager.GetUsers();
    }

    public void DeleteUser(User currentUserSelection)
    {
      _masterManager.DeleteUser(currentUserSelection);
    }

    public void UpdateUser(User currentUserSelection, string username, string name, string password, int role)
    {
      if (string.IsNullOrEmpty(currentUserSelection.Password) || !currentUserSelection.Password.StartsWith(password))
      {
        currentUserSelection.Password = HashUtility.GetEncryptedPass(password);
      }
      currentUserSelection.Name = name;
      currentUserSelection.Role = role;
      _masterManager.UpdateUser(currentUserSelection);
    }

    public void AddUser(string username, string name, string password, int role)
    {
      _masterManager.AddUser(new User(username, HashUtility.GetEncryptedPass(password), name, role, false));
    }
  }
}
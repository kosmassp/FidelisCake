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
    private readonly MasterUserPage _control;
    private readonly MasterManager _masterManager;

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

    /// <summary>
    /// Saves an edited user.
    /// </summary>
    /// <param name="passwordChanged">
    /// Whether the operator actually typed a new password. The screen used to show the first eight
    /// characters of the stored hash and the password was re-hashed only if the typed text no longer
    /// matched that prefix, which coupled this decision to the storage format. The screen now simply
    /// reports whether the field was touched.
    /// </param>
    public void UpdateUser(User currentUserSelection, string username, string name, string password, int role, bool passwordChanged)
    {
      if (passwordChanged)
        currentUserSelection.Password = PasswordHasher.Hash(password);

      currentUserSelection.Name = name;
      currentUserSelection.Role = role;
      _masterManager.UpdateUser(currentUserSelection);
    }

    public void AddUser(string username, string name, string password, int role)
    {
      _masterManager.AddUser(new User(username, PasswordHasher.Hash(password), name, role, false));
    }

    /// <summary>
    /// Whether a username is already taken. Checked here rather than by a database constraint,
    /// because deployed databases have no unique index on it.
    /// </summary>
    public bool IsUsernameTaken(string username, User excluding)
    {
      foreach (User user in GetUsers())
      {
        if (excluding != null && user.Id == excluding.Id)
          continue;
        if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }
  }
}

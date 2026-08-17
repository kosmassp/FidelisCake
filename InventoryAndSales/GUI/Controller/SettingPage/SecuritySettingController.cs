using System;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Popup.SettingPage;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the security settings page - currently just whether the built-in recovery account is
  /// accepted on this installation.
  /// </summary>
  internal class SecuritySettingController
  {
    private readonly SecuritySettingForm _view;
    private readonly LoginManager _loginManager;

    public SecuritySettingController(SecuritySettingForm view)
    {
      _view = view;
      _loginManager = BusinessFactory.GetInstance().LoginManager;
    }

    public bool IsBuiltInAdminAllowed()
    {
      return _loginManager.IsBuiltInAdminAllowed();
    }

    public string BuiltInAdminUsername
    {
      get { return LoginManager.BuiltInAdminDisplayName; }
    }

    /// <summary>
    /// True when an ordinary account can still administer the system.
    ///
    /// The recovery account exists because administrators have deleted their own account and locked
    /// the shop out of its own till. Turning it off while no other administrator remains would do
    /// exactly that, so the page refuses.
    /// </summary>
    public bool HasRealAdministrator()
    {
      return _loginManager.HasRealAdministrator();
    }

    /// <summary>Empty on success, otherwise a message for the operator.</summary>
    public string Save(bool allowBuiltInAdmin)
    {
      if (!allowBuiltInAdmin && !HasRealAdministrator())
      {
        return "Tidak ada user lain yang dapat mengelola sistem. " +
               "Buat user dengan hak Supervisor atau Admin terlebih dahulu.";
      }

      _loginManager.SetBuiltInAdminAllowed(allowBuiltInAdmin);
      return string.Empty;
    }
  }
}

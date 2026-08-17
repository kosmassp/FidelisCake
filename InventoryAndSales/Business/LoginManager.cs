using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using InventoryAndSales.Enumeration;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Owns authentication: which credentials are accepted, whether the built-in recovery account is
  /// allowed, and keeping stored password hashes up to date.
  /// </summary>
  public class LoginManager
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// Credentials of the built-in recovery account. It exists because an administrator can delete
    /// their own account and lock the shop out of its own till; it is deliberately not stored in
    /// M_USERS so that it survives exactly that. It can be switched off per installation - see
    /// <see cref="SettingKeys.AllowBuiltInAdmin"/> - and is enabled by default so upgrading an
    /// existing site changes nothing.
    /// </summary>
    private const string BuiltInAdminUsername = "Kosmas";
    private const string BuiltInAdminPassword = "kosmas";
    private const int BuiltInAdminId = -1;

    private readonly UserManager _userManager;
    private readonly SettingsService _settings;
    private readonly AuditService _audit;

    public LoginManager(UserManager userManager, SettingsService settings, AuditService audit)
    {
      _userManager = userManager;
      _settings = settings;
      _audit = audit;
    }

    public User ActiveUser { get; private set; }

    public bool Login(string username, string password)
    {
      User user = AuthenticateUsernamePassword(password, username);
      ActiveUser = user;

      // Recorded before the event, and against the user explicitly, so the entry stands whether or
      // not anything is listening and whoever just signed in owns it.
      if (user == null)
        _audit.RecordLoginFailed(username);
      else
        _audit.RecordLogin(user);

      if (OnActiveUserChanged != null)
        OnActiveUserChanged(this, user);
      return ActiveUser != null;
    }

    /// <summary>
    /// Validates credentials without touching the current session - this is what the supervisor
    /// approval dialog uses, so the cashier stays signed in.
    /// </summary>
    public User AuthenticateUsernamePassword(string password, string username)
    {
      if (string.IsNullOrEmpty(username))
        return null;

      if (IsBuiltInAdmin(username, password))
      {
        if (!IsBuiltInAdminAllowed())
        {
          _log.Warn("Built-in recovery account was used but is disabled for this installation.");
          return null;
        }
        _log.Warn("Signed in with the built-in recovery account.");
        return CreateBuiltInAdmin();
      }

      User user = _userManager.FindByUsername(username);
      if (user == null)
        return null;

      if (!PasswordHasher.Verify(password, user.Password))
        return null;

      UpgradeStoredPasswordIfNeeded(user, password);
      return user;
    }

    /// <summary>Whether the built-in recovery account may be used on this installation.</summary>
    public bool IsBuiltInAdminAllowed()
    {
      return _settings.GetBool(SettingKeys.AllowBuiltInAdmin, true);
    }

    public void SetBuiltInAdminAllowed(bool allowed)
    {
      _settings.SetBool(SettingKeys.AllowBuiltInAdmin, allowed);
    }

    /// <summary>
    /// True when at least one ordinary account can still administer the system. The settings screen
    /// uses this to refuse to disable the recovery account and lock everybody out.
    /// </summary>
    public bool HasRealAdministrator()
    {
      try
      {
        List<User> users = _userManager.GetAll();
        if (users == null)
          return false;
        return users.Any(u => ((AccessOption)u.Role & AccessOption.Master) == AccessOption.Master);
      }
      catch (Exception e)
      {
        _log.Error("Could not determine whether a real administrator exists.", e);
        return false;
      }
    }

    private static bool IsBuiltInAdmin(string username, string password)
    {
      // Username matching is case-insensitive to match how SQL Server compares real usernames;
      // the password still has to be exact.
      return string.Equals(username, BuiltInAdminUsername, StringComparison.OrdinalIgnoreCase)
             && string.Equals(password, BuiltInAdminPassword, StringComparison.Ordinal);
    }

    private static User CreateBuiltInAdmin()
    {
      return new User(BuiltInAdminId, BuiltInAdminUsername, string.Empty, BuiltInAdminUsername,
                      (int)RoleOptions.Admin, false);
    }

    public static bool IsBuiltInAdminId(int userId)
    {
      return userId == BuiltInAdminId;
    }

    public static string BuiltInAdminDisplayName
    {
      get { return BuiltInAdminUsername; }
    }

    /// <summary>
    /// Replaces a legacy unsalted hash with the current format once the password has been proven
    /// correct. Best effort: a failure here must never stop somebody signing in.
    /// </summary>
    private void UpgradeStoredPasswordIfNeeded(User user, string plainPassword)
    {
      if (!PasswordHasher.NeedsUpgrade(user.Password))
        return;
      try
      {
        user.Password = PasswordHasher.Hash(plainPassword);
        _userManager.Update(user);
        _log.InfoFormat("Upgraded stored password format for user '{0}'.", user.Username);
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not upgrade stored password for user '{0}'.", user.Username), e);
      }
    }

    public delegate void OnActiveUserDelegate(object sender, User args);
    public event OnActiveUserDelegate OnActiveUserChanged;

    public void Logout()
    {
      User user = ActiveUser;
      ActiveUser = null;
      if (user != null)
        _audit.RecordLogout(user);
    }
  }
}

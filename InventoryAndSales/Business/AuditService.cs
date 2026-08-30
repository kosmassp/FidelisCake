using System;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Records who changed what, so a question asked days later — who dropped that price, who voided
  /// that sale, who turned the recovery account back on — has an answer.
  ///
  /// Two rules shape this class:
  ///
  ///  - **Auditing never breaks the thing it audits.** Every write is swallowed and logged, and
  ///    mirrored into the application log so the trail survives even when the database is the thing
  ///    that is broken. A till that cannot record an audit row must still be able to take money;
  ///    losing the trail is bad, refusing the sale is worse.
  ///  - **It is recorded after the operation, never before.** An entry therefore describes something
  ///    that actually happened. Where a business transaction is still open — a CSV import runs one
  ///    around the whole file — the entry joins it and is rolled back with it, which is the right
  ///    reading: the change it describes did not happen either. The one deliberate exception is a
  ///    *failed* checkout, recorded after the rollback precisely because that is the case worth
  ///    investigating.
  ///
  /// The actor comes from <see cref="LoginManager.OnActiveUserChanged"/> rather than a reference to
  /// the login manager, because the login manager already depends on this class.
  /// </summary>
  public class AuditService
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    #region Vocabulary

    public const string ActionLogin = "LOGIN";
    public const string ActionLoginFailed = "LOGIN_FAILED";
    public const string ActionLogout = "LOGOUT";
    public const string ActionCreate = "CREATE";
    public const string ActionUpdate = "UPDATE";
    public const string ActionDelete = "DELETE";
    public const string ActionCheckout = "CHECKOUT";
    public const string ActionRevise = "REVISE";
    public const string ActionCancel = "CANCEL";
    public const string ActionSettingChange = "SETTING_CHANGE";
    public const string ActionUpdateApplied = "APP_UPDATE";

    public const string EntityProduct = "PRODUCT";
    public const string EntityUser = "USER";
    public const string EntitySetting = "SETTING";
    public const string EntitySale = "SALE";
    public const string EntitySession = "SESSION";
    public const string EntityApplication = "APPLICATION";

    #endregion

    /// <summary>Longest value each column can hold; anything longer is trimmed rather than lost.</summary>
    private const int MaxActionLength = 40;
    private const int MaxEntityTypeLength = 40;
    private const int MaxEntityKeyLength = 60;
    private const int MaxNameLength = 50;
    private const int MaxWorkstationLength = 60;

    private readonly AuditLogManager _auditLogManager;
    private readonly string _workstation;

    private User _actor;

    public AuditService(AuditLogManager auditLogManager)
    {
      _auditLogManager = auditLogManager;
      _workstation = ResolveWorkstation();
    }

    /// <summary>
    /// Follows the signed-in user, so every later entry knows who is at the till without each caller
    /// having to pass it.
    /// </summary>
    public void Follow(LoginManager loginManager)
    {
      loginManager.OnActiveUserChanged += (sender, user) => _actor = user;
      _actor = loginManager.ActiveUser;
    }

    #region Recording

    public void Record(string action, string entityType, string entityKey, string detail)
    {
      RecordAs(_actor, action, entityType, entityKey, detail);
    }

    /// <summary>
    /// Records against a named actor rather than the signed-in one — a failed sign-in has no user,
    /// and a supervisor who approved a step-up is not the person holding the till.
    /// </summary>
    public void RecordAs(User actor, string action, string entityType, string entityKey, string detail)
    {
      try
      {
        AuditLog entry = new AuditLog
        {
          AuditTime = DateTime.Now,
          UserId = actor == null ? 0 : actor.Id,
          UserName = Trim(actor == null ? "<none>" : actor.Name, MaxNameLength),
          Action = Trim(action, MaxActionLength),
          EntityType = Trim(entityType, MaxEntityTypeLength),
          EntityKey = Trim(entityKey, MaxEntityKeyLength),
          Workstation = _workstation,
          Detail = detail ?? string.Empty,
        };

        _auditLogManager.Save(entry);

        // Mirrored into the application log so one file tells the whole story when the database is
        // the thing being investigated.
        _log.InfoFormat("AUDIT {0} {1} {2} by {3}: {4}",
                        entry.Action, entry.EntityType, entry.EntityKey, entry.UserName, entry.Detail);
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not record audit entry '{0}' on {1} '{2}'.", action, entityType, entityKey), e);
      }
    }

    public void RecordLogin(User user)
    {
      RecordAs(user, ActionLogin, EntitySession, user == null ? string.Empty : user.Username,
               string.Format("Login berhasil (role {0}).", user == null ? 0 : user.Role));
    }

    /// <summary>
    /// A failed sign-in. The username is recorded because a run of them against one account is the
    /// signal worth seeing; the password is never touched.
    /// </summary>
    public void RecordLoginFailed(string username)
    {
      RecordAs(null, ActionLoginFailed, EntitySession, username, "Login gagal.");
    }

    public void RecordLogout(User user)
    {
      RecordAs(user, ActionLogout, EntitySession, user == null ? string.Empty : user.Username, "Logout.");

      // Signing out does not raise OnActiveUserChanged - raising it there would send the shell back
      // to the login page from inside its own navigation - so the tracked actor is cleared here.
      _actor = null;
    }

    #endregion

    private static string Trim(string value, int maxLength)
    {
      if (string.IsNullOrEmpty(value))
        return string.Empty;
      return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    private static string ResolveWorkstation()
    {
      try
      {
        return Trim(Environment.MachineName, MaxWorkstationLength);
      }
      catch (Exception)
      {
        // Never worth failing a startup over.
        return string.Empty;
      }
    }
  }
}

# Authentication and Authorisation

## Sign-in

`LoginPage` → `LoginController.Login` → `LoginManager.Login(username, password)`:

1. The built-in recovery account is checked first (see below).
2. `UserManager.FindByUsername(username)` — a **parameterised** lookup on username only, excluding
   soft-deleted rows, returning the row **only when exactly one matches**. The password is no
   longer part of the query, because a per-user salt cannot be expressed as a SQL predicate.
3. `PasswordHasher.Verify(password, user.Password)` — accepts both stored formats.
4. If the stored hash is still in the legacy format it is transparently re-hashed and saved.
5. `LoginManager.ActiveUser` is assigned (**including `null` on failure**) and
   `OnActiveUserChanged` fires.

`MainFormController.OnActiveUserChanged` reacts: a user → show the name, `EnableMenu(role)`, go to
the cashier page; `null` → clear the status bar, `EnableMenu(0)`, go to the login page.

On failure `LoginPage` shows *"Username atau password tidak benar"*. There is **no attempt limit and
no lockout**.

## The built-in recovery account

`LoginManager.AuthenticateUsernamePassword` short-circuits before touching the database:

```
username == "Kosmas" (case-insensitive) && password == "kosmas"
    → new User(-1, "Kosmas", "", "Kosmas", 1023, false)
```

Role `1023` is full `Admin`. `Id = -1` means it has no `M_USERS` row, so transactions it creates
carry `UserId = -1` and every report shows those as `COALESCE(u.Name, 'ADMIN')` → **"ADMIN"**.

**Why it exists:** administrators have deleted their own account, leaving nobody able to create
users. The operators are not technical, so a database-level recovery is not an option. This account
is deliberately outside `M_USERS` precisely so that deleting every user cannot lock the shop out of
its own till.

**It can be switched off per installation.** `M_SETTINGS` key `ALLOW_BUILTIN_ADMIN`, group
`SECURITY`, edited under *Pengaturan → Keamanan*. Default is **enabled**, so upgrading an existing
site changes nothing. The settings page refuses to disable it while no `M_USERS` account holds
`AccessOption.Master` — otherwise turning it off would recreate exactly the lockout it guards
against.

Every use is logged at WARN (`"Signed in with the built-in recovery account."`), and a rejected
attempt while disabled is logged too.

⚠ The credentials are still compiled into the source, so they cannot be rotated without a rebuild.
Sites that do not want it should disable it once a real administrator exists.

## Permission model

`Enumeration/AccessOption.cs`:

```csharp
[Flags] public enum AccessOption { Admin = 1, Cashier = 2, Master = 4, Laporan = 8 }

public enum RoleOptions
{
    Admin      = 1023,                                              // all low bits
    Cashier    = AccessOption.Cashier,                              // 2
    Supervisor = Cashier | Master | Laporan,                        // 14
}
```

`M_USERS.Role` stores the integer. The role combo box on the user master page is bound to
`RoleOptions`, so operators pick a preset rather than composing bits.

The single check, `GUI/Util/BusinessUtil.cs`:

```csharp
public static bool AllowedRole(int role, AccessOption accessOption)
    => ((AccessOption)role & accessOption) == accessOption;
```

True when the role contains **every** bit requested.

## What each permission gates

| Permission | Gates | Checked in |
|---|---|---|
| `Cashier` (2) | *Transaksi* menu, daily-total check | `MainForm.EnableMenu` |
| `Master` (4) | *Edit* menu (product/user master); revising and cancelling transactions | `MainForm.EnableMenu`, `MainFormController` |
| `Laporan` (8) | *Laporan* (reports) menu | `MainForm.EnableMenu` |
| `Admin` (1) | nothing on its own — it is simply one of the bits `RoleOptions.Admin` sets | — |

Effective access by role:

| Role | Value | Sales | Master data | Revise/cancel | Reports |
|---|---|---|---|---|---|
| `Cashier` | 2 | ✅ | ❌ | via step-up | ❌ |
| `Supervisor` | 14 | ✅ | ✅ | ✅ | ✅ |
| `Admin` | 1023 | ✅ | ✅ | ✅ | ✅ |

**`Laporan` now gates reports.** `MainForm.EnableMenu` previously tested `AccessOption.Admin`
(bit 1), so a Supervisor held the `Laporan` bit but still could not open the reports menu. It now
tests `AccessOption.Laporan` (bit 8), which is what the role presets always implied.

⚠ Behaviour change on upgrade: Supervisors (role 14) gain access to the reports menu. Admins are
unaffected. If a site wants supervisors kept out of reports, give those users the `Cashier` role
instead.

⚠ Menu gating is **visibility only** — `EnableMenu` sets `Visible`, it does not disable the
underlying actions. It is a UI affordance, not a security boundary. The real check for the sensitive
operations is the step-up below.

## Supervisor step-up

Revising or cancelling a transaction requires `AccessOption.Master`. If the signed-in user lacks it,
`MainFormController` opens `AuthenticationForm(AccessOption.Master)`:

1. The supervisor types their own credentials.
2. `LoginManager.AuthenticateUsernamePassword` validates them — **without changing `ActiveUser`**,
   so the cashier stays signed in.
3. `BusinessUtil.AllowedRole(user.Role, requiredOption)` must also pass.
4. Success → `DialogResult.OK` and `AuthenticatedUser` is exposed; failure → *"Akses Ditolak"*.

Cancelling the dialog aborts the operation.

The authorising supervisor is then recorded: `TransactionUpdateController.Checkout` attributes the
revision to `_supervisor.Id`, so the corrected sale is credited to whoever approved it, not to the
cashier. Cancellation records no operator — only `Revision = -1` on the original row.

Failed approvals are logged and the attempt count is shown to the operator
(*"Akses Ditolak. (percobaan ke-N)"*). The message is identical whether the credentials were wrong
or the account simply lacks the permission, so it gives nothing away.

⚠ There is still no throttling or lockout on repeated approval attempts.

⚠ The authorisation decision still lives in `AuthenticationForm` rather than a controller. The
permission-plus-step-up logic used by the menus has been consolidated into
`MainFormController.RequirePermission`.

## Password handling

**Two formats coexist.** `SimpleCommon/Utility/PasswordHasher.cs` reads both and only ever writes
the new one.

| | Legacy | Current |
|---|---|---|
| Shape | `E2-12-5E-…` (191 chars) | `PBKDF2$25000$<salt>$<hash>` (83 chars) |
| Algorithm | one round of SHA-512, **no salt** | PBKDF2-HMAC-SHA1, per-user salt |
| Written by | builds before this change | `PasswordHasher.Hash` |

**Nobody has to change their password.** `Verify` detects the format; a legacy hash that verifies is
re-hashed and saved on the spot (`LoginManager.UpgradeStoredPasswordIfNeeded`). A site therefore
migrates itself, one user at a time, as people sign in. The re-hash is best effort — if the update
fails it is logged and the sign-in still succeeds.

**Cost:** 25,000 iterations, about 175 ms on a modern desktop and a few hundred on the older shop
machines. Deliberately below common public guidance, which targets internet-facing services; here
the database is local to one till and the cost is paid on every supervisor approval too. The
iteration count is stored inside each hash, so raising it later is safe and self-migrating.

- **Editing:** the password boxes show a fixed `********` placeholder for an existing user, and
  `MasterUserPage` tracks a `_passwordChanged` flag from the text-changed event. Leaving the boxes
  alone preserves the password. (Previously the screen displayed the first 8 characters of the
  stored hash and the controller compared against that prefix — which tied the screen to the storage
  format and would have broken outright under the new format.)
- **Validation:** username and name are always required; the password pair is required only when
  adding a user or actually changing it. Username uniqueness is now checked. **No length or
  complexity rule** — deliberate, given who operates these tills.
- **Username is immutable** through the UI — `UpdateUser` accepts the parameter and never assigns it.

## Logout

`MainForm.LoadLoginPage()` calls `controller.Logout()`, which disables menus, clears the status bar,
and calls `LoginManager.Logout()` — which sets `ActiveUser = null` **without raising
`OnActiveUserChanged`**. That is deliberate: the caller has already navigated, and raising the event
would re-enter `LoadLoginPage`.

There is no idle timeout and no automatic sign-out. A terminal left unattended stays signed in.

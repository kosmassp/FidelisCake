# Directory: `InventoryAndSales/Enumeration/`

Namespace `InventoryAndSales.Enumeration`.

---

## `AccessOption.cs`

Two enums that together define authorisation.

### `[Flags] public enum AccessOption`

| Value | Bit | Grants |
|---|---|---|
| `Admin` | 1 | Nothing on its own — simply one of the bits `RoleOptions.Admin` sets |
| `Cashier` | 2 | Sales menu (*Transaksi*) and the cashier daily-total check |
| `Master` | 4 | Master data menu (*Edit*), and revising/cancelling transactions |
| `Laporan` | 8 | Reports menu (*Laporan*) |

### `public enum RoleOptions`

Presets stored in `M_USERS.Role`. Bound to the role combo box on the user master page.

| Value | Numeric | Bits |
|---|---|---|
| `Admin` | `1023` | All ten low bits set — everything |
| `Cashier` | `2` | `Cashier` |
| `Supervisor` | `14` | `Cashier \| Master \| Laporan` |

### How it is checked

`GUI/Util/BusinessUtil.AllowedRole(int role, AccessOption option)` returns
`((AccessOption)role & option) == option`.

`MainForm.EnableMenu` previously gated the reports menu on `AccessOption.Admin` (bit 1) rather than
`AccessOption.Laporan` (bit 8), so a `Supervisor` (14) held the `Laporan` bit but could not see the
menu. It now tests `Laporan`, which is what the role presets always implied.

⚠ Behaviour change on upgrade: Supervisors gain the reports menu.

See [../business-auth-and-roles.md](../business-auth-and-roles.md).

---

## `DisplayPage.cs`

`public enum DisplayPage` — which page the main tab control is showing.

| Value | Page |
|---|---|
| `Login` | `LoginPage` |
| `Cashier` | `CashierPage` |
| `MasterProduct` | `MasterProductPage` |
| `MasterUser` | `MasterUserPage` |
| `Report` | `ReportDisplayPage` |

`MainForm` keeps `currentPage` and uses it to scope the F5/F6/F7 hotkeys to the cashier screen.

`MainForm` sets `currentPage = DisplayPage.Report` when the reports tab is selected, so the F5/F6/F7
cashier hotkeys stop firing while reports are on screen. It previously switched the tab without
updating `currentPage`, leaving them armed.

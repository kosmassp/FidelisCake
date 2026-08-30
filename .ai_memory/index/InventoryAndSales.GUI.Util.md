# Directory: `InventoryAndSales/GUI/Util/`

Small stateless helpers for the UI layer.

---

## `BusinessUtil.cs`

Namespace `InventoryAndSales.GUI.Util`. `class BusinessUtil` (internal, static members).

| Member | Signature | Purpose |
|---|---|---|
| `AllowedRole` | `static bool AllowedRole(int role, AccessOption accessOption)` | `((AccessOption)role & accessOption) == accessOption` — true when the role mask contains **every** bit of the requested option. |

The single authorisation primitive. Callers: `MainForm.EnableMenu`,
`MainFormController.RequestUpdateTransaction` / `RequestDeleteTransaction`,
`AuthenticationForm.buttonAuthenticate_Click`.

⚠ Note: despite the name and the `GUI.Util` namespace this is an authorisation rule, not
presentation. It belongs in the business layer next to `AccessOption`. Anything that calls it is
making a security decision.

---

## `DataTableUtil.cs`

Namespace `InventoryAndSales.GUI` (**not** `.Util` — deliberate, so it can name
`System.Data.DataTable` without colliding with `InventoryAndSales.Database.DataTable`).
`public class DataTableUtil`.

| Member | Signature | Purpose |
|---|---|---|
| `GetDataTable` | `static DataTable GetDataTable(List<Dictionary<string,string>> summaryReport, string tableName)` | Converts report rows into an ADO.NET `DataTable` for `DataGridView.DataSource`. Columns come from the **first** row's keys; each row is added as `dictionary.Values.ToArray()`. Returns an empty `DataTable` for an empty input. |

⚠ Note: the `tableName` parameter is accepted and never used — `dataTable.TableName` is left unset.

⚠ Note: the conversion assumes every dictionary has the same keys **in the same order**, since
columns come from row 0's keys but values are taken positionally. `CustomQuery` populates its
dictionary in reader-ordinal order for every row of a given query, so this holds; it would break for
a heterogeneous list.

All values are strings, so grids sort dates and amounts lexically. Dates are pre-formatted
`dd MMM yyyy` by `CustomQuery`.

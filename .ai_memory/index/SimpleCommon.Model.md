# Directory: `SimpleCommon/Model/`

Namespace `SimpleCommon.Model`.

---

## `ISearchable.cs`

`public interface ISearchable` — contract a row type implements so `SearchableDataView<T>` can
display and filter it without knowing anything about the type.

| Member | Signature | Purpose |
|---|---|---|
| `ToSearchableString` | `string ToSearchableString()` | Flattened text the control could match against. |
| `ToDisplayKeys` | `string[] ToDisplayKeys()` | Column headers. |
| `ToDisplayValues` | `string[] ToDisplayValues()` | One cell value per column, **aligned with `ToDisplayKeys`**. |

This is the dependency-inversion seam that keeps `SimpleCommon` free of application types: the
control depends on this abstraction, and an application model implements it.

⚠ Note: `SearchableDataView<T>` currently filters using `ToDisplayValues()` and never calls
`ToSearchableString()`, and it uses `ToDisplayValues()` for row content but never `ToDisplayKeys()`
(headers are expected to be configured on the grid in the designer). Two of the three members are
therefore unused. If nothing needs `ToSearchableString`, drop it — an interface with dead members
costs every implementer.

⚠ Note: nothing in the solution implements `ISearchable` yet. See
[SimpleCommon.UI.ComponentWinForm.md](SimpleCommon.UI.ComponentWinForm.md).

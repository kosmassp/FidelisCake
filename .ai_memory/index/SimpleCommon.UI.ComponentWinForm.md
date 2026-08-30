# Directory: `SimpleCommon/UI/ComponentWinForm/`

Namespace `SimpleCommon.UI.ComponentWinForm`. Reusable WinForms controls.

---

## `SearcheableDataView.cs`

⚠ Note: file name is misspelled (`Searcheable`); the type is `SearchableDataView<T>`.

`public partial class SearchableDataView<T> : UserControl where T : ISearchable` — a generic
"filter box above a grid" control. **This is the intended extraction of the duplicated logic in
`CashierPage` and `TransactionUpdatePage`, but nothing uses it yet.**

State: `Dictionary<int, T> itemDictionary` (grid row → item), `Dictionary<char, bool>
_specialKeysEvent` (key char → whether to strip it from the search box).

| Member | Signature | Purpose |
|---|---|---|
| `RegisteredEvent` | `void RegisteredEvent(char a, bool isRemoveChar)` | Registers a "special key" (e.g. `+`, `-`) the host wants notified about, and whether it should be stripped from the search text. |
| `SetDatasource` | `void SetDatasource(List<T> ds)` | Rebuilds `itemDictionary` and adds one grid row per item from `ToDisplayValues()`. |
| `SpecialKeyEvent` | `event SpecialKeyEventHandler` | Raised with the key char and the currently selected items. |
| `EnterKeyPressedEvent` | `event SpecialKeyEventHandler` | Raised on Enter with the currently *filtered* items (key char `13`). |
| `CharPress` | `private void CharPress(char key)` | Strips every occurrence of a registered key from the search box, collects the selected items, raises `SpecialKeyEvent`. |
| `dataGridView_KeyPress` / `textBoxFilter_KeyPress` | `private void (object, KeyPressEventArgs)` | Route both grid and search box key presses into `CharPress`. |
| `textBoxFilter_KeyUp` | `private void (object, KeyEventArgs)` | Strips registered chars; Down/Up navigate; Left/Right are swallowed; otherwise re-filters and raises `EnterKeyPressedEvent` on Enter. |
| `FilterItemView` | `private List<T> FilterItemView(string filter, out int selectedIndex)` | Case-insensitive "any display value contains the filter" match. Sets row visibility, returns the matches, and reselects the first visible row **only when visibility actually changed** (`flagChange`) — avoiding the selection jitter `TransactionUpdatePage` has. |
| `ClearFilter` | `void ClearFilter()` | Blanks the search box and shows all rows. |
| `SelectNextVisibleRow` / `SelectPrevVisibleRow` | `private void ()` | Wrap-around navigation skipping hidden rows. |
| `buttonSetting_Click`, `resetToolStripMenuItem_Click`, `allToolStripMenuItem_Click` | `private void (object, EventArgs)` | Empty stubs for planned column-picker features. |

⚠ Note: `SetDatasource` allocates a fresh `itemDictionary` but never clears the grid rows, so
calling it twice duplicates rows while the dictionary only knows the newest — the row → item lookup
then misaligns. Add `dataGridView.Rows.Clear()` before rebuilding if this control is adopted.

⚠ Note: `FilterItemView` mutates its `filter` parameter (`filter = filter.ToLower()`) inside the
loop, and its inner loop sets `Visible = false` on a non-matching value *before* checking the
remaining values — the row is corrected only if a later value matches, so the final state is
correct but the intent is hard to read.

---

## `SpecialKeyEventArgs.cs`

`public class SpecialKeyEventArgs<T> : EventArgs` — payload for both `SearchableDataView<T>` events.

| Member | Type | Purpose |
|---|---|---|
| `KeyChar` | `char { get; set; }` | The key that triggered the event (`13` for Enter). |
| `SelectedItems` | `List<T> { get; set; }` | Selected items for `SpecialKeyEvent`, filtered items for `EnterKeyPressedEvent`. |
| *(ctor)* | `SpecialKeyEventArgs(char keyChar, List<T> selectedItems)` | Assigns both. |

⚠ Note: both properties are settable, so a handler can mutate the args. `{ get; private set; }`
would make the payload immutable.

---

## `SplitButton.cs`

`public class SplitButton : Button` — a button with a dropdown arrow region on its right edge.
Pure `System.Drawing` / `System.Windows.Forms`; no external dependency.

| Member | Signature | Purpose |
|---|---|---|
| `Menu` | `ContextMenuStrip { get; set; }` | The menu shown when the arrow region is clicked. `[Browsable(true)]`, designer-serialisable. |
| `SplitWidth` | `int { get; set; }` | Width of the arrow region in pixels, default `20`. |
| `OnMouseDown` | `protected override void (MouseEventArgs)` | Left-click inside the arrow region → `Menu.Show(this, 0, Height)`. Anywhere else → normal button behaviour. |
| `OnPaint` | `protected override void (PaintEventArgs)` | Draws a filled triangle glyph plus a dotted vertical separator, greyed when disabled. |

A good model for this codebase: extends a framework control, adds two designer-visible properties,
and needs no library.

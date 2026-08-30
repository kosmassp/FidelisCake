# Product and User Master Data

## Products

`M_PRODUCTS` is the sellable catalogue. Managed on `MasterProductPage` (menu *Edit → Daftar Barang*,
requires `AccessOption.Master`).

| Field | Rule |
|---|---|
| `Code` | Required. Short human code, prefix-searchable on the sale screen. Unique — enforced in application code only. |
| `Name` | Required. `LIKE`-searchable. Unique — application code only. |
| `Barcode` | Optional. Exact-match scan. Unique when present — application code only. |
| `Price` | Required, must parse as `decimal`. Stored `decimal(18,0)` — whole rupiah. |
| `Discount` | Optional. Sign-encoded, see [business-cart-and-pricing.md](business-cart-and-pricing.md). |
| `Deleted` | Soft-delete flag; never hard-deleted. |

There is **no stock quantity**. Nothing decrements on sale.

### Uniqueness

`MasterProductPage.ValidateUniqueItem()` scans every product for a duplicate code, name or barcode,
skipping the row being edited **by `Id`**, and reports *every* clash it finds.

It previously skipped the edited row by comparing code, name and barcode together — which let a
genuine duplicate through whenever all three happened to line up — and `break`ed on the first hit, so
only one reason was ever shown.

⚠ Still a client-side scan with no database constraint behind it, so it is racy in principle and
bypassed by CSV import.

### Code generation

*Generate* fills the code box from the name:

1. `GeneratePrefix` — uppercase first letter of each word that starts with a letter.
   `"Kue Coklat Besar"` → `"KCB"`.
2. `GenerateCode` — append a counter zero-padded so the total length is 5, incrementing until the
   code is unused: `KCB01`, `KCB02`, …

⚠ The loop is guarded by `prefix.Length < 5`, so a name yielding a 5+ character prefix returns the
bare prefix with no uniqueness check. A prefix with more than 99,999 collisions is not a real
concern.

### Soft delete

`MasterManager.DeleteProduct` sets `Deleted = true` and updates. Effects:

- `ProductManager.GetAllAvailable` (sale screen, product master) excludes it.
- `MasterManager.GetAllProduct` (revision screen) **includes** it, so old sale lines still reload.
- Reports show deleted products by name via the join; only a product whose row is physically gone
  renders as `COALESCE(p.Name, 'Telah Dihapus')`.

### Search and sort

Search calls `ProductManager.GetAllAvailable(criteria, orderBy)`:

```sql
WHERE Name LIKE '%<criteria>%' AND Deleted = 'False' ORDER BY <orderBy>
```

Spaces in the search text are replaced with `%`, so `"kue coklat"` matches `"kue besar coklat"`.

The sort combo box is populated from `DataTableList.Instance.GetDataTable(typeof(Product)).Columns`
— the column map drives the UI, so adding a mapped column adds a sort option for free.

⚠ `orderBy` is concatenated straight into the SQL. It comes from a fixed combo box today, so it is
safe in practice, but it is an injection point if ever made free-text.

## CSV import and export

Both on `MasterProductPage`. Format:

```
Id,Code,Barcode,Name,Price,Discount
0,KC001,8991234567890,Kue Coklat,15000,500
```

### Export

`SaveFileDialog` → UTF-8 file, header row plus one row per product from the current search and sort.
`QuoteField` trims each value and applies RFC-4180 quoting when it contains `,`, `"` or a newline.
Write failures show *"Gagal menyimpan file !!! …"*.

### Import

`OpenFileDialog` → `ImportProductsFromCsv` → `controller.SetItemForImport` → `Reset()`.

- Header row skipped; blank lines skipped; rows with fewer than 6 fields skipped **silently**.
- `ParseCsvLine` is a hand-written parser handling quoted fields and `""` escapes.
- `Name` is passed through `ConvertToTitleCase` (`en-US` `TextInfo.ToTitleCase`) — **imported names
  are re-cased**, so `"KUE COKLAT"` becomes `"Kue Coklat"`.
- Per-row parse errors show a message box and skip that row.

`SetItemForImport` decides insert vs update per row:

| `Id` | Action |
|---|---|
| `0` or empty | Insert a new product |
| anything else | **Update the product with that `Id`** |

So export → edit in a spreadsheet → import is a supported bulk-edit round trip: keep the `Id` to
update, clear it to create.

**Import is now transactional.** `MasterProductController.SetItemForImport` wraps the whole loop in
one database transaction, so a failure part way through leaves the catalogue exactly as it was. It
returns how many rows were inserted and updated, and the screen reports that instead of an
unconditional *"File imported successfully!"*. The operator confirms the row count before anything
is applied, and an empty or unreadable file says so.

Apostrophes in imported values no longer break the SQL — the DAO layer parameterises.

⚠ **Import still bypasses the uniqueness and price validation** the edit panel applies, and an `Id`
that does not exist updates zero rows silently.

## Users

`M_USERS`, managed on `MasterUserPage` (menu *Edit → Daftar User*, requires `AccessOption.Master`).

| Field | Rule |
|---|---|
| `Username` | Required on create. **Cannot be changed** — `UpdateUser` accepts it and never assigns it. |
| `Name` | Required. Printed on receipts, shown in reports. |
| `Password` | Required, must match the re-password box. No length or complexity rule. Stored as unsalted SHA-512. |
| `Role` | Chosen from the `RoleOptions` presets. |
| `Deleted` | Soft delete. |

The grid shows exactly `Username`, `Name` and `RoleOption` because every other property on `User` is
`[Browsable(false)]`.

Password editing shows a `********` placeholder and tracks whether the field was touched — see
[business-auth-and-roles.md](business-auth-and-roles.md). Leaving it alone preserves the existing
password. Usernames are checked for duplicates.

Deleting a user is a soft delete, so past sales keep resolving the cashier's name in reports.

⚠ Nothing prevents deleting the last administrator. The built-in recovery account is the de-facto
recovery path, which is why the security settings page refuses to disable it while no `M_USERS`
account holds `AccessOption.Master` — see [business-auth-and-roles.md](business-auth-and-roles.md).

## Edit-mode pattern

Both master pages share one shape, worth following for any new master screen:

`OnEditMasterItem(bool edit)` / `OnEditMasterUser(bool edit)` is the single place that:

- swaps Add/Update/Delete buttons for OK/Cancel,
- enables or disables every detail field,
- disables and greys the grid so the selection cannot move mid-edit,
- **and on leaving edit mode, re-queries and rebinds the grid.**

Having one method own the whole mode transition is why the two pages stay consistent. Keep new
fields wired into it rather than toggling controls ad hoc.

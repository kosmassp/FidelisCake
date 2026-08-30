# Directory: `InventoryAndSales/Business/Enum/`

Namespace `InventoryAndSales.Business.Enum`.

⚠ Note: the namespace segment `Enum` collides with `System.Enum`. Files that use both must
disambiguate. `MasterUserPage.cs` calls `Enum.GetValues(typeof(RoleOptions))` and only compiles
because it does not import this namespace. Prefer a non-colliding segment (e.g. `Enums`) for any
new namespace — see [../rules-csharp.md](../rules-csharp.md).

---

## `TransactionStatus.cs`

`public enum TransactionStatus` — result of a checkout attempt.

| Value | Meaning |
|---|---|
| `INITIATE` | Initial value before the attempt resolves. Never returned by a completed `Checkout`. |
| `SUCCESS` | Transaction was persisted. Printing may still have failed — check the `out message`. |
| `FAILED` | Persistence failed; nothing was written. |

Produced by `CashierManager.Checkout`, consumed by
`GUI/Controller/CashierController.cs → Checkout`.

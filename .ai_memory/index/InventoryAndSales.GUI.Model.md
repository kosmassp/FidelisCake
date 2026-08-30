# Directory: `InventoryAndSales/GUI/Model/`

Namespace `InventoryAndSales.GUI.Model`. Intended for view models — presentation-shaped types that
keep persistence models out of the UI.

---

## `ViewCartModel.cs`

`public class ViewItemMaster` — a flattened product for grid display.

| Member | Type | Purpose |
|---|---|---|
| `Id`, `Code`, `Name` | `int`, `string`, `string` | Identity and labels. |
| `Price`, `Discount` | `decimal` | Same sign-encoding as `Product.Discount`. |
| `DiscountAmount` | `decimal { get; }` | `Discount < 0` → `Price * (-Discount / 100)`, then `Math.Min(Price, discount)`. |
| `NetPrice` | `decimal { get; }` | `Price - DiscountAmount`. |
| *(ctor)* | `ViewItemMaster(Product product)` | **Empty body — copies nothing.** |

⚠ Note: **this class is dead code.** Nothing constructs or references it. The file name
(`ViewCartModel.cs`) does not match the type name (`ViewItemMaster`), the constructor silently
produces an all-default instance, and `DiscountAmount` / `NetPrice` duplicate the logic already on
`Database.Model.Product`.

If view models are reintroduced here, map explicitly from `Product` and **delegate** the pricing to
`Product.DiscountAmount` / `Product.NetPrice` rather than re-implementing it — duplicated pricing
rules are the kind of drift that produces wrong receipts. See
[../business-cart-and-pricing.md](../business-cart-and-pricing.md).

# Cart and Pricing

The pricing rules are the part of this system most worth getting right — they decide what the
customer pays and what the receipt says. They live in exactly three places:

| Rule | Where |
|---|---|
| Resolve a product's discount to rupiah | `Database/Model/Product.cs` → `DiscountAmount` |
| Line subtotals | `Database/Model/TransactionDetail.cs` → `UpdateQuantity` |
| Cart total | `Business/Cart.cs` → `GetTotal` |

Nothing else should recompute them. (`GUI/Model/ViewCartModel.cs` duplicates the first — it is dead
code and should not be revived as-is.)

## Discount encoding

`M_PRODUCTS.Discount` is a single `decimal` column carrying two different meanings, distinguished by
sign:

| Stored value | Meaning | Example |
|---|---|---|
| `> 0` | Flat rupiah discount per unit | `500` → Rp 500 off |
| `< 0` | Percentage, stored negated | `-15` → 15 % off |
| `0` | No discount | |

The UI encodes this in `MasterProductPage.GetItemDetail` (percent box → `-value`, amount box →
`+value`) and decodes it in `UpdateDetailBarang`.

⚠ This is a compact but implicit convention. Any new code touching `Discount` **must** go through
`Product.DiscountAmount` rather than reading the raw column, or it will treat `-15` as a negative
price.

## Unit price formulas

```csharp
// Product.DiscountAmount
var discount = Discount;
if (Discount < 0)
    discount = Price * (-Discount / 100);   // percentage → rupiah
return Math.Min(Price, discount);           // never exceed the price

// Product.NetPrice
return Price - DiscountAmount;
```

The `Math.Min(Price, discount)` clamp is a hard invariant: **a line can never go negative.** A
Rp 5,000 discount on a Rp 3,000 product yields Rp 3,000 off, i.e. free — not a Rp 2,000 credit.

`Product.DisplayDiscount` is presentation only: `"15 %"` for percentages, otherwise the clamped
amount formatted with `Constant.DISPLAY_CURRENCY` (`#,##0.00`).

## Line subtotals

`TransactionDetail(Product product, int quantity)` snapshots the product, then calls
`UpdateQuantity`:

```csharp
public void UpdateQuantity(int quantity)
{
    if (quantity < 0) quantity = 0;
    Quantity         = quantity;
    SubtotalDiscount = ProductDiscount * quantity;
    SubtotalPrice    = ProductPrice    * quantity;
    Subtotal         = SubtotalPrice - SubtotalDiscount;
}
```

Snapshotted fields:

| Field | From | Why |
|---|---|---|
| `ProductPrice` | `product.Price` | Price at the moment of sale |
| `ProductDiscount` | `product.**DiscountAmount**` | **Already resolved to rupiah** — the percentage is never stored on the line |
| `ProductName` | `product.Name` | Display only; **not persisted** |
| `ProductId` | `product.Id` | Link back to the catalogue |

Because the discount is resolved at sale time, repricing a product later cannot change what a past
sale charged.

**`UpdateQuantity` is the only correct way to change a quantity.** Assigning `Quantity` directly
leaves the three subtotals stale, and they are what gets persisted, totalled and printed.

## Cart total

`Cart.GetTotal(out totalPrice, out totalDiscount)` sums the lines and returns
`totalPrice - totalDiscount` — the amount owed. The two `out` values feed the receipt's *Total Item*
and *Total Disc* lines.

There is **no transaction-level discount, tax or rounding step.** The order total is exactly the sum
of the lines.

## Cart state

`Business/Cart.cs` holds `Dictionary<int, TransactionDetail> _items`, keyed by `Product.Id`, guarded
by `_lockItems`. One entry per product — quantities aggregate, they do not create separate lines.

**Each screen owns its own instance.** `CashierController` and `TransactionUpdateController` each
construct a `Cart` in their constructor. The cart used to live on the `CashierManager` singleton,
which meant opening the correction window replaced whatever the cashier was ringing up and left both
screens reacting to each other's changes.

| Operation | Method | Behaviour |
|---|---|---|
| Add one | `Add(product, 1)` | Existing line → `UpdateQuantity(current + 1)`. New line → created if the delta is positive. |
| Remove one | `Add(product, -1)` | Decrements. Reaching 0 leaves a zero-quantity line, which the grid hides. |
| Set quantity | `SetQuantity(product, n)` | Absolute. `n <= 0` removes the line. |
| Remove line | `Remove(product)` | Drops the entry, raises `CartChange` with quantity `0`. |
| Clear | `Clear()` | Empties it. **Raises no event** — callers reset their own grid, and one event per line would make it flicker. |
| Read lines | `GetLines()` | Copy of the list (not of the lines) for checkout. |

All mutations are wrapped in `try`/`catch` that logs and returns `false` rather than throwing, so a
cart error never crashes the sale screen.

`Add` with a negative delta for a product **not** in the cart is now a no-op returning `true`.
Previously it skipped the insert and then indexed the missing key, throwing
`KeyNotFoundException` — caught and logged, but it meant `false` did not reliably mean failure.

### Change notification

```csharp
public delegate void CartChangeDelegate(object sender, KeyValuePair<Product, int> args);
public event CartChangeDelegate CartChange;
```

Raised after every successful mutation with the product and its **new quantity**. The controller
handles it, calls `view.UpdateDataGridViewCart(product, quantity)` and recomputes the total. One
path updates the display regardless of whether the change came from a button, a `+` keypress or a
barcode scan.

## Product lookup on the sale screen

`CashierPage.FilterItemView(filter, out byBarcode)` decides row visibility:

1. **Barcode** — exact, case-sensitive equality against `Product.Barcode`; sets `byBarcode = true`.
2. **Name** — case-insensitive `Contains`.
3. **Code** — case-insensitive `StartsWith`.

It returns the product **only when exactly one row matched**, which is what makes the scanner flow
work: the scanner types the full barcode then sends Enter, `FilterItemView` returns the unique
match, the item is added, and because `byBarcode` is true the filter box is cleared for the next
scan.

Keyboard: `+` adds the selected row, `-` removes one, Up/Down navigate visible rows with wrap-around,
Enter adds a unique match. `MainForm` adds **F5** focus filter, **F6** focus payment, **F7**
checkout.

## Known issues

⚠ **Percentage discounts can round.** `Price * (-Discount / 100)` produces a fraction in memory, but
every money column is `decimal(18,0)`, so the stored value is rounded. Recomputing a line in C# can
differ by a rupiah from what was stored. Reports read stored values, so they remain self-consistent.

⚠ **`GUI/Model/ViewCartModel.cs`** re-implements `DiscountAmount` and `NetPrice`. It is unused. If
view models return, delegate to `Product` rather than copying the formulas.

⚠ **`CashierPage` and `TransactionUpdatePage` remain near-duplicates** of each other. The cart is no
longer shared, but the grid, filter and keyboard handling is still copy-pasted, so a fix in one has
to be repeated in the other.

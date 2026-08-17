# Checkout

Turning a cart into a persisted sale plus a printed receipt.

## The path

```
CashierPage.buttonCheckout_Click
    └─ decimal.TryParse guard on the payment box
        └─ CashierController.Checkout(payment, notes, out successMessage)
            ├─ business validation (rules below)
            └─ CashierManager.Checkout(cart, payment, notes, userId, customerId: 1, out message)
                ├─ GenerateTransactionAndDetails(cart, ...)  build header + lines
                ├─ TransactionManager.SaveCompleteTransaction(...)   ← atomic
                ├─ _lastFactur = transaction.Factur
                └─ PrintPaymentNote(...)                   ← failure does NOT fail the sale
```

The cart is passed in rather than held on `CashierManager`, so the sale screen and the correction
screen cannot disturb each other's baskets.

## Validation

`CashierController.Checkout` applies these rules, in order, returning an Indonesian message string —
**empty string means success**:

| # | Condition | Message |
|---|---|---|
| 0 | no signed-in user | *"Sesi telah berakhir. Silahkan login kembali."* |
| 1 | `payment < 0` | *"Pembayaran kurang dari 0"* |
| 2 | cart total `<= 0` | *"Tidak ada pembelian. Silahkan tambahkan item yang dibeli"* |
| 3 | `payment - total < 0` | *"Pembayaran kurang dari harga yang harus dibayarkan."* |

Rule 2 makes an empty cart and a fully-discounted-to-zero cart equally unsellable. Rule 3 means
**cash must cover the total** — no partial payment, no credit.

Before any of this, the view checks the payment box parses as a `decimal`
(*"Pembayaran Tidak Valid"*). That parse is culture-sensitive, which is why the app pins `en-US`.

## Building the transaction

`CashierManager.GenerateTransactionAndDetails`:

| Field | Value |
|---|---|
| `Time` | `DateTime.Now` |
| `Factur` | `GenerateFactur()` — see below |
| `Payment` | as tendered |
| `UserId` | `LoginManager.ActiveUser.Id` |
| `CustomerId` | **hardcoded `1`** |
| `Notes` | operator's free text |
| `TotalPrice` | Σ line `SubtotalPrice` |
| `TotalDiscount` | Σ line `SubtotalDiscount` |
| `Total` | Σ (`SubtotalPrice` − `SubtotalDiscount`) |
| `Exchange` | `Payment − Total` |
| `Revision` | left at `0` — active |

Detail rows are the cart's `TransactionDetail` objects **as-is**, so the price and discount snapshots
taken when the item was added are what gets stored.

## Faktur (invoice number)

```csharp
private string GenerateFactur() => DateTime.Now.Ticks.ToString();
```

An 18-digit tick count, stored in `varchar(20)` under a **unique** index.

| Property | Assessment |
|---|---|
| Unique | Yes in practice — 100 ns resolution, single terminal, one sale at a time. |
| Sortable | Yes, chronologically, as a string of fixed width. |
| Human-friendly | **No.** `638912345678901234` is unreadable and unspeakable over the phone. |
| Multi-terminal safe | **No.** Two terminals could collide, and the unique index would reject the second sale. |
| Restart safe | Yes — ticks come from the wall clock, not a counter. |

⚠ Moving the clock backwards can produce a duplicate and fail the insert. A commented-out
`ConvertToChar` in `CashierManager` shows an abandoned attempt at a shorter alphanumeric code.

## Persistence — atomicity

`TransactionManager.SaveCompleteTransaction`:

```csharp
bool newTransaction = DBFactory.GetInstance().BeginTransaction();
try
{
    _trxDao.Save(transaction);                       // header first — generates Id
    foreach (var tDetail in transactionDetails)
    {
        tDetail.TransactionId = transaction.Id;      // stamp the FK
        _tdManager.Save(tDetail);
    }
    if (newTransaction) DBFactory.GetInstance().CommitTransaction();
}
catch (Exception e)
{
    if (newTransaction) DBFactory.GetInstance().RollbackTransaction();
    throw;
}
```

The header must be inserted first because its identity becomes each line's `TransactionId`. The
whole thing commits or rolls back as one unit — **a sale never lands as a header with missing
lines.**

**A failed line can no longer vanish.** `DBUtility.ExecuteNonQuery` used to swallow SQL exceptions
and return `-1`, so `SaveCompleteTransaction` carried on to the next line and committed — a detail
row that failed for a data reason (an apostrophe breaking the generated SQL, say) was silently
dropped while the sale still reported success. Writes now throw, the transaction rolls back, and the
cashier is told the sale failed. Values are parameterised, so the apostrophe case does not arise in
the first place.

## Outcome

`CashierManager.Checkout` returns a `TransactionStatus` plus an `out string message`:

| Status | When | `message` |
|---|---|---|
| `SUCCESS` | Saved, printed | empty |
| `SUCCESS` | Saved, **print failed** | *"Transaksi berhasil namun gagal mencetak. Pastikan printer terhubung dan cetak laporan melalui menu."* |
| `FAILED` | Save threw | *"Gagal menyimpan transaksi. Silahkan coba lagi."* |
| `INITIATE` | — | initial value; never returned |

**A printer failure never fails a sale.** The money is taken and the record is written; the operator
is told to reprint from the menu. This is the right trade-off for a shop — losing the sale record
because the paper ran out would be worse.

On `SUCCESS` the controller builds *"Transaksi Berhasil. Kembalian Rp {change}"*, appends any print
warning, and calls `NewCart()` to clear both the cart and the grid. On `FAILED` **the cart is left
intact** so the cashier can retry without re-scanning.

An unexpected exception is logged in full and reported as *"Transaksi gagal karena kesalahan
sistem. Silahkan coba lagi."*. It previously returned `e.Message + StackTrace` straight into a
message box.

## After the sale

`_lastFactur` records the faktur **in memory** for the *Print Last Receipt* menu item. It is not
persisted, so after a restart that menu reports *"Transaksi terakhir tidak ada"* and the operator
must use *Print Ulang Transaksi*, which searches by date range.

## Notes for change

- `customerId` is hardcoded to `1` in both `CashierController` and `TransactionUpdateController`.
  Finishing the customer feature means threading a real id through both, plus a customer picker,
  plus fixing the `MemberType`/`Type` column mismatch documented in
  [business-data-model.md](business-data-model.md).
- There is no cash-drawer, no rounding step and no tax line. Adding any of them belongs in
  `GenerateTransactionAndDetails`, next to the existing totals — not in the view.
- `Notes` is `varchar(100)`; `TrimNotes` truncates and logs rather than letting the insert fail.

## Payment methods

A sale is paid one of three ways. The method is chosen **first** — it is the top field in the payment
column — because it decides what the rest of them mean.

| | Cash | EDC (card) | QRIS |
|---|---|---|---|
| Amount | Cashier types what was handed over | The total, always | The total, always |
| Change | `tendered − total`, must not be negative | Always zero | Always zero |
| Extra input | — | Which terminal | Which provider, and Statis or Dinamis |
| Stored | `PaymentMethod = CASH` | `EDC` + `PaymentReference` | `QRIS` + `PaymentReference` + `PaymentVariant` |

Default is **Tunai**. Shortcuts: **Ctrl+1** cash, **Ctrl+2** EDC, **Ctrl+3** QRIS, handled in
`MainForm_KeyUp` alongside F5/F6/F7 and only while the cashier page is showing. A shortcut for a
method that is not on offer says why rather than doing nothing.

`Business/PaymentDetail.cs` carries method, amount, reference and variant as one object rather than
four more parameters on `Checkout`, and owns the change rule (`ChangeFor`) — zero for anything that
takes the exact total. `PaymentDetail.Parse` reads a stored code, treating anything unrecognised
(including the blank on sales that predate this) as cash.

**Terminals and providers are configured, not typed.** `EDC_TERMINALS` and `QRIS_PROVIDERS` each hold
one name per line, edited under *Pengaturan → Pembayaran* (requires `Master`). A method whose list is
empty is **left out of the selector entirely** — choosing it could never lead to a completed sale.
The chosen name is **re-checked against the list at checkout**, not merely taken from the screen: the
list can be edited while a sale is being rung up, and a payment must never be recorded against a
terminal or provider the shop has dropped.

Receipts print the method in place of *Tunai* / *Kembalian*, which would otherwise always read zero —
`EDC` with its terminal, or `QRIS` with its provider and code type.

⚠ A correction keeps the method, reference and variant of the sale it replaces. Correcting a QRIS
sale does not turn it into a cash one.

⚠ **Sales made before payment methods existed have none and are treated as cash**, which they were.
The migration backfills `'CASH'`, and the daily-takings query also treats a missing method as cash,
so a database where the backfill did not run still reports correctly.

### Why the daily total is split

*Jumlah Setoran* reports cash, EDC and QRIS separately, listing only the non-zero ones and falling
back to a single figure when everything was cash. Only the cash is money the cashier physically hands
over at the end of the day — card and QRIS takings settle through the bank — so a single combined
figure would tell them to hand over more than they hold.

# Transaction Revision and Cancellation

Sales are **immutable**. A mistake is corrected by writing a *new* transaction and marking the old
one superseded; a void marks the old one cancelled. No sale row is ever updated in place except its
`Revision` column, and none is ever deleted.

## The `Revision` column

`T_TRANSACTIONS.Revision bigint` is a small state machine:

| Value | State | Meaning |
|---|---|---|
| `0` | **Active** | The live version. The only state any report sees. |
| `> 0` | **Superseded** | Corrected. The value is the `Id` of the replacement transaction. |
| `-1` | **Cancelled** | Voided. No replacement. |

Added by `DBUtility.UpdateTableTransaction()` on upgrade, backfilled to `0` for existing rows.

**Every report and the transaction browser filter `WHERE t.Revision = 0.`** That single predicate is
what makes revisions and cancellations invisible to totals while keeping the full history on disk.

A correction chain is followable in both directions: the old row points forward via `Revision`, and
the new row's `Notes` names the old faktur.

## Revising a sale

Menu: *Ubah Transaksi*.

```
MainFormController.RequestUpdateTransaction()
    ├─ AccessOption.Master required
    │    └─ if the signed-in user lacks it → AuthenticationForm (supervisor step-up)
    ├─ TransactionHistory  → operator picks a transaction, returns its Factur
    └─ TransactionUpdateForm(factur, supervisor).ShowDialog()
         └─ TransactionUpdatePage.Init(factur, supervisor)
              └─ TransactionUpdateController.Init(...)
```

`Init` loads the original header and its lines. `Reset()` then rebuilds the cart from them —
`ResetByTransaction()` builds a `ProductId → Quantity` map and replays it through `UpdateCart` — and
prefills the payment box with the original payment. The operator now edits a cart that looks exactly
like the original sale.

⚠ `GetItems()` here calls `MasterManager.GetAllProduct()`, which **includes soft-deleted products**,
so a line referencing a since-deleted item still reloads. The regular sale screen uses
`GetAllAvailable()` and excludes them.

⚠ Replaying through `UpdateCart` re-reads each product's **current** price and discount. A revision
therefore reprices the whole basket at today's prices, not the prices originally charged. Deliberate
or not, that is the behaviour.

Checkout runs the same three validation rules as a normal sale, then:

```csharp
CashierManager.UpdateCheckout(original, payment, notes, supervisor.Id, customerId: 1)
```

which prefixes the notes and delegates to the atomic writer:

```csharp
notes = $"Ralat Dari Transaksi: {original.Id}, No Faktur: {original.Factur}." + notes;
```

`TransactionManager.UpdateCompleteTransaction`, all in one database transaction:

1. Insert the **new** transaction — it gets a fresh `Id` and a fresh `Factur`, `Revision = 0`.
2. Set `original.Revision = transaction.Id` and update the original.
3. Insert the new detail rows against the new `Id`.

Then it prints a receipt for the corrected sale.

Consequences:

- The revision is **attributed to the supervisor**, not the original cashier — so per-cashier
  reports move the sale to whoever approved the correction.
- The original's detail rows stay on disk, orphaned from every report but still linked to their
  header.
- The new transaction is itself revisable, forming a chain.
- ⚠ `Notes` is `varchar(100)` and the prefix consumes roughly half of it, leaving little room for
  the operator's own explanation. It is not truncated before insert.

## Cancelling a sale

Menu: *Hapus Transaksi* ("delete"), though nothing is deleted.

```
MainFormController.RequestDeleteTransaction()
    ├─ AccessOption.Master required (same step-up)
    ├─ TransactionHistory → pick a transaction
    └─ CashierManager.CancelTransaction(factur, supervisor.Id)
         └─ TransactionManager.CancelTransaction(original, cancelledByUserId)
              ├─ original.Revision = -1;  _trxDao.Update(original);
              └─ RecordCancellationAudit(id, cancelledByUserId)      (both in one transaction)
```

Rows stay; the sale simply stops counting.

On success `MainForm` shows *"Transaksi dihapus."*, then returns to the cashier page. Exceptions are
logged and reported as *"Terdapat kesalahan sistem. Tolong check kembali."*. Cancelling a faktur that
cannot be found now throws rather than silently doing nothing.

**Cancellations are attributed.** `T_TRANSACTIONS.CancelledBy` and `CancelledAt` record who approved
the void and when. Written by a targeted `UPDATE` outside the column map and tolerated if it fails,
so an installation that has not yet picked up those columns can still void a sale — see
[business-data-model.md](business-data-model.md). Rows voided by older builds keep NULL.

⚠ Still no reason field, and nothing displays the audit columns yet — they are write-only, readable
with SQL.

⚠ **Cancellation is not reversible through the UI.** Setting `Revision = -1` is one-way; restoring a
sale needs direct SQL.

⚠ **Daily totals shift retroactively.** Cancelling or revising a sale changes the answer to "what
did we take today" for a day already closed. `MainForm.jumlahSetoranToolStripMenuItem_Click` warns
about exactly this: *"Jika terdapat perubahan transaksi, Jumlah kemungkinan tidak sesuai."*

## Cart isolation

The sale screen and the correction screen each own a `Cart` instance, created in their controller's
constructor.

This used to be a single `_cart` on the `CashierManager` singleton, which meant opening the
correction window replaced whatever the cashier was ringing up, both screens reacted to every
change, and the second `CartChange` subscription leaked whenever the window was closed with the
title-bar **X** (only the Back button unsubscribed). `Unload()` and the controller finalizer that
existed to paper over that are gone — there is nothing shared left to release.

## Quick reference

| Question | Answer |
|---|---|
| How do I list only live sales? | `WHERE Revision = 0` |
| How do I find what replaced a sale? | Its `Revision` value is the replacement's `Id` |
| How do I find what a sale replaced? | Parse the `Notes` prefix, or search `Revision = <thisId>` |
| Is history preserved? | Yes — nothing is deleted |
| Who approved a correction? | The new transaction's `UserId` |
| Who cancelled a sale? | `CancelledBy` / `CancelledAt` (NULL for voids made by older builds) |

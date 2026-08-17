# Receipt Printing

## Design

Receipt rendering is a **static pure function**, in its own class `Business/ReceiptBuilder.cs`:

```csharp
public static List<StringPrint> Build(
    string headerNotes, string footerNotes,
    Transaction transaction, List<TransactionDetail> transactionDetails, string cashierName)
```

It takes data and returns lines. No database, no printer, no side effects. That is what lets the
settings preview (`HeaderAndFooterController.GetExample`) render a fake transaction through the
*same* code the printer uses — **the preview and the printout can never diverge.** Keep it that way.

It takes a cashier **name** rather than a `User`, so it needs nothing it cannot be handed directly.
It previously lived on `CashierManager`, which also owned the cart, checkout and cancellation.

Around it:

- `CashierManager.PrintPaymentNote` — resolves the cashier by `UserId`, fetches the header/footer
  settings, calls the pure function, hands the result to `PrinterUtility.Print`.
- `SimpleCommon.Utility.PrinterUtility` — the physical print, over `System.Drawing.Printing`.

## Layout

```
        FIDELIS CAKE AND BAKERY           ← header lines, centred
        JL MAYJEND SUTOYO NO 1
        BANJARNEGARA
        (0286) 594573
=================================
TANGGAL : 17-08-2026 14:32
FACTUR  : 638912345678901234
KASIR   : Budi
=================================
Kue Coklat
2 x Rp.15,000.00 = 30,000.00
Discount: Rp.1,000.00              ← only when ProductDiscount > 0
Roti Tawar
1 x Rp.8,000.00 = 8,000.00
=================================
Total Item   : Rp. 38,000.00
Total Disc   : Rp. 1,000.00
Total Belanja: Rp. 37,000.00

Tunai        : Rp. 50,000.00
Kembalian    : Rp. 13,000.00

        TERIMA KASIH                      ← footer lines, centred
        SELAMAT MENIKMATI
```

- Header and footer are split on `Environment.NewLine`; each line is centred.
- Everything between the separators is left-aligned.
- Separator: 33 `=` characters — sized for an 80 mm roll at `Courier New 9pt`.
- Amounts use `ToString("N")`, dates `dd-MM-yyyy HH:mm`. Both depend on the `en-US` culture pin.
- A line's discount row appears only when the product carried one.

## Printer configuration

`PrinterUtility` → `PrintObject.Print()`:

| Setting | Value |
|---|---|
| Printer | `ConfigurationManager.AppSettings["PrinterName"]` from `App.config` |
| Paper | `new PaperSize("Receipt", 265, 10000)` — ~2.65 in wide, effectively unbounded length |
| Margins | `new Margins(0, 0, 0, 0)` |
| Font | `Courier New` 9pt, fixed-width so the column alignment holds |

`App.config` currently selects `Microsoft Print to PDF`; the commented alternative
`EPSON TM-U220 Receipt` is the real thermal printer. **Switching printers is a config change, not a
code change.**

`pd_PrintPage` computes lines per page from the font height, draws each line into a `RectangleF`
with its `StringFormat`, and sets `HasMorePages` while lines remain — so one receipt is one long
page in practice.

## Failure handling

**A printing failure never fails a sale.** `CashierManager.Checkout` wraps `PrintPaymentNote` in its
own `try`/`catch`:

```csharp
catch (Exception e)
{
    _log.Error(e);
    message = "Transaksi berhasil namun gagal mencetak. "
            + "Pastikan printer terhubung dan cetak laporan melalui menu.";
}
```

The status stays `SUCCESS`, the money is taken, the record is written, and the operator is told to
reprint from the menu. Losing the sale record because the paper ran out would be the worse outcome.

**A receipt is always produced.** `CashierManager.ResolveCashierName` returns the built-in recovery
account's name for `UserId = -1`, the stored `Name` for a real user, and falls back to `"ADMIN"` with
a warning if the lookup fails. `PrintPaymentNote` previously returned **silently** when the user
lookup came back `null`, which is exactly what happens for the recovery account — so every sale made
under it produced no receipt and no error.

## Reprinting

| Menu | Path | Source of the faktur |
|---|---|---|
| *Print Last Receipt* | `MainFormController.PrintLastReceipt` | `CashierManager.GetLastFactur()` — **in-memory**, lost on restart |
| *Print Ulang Transaksi* | `MainFormController.PrintReceipt` | `TransactionHistory` date-range picker |

Both reload the header and lines from the database and re-render, so a reprint reflects the stored
sale — including the prices as charged. `_lastFactur` being in-memory is why the first option
reports *"Transaksi terakhir tidak ada"* after a restart.

⚠ Reprints are **not marked as copies.** A reprint is byte-identical to the original, so there is no
way to tell a duplicate receipt from the first one.

⚠ Reprinting uses the **current** header/footer settings, so a receipt reprinted after a settings
change will not match the original.

## Header and footer

Stored in `M_SETTINGS` as keys `HEADER` and `FOOTER`, group `GENERAL`, with newlines encoded as
`%NEW_LINE%`. Edited through *Pengaturan → Header and Footer*. See
[business-settings.md](business-settings.md).

## Notes for change

- Layout changes belong in `ReceiptBuilder.Build` — nowhere else. Keep it static and free of I/O.
- `StringPrint.Format` returns a fresh default `StringFormat` when unset, so callers never need a
  null check.
- Alignment is lost in the settings preview (a plain text box), which shows content only.
- The separator width, paper width and font are three coupled constants. Changing the roll size
  means changing all three.
- Everything here uses `System.Drawing.Printing` from the framework — no printing library, no
  external dependency.

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

Both the printer and the paper width are **settings**, edited under *Pengaturan → Printer*
(administrators only). They were an `App.config` entry and a hardcoded constant respectively.

| Setting | Key | Notes |
|---|---|---|
| Printer | `PRINTER_NAME` | Empty means the Windows default printer. Seeded from the old `App.config` entry, so an upgrade keeps printing where it always did. |
| Paper width | `PRINTER_PAPER_WIDTH_MM` | Millimetres — what an operator reads off the roll. Default 67. |
| Margins | — | `Margins(0, 0, 0, 0)` |
| Font | — | `Courier New` 9pt, fixed-width so the column alignment holds |
| Page height | — | 10000 units, i.e. effectively unbounded: a receipt is one long page |

`PrintSettings` converts millimetres to the hundredths of an inch `PaperSize` expects. The default of
67 mm reproduces the 265 units the previous build hardcoded, to within a quarter of a millimetre.

`PrinterUtility` no longer reads configuration — the caller passes a `PrintSettings`, which removes
the hidden dependency the library had on the hosting application's `App.config`.

### The printer settings page

- Dropdown of installed printers, plus an explicit *Windows default* entry. A configured printer
  that is not installed on this machine stays listed and is flagged, rather than being silently
  swapped.
- Paper width in mm, with 58 mm and 80 mm shortcuts for the common rolls.
- **Test print** renders a sample receipt through `ReceiptBuilder` using the values *currently on
  screen*, not the saved ones, so a width can be tried before committing to it. It appends the width
  in use, so a line that wraps or runs off the paper says plainly that the setting is wrong.
- A text preview of the sample, shown in the receipt font.

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

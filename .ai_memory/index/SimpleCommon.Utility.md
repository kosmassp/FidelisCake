# Directory: `SimpleCommon/Utility/`

Namespace `SimpleCommon.Utility`. Stateless helpers used across both projects.

---

## `HashUtility.cs`

`public class HashUtility` — the **legacy** password hash.

| Member | Signature | Purpose |
|---|---|---|
| `GetEncryptedPass` | `static string GetEncryptedPass(string password)` | One unsalted round of SHA-512 over the UTF-8 bytes, rendered by `BitConverter.ToString` — uppercase hex with `-` separators, 191 characters. |

**Kept only so that passwords already stored in deployed databases still verify.** Do not use it for
new passwords — `PasswordHasher` writes the current format and quietly replaces legacy hashes on the
next successful sign-in. The `SHA512` instance is now disposed.

---

## `PasswordHasher.cs`

`public static class PasswordHasher` — password hashing with a per-user salt, plus verification of
the legacy format.

Written format: `PBKDF2$<iterations>$<saltBase64>$<hashBase64>` — 83 characters, well inside
`M_USERS.Password varchar(256)`.

| Member | Signature | Purpose |
|---|---|---|
| `Hash` | `static string Hash(string password)` | 16-byte random salt from `RNGCryptoServiceProvider`, 32-byte derived key. |
| `Verify` | `static bool Verify(string password, string storedHash)` | Accepts **either** format. Returns `false` rather than throwing on null, empty or malformed input. |
| `NeedsUpgrade` | `static bool NeedsUpgrade(string storedHash)` | True while a stored hash is still legacy. |
| `Derive` | `private static byte[] (…)` | `Rfc2898DeriveBytes` — PBKDF2-HMAC-SHA1, the only variant .NET Framework 4.6 offers. |
| `FixedTimeEquals` | `private static bool (byte[], byte[])` | Comparison whose duration does not depend on where the first difference is. |

**Cost:** 25,000 iterations — about 175 ms on a modern desktop, a few hundred on the older shop
machines. Below common public guidance, which targets internet-facing services; here the database is
local to one till and the cost is paid on every supervisor approval too. The iteration count is
stored inside each hash, so raising it later is safe and self-migrating.

Verified behaviour: legacy hashes verify and are flagged for upgrade; the same password produces
different stored values for different users; null, empty and malformed input return `false`.

---

## `PrinterUtility.cs`

Receipt printing over `System.Drawing.Printing`. Three types in one file.

### `public class PrintSettings`

Where and how a receipt is printed, **passed in rather than read from configuration** — the library
no longer depends on the hosting application's `App.config`.

| Member | Purpose |
|---|---|
| `PrinterName` | Windows printer name. Empty or null uses the Windows default. |
| `PaperWidthMm` | Printable width in millimetres — what an operator reads off the roll. |
| `PaperWidthUnits` | The same in the hundredths of an inch `PaperSize` expects. |
| `MillimetresToUnits` / `UnitsToMillimetres` | `static int (int)` conversions. |

### `public class PrinterUtility`

| Member | Signature | Purpose |
|---|---|---|
| `Print` | `static void Print(List<StringPrint> textToPrint, Font font, PrintSettings settings)` | Wraps the lines in a `PrintObject` and prints. |
| `GetInstalledPrinters` | `static List<string> ()` | Printers installed on this machine, for the settings dropdown. |
| `GetDefaultPrinterName` | `static string ()` | What Windows would print to when none is chosen. |
| `IsPrinterAvailable` | `static bool (string printerName)` | Whether a printer exists and reports itself usable, so the operator gets a message rather than an exception. |

### `public class StringPrint`

One printable line: text plus alignment.

| Member | Signature | Purpose |
|---|---|---|
| `Text` | `string { get; set; }` | Line content. |
| `Format` | `StringFormat { get; set; }` | Alignment. **The getter returns a new default `StringFormat` when unset** rather than `null`, so callers never null-check. |
| *(ctors)* | `StringPrint(string text)` / `StringPrint(string text, StringFormat format)` | Left-aligned by default, or explicit. |

### `internal class PrintObject`

| Member | Signature | Purpose |
|---|---|---|
| *(ctor)* | `PrintObject(List<StringPrint> textToPrint, Font font)` | Stores the lines and font, takes a `List<T>.Enumerator`. |
| `Print` | `void Print()` | Builds a `PrintDocument` from the supplied `PrintSettings` — paper size `"Receipt"` at the configured width × 10000, zero margins — subscribes `pd_PrintPage`, prints, and disposes the document. Leaves the printer unset when the name is empty, so Windows picks. |
| `pd_PrintPage` | `private void (object, PrintPageEventArgs)` | Computes lines per page from the font height, draws each line into a `RectangleF` with its `StringFormat`, and sets `HasMorePages` while lines remain. |

Paper is 265 units wide (~2.65 in, matching an 80 mm thermal roll) and effectively unbounded in
length, so one receipt is one long page.

⚠ Note: the enumerator is a **struct** copied into a field; iteration state advances correctly here
because `pd_PrintPage` mutates the field directly, but a `PrintObject` can only be printed once.

⚠ Note: an invalid or offline `PrinterName` throws from `printDoc.Print()`. `CashierManager.Checkout`
catches this and still reports the sale as successful with a warning — see
[../business-receipt-printing.md](../business-receipt-printing.md).

---

## `DelegateUtility.cs`

`public class DelegateUtility` — a family of delegate declarations used as `BeginInvoke` targets so
UI methods can marshal themselves onto the UI thread without declaring a delegate per method.

| Delegate | Shape |
|---|---|
| `VoidHandler` | `()` |
| `OneValueHandler<T>` | `(T)` |
| `TwoValueHandler<T1,T2>` | `(T1, T2)` |
| `ThreeValueHandler<T1,T2,T3>` | `(T1, T2, T3)` |
| `FourValueHandler<T1,T2,T3,T4>` | `(T1, T2, T3, T4)` |
| `OneValueArrayHandler<T>` | `(T[])` |
| `TwoValueArrayHandler<T1,T2>` | `(T1[], T2[])` |
| `ThreeValueArrayHandler<T1,T2,T3>` | `(T1[], T2[], T3[])` |
| `FourValueArrayHandler<T1,T2,T3,T4>` | `(T1[], T2[], T3[], T4[])` |

The pattern they support, used in every thread-marshalled UI method:

```csharp
public void UpdateTotal(decimal total)
{
    if (InvokeRequired)
    {
        this.BeginInvoke(new DelegateUtility.OneValueHandler<decimal>(UpdateTotal), total);
        return;
    }
    // ... real work, guaranteed on the UI thread ...
}
```

⚠ Note: these predate `System.Action<...>`, which is in the framework and covers every one of these
shapes (`Action`, `Action<T>`, `Action<T1,T2>`, `Action<T[]>`, …). New code can use `Action`
directly; this class is kept because the existing call sites name it.

---

## `ControlUtility.cs`

`public class ControlUtility`.

| Member | Signature | Purpose |
|---|---|---|
| `HideTabHeader` | `static void HideTabHeader(TabControl tabControl)` | Sets `Appearance = Normal`, `ItemSize = new Size(0, 1)`, `SizeMode = Fixed` so the tab strip collapses. |

Called once, from `MainForm`'s constructor — this is what turns the shell's `TabControl` into a
plain page container with programmatic navigation.

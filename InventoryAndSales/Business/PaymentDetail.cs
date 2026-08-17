using System;
using System.Globalization;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// How a sale was paid for.
  ///
  /// Deliberately in the Business namespace rather than Business.Enum - that segment already
  /// collides with System.Enum and there is no reason to make it worse.
  /// </summary>
  public enum PaymentMethod
  {
    /// <summary>Notes and coins. The customer may hand over more than the total and get change.</summary>
    Cash,

    /// <summary>Card through an EDC terminal. The exact total is taken, so there is no change.</summary>
    Edc,

    /// <summary>QRIS code. Also exact, and also needs to record who the code came from.</summary>
    Qris,
  }

  /// <summary>Whether a QRIS code is the shop's fixed sticker or one generated per sale.</summary>
  public enum QrisMode
  {
    /// <summary>A printed code at the till. The customer types the amount in themselves.</summary>
    Static,

    /// <summary>A code generated for this sale, already carrying the amount.</summary>
    Dynamic,
  }

  /// <summary>
  /// The payment for one sale: the method, what was handed over, and whatever the method needs to
  /// identify itself - a terminal for a card, a provider and a code type for QRIS.
  ///
  /// Passed as one object rather than four more parameters on Checkout.
  /// </summary>
  public class PaymentDetail
  {
    /// <summary>Stored in T_TRANSACTIONS.PaymentMethod. Sales made before this existed read as CASH.</summary>
    public const string CashCode = "CASH";
    public const string EdcCode = "EDC";
    public const string QrisCode = "QRIS";

    /// <summary>Stored in T_TRANSACTIONS.PaymentVariant for QRIS.</summary>
    public const string StaticCode = "STATIC";
    public const string DynamicCode = "DYNAMIC";

    public PaymentMethod Method { get; private set; }

    /// <summary>
    /// Amount handed over. For anything but cash this is the total itself - a terminal or a QRIS
    /// code takes the exact amount, so there is nothing to give back.
    /// </summary>
    public decimal AmountTendered { get; private set; }

    /// <summary>EDC terminal, or QRIS provider. Empty for cash.</summary>
    public string Reference { get; private set; }

    /// <summary>STATIC or DYNAMIC for QRIS. Empty otherwise.</summary>
    public string Variant { get; private set; }

    private PaymentDetail(PaymentMethod method, decimal amountTendered, string reference, string variant)
    {
      Method = method;
      AmountTendered = amountTendered;
      Reference = reference ?? string.Empty;
      Variant = variant ?? string.Empty;
    }

    public static PaymentDetail Cash(decimal amountTendered)
    {
      return new PaymentDetail(PaymentMethod.Cash, amountTendered, string.Empty, string.Empty);
    }

    /// <summary>A card payment. The amount is the sale total, because that is what the terminal charges.</summary>
    public static PaymentDetail Edc(decimal total, string terminal)
    {
      return new PaymentDetail(PaymentMethod.Edc, total, terminal, string.Empty);
    }

    /// <summary>A QRIS payment, recording which provider issued the code and what kind it was.</summary>
    public static PaymentDetail Qris(decimal total, string provider, QrisMode mode)
    {
      return new PaymentDetail(PaymentMethod.Qris, total, provider,
                               mode == QrisMode.Dynamic ? DynamicCode : StaticCode);
    }

    /// <summary>Rebuilds a payment from what was stored, for correcting an existing sale.</summary>
    public static PaymentDetail FromStored(string methodCode, string reference, string variant, decimal amountTendered)
    {
      switch (Parse(methodCode))
      {
        case PaymentMethod.Edc:
          return new PaymentDetail(PaymentMethod.Edc, amountTendered, reference, string.Empty);
        case PaymentMethod.Qris:
          return new PaymentDetail(PaymentMethod.Qris, amountTendered, reference, variant);
        default:
          return new PaymentDetail(PaymentMethod.Cash, amountTendered, string.Empty, string.Empty);
      }
    }

    /// <summary>
    /// Reads a stored method code. Anything unrecognised - including the empty value on sales that
    /// predate payment methods - is cash, which is what those sales were.
    /// </summary>
    public static PaymentMethod Parse(string methodCode)
    {
      if (string.Equals(methodCode, EdcCode, StringComparison.OrdinalIgnoreCase))
        return PaymentMethod.Edc;
      if (string.Equals(methodCode, QrisCode, StringComparison.OrdinalIgnoreCase))
        return PaymentMethod.Qris;
      return PaymentMethod.Cash;
    }

    public static bool IsEdc(string methodCode)
    {
      return Parse(methodCode) == PaymentMethod.Edc;
    }

    public static bool IsQris(string methodCode)
    {
      return Parse(methodCode) == PaymentMethod.Qris;
    }

    /// <summary>True for any method that takes the exact total, so no change is due.</summary>
    public static bool IsExactAmount(PaymentMethod method)
    {
      return method != PaymentMethod.Cash;
    }

    /// <summary>Value written to T_TRANSACTIONS.PaymentMethod.</summary>
    public string Code
    {
      get
      {
        switch (Method)
        {
          case PaymentMethod.Edc: return EdcCode;
          case PaymentMethod.Qris: return QrisCode;
          default: return CashCode;
        }
      }
    }

    /// <summary>What the operator calls it, for the receipt and the screen.</summary>
    public string DisplayName
    {
      get
      {
        switch (Method)
        {
          case PaymentMethod.Edc: return "EDC";
          case PaymentMethod.Qris: return "QRIS";
          default: return "TUNAI";
        }
      }
    }

    /// <summary>Change owed. Zero for anything that takes the exact total.</summary>
    public decimal ChangeFor(decimal total)
    {
      return IsExactAmount(Method) ? 0m : AmountTendered - total;
    }

    /// <summary>Indonesian label for a stored QRIS variant.</summary>
    public static string DescribeVariant(string variant)
    {
      if (string.Equals(variant, DynamicCode, StringComparison.OrdinalIgnoreCase))
        return "Dinamis";
      if (string.Equals(variant, StaticCode, StringComparison.OrdinalIgnoreCase))
        return "Statis";
      return string.Empty;
    }

    public override string ToString()
    {
      return string.Format(CultureInfo.InvariantCulture, "{0} {1}{2}{3}",
                           Code, AmountTendered,
                           string.IsNullOrEmpty(Reference) ? string.Empty : " via " + Reference,
                           string.IsNullOrEmpty(Variant) ? string.Empty : " (" + Variant + ")");
    }
  }
}

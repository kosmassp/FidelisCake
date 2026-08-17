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
  }

  /// <summary>
  /// The payment for one sale: the method, what was handed over, and which terminal took it.
  ///
  /// Passed as one object rather than three more parameters on Checkout, which was already carrying
  /// enough of them.
  /// </summary>
  public class PaymentDetail
  {
    /// <summary>Stored in T_TRANSACTIONS.PaymentMethod. Sales made before this existed read as CASH.</summary>
    public const string CashCode = "CASH";
    public const string EdcCode = "EDC";

    public PaymentMethod Method { get; private set; }

    /// <summary>
    /// Amount handed over. For a card payment this is the total itself - a terminal takes the exact
    /// amount, so there is nothing to give back.
    /// </summary>
    public decimal AmountTendered { get; private set; }

    /// <summary>Terminal the card was run through. Empty for cash.</summary>
    public string Reference { get; private set; }

    private PaymentDetail(PaymentMethod method, decimal amountTendered, string reference)
    {
      Method = method;
      AmountTendered = amountTendered;
      Reference = reference ?? string.Empty;
    }

    public static PaymentDetail Cash(decimal amountTendered)
    {
      return new PaymentDetail(PaymentMethod.Cash, amountTendered, string.Empty);
    }

    /// <summary>
    /// A card payment. The amount is the sale total, because that is what the terminal charges.
    /// </summary>
    public static PaymentDetail Edc(decimal total, string terminal)
    {
      return new PaymentDetail(PaymentMethod.Edc, total, terminal);
    }

    /// <summary>Rebuilds a payment from what was stored, for correcting an existing sale.</summary>
    public static PaymentDetail FromStored(string methodCode, string reference, decimal amountTendered)
    {
      return IsEdc(methodCode)
        ? new PaymentDetail(PaymentMethod.Edc, amountTendered, reference)
        : new PaymentDetail(PaymentMethod.Cash, amountTendered, string.Empty);
    }

    public static bool IsEdc(string methodCode)
    {
      return string.Equals(methodCode, EdcCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Value written to T_TRANSACTIONS.PaymentMethod.</summary>
    public string Code
    {
      get { return Method == PaymentMethod.Edc ? EdcCode : CashCode; }
    }

    /// <summary>What the operator calls it, for the receipt and the screen.</summary>
    public string DisplayName
    {
      get { return Method == PaymentMethod.Edc ? "EDC" : "TUNAI"; }
    }

    /// <summary>
    /// Change owed. Always zero for a card payment.
    /// </summary>
    public decimal ChangeFor(decimal total)
    {
      return Method == PaymentMethod.Edc ? 0m : AmountTendered - total;
    }

    public override string ToString()
    {
      return string.Format(CultureInfo.InvariantCulture, "{0} {1}{2}",
                           Code, AmountTendered,
                           string.IsNullOrEmpty(Reference) ? string.Empty : " via " + Reference);
    }
  }
}

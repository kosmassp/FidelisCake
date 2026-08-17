using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Business.Enum;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using SimpleCommon.Utility;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Turns a <see cref="Cart"/> into a persisted sale and a printed receipt, and handles correcting
  /// and voiding past sales.
  ///
  /// The cart itself is not held here - each screen owns its own - so this class is stateless apart
  /// from remembering the last faktur for the "reprint last receipt" menu item.
  /// </summary>
  public class CashierManager
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly TransactionManager _transactionManager;
    private readonly UserManager _userManager;
    private readonly SettingsService _settings;
    private readonly AuditService _audit;
    private readonly Font _printFont = ReceiptBuilder.CreateReceiptFont();

    public CashierManager(TransactionManager transactionManager, UserManager userManager, SettingsService settings,
                          AuditService audit)
    {
      _transactionManager = transactionManager;
      _userManager = userManager;
      _settings = settings;
      _audit = audit;
    }

    #region Receipt header and footer

    public string GetHeaderNote()
    {
      return _settings.GetMultiLine(SettingKeys.Header, string.Empty);
    }

    public string GetFooterNote()
    {
      return _settings.GetMultiLine(SettingKeys.Footer, string.Empty);
    }

    public void SetHeaderNote(string header)
    {
      _settings.SetMultiLine(SettingKeys.Header, header);
    }

    public void SetFooterNote(string footer)
    {
      _settings.SetMultiLine(SettingKeys.Footer, footer);
    }

    #endregion

    #region Checkout

    /// <summary>
    /// Persists the cart as a new sale and prints the receipt.
    ///
    /// A printing failure does not fail the sale: the money has been taken and the record written,
    /// so the status stays SUCCESS and <paramref name="message"/> tells the operator to reprint.
    /// </summary>
    public TransactionStatus Checkout(Cart cart, PaymentDetail payment, string notes, int userId, long customerId, out string message)
    {
      message = string.Empty;
      List<TransactionDetail> transactionDetails;
      Transaction transaction = GenerateTransactionAndDetails(cart, notes, payment, userId, customerId, out transactionDetails);

      try
      {
        _transactionManager.SaveCompleteTransaction(transaction, transactionDetails);
        _lastFactur = transaction.Factur;
      }
      catch (Exception e)
      {
        _log.Error("Failed to save transaction.", e);
        // A sale that could not be saved is exactly the event worth investigating, so it is recorded
        // too - the audit write is outside the failed transaction and survives its rollback.
        _audit.Record(AuditService.ActionCheckout, AuditService.EntitySale, transaction.Factur,
                      string.Format(CultureInfo.InvariantCulture, "GAGAL disimpan: {0}", e.Message));
        message = "Gagal menyimpan transaksi. Silahkan coba lagi.";
        return TransactionStatus.FAILED;
      }

      _log.InfoFormat("Sale {0} completed: total {1}, {2}, {3} lines.",
                      transaction.Factur, transaction.Total, transaction.PaymentMethod, transactionDetails.Count);
      _audit.Record(AuditService.ActionCheckout, AuditService.EntitySale, transaction.Factur, Describe(transaction, transactionDetails));

      try
      {
        PrintPaymentNote(transaction, transactionDetails);
      }
      catch (Exception e)
      {
        _log.Error(e);
        message = "Transaksi berhasil namun gagal mencetak. Pastikan printer terhubung dan cetak laporan melalui menu.";
      }
      return TransactionStatus.SUCCESS;
    }

    /// <summary>
    /// Records a correction: writes a new sale and marks the original as superseded by it.
    /// </summary>
    public void UpdateCheckout(Cart cart, Transaction originalTransaction, PaymentDetail payment, string notes, int userId, long customerId)
    {
      List<TransactionDetail> transactionDetails;
      notes = string.Format("Ralat Dari Transaksi: {0}, No Faktur: {1}.", originalTransaction.Id, originalTransaction.Factur) + notes;
      Transaction transaction = GenerateTransactionAndDetails(cart, notes, payment, userId, customerId, out transactionDetails);

      _transactionManager.UpdateCompleteTransaction(originalTransaction, transaction, transactionDetails);
      _lastFactur = transaction.Factur;

      _log.InfoFormat("Sale {0} revised as {1}.", originalTransaction.Factur, transaction.Factur);
      _audit.Record(AuditService.ActionRevise, AuditService.EntitySale, transaction.Factur,
                    string.Format(CultureInfo.InvariantCulture, "Ralat dari {0} (total {1}) menjadi {2}",
                                  originalTransaction.Factur, originalTransaction.Total,
                                  Describe(transaction, transactionDetails)));

      try
      {
        PrintPaymentNote(transaction, transactionDetails);
      }
      catch (Exception e)
      {
        _log.Error(e);
      }
    }

    /// <summary>Voids a sale. Nothing is deleted; it simply stops counting.</summary>
    public void CancelTransaction(string transactionFactur, int cancelledByUserId)
    {
      List<TransactionDetail> details;
      Transaction transaction = GetTransaction(transactionFactur, out details);
      if (transaction == null)
        throw new InvalidOperationException(string.Format("No transaction found for faktur {0}.", transactionFactur));
      _transactionManager.CancelTransaction(transaction, cancelledByUserId);

      _log.InfoFormat("Sale {0} cancelled by user {1}.", transactionFactur, cancelledByUserId);
      _audit.Record(AuditService.ActionCancel, AuditService.EntitySale, transactionFactur,
                    string.Format(CultureInfo.InvariantCulture, "Dibatalkan, total {0}, disetujui user {1}",
                                  transaction.Total, cancelledByUserId));
    }

    /// <summary>
    /// A sale as one readable line for the audit trail. Invariant formatting: the row is read later,
    /// possibly elsewhere, and must not depend on the reading machine's culture.
    /// </summary>
    private static string Describe(Transaction transaction, List<TransactionDetail> details)
    {
      return string.Format(CultureInfo.InvariantCulture,
                           "total={0}, diskon={1}, bayar={2}, kembali={3}, metode={4}{5}, baris={6}",
                           transaction.Total, transaction.TotalDiscount, transaction.Payment, transaction.Exchange,
                           transaction.PaymentMethod,
                           string.IsNullOrEmpty(transaction.PaymentReference) ? string.Empty : " (" + transaction.PaymentReference + ")",
                           details == null ? 0 : details.Count);
    }

    private Transaction GenerateTransactionAndDetails(Cart cart, string notes, PaymentDetail payment, int userId, long customerId,
                                                      out List<TransactionDetail> transactionDetails)
    {
      Transaction transaction = new Transaction();
      transaction.TotalPrice = 0;
      transaction.TotalDiscount = 0;
      transaction.Total = 0;
      transaction.Notes = TrimNotes(notes);
      transaction.Time = DateTime.Now;
      transaction.Factur = GenerateFactur();
      transaction.UserId = userId;
      transaction.CustomerId = customerId;
      transaction.PaymentMethod = payment.Code;
      transaction.PaymentReference = payment.Reference;
      transaction.PaymentVariant = payment.Variant;

      transactionDetails = cart.GetLines();
      foreach (TransactionDetail td in transactionDetails)
      {
        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
      }

      // A terminal or a QRIS code takes the exact total, so what was "tendered" is the total and
      // there is no change. Recording the total keeps the takings columns meaning the same thing
      // whichever way the customer paid.
      transaction.Payment = PaymentDetail.IsExactAmount(payment.Method) ? transaction.Total : payment.AmountTendered;
      transaction.Exchange = payment.ChangeFor(transaction.Total);
      return transaction;
    }

    /// <summary>
    /// T_TRANSACTIONS.Notes is varchar(100) and a correction already spends about half of that on
    /// its automatic prefix. Trim rather than let the insert fail.
    /// </summary>
    private const int NotesMaxLength = 100;

    private static string TrimNotes(string notes)
    {
      if (string.IsNullOrEmpty(notes) || notes.Length <= NotesMaxLength)
        return notes;
      _log.WarnFormat("Notes truncated from {0} to {1} characters.", notes.Length, NotesMaxLength);
      return notes.Substring(0, NotesMaxLength);
    }

    private string _lastFactur;

    /// <summary>Faktur of the last sale made in this session. Not persisted across restarts.</summary>
    public string GetLastFactur()
    {
      return _lastFactur;
    }

    private static string GenerateFactur()
    {
      return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
    }

    public Transaction GetTransaction(string facturNumber, out List<TransactionDetail> details)
    {
      return _transactionManager.GetTransaction(facturNumber, out details);
    }

    #endregion

    #region Printing

    public Font GetPrintFont()
    {
      return _printFont;
    }

    /// <summary>Printer and paper as configured for this installation.</summary>
    public PrintSettings GetPrintSettings()
    {
      return new PrintSettings(
        _settings.GetString(SettingKeys.PrinterName, string.Empty),
        _settings.GetInt(SettingKeys.PrinterPaperWidthMm, SettingKeys.DefaultPaperWidthMm));
    }

    public void PrintPaymentNote(Transaction transaction, List<TransactionDetail> transactionDetails)
    {
      List<StringPrint> stringToPrint = ReceiptBuilder.Build(
        GetHeaderNote(), GetFooterNote(), transaction, transactionDetails, ResolveCashierName(transaction.UserId));

      PrinterUtility.Print(stringToPrint, _printFont, GetPrintSettings());
    }

    /// <summary>
    /// Name to print as the cashier.
    ///
    /// The built-in recovery account has no M_USERS row, which used to make this method give up and
    /// return without printing anything - a sale made under it produced no receipt at all and no
    /// error. Fall back to a name instead so a receipt is always produced.
    /// </summary>
    private string ResolveCashierName(int userId)
    {
      if (LoginManager.IsBuiltInAdminId(userId))
        return LoginManager.BuiltInAdminDisplayName;

      try
      {
        User cashier = _userManager.FindById(userId);
        if (cashier != null && !string.IsNullOrEmpty(cashier.Name))
          return cashier.Name;
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not load cashier {0} for the receipt.", userId), e);
      }

      _log.WarnFormat("Printing receipt without a known cashier name for user {0}.", userId);
      return "ADMIN";
    }

    #endregion
  }
}

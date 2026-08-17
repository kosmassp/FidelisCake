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
    private readonly Font _printFont = ReceiptBuilder.CreateReceiptFont();

    public CashierManager(TransactionManager transactionManager, UserManager userManager, SettingsService settings)
    {
      _transactionManager = transactionManager;
      _userManager = userManager;
      _settings = settings;
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
        message = "Gagal menyimpan transaksi. Silahkan coba lagi.";
        return TransactionStatus.FAILED;
      }

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

      transactionDetails = cart.GetLines();
      foreach (TransactionDetail td in transactionDetails)
      {
        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
      }

      // A card terminal takes the exact total, so what was "tendered" is the total and there is no
      // change. Recording the total keeps the takings columns meaning the same thing for both.
      transaction.Payment = payment.Method == PaymentMethod.Edc ? transaction.Total : payment.AmountTendered;
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

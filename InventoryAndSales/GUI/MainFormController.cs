using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Database;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;
using InventoryAndSales.Enumeration;
using InventoryAndSales.GUI.Popup;
using InventoryAndSales.GUI.Util;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI
{
  public class MainFormController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly MainForm _mainForm;
    private readonly CashierManager _cashierManager;
    private readonly LoginManager _loginManager;
    private readonly ReportManager _reportManager;
    private readonly HeldCartService _heldCarts;
    private readonly ShopService _shop;
    private readonly AuditService _audit;

    public MainFormController(MainForm mainForm)
    {
      _mainForm = mainForm;
      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      _reportManager = BusinessFactory.GetInstance().ReportManager;
      _heldCarts = BusinessFactory.GetInstance().HeldCarts;
      _shop = BusinessFactory.GetInstance().Shop;
      _audit = BusinessFactory.GetInstance().Audit;

      _loginManager.OnActiveUserChanged += OnActiveUserChanged;
    }

    /// <summary>
    /// What this shop is called. Read on demand rather than cached, so the title picks up a rename
    /// as soon as the settings dialog is closed.
    /// </summary>
    public string GetShopName()
    {
      return _shop.GetName();
    }

    public void OnActiveUserChanged(object sender, User activeUser)
    {
      // Held baskets belong to whoever is signed in. Dropped on any change of user, so nothing
      // survives a logout and no cashier inherits another's holds.
      _heldCarts.Clear();

      if (activeUser == null)
      {
        _mainForm.UpdateActiveUser(string.Empty);
        _mainForm.EnableMenu(0);
        _mainForm.LoadLoginPage();
      }
      else
      {
        _mainForm.UpdateActiveUser(activeUser.Name);
        _mainForm.EnableMenu(activeUser.Role);
        _mainForm.LoadCashierPage();
      }
    }

    public void Logout()
    {
      _log.Info("Logout requested.");
      _heldCarts.Clear();
      _mainForm.EnableMenu(0);
      _mainForm.UpdateActiveUser("");
      _loginManager.Logout();
    }

    public bool PrintLastReceipt()
    {
      string facturNumber = _cashierManager.GetLastFactur();
      if (string.IsNullOrEmpty(facturNumber))
        return false;

      return ReprintByFactur(facturNumber);
    }

    /// <summary>
    /// Lets the operator pick a past sale and reprints it.
    /// </summary>
    /// <returns>False only when a chosen sale could not be reprinted; cancelling counts as success.</returns>
    public bool PrintReceipt()
    {
      using (TransactionHistory th = new TransactionHistory())
      {
        if (th.ShowDialog() != DialogResult.OK)
          return true;
        return ReprintByFactur(th.SelectedTransactionFactur);
      }
    }

    private bool ReprintByFactur(string facturNumber)
    {
      List<TransactionDetail> details;
      Transaction t = _cashierManager.GetTransaction(facturNumber, out details);
      if (t == null || details == null)
      {
        _log.WarnFormat("Reprint requested for faktur '{0}', which could not be loaded.", facturNumber);
        return false;
      }
      _log.InfoFormat("Reprinting receipt for faktur '{0}'.", facturNumber);
      _cashierManager.PrintPaymentNote(t, details);
      return true;
    }

    /// <summary>
    /// Confirms that the operation may go ahead, asking a supervisor to approve when the signed-in
    /// user does not hold the permission themselves.
    /// </summary>
    /// <returns>The approving user, or null when approval was refused or cancelled.</returns>
    private User RequirePermission(AccessOption required)
    {
      User activeUser = _loginManager.ActiveUser;
      if (activeUser == null)
        return null;

      if (BusinessUtil.AllowedRole(activeUser.Role, required))
        return activeUser;

      using (AuthenticationForm authenticationForm = new AuthenticationForm(required))
      {
        if (authenticationForm.ShowDialog() != DialogResult.OK)
        {
          _log.InfoFormat("Supervisor approval for {0} was refused or cancelled, requested by '{1}'.",
                          required, activeUser.Name);
          return null;
        }

        User approver = authenticationForm.AuthenticatedUser;
        // Somebody borrowing a supervisor's rights is exactly the kind of thing a shop asks about
        // later, so the approval is recorded against the approver, naming who asked.
        _audit.RecordAs(approver, AuditService.ActionLogin, AuditService.EntitySession,
                        approver == null ? string.Empty : approver.Username,
                        string.Format("Menyetujui akses {0} untuk '{1}'.", required, activeUser.Name));
        return approver;
      }
    }

    public void RequestUpdateTransaction()
    {
      User supervisor = RequirePermission(AccessOption.Master);
      if (supervisor == null)
        return;

      using (TransactionHistory th = new TransactionHistory())
      {
        if (th.ShowDialog() != DialogResult.OK)
          return;

        using (TransactionUpdateForm transactionUpdateForm =
               new TransactionUpdateForm(th.SelectedTransactionFactur, supervisor))
        {
          transactionUpdateForm.ShowDialog();
        }
      }
    }

    public bool RequestDeleteTransaction()
    {
      User supervisor = RequirePermission(AccessOption.Master);
      if (supervisor == null)
        return false;

      using (TransactionHistory th = new TransactionHistory())
      {
        if (th.ShowDialog() != DialogResult.OK)
          return false;

        // The approving supervisor is recorded against the cancelled sale.
        _cashierManager.CancelTransaction(th.SelectedTransactionFactur, supervisor.Id);
        return true;
      }
    }

    /// <summary>
    /// Today's takings for the signed-in cashier, cash and card shown separately.
    ///
    /// Only the cash is money that has to come out of the drawer, so a single combined figure would
    /// tell them to hand over more than they hold.
    /// </summary>
    public string GetCurrentDayTotalTransaction()
    {
      User activeUser = _loginManager.ActiveUser;
      if (activeUser == null)
        return "Rp. 0";

      CashierDayTotals totals = _reportManager.GetTodaySummaryByCashier(activeUser, DateTime.Today);
      if (totals.CashOnly)
        return "Rp. " + totals.Cash;

      string summary = "Tunai : Rp. " + totals.Cash;
      if (!totals.EdcIsZero)
        summary += Environment.NewLine + "EDC   : Rp. " + totals.Edc;
      if (!totals.QrisIsZero)
        summary += Environment.NewLine + "QRIS  : Rp. " + totals.Qris;
      return summary;
    }
  }
}

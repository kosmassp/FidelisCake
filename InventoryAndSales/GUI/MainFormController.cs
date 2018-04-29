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
    private MainForm _mainForm;
    private CashierManager _cashierManager;
    private LoginManager _loginManager;
    public MainFormController(MainForm mainForm)
    {
      _mainForm = mainForm;
      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;

      _loginManager.OnActiveUserChanged += OnActiveUserChanged;
    }

    public void OnActiveUserChanged(object sender, User activeUser)
    {
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
      _mainForm.EnableMenu(0);
      _mainForm.UpdateActiveUser("");
      _loginManager.Logout();
    }

    public bool PrintLastReceipt()
    {
      string facturNumber = _cashierManager.GetLastFactur();
      if (!string.IsNullOrEmpty(facturNumber))
      {
        List<TransactionDetail> details;
        Transaction t = _cashierManager.GetTransaction(facturNumber, out details);
        if (t != null && details != null)
        {
          _cashierManager.PrintPaymentNote(t, details);
          return true;
        }
      }
      return false;
    }

    public bool PrintReceipt()
    {
      TransactionHistory th = new TransactionHistory();
      DialogResult result = th.ShowDialog();
      if (result == DialogResult.OK)
      {
        List<TransactionDetail> details;
        Transaction t = _cashierManager.GetTransaction(th.SelectedTransactionFactur, out details);
        if (t != null && details != null)
        {
          _cashierManager.PrintPaymentNote(t, details);
          return true;
        }
        return false;
      }
      return true;
    }

    public void RequestUpdateTransaction()
    {
      if (_loginManager.ActiveUser != null)
      {
        User supervisor = _loginManager.ActiveUser;
        if (!BusinessUtil.AllowedRole(_loginManager.ActiveUser.Role, AccessOption.Master))
        {
          AuthenticationForm authenticationForm = new AuthenticationForm(AccessOption.Master);
          DialogResult dr = authenticationForm.ShowDialog();
          if(dr == DialogResult.Cancel)
          {
            return;
          }
          supervisor = authenticationForm.AuthenticatedUser;
        }
        TransactionHistory th = new TransactionHistory();
        DialogResult result = th.ShowDialog();
        if (result == DialogResult.OK)
        {
          TransactionUpdateForm transactionUpdateForm = new TransactionUpdateForm(th.SelectedTransactionFactur, supervisor);
          transactionUpdateForm.ShowDialog();
        }
      }
    }
  }
}

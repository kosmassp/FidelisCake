using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.Enumeration;
using InventoryAndSales.GUI.Util;

namespace InventoryAndSales.GUI.Popup
{
  public partial class AuthenticationForm : Form
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly LoginManager _loginManager;
    private readonly AccessOption _accessOption;
    private int _failed;
    public User AuthenticatedUser { get; private set; }
    public AuthenticationForm(AccessOption accessOption)
    {
      InitializeComponent();
      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _accessOption = accessOption;
      this.DialogResult = DialogResult.Cancel;
      _failed = 0;
    }

    private void buttonAuthenticate_Click(object sender, EventArgs e)
    {
      string username = textBoxUsername.Text;
      string password = textBoxPassword.Text;

      AuthenticatedUser = _loginManager.AuthenticateUsernamePassword(password, username);
      if (AuthenticatedUser != null && BusinessUtil.AllowedRole(AuthenticatedUser.Role, _accessOption))
      {
        this.DialogResult = DialogResult.OK;
        Close();
        return;
      }

      // Deliberately the same message whether the credentials were wrong or the account simply
      // lacks the permission, so it gives nothing away.
      AuthenticatedUser = null;
      _failed++;
      _log.WarnFormat("Rejected approval attempt {0} for '{1}' requiring {2}.", _failed, username, _accessOption);
      labelInvalidAuthentication.Text = string.Format("Akses Ditolak. (percobaan ke-{0})", _failed);
      textBoxPassword.Clear();
      textBoxPassword.Focus();
    }

    private void buttonBack_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}

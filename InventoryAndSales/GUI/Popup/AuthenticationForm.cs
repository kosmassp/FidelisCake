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
    private LoginManager _loginManager; //should not be in the UI form;
    private AccessOption _accessOption; //should not be in the UI form;
    private int _failed; //should not be in the UI form;
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
      if (AuthenticatedUser != null)
      {
        if (BusinessUtil.AllowedRole(AuthenticatedUser.Role, _accessOption))
        {
          this.DialogResult = DialogResult.OK;
          Close();
        } 
        else
        {
          labelInvalidAuthentication.Text = string.Format("Akses Ditolak", ++_failed);
        }
      } 
      else
      {
        labelInvalidAuthentication.Text = string.Format("Akses Ditolak.", ++_failed);
      }
    }

    private void buttonBack_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}

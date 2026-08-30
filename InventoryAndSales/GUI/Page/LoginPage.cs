using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller;

namespace InventoryAndSales.GUI.Page
{
  public partial class LoginPage : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private LoginController controller;
    public LoginPage()
    {
      InitializeComponent();
      controller = new LoginController(this);
    }

    public void Reset()
    {
      textBoxUsername.Text = string.Empty;
      textBoxPassword.Text = string.Empty;
      labelCannotLogin.Text = string.Empty;
      textBoxUsername.Focus();
    }
    private void buttonLogin_Click(object sender, EventArgs e)
    {
      // Verifying a password is deliberately slow, so show the operator that something is happening.
      Cursor previous = Cursor;
      Cursor = Cursors.WaitCursor;
      buttonLogin.Enabled = false;
      try
      {
        bool success = controller.Login(textBoxUsername.Text, textBoxPassword.Text);
        if (!success)
        {
          labelCannotLogin.Text = "Username atau password tidak benar";
          textBoxPassword.Clear();
          textBoxPassword.Focus();
        }
      }
      catch (Exception ex)
      {
        _log.Error("Login failed.", ex);
        labelCannotLogin.Text = "Tidak dapat menghubungi database. Silahkan coba lagi.";
      }
      finally
      {
        buttonLogin.Enabled = true;
        Cursor = previous;
      }
    }

    private void textBoxUsername_Enter(object sender, EventArgs e)
    {
      textBoxUsername.SelectAll();
    }

    private void textBoxPassword_Enter(object sender, EventArgs e)
    {
      textBoxPassword.SelectAll();
    }

    private void textBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        e.Handled = true;
        textBoxPassword.Focus();
      }
    }

    private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\r')
      {
        e.Handled = true;
        buttonLogin_Click(sender, null);
      }
    }

  }
}

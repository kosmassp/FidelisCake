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
      bool success = controller.Login(textBoxUsername.Text, textBoxPassword.Text);
      if (!success)
        labelCannotLogin.Text = "Username atau password tidak benar";
      //else
      //  tabControlPage.SelectedTab = tabPageCashier;
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

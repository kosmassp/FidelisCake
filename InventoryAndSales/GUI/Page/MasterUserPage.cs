using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Database.Model;
using InventoryAndSales.Enumeration;
using InventoryAndSales.GUI.Controller;

namespace InventoryAndSales.GUI.Page
{
  public partial class MasterUserPage : UserControl
  {
    private MasterUserController controller;
    public MasterUserPage()
    {
      InitializeComponent();
      controller = new MasterUserController(this);
      comboBoxRoleMaster.DataSource = Enum.GetValues(typeof (RoleOptions));
    }

    public void Reset()
    {
      OnEditMasterUser(false);
    }

    private User _currentUserSelection;
    private void buttonOkUserMaster_Click(object sender, EventArgs e)
    {
      string errorMsg = ValidateDetailUser();
      if (!string.IsNullOrEmpty(errorMsg))
      {
        MessageBox.Show(errorMsg);
        return;
      }

      string username;
      string name;
      string password;
      int role;
      GetUserDetail(out username, out name, out password, out role);
      if (isUpdatingUser)
      {
        controller.UpdateUser(_currentUserSelection, username, name, password, role);
        isUpdatingUser = false;
      }
      else if (isAddingUser)
      {
        controller.AddUser(username, name, password, role);
        isAddingUser = false;
      }
      OnEditMasterUser(false);
    }

    private void GetUserDetail(out string username, out string name, out string password, out int role)
    {
      username = textBoxUsernameMaster.Text;
      name = textBoxNameMaster.Text;
      password = textBoxPasswordMaster.Text;
      RoleOptions selectedRole = (RoleOptions)comboBoxRoleMaster.SelectedValue;
      role = (int)selectedRole;
    }

    private void buttonCancelUserMaster_Click(object sender, EventArgs e)
    {
      OnEditMasterUser(false);
      _currentUserSelection = null;
      UpdateSelectedUser();
    }

    private void UpdateSelectedUser()
    {
      DataGridViewSelectedCellCollection selectedCells = dataGridViewUserMaster.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewUserMaster.Rows[viewCell.RowIndex];
        var user = viewRow.DataBoundItem as User;
        if (user != null)
        {
          UpdateDetailUser(user);
        }
      }
    }

    private void UpdateDetailUser(User user)
    {
      if (_currentUserSelection == user)
        return;
      _currentUserSelection = user;
      textBoxUsernameMaster.Text = user.Username;
      string password = user.Password;
      if (!string.IsNullOrEmpty(password) && password.Length > 8)
      {
        password = password.Substring(0, 8);
      }
      textBoxPasswordMaster.Text = password;
      textBoxRePasswordMaster.Text = password;
      textBoxNameMaster.Text = user.Name;
      comboBoxRoleMaster.SelectedItem = (RoleOptions)user.Role;

    }


    private void buttonAddUserMaster_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditUser)
      {
        OnEditMasterUser(true);
        isAddingUser = true;
        ClearFieldUser();
        return;
      }
    }

    private void ClearFieldUser()
    {
      textBoxUsernameMaster.Text = string.Empty;
      textBoxNameMaster.Text = string.Empty;
      textBoxPasswordMaster.Text = string.Empty;
      textBoxRePasswordMaster.Text = string.Empty;
    }

    private void buttonEditUserMaster_Click(object sender, EventArgs e)
    {
      if (!isOnAddEditUser)
      {
        UpdateSelectedUser();
        OnEditMasterUser(true);
        isUpdatingUser = true;
        return;
      }
    }

    private void buttonDeleteUserMaster_Click(object sender, EventArgs e)
    {
      if (_currentUserSelection != null)
      {
        DialogResult dr = MessageBox.Show(
          string.Format("Apakah anda benar ingin menghapus User {0}", _currentUserSelection.Name),
          "Konfirmasi Hapus",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        if (dr == DialogResult.OK)
        {
          controller.DeleteUser(_currentUserSelection);
          OnEditMasterUser(false);
        }
      }
    }

    private void dataGridViewUserMaster_SelectionChanged(object sender, EventArgs e)
    {
      DataGridViewSelectedCellCollection selectedCells = dataGridViewUserMaster.SelectedCells;
      if (selectedCells.Count > 0)
      {
        DataGridViewCell viewCell = selectedCells[0];
        DataGridViewRow viewRow = dataGridViewUserMaster.Rows[viewCell.RowIndex];
        var user = viewRow.DataBoundItem as User;
        if (user != null)
        {
          UpdateDetailUser(user);
        }
      }
      else
      {
        ClearFieldUser();
      }

    }


    private bool isOnAddEditUser = false;
    private bool isUpdatingUser = false;
    private bool isAddingUser = false;

    private void OnEditMasterUser(bool edit)
    {
      isOnAddEditUser = edit;
      buttonAddUserMaster.Visible = !edit;
      buttonEditUserMaster.Visible = !edit;
      buttonDeleteUserMaster.Visible = !edit;
      buttonOkUserMaster.Visible = edit;
      buttonCancelUserMaster.Visible = edit;
      textBoxUsernameMaster.Enabled = edit;
      textBoxNameMaster.Enabled = edit;
      textBoxPasswordMaster.Enabled = edit;
      textBoxRePasswordMaster.Enabled = edit;
      comboBoxRoleMaster.Enabled = edit;

      dataGridViewUserMaster.Enabled = !edit;
      dataGridViewUserMaster.ForeColor = edit ? Color.Gray : Color.Black;

      if (!edit)
      {
        List<User> users = controller.GetUsers();
        dataGridViewUserMaster.DataSource = null;
        dataGridViewUserMaster.DataSource = users;
      }
    }



    private string ValidateDetailUser()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(string.IsNullOrEmpty(textBoxUsernameMaster.Text) ? "Harap isi username" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxNameMaster.Text) ? "Harap isi nama user" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxPasswordMaster.Text) ? "Harap isi password" : string.Empty);
      sb.AppendLine(string.IsNullOrEmpty(textBoxRePasswordMaster.Text) ? "Harap isi re-password " : string.Empty);
      sb.AppendLine(textBoxPasswordMaster.Text != textBoxRePasswordMaster.Text ? "Password tidak sesuai dengan re-password" : string.Empty);
      return sb.ToString().Trim();
    }

    private void dataGridViewUserMaster_Click(object sender, EventArgs e)
    {
      UpdateSelectedUser();
    }
  }
}

using System;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Controls whether the built-in recovery account may be used on this installation.
  /// </summary>
  public partial class SecuritySettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private SecuritySettingController _controller;
    private bool _loading;

    public SecuritySettingForm()
    {
      InitializeComponent();
    }

    private void SecuritySettingForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      _controller = new SecuritySettingController(this);

      _loading = true;
      try
      {
        checkBoxAllowBuiltInAdmin.Checked = _controller.IsBuiltInAdminAllowed();
      }
      finally
      {
        _loading = false;
      }

      labelDescription.Text =
        "Akun pemulihan (\"" + _controller.BuiltInAdminUsername + "\") selalu dapat login walaupun " +
        "semua user di daftar user terhapus. Akun ini disediakan karena admin kadang menghapus " +
        "akunnya sendiri sehingga tidak ada lagi yang bisa membuat user baru." +
        Environment.NewLine + Environment.NewLine +
        "Matikan pilihan ini bila Anda ingin hanya user pada Daftar User yang boleh login. " +
        "Pastikan masih ada user dengan hak Supervisor atau Admin sebelum mematikannya.";

      buttonSave.Enabled = false;
      RefreshWarning();
    }

    private void RefreshWarning()
    {
      if (_controller == null)
        return;

      if (!_controller.HasRealAdministrator())
      {
        labelWarning.Text =
          "Saat ini tidak ada user lain yang dapat mengelola sistem, sehingga akun pemulihan " +
          "tidak dapat dimatikan.";
        checkBoxAllowBuiltInAdmin.Enabled = _controller.IsBuiltInAdminAllowed() == false;
      }
      else
      {
        labelWarning.Text = string.Empty;
        checkBoxAllowBuiltInAdmin.Enabled = true;
      }
    }

    private void checkBoxAllowBuiltInAdmin_CheckedChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;
      buttonSave.Enabled = true;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        string problem = _controller.Save(checkBoxAllowBuiltInAdmin.Checked);
        if (!string.IsNullOrEmpty(problem))
        {
          MessageBox.Show(problem, "Belum Tersimpan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          _loading = true;
          try
          {
            checkBoxAllowBuiltInAdmin.Checked = _controller.IsBuiltInAdminAllowed();
          }
          finally
          {
            _loading = false;
          }
          return;
        }

        buttonSave.Enabled = false;
        RefreshWarning();
        MessageBox.Show("Pengaturan keamanan berhasil disimpan.", "BERHASIL",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save the security setting.", ex);
        MessageBox.Show("Pengaturan keamanan gagal disimpan.", "GAGAL",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

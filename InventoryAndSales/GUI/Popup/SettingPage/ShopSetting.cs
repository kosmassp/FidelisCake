using System;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Names the shop. The name titles the main window and heads every generated report, so the same
  /// build can run in more than one shop.
  /// </summary>
  public partial class ShopSettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private ShopSettingController _controller;
    private bool _loading;

    public ShopSettingForm()
    {
      InitializeComponent();
    }

    private void ShopSettingForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      _controller = new ShopSettingController(this);

      _loading = true;
      try
      {
        textBoxShopName.Text = _controller.GetName();
      }
      finally
      {
        _loading = false;
      }

      labelDescription.Text =
        "Nama toko dipakai pada judul jendela aplikasi dan pada bagian atas setiap laporan yang " +
        "dibuat." + Environment.NewLine + Environment.NewLine +
        "Nama ini terpisah dari teks header struk. Bila Anda juga ingin mengubah tulisan yang " +
        "tercetak di atas struk - termasuk alamat dan nomor telepon - ubah melalui halaman " +
        "pengaturan \"Nota\".";

      RefreshInheritedNote();
      buttonSave.Enabled = false;
    }

    /// <summary>
    /// Says plainly when the box was filled from the receipt header rather than from a saved name,
    /// so nobody wonders why a name they never typed here is showing.
    /// </summary>
    private void RefreshInheritedNote()
    {
      labelInherited.Text = _controller.IsNameInherited()
        ? "Saat ini nama diambil dari baris pertama header struk. Simpan untuk menetapkannya di sini."
        : string.Empty;
    }

    private void textBoxShopName_TextChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;
      buttonSave.Enabled = true;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      string problem = _controller.ValidateName(textBoxShopName.Text);
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Belum Tersimpan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        textBoxShopName.Focus();
        return;
      }

      try
      {
        string requested = textBoxShopName.Text.Trim();
        _controller.Save(requested);

        _loading = true;
        try
        {
          // Read back rather than trusted: a settings row that never got seeded is skipped by the
          // write with only a log line, and telling the operator it saved would be a lie.
          textBoxShopName.Text = _controller.GetName();
        }
        finally
        {
          _loading = false;
        }

        RefreshInheritedNote();

        if (!string.Equals(textBoxShopName.Text, requested, StringComparison.Ordinal))
        {
          _log.WarnFormat("Shop name '{0}' did not persist; it reads back as '{1}'.",
                          requested, textBoxShopName.Text);
          MessageBox.Show(
            "Nama toko tidak tersimpan. Silahkan coba lagi, dan hubungi administrator bila tetap gagal.",
            "GAGAL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        buttonSave.Enabled = false;
        MessageBox.Show(
          "Nama toko berhasil disimpan." + Environment.NewLine +
          "Judul jendela akan mengikuti setelah halaman pengaturan ditutup.",
          "BERHASIL", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save the shop name.", ex);
        MessageBox.Show("Nama toko gagal disimpan.", "GAGAL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

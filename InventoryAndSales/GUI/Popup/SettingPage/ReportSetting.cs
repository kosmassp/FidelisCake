using System;
using System.Drawing;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Controller.SettingPage;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Lets the operator choose where reports are written.
  ///
  /// Reports used to go to a hardcoded c:\temp\Report, with supporting files somebody had to unpack
  /// there by hand.
  /// </summary>
  public partial class ReportSettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private ReportSettingController _controller;
    private bool _loading;

    public ReportSettingForm()
    {
      InitializeComponent();
    }

    private void ReportSettingForm_Load(object sender, EventArgs e)
    {
      // The designer must not reach the database.
      if (DesignMode)
        return;

      _controller = new ReportSettingController(this);

      _loading = true;
      try
      {
        textBoxFolder.Text = _controller.GetReportDirectory();
      }
      finally
      {
        _loading = false;
      }

      buttonSave.Enabled = false;
      RefreshAssetStatus();
    }

    private void RefreshAssetStatus()
    {
      if (_controller == null)
        return;

      if (!_controller.IsAssetSourcePresent())
      {
        labelAssetStatus.ForeColor = Color.Firebrick;
        labelAssetStatus.Text =
          "Folder '" + ReportService.AssetSourceFolderName + "' tidak ditemukan di folder aplikasi. " +
          "Laporan tetap dapat dibuat, namun tanpa fitur urut, cari dan export.";
        return;
      }

      if (_controller.AreAssetsReady())
      {
        labelAssetStatus.ForeColor = Color.ForestGreen;
        labelAssetStatus.Text = "File pendukung laporan sudah siap.";
      }
      else
      {
        labelAssetStatus.ForeColor = Color.Firebrick;
        labelAssetStatus.Text = "File pendukung laporan belum dapat disiapkan di folder tersebut.";
      }
    }

    private void textBoxFolder_TextChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;
      buttonSave.Enabled = true;
    }

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
      using (FolderBrowserDialog dialog = new FolderBrowserDialog())
      {
        dialog.Description = "Pilih folder untuk menyimpan laporan";
        dialog.ShowNewFolderButton = true;
        try
        {
          if (System.IO.Directory.Exists(textBoxFolder.Text))
            dialog.SelectedPath = textBoxFolder.Text;
        }
        catch (Exception ex)
        {
          _log.Warn("Could not preselect the current report folder.", ex);
        }

        if (dialog.ShowDialog() == DialogResult.OK)
          textBoxFolder.Text = dialog.SelectedPath;
      }
    }

    private void buttonDefault_Click(object sender, EventArgs e)
    {
      textBoxFolder.Text = _controller.GetDefaultReportDirectory();
    }

    private void buttonOpenFolder_Click(object sender, EventArgs e)
    {
      try
      {
        _controller.OpenReportFolder();
      }
      catch (Exception ex)
      {
        _log.Error("Could not open the report folder.", ex);
        MessageBox.Show("Folder laporan tidak dapat dibuka.", "Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      Cursor previous = Cursor;
      Cursor = Cursors.WaitCursor;
      try
      {
        string problem = _controller.Save(textBoxFolder.Text);
        if (!string.IsNullOrEmpty(problem))
        {
          MessageBox.Show(problem, "Belum Tersimpan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }

        buttonSave.Enabled = false;
        RefreshAssetStatus();
        MessageBox.Show("Folder laporan berhasil disimpan.", "BERHASIL",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save the report folder.", ex);
        MessageBox.Show("Folder laporan gagal disimpan.", "GAGAL",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
      finally
      {
        Cursor = previous;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Choose the receipt printer, set the paper width, and test the result.
  ///
  /// Administrators only - see <see cref="SettingForm"/>.
  /// </summary>
  public partial class PrinterSettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>Shown for the empty printer name, which means "whatever Windows is set to".</summary>
    private const string UseWindowsDefault = "<Printer bawaan Windows>";

    private PrinterSettingController _controller;
    private bool _loading;

    public PrinterSettingForm()
    {
      InitializeComponent();
    }

    private void PrinterSettingForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      _controller = new PrinterSettingController(this);
      textBoxPreview.Font = new Font("Courier New", 8);

      _loading = true;
      try
      {
        comboBoxPrinter.Items.Clear();
        comboBoxPrinter.Items.Add(UseWindowsDefault);
        foreach (string printer in _controller.GetInstalledPrinters())
          comboBoxPrinter.Items.Add(printer);

        string configured = _controller.GetPrinterName();
        if (string.IsNullOrEmpty(configured))
        {
          comboBoxPrinter.SelectedIndex = 0;
        }
        else if (comboBoxPrinter.Items.Contains(configured))
        {
          comboBoxPrinter.SelectedItem = configured;
        }
        else
        {
          // Configured printer is not installed on this machine - keep it visible rather than
          // silently switching, so the operator can see what is wrong.
          comboBoxPrinter.Items.Add(configured);
          comboBoxPrinter.SelectedItem = configured;
        }

        numericPaperWidth.Minimum = PrinterSettingController.MinPaperWidthMm;
        numericPaperWidth.Maximum = PrinterSettingController.MaxPaperWidthMm;
        numericPaperWidth.Value = Clamp(_controller.GetPaperWidthMm());
      }
      finally
      {
        _loading = false;
      }

      buttonSave.Enabled = false;
      RefreshPrinterStatus();
      RefreshPreview();
    }

    private decimal Clamp(int millimetres)
    {
      if (millimetres < PrinterSettingController.MinPaperWidthMm)
        return PrinterSettingController.MinPaperWidthMm;
      if (millimetres > PrinterSettingController.MaxPaperWidthMm)
        return PrinterSettingController.MaxPaperWidthMm;
      return millimetres;
    }

    /// <summary>Empty string when the Windows default is chosen.</summary>
    private string SelectedPrinterName
    {
      get
      {
        string selected = comboBoxPrinter.SelectedItem as string;
        return selected == UseWindowsDefault ? string.Empty : (selected ?? string.Empty);
      }
    }

    private int SelectedPaperWidthMm
    {
      get { return (int)numericPaperWidth.Value; }
    }

    private void RefreshPrinterStatus()
    {
      if (_controller == null)
        return;

      string name = SelectedPrinterName;
      if (string.IsNullOrEmpty(name))
      {
        string windowsDefault = _controller.GetDefaultPrinterName();
        if (string.IsNullOrEmpty(windowsDefault))
        {
          labelPrinterStatus.ForeColor = Color.Firebrick;
          labelPrinterStatus.Text = "Windows tidak memiliki printer bawaan. Pilih printer secara langsung.";
        }
        else
        {
          labelPrinterStatus.ForeColor = Color.ForestGreen;
          labelPrinterStatus.Text = "Menggunakan printer bawaan Windows: " + windowsDefault;
        }
        return;
      }

      if (_controller.IsPrinterAvailable(name))
      {
        labelPrinterStatus.ForeColor = Color.ForestGreen;
        labelPrinterStatus.Text = "Printer siap digunakan.";
      }
      else
      {
        labelPrinterStatus.ForeColor = Color.Firebrick;
        labelPrinterStatus.Text = "Printer tidak ditemukan di komputer ini. Nota tidak akan tercetak.";
      }
    }

    /// <summary>
    /// Shows the sample receipt as text. Only the content is shown - alignment and the real paper
    /// width are what the test print is for.
    /// </summary>
    private void RefreshPreview()
    {
      if (_controller == null)
        return;
      try
      {
        StringBuilder sb = new StringBuilder();
        foreach (StringPrint line in _controller.BuildTestReceipt(SelectedPaperWidthMm))
          sb.AppendLine(line.Text);
        textBoxPreview.Text = sb.ToString();
      }
      catch (Exception ex)
      {
        _log.Error("Could not build the receipt preview.", ex);
        textBoxPreview.Text = string.Empty;
      }
    }

    private void comboBoxPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;
      buttonSave.Enabled = true;
      RefreshPrinterStatus();
    }

    private void numericPaperWidth_ValueChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;
      buttonSave.Enabled = true;
      RefreshPreview();
    }

    private void buttonWidth58_Click(object sender, EventArgs e)
    {
      numericPaperWidth.Value = 58;
    }

    private void buttonWidth80_Click(object sender, EventArgs e)
    {
      numericPaperWidth.Value = 80;
    }

    private void buttonTestPrint_Click(object sender, EventArgs e)
    {
      Cursor previous = Cursor;
      Cursor = Cursors.WaitCursor;
      buttonTestPrint.Enabled = false;
      try
      {
        // Uses what is on screen, not what is saved, so a width can be tried before committing.
        string problem = _controller.TestPrint(SelectedPrinterName, SelectedPaperWidthMm);
        if (!string.IsNullOrEmpty(problem))
        {
          MessageBox.Show(problem, "Tes Cetak Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }
        MessageBox.Show(
          "Nota contoh telah dikirim ke printer." + Environment.NewLine + Environment.NewLine +
          "Periksa hasilnya: jika ada baris yang terpotong atau turun ke bawah, ubah lebar kertas.",
          "Tes Cetak", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      finally
      {
        buttonTestPrint.Enabled = true;
        Cursor = previous;
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        string problem = _controller.Save(SelectedPrinterName, SelectedPaperWidthMm);
        if (!string.IsNullOrEmpty(problem))
        {
          MessageBox.Show(problem, "Belum Tersimpan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }
        buttonSave.Enabled = false;
        RefreshPrinterStatus();
        MessageBox.Show("Pengaturan printer berhasil disimpan.", "BERHASIL",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save printer settings.", ex);
        MessageBox.Show("Pengaturan printer gagal disimpan.", "GAGAL",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

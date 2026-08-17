using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Add and remove the EDC terminals and QRIS providers a cashier can charge a sale to.
  ///
  /// Both lists live on one page because they are the same decision made twice; an empty list simply
  /// means that method is not offered at the till.
  /// </summary>
  public partial class PaymentOptionSettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private PaymentOptionSettingController _controller;

    public PaymentOptionSettingForm()
    {
      InitializeComponent();
    }

    private void PaymentOptionSettingForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      _controller = new PaymentOptionSettingController(this);

      Fill(listBoxEdc, _controller.GetEdcTerminals());
      Fill(listBoxQris, _controller.GetQrisProviders());
      buttonSave.Enabled = false;
    }

    private static void Fill(ListBox list, List<string> values)
    {
      list.Items.Clear();
      foreach (string value in values)
        list.Items.Add(value);
    }

    private static List<string> ItemsOf(ListBox list)
    {
      List<string> values = new List<string>();
      foreach (object item in list.Items)
        values.Add(item as string);
      return values;
    }

    /// <summary>Shared by both lists - the only difference is which controls they use.</summary>
    private void Add(TextBox input, ListBox list)
    {
      string problem = _controller.ValidateNew(input.Text, ItemsOf(list));
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Ditambahkan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      list.Items.Add(input.Text.Trim());
      input.Clear();
      input.Focus();
      buttonSave.Enabled = true;
    }

    private void Remove(ListBox list, string what)
    {
      int index = list.SelectedIndex;
      if (index < 0)
      {
        MessageBox.Show("Pilih " + what + " yang akan dihapus.", "Belum Dipilih",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      DialogResult confirm = MessageBox.Show(
        string.Format("Hapus \"{0}\" dari daftar?", list.Items[index]) + Environment.NewLine + Environment.NewLine +
        "Transaksi yang sudah tercatat tidak berubah.",
        "Konfirmasi Hapus", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
      if (confirm != DialogResult.OK)
        return;

      list.Items.RemoveAt(index);
      buttonSave.Enabled = true;
    }

    private void buttonAddEdc_Click(object sender, EventArgs e)
    {
      Add(textBoxNewEdc, listBoxEdc);
    }

    private void buttonRemoveEdc_Click(object sender, EventArgs e)
    {
      Remove(listBoxEdc, "terminal");
    }

    private void buttonAddQris_Click(object sender, EventArgs e)
    {
      Add(textBoxNewQris, listBoxQris);
    }

    private void buttonRemoveQris_Click(object sender, EventArgs e)
    {
      Remove(listBoxQris, "provider");
    }

    // Enter adds, so a list can be typed straight through without reaching for the mouse.
    private void textBoxNewEdc_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
        return;
      e.SuppressKeyPress = true;
      Add(textBoxNewEdc, listBoxEdc);
    }

    private void textBoxNewQris_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
        return;
      e.SuppressKeyPress = true;
      Add(textBoxNewQris, listBoxQris);
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        _controller.Save(ItemsOf(listBoxEdc), ItemsOf(listBoxQris));
        buttonSave.Enabled = false;
        MessageBox.Show("Pengaturan pembayaran berhasil disimpan.", "BERHASIL",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save payment options.", ex);
        MessageBox.Show("Pengaturan pembayaran gagal disimpan.", "GAGAL",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

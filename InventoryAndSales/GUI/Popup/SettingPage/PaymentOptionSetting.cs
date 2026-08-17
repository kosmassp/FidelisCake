using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryAndSales.Business;
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

      _loading = true;
      try
      {
        listBoxEdc.Items.Clear();
        foreach (string terminal in _controller.GetEdcTerminals())
          listBoxEdc.Items.Add(terminal);

        listBoxQris.Items.Clear();
        foreach (QrisProvider provider in _controller.GetQrisProviders())
          listBoxQris.Items.Add(provider);

        comboBoxQrisMode.Items.Clear();
        comboBoxQrisMode.Items.Add("Statis");
        comboBoxQrisMode.Items.Add("Dinamis");
        comboBoxQrisMode.SelectedIndex = 0;
      }
      finally
      {
        _loading = false;
      }

      buttonSave.Enabled = false;
    }

    private bool _loading;

    private QrisMode SelectedMode
    {
      get { return comboBoxQrisMode.SelectedIndex == 1 ? QrisMode.Dynamic : QrisMode.Static; }
    }

    private static List<string> ItemsOf(ListBox list)
    {
      List<string> values = new List<string>();
      foreach (object item in list.Items)
        values.Add(item as string);
      return values;
    }

    private List<QrisProvider> QrisItems()
    {
      List<QrisProvider> providers = new List<QrisProvider>();
      foreach (object item in listBoxQris.Items)
        providers.Add(item as QrisProvider);
      return providers;
    }

    private List<string> QrisNames()
    {
      List<string> names = new List<string>();
      foreach (QrisProvider provider in QrisItems())
        names.Add(provider.Name);
      return names;
    }

    /// <summary>Shows the selected provider's code type, so the combo always reflects the list.</summary>
    private void listBoxQris_SelectedIndexChanged(object sender, EventArgs e)
    {
      QrisProvider provider = listBoxQris.SelectedItem as QrisProvider;
      if (provider == null)
        return;

      _loading = true;
      try
      {
        comboBoxQrisMode.SelectedIndex = provider.Mode == QrisMode.Dynamic ? 1 : 0;
      }
      finally
      {
        _loading = false;
      }
    }

    /// <summary>
    /// Changing the type while a provider is selected edits that provider, so an existing entry can
    /// be corrected without removing and re-adding it.
    /// </summary>
    private void comboBoxQrisMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (_loading)
        return;

      int index = listBoxQris.SelectedIndex;
      if (index < 0)
        return;

      QrisProvider selected = listBoxQris.Items[index] as QrisProvider;
      if (selected == null || selected.Mode == SelectedMode)
        return;

      listBoxQris.Items[index] = new QrisProvider(selected.Name, SelectedMode);
      listBoxQris.SelectedIndex = index;
      buttonSave.Enabled = true;
    }

    /// <summary>Adds an EDC terminal, which is only a name.</summary>
    private void AddEdc()
    {
      string problem = _controller.ValidateNew(textBoxNewEdc.Text, ItemsOf(listBoxEdc));
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Ditambahkan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      listBoxEdc.Items.Add(textBoxNewEdc.Text.Trim());
      textBoxNewEdc.Clear();
      textBoxNewEdc.Focus();
      buttonSave.Enabled = true;
    }

    /// <summary>Adds a QRIS provider together with the code type it issues.</summary>
    private void AddQris()
    {
      string problem = _controller.ValidateNewProvider(textBoxNewQris.Text, QrisNames());
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Ditambahkan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      listBoxQris.Items.Add(new QrisProvider(textBoxNewQris.Text.Trim(), SelectedMode));
      textBoxNewQris.Clear();
      textBoxNewQris.Focus();
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
      AddEdc();
    }

    private void buttonRemoveEdc_Click(object sender, EventArgs e)
    {
      Remove(listBoxEdc, "terminal");
    }

    private void buttonAddQris_Click(object sender, EventArgs e)
    {
      AddQris();
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
      AddEdc();
    }

    private void textBoxNewQris_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
        return;
      e.SuppressKeyPress = true;
      AddQris();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        _controller.Save(ItemsOf(listBoxEdc), QrisItems());
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

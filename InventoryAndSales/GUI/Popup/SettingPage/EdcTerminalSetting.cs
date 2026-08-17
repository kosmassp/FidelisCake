using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  /// <summary>
  /// Add and remove the EDC terminals a cashier can charge a card to.
  /// </summary>
  public partial class EdcTerminalSettingForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private EdcTerminalSettingController _controller;

    public EdcTerminalSettingForm()
    {
      InitializeComponent();
    }

    private void EdcTerminalSettingForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      _controller = new EdcTerminalSettingController(this);

      listBoxTerminals.Items.Clear();
      foreach (string terminal in _controller.GetTerminals())
        listBoxTerminals.Items.Add(terminal);

      buttonSave.Enabled = false;
      RefreshButtons();
    }

    private void RefreshButtons()
    {
      buttonRemove.Enabled = listBoxTerminals.SelectedIndex >= 0;
    }

    private List<string> CurrentTerminals()
    {
      List<string> terminals = new List<string>();
      foreach (object item in listBoxTerminals.Items)
        terminals.Add(item as string);
      return terminals;
    }

    private void listBoxTerminals_SelectedIndexChanged(object sender, EventArgs e)
    {
      RefreshButtons();
    }

    private void textBoxNewTerminal_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
        return;
      // Enter adds, so a list can be typed straight through without reaching for the mouse.
      e.SuppressKeyPress = true;
      AddTerminal();
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
      AddTerminal();
    }

    private void AddTerminal()
    {
      string problem = _controller.ValidateNew(textBoxNewTerminal.Text, CurrentTerminals());
      if (!string.IsNullOrEmpty(problem))
      {
        MessageBox.Show(problem, "Tidak Dapat Ditambahkan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      listBoxTerminals.Items.Add(textBoxNewTerminal.Text.Trim());
      textBoxNewTerminal.Clear();
      textBoxNewTerminal.Focus();
      buttonSave.Enabled = true;
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      int index = listBoxTerminals.SelectedIndex;
      if (index < 0)
        return;

      string terminal = listBoxTerminals.Items[index] as string;
      DialogResult confirm = MessageBox.Show(
        string.Format("Hapus terminal \"{0}\" dari daftar?", terminal) + Environment.NewLine + Environment.NewLine +
        "Transaksi yang sudah tercatat pada terminal ini tidak berubah.",
        "Konfirmasi Hapus", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
      if (confirm != DialogResult.OK)
        return;

      listBoxTerminals.Items.RemoveAt(index);
      buttonSave.Enabled = true;
      RefreshButtons();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        _controller.Save(CurrentTerminals());
        buttonSave.Enabled = false;
        MessageBox.Show("Daftar terminal berhasil disimpan.", "BERHASIL",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        _log.Error("Could not save EDC terminals.", ex);
        MessageBox.Show("Daftar terminal gagal disimpan.", "GAGAL",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

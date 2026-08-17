using InventoryAndSales.GUI.Controller;
using InventoryAndSales.GUI.Popup.SettingPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventoryAndSales.GUI.Popup
{
  /// <summary>
  /// Host for the individual settings pages: a list on the left, the selected page on the right.
  ///
  /// Add a page by adding one line to <see cref="Initialize"/>.
  /// </summary>
  public partial class SettingForm : Form
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>
    /// A page in the list. The control is built the first time the page is selected rather than
    /// when the dialog opens, so opening Settings does not run every page's database reads.
    /// </summary>
    private class SettingPageEntry
    {
      private readonly Func<UserControl> _factory;
      private UserControl _instance;

      public string Tag { get; private set; }

      public SettingPageEntry(string tag, Func<UserControl> factory)
      {
        Tag = tag;
        _factory = factory;
      }

      public UserControl GetControl()
      {
        if (_instance == null)
          _instance = _factory();
        return _instance;
      }

      public override string ToString()
      {
        return Tag;
      }
    }

    private SettingPageController _controller;

    public SettingForm()
    {
      InitializeComponent();
      _controller = new SettingPageController(this);
      listBoxSettingSelection.DisplayMember = "Tag";
      Initialize();
    }

    private void Initialize()
    {
      listBoxSettingSelection.Items.Add(new SettingPageEntry("Nota", () => new HeaderAndFooterForm()));
      listBoxSettingSelection.Items.Add(new SettingPageEntry("Laporan", () => new ReportSettingForm()));
      listBoxSettingSelection.Items.Add(new SettingPageEntry("Keamanan", () => new SecuritySettingForm()));
    }

    private void listBoxSettingSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
      SettingPageEntry entry = listBoxSettingSelection.SelectedItem as SettingPageEntry;
      if (entry == null)
        return;

      try
      {
        UserControl page = entry.GetControl();

        // Replace rather than stack. Previously each selection added another control on top of the
        // last one, so every page ever opened stayed alive underneath the visible one.
        panelSettingPage.SuspendLayout();
        panelSettingPage.Controls.Clear();
        page.Dock = DockStyle.Fill;
        panelSettingPage.Controls.Add(page);
        panelSettingPage.ResumeLayout();
      }
      catch (Exception ex)
      {
        _log.Error(string.Format("Could not open settings page '{0}'.", entry.Tag), ex);
        MessageBox.Show("Halaman pengaturan tidak dapat dibuka.", "Gagal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
  }
}

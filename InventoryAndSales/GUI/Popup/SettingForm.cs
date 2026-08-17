using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.Enumeration;
using InventoryAndSales.GUI.Controller;
using InventoryAndSales.GUI.Popup.SettingPage;
using InventoryAndSales.GUI.Util;
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

      /// <summary>Permission a user must hold for this page to be listed at all.</summary>
      public AccessOption Required { get; private set; }

      public SettingPageEntry(string tag, AccessOption required, Func<UserControl> factory)
      {
        Tag = tag;
        Required = required;
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

    /// <summary>
    /// Registers the pages. A page is only listed when the signed-in user holds the permission it
    /// declares, so a cashier who reaches this dialog simply does not see it.
    /// </summary>
    private void Initialize()
    {
      List<SettingPageEntry> pages = new List<SettingPageEntry>
      {
        new SettingPageEntry("Nota", AccessOption.Master, () => new HeaderAndFooterForm()),
        new SettingPageEntry("Laporan", AccessOption.Master, () => new ReportSettingForm()),
        // Printer and security are administrator-only: one decides where every receipt goes, the
        // other whether the recovery account still works.
        new SettingPageEntry("Printer", AccessOption.Admin, () => new PrinterSettingForm()),
        new SettingPageEntry("Keamanan", AccessOption.Admin, () => new SecuritySettingForm()),
      };

      User activeUser = BusinessFactory.GetInstance().LoginManager.ActiveUser;
      int role = activeUser == null ? 0 : activeUser.Role;

      foreach (SettingPageEntry page in pages)
      {
        if (BusinessUtil.AllowedRole(role, page.Required))
          listBoxSettingSelection.Items.Add(page);
        else
          _log.InfoFormat("Hiding settings page '{0}' - requires {1}.", page.Tag, page.Required);
      }

      if (listBoxSettingSelection.Items.Count == 0)
      {
        MessageBox.Show("Anda tidak memiliki akses ke pengaturan.", "Akses Ditolak",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        BeginInvoke(new MethodInvoker(Close));
      }
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

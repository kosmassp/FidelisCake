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
  public partial class SettingForm : Form
  {
    private SettingPageController _controller;
    public SettingForm()
    {
      InitializeComponent();
      Initialize();
      _controller = new SettingPageController(this);
      listBoxSettingSelection.DisplayMember = "Tag";
    }

    private void Initialize()
    {
      listBoxSettingSelection.Items.Add(new HeaderAndFooterForm());
    }

    private void listBoxSettingSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
      var selectedItem = (Control) listBoxSettingSelection.SelectedItem;
      if(selectedItem != null)
      {
        panelSettingPage.Controls.Add(selectedItem);
        selectedItem.Dock = DockStyle.Fill;
      }
    }
  }
}

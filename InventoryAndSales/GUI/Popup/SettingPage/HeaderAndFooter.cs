using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryAndSales.GUI.Controller.SettingPage;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Util;

namespace InventoryAndSales.GUI.Popup.SettingPage
{
  public partial class HeaderAndFooterForm : UserControl
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private HeaderAndFooterController _controller;
    public HeaderAndFooterForm()
    {
      InitializeComponent();
      _controller = new HeaderAndFooterController(this);
      buttonSave.Enabled = false;
    }

    private void HeaderAndFooterForm_Load(object sender, EventArgs e)
    {
      if (DesignMode)
        return;

      textBoxHeader.Text = _controller.GetHeader();
      textBoxFooter.Text = _controller.GetFooter();

      BuildExample();
    }

    internal void SetPaymentNoteFont(Font font)
    {
      textBoxExample.Font = font;
    }

    private void BuildExample()
    {
      var example =  _controller.GetExample(textBoxHeader.Text, textBoxFooter.Text);
      StringBuilder sb = new StringBuilder();
      foreach (var ex in example)
      {
        sb.AppendLine(ex.Text);
      }
      textBoxExample.Text = sb.ToString();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        _controller.SetHeader(textBoxHeader.Text);
        _controller.SetFooter(textBoxFooter.Text);
        buttonSave.Enabled = false;
        MessageBox.Show("Tampilan Nota Berhasil Terubah.", "BERHASIL", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch(Exception ex)
      {
        _log.Error(ex);
        MessageBox.Show("Tampilan Nota Belum Berubah.", "GAGAL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void textBoxHeader_TextChanged(object sender, EventArgs e)
    {
      BuildExample();
      buttonSave.Enabled = true;
    }

    private void textBoxFooter_TextChanged(object sender, EventArgs e)
    {
      BuildExample();
      buttonSave.Enabled = true;
    }
  }
}

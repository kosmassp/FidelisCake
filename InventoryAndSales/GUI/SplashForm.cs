using InventoryAndSales.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryAndSales.GUI
{
  public partial class SplashForm : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public SplashForm()
    {
      InitializeComponent();
      Thread thread = new Thread(() => Start());
      thread.Start();
    }

    public bool InitializationCheckSuccess = false;

    private void SplashForm_Load(object sender, EventArgs e)
    {
    }

    delegate void IntegerStringHandler(int handle, string status);
    private void SetProgressBar(int progress, string status)
    {
      if (InvokeRequired)
      {
        this.BeginInvoke(new IntegerStringHandler(SetProgressBar),progress, status);
        return;
      }
      log.Info("ProgressBarUpdate: " + status);
      progressBarLoading.Value = progress;
      labelStatus.Text = status;
      if (progress == 100)
      {
        Thread.Sleep(200);
        InitializationCheckSuccess = true;
        Close();
      }
    }

    public void Start()
    {
      Thread.Sleep(200);
      SetProgressBar(10, "Initializing");
      Thread.Sleep(200);
      SetProgressBar(40, "Checking Database");
      DBUtility.CheckForDatabaseTable();
      SetProgressBar(90, "Inserting Important Row in the Database");
      DBUtility.CheckForDatabaseRow();
      Thread.Sleep(500);
      SetProgressBar(100, "Starting");
      Thread.Sleep(200);

    }

  }
}

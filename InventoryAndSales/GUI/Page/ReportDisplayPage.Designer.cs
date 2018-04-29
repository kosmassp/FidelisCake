namespace InventoryAndSales.GUI.Page
{
  partial class ReportDisplayPage
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabControlSummaryReport = new System.Windows.Forms.TabControl();
      this.tabPageReportPerCashier = new System.Windows.Forms.TabPage();
      this.dataGridViewLaporanKasir = new System.Windows.Forms.DataGridView();
      this.tabPageReportPerProduct = new System.Windows.Forms.TabPage();
      this.dataGridViewLaporanProduct = new System.Windows.Forms.DataGridView();
      this.tabPageReportPerTransaction = new System.Windows.Forms.TabPage();
      this.dataGridViewLaporanTransaksi = new System.Windows.Forms.DataGridView();
      this.tabPageReportDetail = new System.Windows.Forms.TabPage();
      this.dataGridViewLaporanDetail = new System.Windows.Forms.DataGridView();
      this.groupBoxReportFilter = new System.Windows.Forms.GroupBox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.labelLaporanStart = new System.Windows.Forms.Label();
      this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
      this.buttonShowReportSummary = new System.Windows.Forms.Button();
      this.dateTimePickerStop = new System.Windows.Forms.DateTimePicker();
      this.labelLaporanEnd = new System.Windows.Forms.Label();
      this.tabControlSummaryReport.SuspendLayout();
      this.tabPageReportPerCashier.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanKasir)).BeginInit();
      this.tabPageReportPerProduct.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanProduct)).BeginInit();
      this.tabPageReportPerTransaction.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanTransaksi)).BeginInit();
      this.tabPageReportDetail.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanDetail)).BeginInit();
      this.groupBoxReportFilter.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControlSummaryReport
      // 
      this.tabControlSummaryReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControlSummaryReport.Controls.Add(this.tabPageReportPerCashier);
      this.tabControlSummaryReport.Controls.Add(this.tabPageReportPerProduct);
      this.tabControlSummaryReport.Controls.Add(this.tabPageReportPerTransaction);
      this.tabControlSummaryReport.Controls.Add(this.tabPageReportDetail);
      this.tabControlSummaryReport.Location = new System.Drawing.Point(27, 194);
      this.tabControlSummaryReport.Name = "tabControlSummaryReport";
      this.tabControlSummaryReport.SelectedIndex = 0;
      this.tabControlSummaryReport.Size = new System.Drawing.Size(1075, 454);
      this.tabControlSummaryReport.TabIndex = 17;
      // 
      // tabPageReportPerCashier
      // 
      this.tabPageReportPerCashier.Controls.Add(this.dataGridViewLaporanKasir);
      this.tabPageReportPerCashier.Location = new System.Drawing.Point(4, 22);
      this.tabPageReportPerCashier.Name = "tabPageReportPerCashier";
      this.tabPageReportPerCashier.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReportPerCashier.Size = new System.Drawing.Size(1067, 428);
      this.tabPageReportPerCashier.TabIndex = 0;
      this.tabPageReportPerCashier.Text = "Per Kasir";
      this.tabPageReportPerCashier.UseVisualStyleBackColor = true;
      // 
      // dataGridViewLaporanKasir
      // 
      this.dataGridViewLaporanKasir.AllowUserToAddRows = false;
      this.dataGridViewLaporanKasir.AllowUserToDeleteRows = false;
      this.dataGridViewLaporanKasir.AllowUserToOrderColumns = true;
      this.dataGridViewLaporanKasir.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewLaporanKasir.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.dataGridViewLaporanKasir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLaporanKasir.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewLaporanKasir.Location = new System.Drawing.Point(3, 3);
      this.dataGridViewLaporanKasir.Name = "dataGridViewLaporanKasir";
      this.dataGridViewLaporanKasir.ReadOnly = true;
      this.dataGridViewLaporanKasir.RowHeadersVisible = false;
      this.dataGridViewLaporanKasir.Size = new System.Drawing.Size(1061, 422);
      this.dataGridViewLaporanKasir.TabIndex = 14;
      // 
      // tabPageReportPerProduct
      // 
      this.tabPageReportPerProduct.Controls.Add(this.dataGridViewLaporanProduct);
      this.tabPageReportPerProduct.Location = new System.Drawing.Point(4, 22);
      this.tabPageReportPerProduct.Name = "tabPageReportPerProduct";
      this.tabPageReportPerProduct.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReportPerProduct.Size = new System.Drawing.Size(1067, 428);
      this.tabPageReportPerProduct.TabIndex = 1;
      this.tabPageReportPerProduct.Text = "Per Product";
      this.tabPageReportPerProduct.UseVisualStyleBackColor = true;
      // 
      // dataGridViewLaporanProduct
      // 
      this.dataGridViewLaporanProduct.AllowUserToAddRows = false;
      this.dataGridViewLaporanProduct.AllowUserToDeleteRows = false;
      this.dataGridViewLaporanProduct.AllowUserToOrderColumns = true;
      this.dataGridViewLaporanProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewLaporanProduct.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.dataGridViewLaporanProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLaporanProduct.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewLaporanProduct.Location = new System.Drawing.Point(3, 3);
      this.dataGridViewLaporanProduct.Name = "dataGridViewLaporanProduct";
      this.dataGridViewLaporanProduct.ReadOnly = true;
      this.dataGridViewLaporanProduct.RowHeadersVisible = false;
      this.dataGridViewLaporanProduct.Size = new System.Drawing.Size(1061, 422);
      this.dataGridViewLaporanProduct.TabIndex = 7;
      // 
      // tabPageReportPerTransaction
      // 
      this.tabPageReportPerTransaction.Controls.Add(this.dataGridViewLaporanTransaksi);
      this.tabPageReportPerTransaction.Location = new System.Drawing.Point(4, 22);
      this.tabPageReportPerTransaction.Name = "tabPageReportPerTransaction";
      this.tabPageReportPerTransaction.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReportPerTransaction.Size = new System.Drawing.Size(1067, 428);
      this.tabPageReportPerTransaction.TabIndex = 2;
      this.tabPageReportPerTransaction.Text = "Per Transaksi";
      this.tabPageReportPerTransaction.UseVisualStyleBackColor = true;
      // 
      // dataGridViewLaporanTransaksi
      // 
      this.dataGridViewLaporanTransaksi.AllowUserToAddRows = false;
      this.dataGridViewLaporanTransaksi.AllowUserToDeleteRows = false;
      this.dataGridViewLaporanTransaksi.AllowUserToOrderColumns = true;
      this.dataGridViewLaporanTransaksi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewLaporanTransaksi.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.dataGridViewLaporanTransaksi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLaporanTransaksi.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewLaporanTransaksi.Location = new System.Drawing.Point(3, 3);
      this.dataGridViewLaporanTransaksi.Name = "dataGridViewLaporanTransaksi";
      this.dataGridViewLaporanTransaksi.ReadOnly = true;
      this.dataGridViewLaporanTransaksi.RowHeadersVisible = false;
      this.dataGridViewLaporanTransaksi.Size = new System.Drawing.Size(1061, 422);
      this.dataGridViewLaporanTransaksi.TabIndex = 14;
      // 
      // tabPageReportDetail
      // 
      this.tabPageReportDetail.Controls.Add(this.dataGridViewLaporanDetail);
      this.tabPageReportDetail.Location = new System.Drawing.Point(4, 22);
      this.tabPageReportDetail.Name = "tabPageReportDetail";
      this.tabPageReportDetail.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageReportDetail.Size = new System.Drawing.Size(1067, 428);
      this.tabPageReportDetail.TabIndex = 3;
      this.tabPageReportDetail.Text = "Detail Laporan";
      this.tabPageReportDetail.UseVisualStyleBackColor = true;
      // 
      // dataGridViewLaporanDetail
      // 
      this.dataGridViewLaporanDetail.AllowUserToAddRows = false;
      this.dataGridViewLaporanDetail.AllowUserToDeleteRows = false;
      this.dataGridViewLaporanDetail.AllowUserToOrderColumns = true;
      this.dataGridViewLaporanDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
      this.dataGridViewLaporanDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
      this.dataGridViewLaporanDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewLaporanDetail.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewLaporanDetail.Location = new System.Drawing.Point(3, 3);
      this.dataGridViewLaporanDetail.Name = "dataGridViewLaporanDetail";
      this.dataGridViewLaporanDetail.ReadOnly = true;
      this.dataGridViewLaporanDetail.RowHeadersVisible = false;
      this.dataGridViewLaporanDetail.Size = new System.Drawing.Size(1061, 422);
      this.dataGridViewLaporanDetail.TabIndex = 13;
      // 
      // groupBoxReportFilter
      // 
      this.groupBoxReportFilter.Controls.Add(this.checkBox1);
      this.groupBoxReportFilter.Controls.Add(this.labelLaporanStart);
      this.groupBoxReportFilter.Controls.Add(this.dateTimePickerStart);
      this.groupBoxReportFilter.Controls.Add(this.buttonShowReportSummary);
      this.groupBoxReportFilter.Controls.Add(this.dateTimePickerStop);
      this.groupBoxReportFilter.Controls.Add(this.labelLaporanEnd);
      this.groupBoxReportFilter.Location = new System.Drawing.Point(27, 10);
      this.groupBoxReportFilter.Name = "groupBoxReportFilter";
      this.groupBoxReportFilter.Size = new System.Drawing.Size(325, 167);
      this.groupBoxReportFilter.TabIndex = 16;
      this.groupBoxReportFilter.TabStop = false;
      this.groupBoxReportFilter.Text = "Laporan Periode";
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(12, 132);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(102, 17);
      this.checkBox1.TabIndex = 6;
      this.checkBox1.Text = "Detail Transaksi";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // labelLaporanStart
      // 
      this.labelLaporanStart.AutoSize = true;
      this.labelLaporanStart.Location = new System.Drawing.Point(25, 26);
      this.labelLaporanStart.Name = "labelLaporanStart";
      this.labelLaporanStart.Size = new System.Drawing.Size(71, 13);
      this.labelLaporanStart.TabIndex = 2;
      this.labelLaporanStart.Text = "Dari Tanggal:";
      // 
      // dateTimePickerStart
      // 
      this.dateTimePickerStart.CustomFormat = "dd  MMM yyyy";
      this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStart.Location = new System.Drawing.Point(175, 26);
      this.dateTimePickerStart.MaxDate = new System.DateTime(2115, 12, 31, 0, 0, 0, 0);
      this.dateTimePickerStart.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerStart.Name = "dateTimePickerStart";
      this.dateTimePickerStart.Size = new System.Drawing.Size(128, 20);
      this.dateTimePickerStart.TabIndex = 0;
      // 
      // buttonShowReportSummary
      // 
      this.buttonShowReportSummary.Location = new System.Drawing.Point(131, 128);
      this.buttonShowReportSummary.Name = "buttonShowReportSummary";
      this.buttonShowReportSummary.Size = new System.Drawing.Size(175, 23);
      this.buttonShowReportSummary.TabIndex = 4;
      this.buttonShowReportSummary.Text = "Lihat Laporan";
      this.buttonShowReportSummary.UseVisualStyleBackColor = true;
      this.buttonShowReportSummary.Click += new System.EventHandler(this.buttonShowReportSummary_Click);
      // 
      // dateTimePickerStop
      // 
      this.dateTimePickerStop.CustomFormat = "dd  MMM yyyy";
      this.dateTimePickerStop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.dateTimePickerStop.Location = new System.Drawing.Point(175, 63);
      this.dateTimePickerStop.MaxDate = new System.DateTime(2115, 12, 31, 0, 0, 0, 0);
      this.dateTimePickerStop.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
      this.dateTimePickerStop.Name = "dateTimePickerStop";
      this.dateTimePickerStop.Size = new System.Drawing.Size(128, 20);
      this.dateTimePickerStop.TabIndex = 1;
      // 
      // labelLaporanEnd
      // 
      this.labelLaporanEnd.AutoSize = true;
      this.labelLaporanEnd.Location = new System.Drawing.Point(9, 63);
      this.labelLaporanEnd.Name = "labelLaporanEnd";
      this.labelLaporanEnd.Size = new System.Drawing.Size(87, 13);
      this.labelLaporanEnd.TabIndex = 3;
      this.labelLaporanEnd.Text = "Sampai Tanggal:";
      // 
      // ReportDisplayPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControlSummaryReport);
      this.Controls.Add(this.groupBoxReportFilter);
      this.Name = "ReportDisplayPage";
      this.Size = new System.Drawing.Size(1129, 658);
      this.tabControlSummaryReport.ResumeLayout(false);
      this.tabPageReportPerCashier.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanKasir)).EndInit();
      this.tabPageReportPerProduct.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanProduct)).EndInit();
      this.tabPageReportPerTransaction.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanTransaksi)).EndInit();
      this.tabPageReportDetail.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaporanDetail)).EndInit();
      this.groupBoxReportFilter.ResumeLayout(false);
      this.groupBoxReportFilter.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControlSummaryReport;
    private System.Windows.Forms.TabPage tabPageReportPerCashier;
    private System.Windows.Forms.DataGridView dataGridViewLaporanKasir;
    private System.Windows.Forms.TabPage tabPageReportPerProduct;
    private System.Windows.Forms.DataGridView dataGridViewLaporanProduct;
    private System.Windows.Forms.TabPage tabPageReportPerTransaction;
    private System.Windows.Forms.DataGridView dataGridViewLaporanTransaksi;
    private System.Windows.Forms.TabPage tabPageReportDetail;
    private System.Windows.Forms.DataGridView dataGridViewLaporanDetail;
    private System.Windows.Forms.GroupBox groupBoxReportFilter;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Label labelLaporanStart;
    private System.Windows.Forms.DateTimePicker dateTimePickerStart;
    private System.Windows.Forms.Button buttonShowReportSummary;
    private System.Windows.Forms.DateTimePicker dateTimePickerStop;
    private System.Windows.Forms.Label labelLaporanEnd;
  }
}

using System;

namespace InventoryAndSales.GUI
{
  partial class TransactionHistory
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
      this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
      this.labelCashierName = new System.Windows.Forms.Label();
      this.comboBoxCashierName = new System.Windows.Forms.ComboBox();
      this.labelDateFrom = new System.Windows.Forms.Label();
      this.labelDateTo = new System.Windows.Forms.Label();
      this.buttonSearch = new System.Windows.Forms.Button();
      this.dataGridViewSearch = new System.Windows.Forms.DataGridView();
      this.buttonNext = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).BeginInit();
      this.SuspendLayout();
      // 
      // dateTimePickerFrom
      // 
      this.dateTimePickerFrom.Location = new System.Drawing.Point(78, 12);
      this.dateTimePickerFrom.Name = "dateTimePickerFrom";
      this.dateTimePickerFrom.Size = new System.Drawing.Size(200, 20);
      this.dateTimePickerFrom.TabIndex = 0;
      this.dateTimePickerFrom.Value = new System.DateTime(2018, 4, 28, 20, 42, 17, 204);
      // 
      // dateTimePickerTo
      // 
      this.dateTimePickerTo.Location = new System.Drawing.Point(78, 38);
      this.dateTimePickerTo.Name = "dateTimePickerTo";
      this.dateTimePickerTo.Size = new System.Drawing.Size(200, 20);
      this.dateTimePickerTo.TabIndex = 1;
      // 
      // labelCashierName
      // 
      this.labelCashierName.AutoSize = true;
      this.labelCashierName.Location = new System.Drawing.Point(371, 18);
      this.labelCashierName.Name = "labelCashierName";
      this.labelCashierName.Size = new System.Drawing.Size(33, 13);
      this.labelCashierName.TabIndex = 2;
      this.labelCashierName.Text = "Kasir:";
      this.labelCashierName.Visible = false;
      // 
      // comboBoxCashierName
      // 
      this.comboBoxCashierName.FormattingEnabled = true;
      this.comboBoxCashierName.Location = new System.Drawing.Point(410, 16);
      this.comboBoxCashierName.Name = "comboBoxCashierName";
      this.comboBoxCashierName.Size = new System.Drawing.Size(126, 21);
      this.comboBoxCashierName.TabIndex = 3;
      this.comboBoxCashierName.Visible = false;
      // 
      // labelDateFrom
      // 
      this.labelDateFrom.AutoSize = true;
      this.labelDateFrom.Location = new System.Drawing.Point(18, 16);
      this.labelDateFrom.Name = "labelDateFrom";
      this.labelDateFrom.Size = new System.Drawing.Size(26, 13);
      this.labelDateFrom.TabIndex = 4;
      this.labelDateFrom.Text = "Dari";
      // 
      // labelDateTo
      // 
      this.labelDateTo.AutoSize = true;
      this.labelDateTo.Location = new System.Drawing.Point(18, 41);
      this.labelDateTo.Name = "labelDateTo";
      this.labelDateTo.Size = new System.Drawing.Size(42, 13);
      this.labelDateTo.TabIndex = 5;
      this.labelDateTo.Text = "Sampai";
      // 
      // buttonSearch
      // 
      this.buttonSearch.Location = new System.Drawing.Point(284, 38);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(75, 23);
      this.buttonSearch.TabIndex = 6;
      this.buttonSearch.Text = "Cari";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
      // 
      // dataGridViewSearch
      // 
      this.dataGridViewSearch.AllowUserToAddRows = false;
      this.dataGridViewSearch.AllowUserToDeleteRows = false;
      this.dataGridViewSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridViewSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewSearch.Location = new System.Drawing.Point(12, 80);
      this.dataGridViewSearch.MultiSelect = false;
      this.dataGridViewSearch.Name = "dataGridViewSearch";
      this.dataGridViewSearch.ReadOnly = true;
      this.dataGridViewSearch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      this.dataGridViewSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dataGridViewSearch.Size = new System.Drawing.Size(524, 307);
      this.dataGridViewSearch.TabIndex = 7;
      // 
      // buttonNext
      // 
      this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonNext.Location = new System.Drawing.Point(461, 405);
      this.buttonNext.Name = "buttonNext";
      this.buttonNext.Size = new System.Drawing.Size(75, 23);
      this.buttonNext.TabIndex = 8;
      this.buttonNext.Text = "Pilih";
      this.buttonNext.UseVisualStyleBackColor = true;
      this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.Location = new System.Drawing.Point(374, 405);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 9;
      this.buttonCancel.Text = "Batal";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // TransactionHistory
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(548, 441);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonNext);
      this.Controls.Add(this.dataGridViewSearch);
      this.Controls.Add(this.buttonSearch);
      this.Controls.Add(this.labelDateTo);
      this.Controls.Add(this.labelDateFrom);
      this.Controls.Add(this.comboBoxCashierName);
      this.Controls.Add(this.labelCashierName);
      this.Controls.Add(this.dateTimePickerTo);
      this.Controls.Add(this.dateTimePickerFrom);
      this.Name = "TransactionHistory";
      this.Text = "HistoryTransaksi";
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
    private System.Windows.Forms.DateTimePicker dateTimePickerTo;
    private System.Windows.Forms.Label labelCashierName;
    private System.Windows.Forms.ComboBox comboBoxCashierName;
    private System.Windows.Forms.Label labelDateFrom;
    private System.Windows.Forms.Label labelDateTo;
    private System.Windows.Forms.Button buttonSearch;
    private System.Windows.Forms.DataGridView dataGridViewSearch;
    private System.Windows.Forms.Button buttonNext;
    private System.Windows.Forms.Button buttonCancel;
  }
}
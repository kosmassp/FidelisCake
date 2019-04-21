namespace SimpleCommon.UI.ComponentWinForm
{
  partial class SearchableDataView<T>
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
      this.components = new System.ComponentModel.Container();
      this.dataGridView = new System.Windows.Forms.DataGridView();
      this.panelDataGrid = new System.Windows.Forms.Panel();
      this.panelSearch = new System.Windows.Forms.Panel();
      this.splitButton1 = new SimpleCommon.UI.ComponentWinForm.SplitButton();
      this.contextMenuStripSettings = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.labelSearch = new System.Windows.Forms.Label();
      this.textBoxSearch = new System.Windows.Forms.TextBox();
      this.filterByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.sortByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
      this.panelDataGrid.SuspendLayout();
      this.panelSearch.SuspendLayout();
      this.contextMenuStripSettings.SuspendLayout();
      this.SuspendLayout();
      // 
      // dataGridView
      // 
      this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridView.Location = new System.Drawing.Point(0, 0);
      this.dataGridView.Name = "dataGridView";
      this.dataGridView.Size = new System.Drawing.Size(345, 296);
      this.dataGridView.TabIndex = 0;
      this.dataGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridView_KeyPress);
      // 
      // panelDataGrid
      // 
      this.panelDataGrid.Controls.Add(this.dataGridView);
      this.panelDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelDataGrid.Location = new System.Drawing.Point(0, 40);
      this.panelDataGrid.Name = "panelDataGrid";
      this.panelDataGrid.Size = new System.Drawing.Size(345, 296);
      this.panelDataGrid.TabIndex = 1;
      // 
      // panelSearch
      // 
      this.panelSearch.Controls.Add(this.splitButton1);
      this.panelSearch.Controls.Add(this.labelSearch);
      this.panelSearch.Controls.Add(this.textBoxSearch);
      this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelSearch.Location = new System.Drawing.Point(0, 0);
      this.panelSearch.Name = "panelSearch";
      this.panelSearch.Size = new System.Drawing.Size(345, 40);
      this.panelSearch.TabIndex = 2;
      // 
      // splitButton1
      // 
      this.splitButton1.BackgroundImage = global::SimpleCommon.Properties.Resources.Setting;
      this.splitButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.splitButton1.Location = new System.Drawing.Point(291, 6);
      this.splitButton1.Menu = this.contextMenuStripSettings;
      this.splitButton1.Name = "splitButton1";
      this.splitButton1.Size = new System.Drawing.Size(46, 26);
      this.splitButton1.TabIndex = 3;
      this.splitButton1.Text = " ";
      this.splitButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.splitButton1.UseVisualStyleBackColor = true;
      // 
      // contextMenuStripSettings
      // 
      this.contextMenuStripSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterByToolStripMenuItem,
            this.sortByToolStripMenuItem,
            this.resetToolStripMenuItem});
      this.contextMenuStripSettings.Name = "contextMenuStripSettings";
      this.contextMenuStripSettings.Size = new System.Drawing.Size(153, 92);
      // 
      // labelSearch
      // 
      this.labelSearch.AutoSize = true;
      this.labelSearch.Location = new System.Drawing.Point(3, 13);
      this.labelSearch.Name = "labelSearch";
      this.labelSearch.Size = new System.Drawing.Size(47, 13);
      this.labelSearch.TabIndex = 2;
      this.labelSearch.Text = "Search :";
      // 
      // textBoxSearch
      // 
      this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSearch.Location = new System.Drawing.Point(56, 10);
      this.textBoxSearch.Name = "textBoxSearch";
      this.textBoxSearch.Size = new System.Drawing.Size(229, 20);
      this.textBoxSearch.TabIndex = 0;
      // 
      // filterByToolStripMenuItem
      // 
      this.filterByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem});
      this.filterByToolStripMenuItem.Name = "filterByToolStripMenuItem";
      this.filterByToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.filterByToolStripMenuItem.Text = "Pencarian";
      // 
      // sortByToolStripMenuItem
      // 
      this.sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
      this.sortByToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.sortByToolStripMenuItem.Text = "Urut";
      // 
      // resetToolStripMenuItem
      // 
      this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      this.resetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.resetToolStripMenuItem.Text = "Reset";
      this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
      // 
      // allToolStripMenuItem
      // 
      this.allToolStripMenuItem.Name = "allToolStripMenuItem";
      this.allToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.allToolStripMenuItem.Text = "Semua";
      this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
      // 
      // SearchableDataView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::SimpleCommon.Properties.Resources.Setting;
      this.Controls.Add(this.panelDataGrid);
      this.Controls.Add(this.panelSearch);
      this.Name = "SearchableDataView";
      this.Size = new System.Drawing.Size(345, 336);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
      this.panelDataGrid.ResumeLayout(false);
      this.panelSearch.ResumeLayout(false);
      this.panelSearch.PerformLayout();
      this.contextMenuStripSettings.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView;
    private System.Windows.Forms.Panel panelDataGrid;
    private System.Windows.Forms.Panel panelSearch;
    private System.Windows.Forms.TextBox textBoxSearch;
    private System.Windows.Forms.Label labelSearch;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripSettings;
    private SplitButton splitButton1;
    private System.Windows.Forms.ToolStripMenuItem filterByToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem sortByToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
  }
}

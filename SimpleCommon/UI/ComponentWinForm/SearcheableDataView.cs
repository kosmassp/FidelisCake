using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCommon.Model;

namespace SimpleCommon.UI.ComponentWinForm
{
  public partial class SearchableDataView<T> : UserControl where T : ISearchable
  {
    public SearchableDataView()
    {
      InitializeComponent();
      _specialKeysEvent = new Dictionary<char, bool>();
    }

    private Dictionary<char, bool> _specialKeysEvent;
    public void RegisteredEvent(char a, bool isRemoveChar)
    {
      _specialKeysEvent.Add(a, isRemoveChar);
    }

    private Dictionary<int, T> itemDictionary;
    public void SetDatasource(List<T> ds)
    {
      if (ds != null)
      {
        itemDictionary = new Dictionary<int, T>();
        foreach (T data in ds)
        {
          int rowId = dataGridView.Rows.Add(data.ToDisplayValues());
          itemDictionary.Add(rowId, data);
        }
      }
    }

    private void buttonSetting_Click(object sender, EventArgs e)
    {

    }

    public delegate void SpecialKeyEventHandler(object sender, SpecialKeyEventArgs<T> args);

    public event SpecialKeyEventHandler SpecialKeyEvent;
    public event SpecialKeyEventHandler EnterKeyPressedEvent;

    private void dataGridView_KeyPress(object sender, KeyPressEventArgs e)
    {
      char key = e.KeyChar;
      CharPress(key);
    }

    private void CharPress(char key)
    {
      if (_specialKeysEvent.ContainsKey(key))
      {
        var selectedRows = dataGridView.SelectedRows;
        while (textBoxSearch.Text.IndexOf(key) >= 0)
          textBoxSearch.Text = textBoxSearch.Text.Remove(textBoxSearch.Text.IndexOf(key), 1);
        List<T> list = new List<T>(selectedRows.Count);
        foreach (DataGridViewRow dataGridViewRow in selectedRows)
        {
          list.Add(itemDictionary[dataGridViewRow.Index]);
        }
        if(SpecialKeyEvent != null )
        {
          SpecialKeyEvent(this, new SpecialKeyEventArgs<T>(key, list));
        }
      }
    }

    private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
    {
      CharPress(e.KeyChar);
    }
    private void SelectNextVisibleRow() //Down Arrow
    {
      var selectedRows = dataGridView.SelectedRows;
      int selectedIndex = 0;
      if (selectedRows.Count > 0)
      {
        var selectedRow = selectedRows[0];
        selectedIndex = selectedRow.Index;
      }
      int stopLoop = selectedIndex;
      dataGridView.ClearSelection();
      while (true)
      {
        selectedIndex++;
        if (selectedIndex >= dataGridView.Rows.Count)
          selectedIndex = 0;
        DataGridViewRow row = dataGridView.Rows[selectedIndex];
        if (row.Visible)
        {
          row.Selected = true;
          dataGridView.FirstDisplayedScrollingRowIndex = selectedIndex;
          break;
        }
        if (stopLoop == selectedIndex)
          return;
      }
    }

    private void SelectPrevVisibleRow()
    {
      var selectedRows = dataGridView.SelectedRows;
      int selectedIndex = 0;
      if (selectedRows.Count > 0)
      {
        var selectedRow = selectedRows[0];
        selectedIndex = selectedRow.Index;
      }
      int stopLoop = selectedIndex;
      dataGridView.ClearSelection();
      while (true)
      {
        selectedIndex--;
        if (selectedIndex < 0)
          selectedIndex = dataGridView.Rows.Count - 1;
        DataGridViewRow row = dataGridView.Rows[selectedIndex];
        if (row.Visible)
        {
          row.Selected = true;
          dataGridView.FirstDisplayedScrollingRowIndex = selectedIndex;
          break;
        }
        if (stopLoop == selectedIndex)
          return;
      }
    }

    private List<T> FilterItemView(string filter, out int selectedIndex)
    {
      List<T> selectedItems = new List<T>();
      selectedIndex = -1;
      bool flagChange = false;
      foreach (KeyValuePair<int, T> indexToItem in itemDictionary)
      {
        bool lastState = dataGridView.Rows[indexToItem.Key].Visible;
        T item = indexToItem.Value;
        if (!string.IsNullOrEmpty(filter))
        {
          filter = filter.ToLower();
          foreach (string value in item.ToDisplayValues())
          {
            if(string.IsNullOrEmpty(value))
              continue;
            if(value.ToLower().Contains(filter))
            {
              dataGridView.Rows[indexToItem.Key].Visible = true;
              selectedItems.Add(item);
              if (selectedIndex < 0)
                selectedIndex = indexToItem.Key;
              break;
            }
            dataGridView.Rows[indexToItem.Key].Visible = false;
          }
        }
        else
        {
          dataGridView.Rows[indexToItem.Key].Visible = true;
          if (selectedIndex < 0)
            selectedIndex = indexToItem.Key;
        }
        if (!flagChange && lastState != dataGridView.Rows[indexToItem.Key].Visible)
          flagChange = true;
      }

      if (selectedIndex >= 0 && flagChange)
      {
        dataGridView.ClearSelection();
        dataGridView.Rows[selectedIndex].Selected = true;
        dataGridView.FirstDisplayedScrollingRowIndex = selectedIndex;
      }
      return selectedItems;
    }

    public void ClearFilter()
    {
      textBoxSearch.Text = string.Empty;
      int selectedIndex;
      FilterItemView(string.Empty, out selectedIndex);
    }

    private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
    {
      int selectedIndex;
      //Note: think later.
      foreach (KeyValuePair<char, bool> specialKey in _specialKeysEvent)
      {
        if (specialKey.Value && textBoxSearch.Text.IndexOf(specialKey.Key) >= 0)
        {
          textBoxSearch.Text = textBoxSearch.Text.Remove(textBoxSearch.Text.IndexOf(specialKey.Key), 1);
          e.Handled = false;
          return;
        }
      }
      if (e.KeyData == Keys.Down)
      {
        SelectNextVisibleRow();
      }
      else if (e.KeyData == Keys.Up)
      {
        SelectPrevVisibleRow();
      }
      else if (e.KeyData == Keys.Left || e.KeyData == Keys.Right)
      {
        e.Handled = true;
      }
      else
      {
        List<T> filteredItems = FilterItemView(textBoxSearch.Text.Trim(), out selectedIndex);
        //Comment here if barcode needed to be keypress
        if (filteredItems != null && e.KeyData == Keys.Enter)
        {
          if (EnterKeyPressedEvent != null)
            EnterKeyPressedEvent(this, new SpecialKeyEventArgs<T>((char)13,filteredItems));
          e.Handled = true;
        }
      }
    }

    private void resetToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void allToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }
  }
}

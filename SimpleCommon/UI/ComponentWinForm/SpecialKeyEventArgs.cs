using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCommon.UI.ComponentWinForm
{
  public class SpecialKeyEventArgs<T> : EventArgs
  {
    public SpecialKeyEventArgs (char keyChar, List<T> selectedItems )
    {
      KeyChar = keyChar;
      SelectedItems = selectedItems;
    }
    public char KeyChar { get; set; }
    public List<T> SelectedItems { get; set; }
  }
}

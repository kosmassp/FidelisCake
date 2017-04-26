using System.Drawing;
using System.Windows.Forms;

namespace SimpleCommon.Utility
{
  public class ControlUtility
  {
    public static void HideTabHeader(TabControl tabControl)
    {
      tabControl.Appearance = TabAppearance.Normal;
      tabControl.ItemSize = new Size(0, 1);
      tabControl.SizeMode = TabSizeMode.Fixed;
    }
  }
}

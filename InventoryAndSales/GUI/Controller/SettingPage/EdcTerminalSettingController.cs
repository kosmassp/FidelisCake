using System;
using System.Collections.Generic;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Popup.SettingPage;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the EDC terminal settings page: the list of card terminals a cashier can pick from.
  /// </summary>
  internal class EdcTerminalSettingController
  {
    private readonly EdcTerminalSettingForm _view;
    private readonly EdcTerminalService _terminals;

    public EdcTerminalSettingController(EdcTerminalSettingForm view)
    {
      _view = view;
      _terminals = BusinessFactory.GetInstance().EdcTerminals;
    }

    public List<string> GetTerminals()
    {
      return _terminals.GetTerminals();
    }

    /// <summary>Empty when the name can be added, otherwise a message for the operator.</summary>
    public string ValidateNew(string terminal, IEnumerable<string> current)
    {
      if (string.IsNullOrWhiteSpace(terminal))
        return "Nama terminal belum diisi.";

      string trimmed = terminal.Trim();
      if (trimmed.Length > 50)
        return "Nama terminal terlalu panjang (maksimal 50 karakter).";

      foreach (string existing in current)
      {
        if (string.Equals(existing, trimmed, StringComparison.OrdinalIgnoreCase))
          return "Terminal tersebut sudah ada.";
      }
      return string.Empty;
    }

    public void Save(IEnumerable<string> terminals)
    {
      _terminals.SetTerminals(terminals);
    }
  }
}

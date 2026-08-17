using System;
using System.Collections.Generic;
using InventoryAndSales.Business;
using InventoryAndSales.GUI.Popup.SettingPage;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the payment options settings page: the EDC terminals and QRIS providers a cashier can
  /// pick from.
  /// </summary>
  internal class PaymentOptionSettingController
  {
    private readonly PaymentOptionSettingForm _view;
    private readonly PaymentOptionService _options;

    public PaymentOptionSettingController(PaymentOptionSettingForm view)
    {
      _view = view;
      _options = BusinessFactory.GetInstance().PaymentOptions;
    }

    public List<string> GetEdcTerminals()
    {
      return _options.GetEdcTerminals();
    }

    public List<string> GetQrisProviders()
    {
      return _options.GetQrisProviders();
    }

    /// <summary>Empty when the name can be added, otherwise a message for the operator.</summary>
    public string ValidateNew(string name, IEnumerable<string> current)
    {
      if (string.IsNullOrWhiteSpace(name))
        return "Nama belum diisi.";

      string trimmed = name.Trim();
      if (trimmed.Length > 50)
        return "Nama terlalu panjang (maksimal 50 karakter).";

      foreach (string existing in current)
      {
        if (string.Equals(existing, trimmed, StringComparison.OrdinalIgnoreCase))
          return "Nama tersebut sudah ada.";
      }
      return string.Empty;
    }

    public void Save(IEnumerable<string> edcTerminals, IEnumerable<string> qrisProviders)
    {
      _options.SetEdcTerminals(edcTerminals);
      _options.SetQrisProviders(qrisProviders);
    }
  }
}

using InventoryAndSales.Business;
using InventoryAndSales.GUI.Popup.SettingPage;

namespace InventoryAndSales.GUI.Controller.SettingPage
{
  /// <summary>
  /// Backs the shop settings page: what this shop is called.
  /// </summary>
  internal class ShopSettingController
  {
    private readonly ShopSettingForm _view;
    private readonly ShopService _shop;

    public ShopSettingController(ShopSettingForm view)
    {
      _view = view;
      _shop = BusinessFactory.GetInstance().Shop;
    }

    public string GetName()
    {
      return _shop.GetName();
    }

    /// <summary>True when the shown name is only inherited from the receipt header.</summary>
    public bool IsNameInherited()
    {
      return _shop.IsNameInherited();
    }

    public string ValidateName(string name)
    {
      return _shop.ValidateName(name);
    }

    public void Save(string name)
    {
      _shop.SetName(name);
    }
  }
}

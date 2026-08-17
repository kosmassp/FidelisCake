using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Business.Enum;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Controller
{
  public class CashierController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>Customers are not implemented yet; every sale is booked against this placeholder.</summary>
    private const long PlaceholderCustomerId = 1;

    private readonly CashierPage _control;
    private readonly LoginManager _loginManager;
    private readonly CashierManager _cashierManager;
    private readonly MasterManager _masterManager;

    /// <summary>This screen's basket. Not shared with the correction screen.</summary>
    private readonly Cart _cart = new Cart();

    public CashierController(CashierPage cashierControl)
    {
      _control = cashierControl;

      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;

      _cart.CartChange += CartChange;
    }

    private void CartChange(object sender, KeyValuePair<Product, int> args)
    {
      _control.UpdateDataGridViewCart(args.Key, args.Value);
      decimal totalPrice, totalDiscount;
      decimal total = _cart.GetTotal(out totalPrice, out totalDiscount);
      _control.UpdateTotal(total);
    }

    public List<Product> GetItems()
    {
      return _masterManager.GetAllAvailable(string.Empty, string.Empty);
    }

    /// <summary>
    /// Validates and completes the sale.
    /// </summary>
    /// <returns>An Indonesian error message, or an empty string when the sale went through.</returns>
    public string Checkout(decimal payment, string notes, out string successMessage)
    {
      successMessage = string.Empty;

      if (_loginManager.ActiveUser == null)
        return "Sesi telah berakhir. Silahkan login kembali.";

      if (payment < 0)
        return "Pembayaran kurang dari 0";

      decimal totalPrice, totalDiscount;
      decimal total = _cart.GetTotal(out totalPrice, out totalDiscount);
      if (total <= 0)
        return "Tidak ada pembelian. Silahkan tambahkan item yang dibeli";

      decimal changes = payment - total;
      if (changes < 0)
        return "Pembayaran kurang dari harga yang harus dibayarkan.";

      try
      {
        string message;
        TransactionStatus status = _cashierManager.Checkout(
          _cart, payment, notes, _loginManager.ActiveUser.Id, PlaceholderCustomerId, out message);

        if (status != TransactionStatus.SUCCESS)
          return message;

        successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ",
                                       changes.ToString(Constant.DISPLAY_CURRENCY));
        if (!string.IsNullOrEmpty(message))
          successMessage += "\n " + message;
        NewCart();
      }
      catch (Exception e)
      {
        // The cart is deliberately left alone so the cashier can retry without rescanning.
        _log.Error("Checkout failed.", e);
        return "Transaksi gagal karena kesalahan sistem. Silahkan coba lagi.";
      }
      return string.Empty;
    }

    public void AddToCart(Product product)
    {
      _cart.Add(product, 1);
    }

    public void RemoveFromCart(Product product)
    {
      _cart.Add(product, -1);
    }

    public void NewCart()
    {
      _cart.Clear();
      _control.ResetCart();
    }

    public void UpdateCart(Product product, int value)
    {
      _cart.SetQuantity(product, value);
    }
  }
}

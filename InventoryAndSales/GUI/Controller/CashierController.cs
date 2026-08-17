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
    private readonly PaymentOptionService _paymentOptions;

    /// <summary>This screen's basket. Not shared with the correction screen.</summary>
    private readonly Cart _cart = new Cart();

    public CashierController(CashierPage cashierControl)
    {
      _control = cashierControl;

      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      _paymentOptions = BusinessFactory.GetInstance().PaymentOptions;

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

    /// <summary>The terminals a cashier can choose from. Empty means this shop takes no cards.</summary>
    public List<string> GetEdcTerminals()
    {
      return _paymentOptions.GetEdcTerminals();
    }

    /// <summary>The QRIS providers a cashier can choose from. Empty means this shop takes no QRIS.</summary>
    public List<string> GetQrisProviders()
    {
      return _paymentOptions.GetQrisProviders();
    }

    /// <summary>
    /// Whether a method can be offered at all. A method whose list is empty is not shown, because
    /// choosing it could never lead to a completed sale.
    /// </summary>
    public bool IsMethodAvailable(PaymentMethod method)
    {
      switch (method)
      {
        case PaymentMethod.Edc: return _paymentOptions.HasEdcTerminals();
        case PaymentMethod.Qris: return _paymentOptions.HasQrisProviders();
        default: return true;
      }
    }

    /// <summary>Amount owed, so the screen can show it and a card payment can take exactly that.</summary>
    public decimal GetCartTotal()
    {
      decimal totalPrice, totalDiscount;
      return _cart.GetTotal(out totalPrice, out totalDiscount);
    }

    /// <summary>
    /// Validates and completes the sale.
    /// </summary>
    /// <param name="method">How the customer is paying.</param>
    /// <param name="tendered">Cash handed over. Ignored for a card payment, which takes the total.</param>
    /// <param name="terminal">Terminal for a card payment.</param>
    /// <returns>An Indonesian error message, or an empty string when the sale went through.</returns>
    public string Checkout(PaymentMethod method, decimal tendered, string reference, QrisMode qrisMode,
                           string notes, out string successMessage)
    {
      successMessage = string.Empty;

      if (_loginManager.ActiveUser == null)
        return "Sesi telah berakhir. Silahkan login kembali.";

      decimal totalPrice, totalDiscount;
      decimal total = _cart.GetTotal(out totalPrice, out totalDiscount);
      if (total <= 0)
        return "Tidak ada pembelian. Silahkan tambahkan item yang dibeli";

      PaymentDetail payment;
      switch (method)
      {
        case PaymentMethod.Edc:
          if (string.IsNullOrWhiteSpace(reference))
            return "Silahkan pilih terminal EDC.";
          // Re-checked here rather than trusting the screen: the list can be edited while a sale is
          // being rung up, and a payment must never be recorded against a terminal the shop dropped.
          if (!_paymentOptions.IsKnownEdcTerminal(reference))
            return "Terminal EDC tersebut tidak terdaftar. Silahkan pilih ulang.";
          payment = PaymentDetail.Edc(total, reference);
          break;

        case PaymentMethod.Qris:
          if (string.IsNullOrWhiteSpace(reference))
            return "Silahkan pilih provider QRIS.";
          if (!_paymentOptions.IsKnownQrisProvider(reference))
            return "Provider QRIS tersebut tidak terdaftar. Silahkan pilih ulang.";
          payment = PaymentDetail.Qris(total, reference, qrisMode);
          break;

        default:
          if (tendered < 0)
            return "Pembayaran kurang dari 0";
          if (tendered - total < 0)
            return "Pembayaran kurang dari harga yang harus dibayarkan.";
          payment = PaymentDetail.Cash(tendered);
          break;
      }

      try
      {
        string message;
        TransactionStatus status = _cashierManager.Checkout(
          _cart, payment, notes, _loginManager.ActiveUser.Id, PlaceholderCustomerId, out message);

        if (status != TransactionStatus.SUCCESS)
          return message;

        successMessage = PaymentDetail.IsExactAmount(payment.Method)
          ? string.Format("Transaksi Berhasil. \n{0} {1} Rp {2}. ",
                          payment.DisplayName, reference, total.ToString(Constant.DISPLAY_CURRENCY))
          : string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ",
                          payment.ChangeFor(total).ToString(Constant.DISPLAY_CURRENCY));
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

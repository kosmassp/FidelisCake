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
    private CashierPage _control;
    private LoginManager _loginManager;
    private CashierManager _cashierManager;
    private MasterManager _masterManager;


    public CashierController(CashierPage cashierControl)
    {
      _control = cashierControl;

      _loginManager = BusinessFactory.GetInstance().LoginManager;
      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      //Event
      _cashierManager.CartChange += CartChange;

    }

    private void CartChange(object sender, KeyValuePair<Product, int> args)
    {
      _control.UpdateDataGridViewCart(args.Key, args.Value);
      decimal x, y;
      decimal total = _cashierManager.GetCartTotal(out x, out y);
      _control.UpdateTotal(total);
    }




    public List<Product> GetItems()
    {
      return _masterManager.GetAllAvailable(string.Empty, string.Empty);
    }

    public string Checkout(decimal payment, string notes, out string successMessage)
    {
      successMessage = string.Empty;
      if (payment < 0)
        return "Pembayaran kurang dari 0";

      decimal x, y;
      decimal total = _cashierManager.GetCartTotal(out x, out y);
      if (total <= 0)
        return "Tidak ada pembelian. Silahkan tambahkan item yang dibeli";
      decimal changes = payment - total;

      if (changes < 0)
        return "Pembayaran kurang dari harga yang harus dibayarkan.";

      try
      {
        string message;
        TransactionStatus status = _cashierManager.Checkout(payment, notes, _loginManager.ActiveUser.Id, 1, out message);
        if (status == TransactionStatus.SUCCESS)
        {
          successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ",
                                         changes.ToString(Constant.DISPLAY_CURRENCY));
          successMessage += "\n " + message;
          NewCart();
        } 
        else
        {
          return message;
        }
      }
      catch (Exception e)
      {
        return e.Message + Environment.NewLine + e.StackTrace;
      }
      return string.Empty;
    }


    public void AddToCart(Product product)
    {
      _cashierManager.AddToCart(product, 1);
    }

    public void RemoveFromCart(Product product)
    {
      _cashierManager.AddToCart(product, -1);
    }

    public void NewCart()
    {
      _cashierManager.NewCart();
      _control.ResetCart();
    }

    public void UpdateCart(Product product, int value)
    {
      _cashierManager.UpdateItemCart(product, value);
    }

  }
}

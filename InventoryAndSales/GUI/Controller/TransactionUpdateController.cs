using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InventoryAndSales.Business;
using InventoryAndSales.Database.Model;
using InventoryAndSales.GUI.Page;
using InventoryAndSales.GUI.Utility;
using SimpleCommon.Utility;

namespace InventoryAndSales.GUI.Controller
{
  public class TransactionUpdateController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private TransactionUpdatePage _view;
    private User _supervisor;
    private CashierManager _cashierManager;
    private MasterManager _masterManager;

    public Transaction OriginalTransaction { get; private set; }
    private List<TransactionDetail> _originalTransactionDetails;

    public TransactionUpdateController(TransactionUpdatePage transactionUpdatePage)
    {
      _view = transactionUpdatePage;

      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;
      //Event
    }

    public void Init(string facturNumber, User user)
    {
      _supervisor = user;
      OriginalTransaction = _cashierManager.GetTransaction(facturNumber, out _originalTransactionDetails);
      _cashierManager.CartChange += CartChange;
    }

    ~TransactionUpdateController()
    {
      UnregisterEvent();
    }

    private void UnregisterEvent()
    {
      _cashierManager.CartChange -= CartChange;
    }


    private void CartChange(object sender, KeyValuePair<Product, int> args)
    {
      _view.UpdateDataGridViewCart(args.Key, args.Value);
      decimal x, y;
      decimal total = _cashierManager.GetCartTotal(out x, out y);
      _view.UpdateTotal(total);
    }




    public List<Product> GetItems()
    {
      return _masterManager.GetAllProduct();
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
        _cashierManager.UpdateCheckout(OriginalTransaction, payment, notes, _supervisor.Id, 1);

        successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ", changes.ToString(Constant.DISPLAY_CURRENCY));
      }
      catch (Exception e)
      {
        _log.Error(e);
        return e.Message + Environment.NewLine + e.StackTrace;
      }
      return string.Empty;
    }


    public void AddToCart(Product product)
    {
      _cashierManager.AddToCart(product, 1);
    }

    public void NewCart()
    {
      _cashierManager.NewCart();
      _view.ResetCart();
    }

    public void UpdateCart(Product product, int value)
    {
      _cashierManager.UpdateItemCart(product, value);
    }

    public void ResetByTransaction()
    {
      if (_originalTransactionDetails != null)
      {
        List<Product> p = GetItems();
        Dictionary<int, int> map = new Dictionary<int, int>();
        foreach (TransactionDetail td in _originalTransactionDetails)
        {
          if (td.Quantity > 0)
            map.Add(td.ProductId, td.Quantity);
        }

        foreach (Product product in p)
        {
          if (map.ContainsKey(product.Id))
          {
            UpdateCart(product, map[product.Id]);
          }
        }
      }
    }

    public void Unload()
    {
      UnregisterEvent();
    }
  }
}

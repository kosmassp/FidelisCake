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
  /// <summary>
  /// Backs the correction screen. Behaves like the sale screen, except that completing it writes a
  /// revision of an existing sale and credits it to the supervisor who authorised the change.
  /// </summary>
  public class TransactionUpdateController
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private const long PlaceholderCustomerId = 1;

    private readonly TransactionUpdatePage _view;
    private readonly CashierManager _cashierManager;
    private readonly MasterManager _masterManager;

    /// <summary>This screen's own basket - editing it must not disturb the sale screen.</summary>
    private readonly Cart _cart = new Cart();

    private User _supervisor;
    private List<TransactionDetail> _originalTransactionDetails;

    public Transaction OriginalTransaction { get; private set; }

    public TransactionUpdateController(TransactionUpdatePage transactionUpdatePage)
    {
      _view = transactionUpdatePage;

      _masterManager = BusinessFactory.GetInstance().MasterManager;
      _cashierManager = BusinessFactory.GetInstance().CashierManager;

      _cart.CartChange += CartChange;
    }

    public void Init(string facturNumber, User user)
    {
      _supervisor = user;
      OriginalTransaction = _cashierManager.GetTransaction(facturNumber, out _originalTransactionDetails);
      if (OriginalTransaction == null)
        throw new InvalidOperationException(string.Format("No transaction found for faktur {0}.", facturNumber));
    }

    private void CartChange(object sender, KeyValuePair<Product, int> args)
    {
      _view.UpdateDataGridViewCart(args.Key, args.Value);
      decimal totalPrice, totalDiscount;
      decimal total = _cart.GetTotal(out totalPrice, out totalDiscount);
      _view.UpdateTotal(total);
    }

    /// <summary>
    /// Every product, soft deleted ones included, so a line referring to a withdrawn product can
    /// still be loaded back onto the screen.
    /// </summary>
    public List<Product> GetItems()
    {
      return _masterManager.GetAllProduct();
    }

    public string Checkout(decimal payment, string notes, out string successMessage)
    {
      successMessage = string.Empty;

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
        _cashierManager.UpdateCheckout(_cart, OriginalTransaction, payment, notes, _supervisor.Id, PlaceholderCustomerId);
        successMessage = string.Format("Transaksi Berhasil. \nKembalian Rp {0}. ", changes.ToString(Constant.DISPLAY_CURRENCY));
      }
      catch (Exception e)
      {
        _log.Error("Transaction correction failed.", e);
        return "Perubahan transaksi gagal disimpan. Silahkan coba lagi.";
      }
      return string.Empty;
    }

    public void AddToCart(Product product)
    {
      _cart.Add(product, 1);
    }

    public void NewCart()
    {
      _cart.Clear();
      _view.ResetCart();
    }

    public void UpdateCart(Product product, int value)
    {
      _cart.SetQuantity(product, value);
    }

    /// <summary>
    /// Refills the basket from the sale being corrected.
    ///
    /// Quantities are replayed through the live catalogue, so the corrected sale is priced at
    /// today's prices rather than the prices originally charged.
    /// </summary>
    public void ResetByTransaction()
    {
      if (_originalTransactionDetails == null)
        return;

      Dictionary<int, int> quantityByProduct = new Dictionary<int, int>();
      foreach (TransactionDetail td in _originalTransactionDetails)
      {
        if (td.Quantity <= 0)
          continue;
        // Defensive: a sale should never hold the same product twice, but a hand edited database
        // could, and duplicate keys would throw.
        if (quantityByProduct.ContainsKey(td.ProductId))
          quantityByProduct[td.ProductId] += td.Quantity;
        else
          quantityByProduct.Add(td.ProductId, td.Quantity);
      }

      foreach (Product product in GetItems())
      {
        int quantity;
        if (quantityByProduct.TryGetValue(product.Id, out quantity))
          UpdateCart(product, quantity);
      }
    }
  }
}

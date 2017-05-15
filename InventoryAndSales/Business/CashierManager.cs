using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  public class CashierManager
  {
    private readonly TransactionManager _transactionManager;
    public CashierManager(TransactionManager transactionManager)
    {
      _transactionManager = transactionManager;
      _cart = new Dictionary<int, TransactionDetail>();
    }



    public decimal GetCartTotal(out decimal totalPrice, out decimal totalDiscount)
    {
      totalPrice = 0;
      totalDiscount = 0;
      foreach (KeyValuePair<int, TransactionDetail> cartItem in _cart)
      {
        totalPrice += cartItem.Value.SubtotalPrice;
        totalDiscount += cartItem.Value.SubtotalDiscount;
      }
      return totalPrice - totalDiscount;
    }
    private Dictionary<int, TransactionDetail> _cart;
    private readonly object _lockCart = new object();
    
    public delegate void CartChangeDelegate(object sender, KeyValuePair<Product, int> args);
    public event CartChangeDelegate CartChange;
    public bool AddToCart(Product product, int quantity)
    {
      lock (_lockCart)
      {
        try
        {
          if (_cart.ContainsKey(product.Id))
            _cart[product.Id].UpdateQuantity(_cart[product.Id].Quantity + quantity);
          else
            _cart.Add(product.Id, new TransactionDetail(product, quantity));
          InvokeCartChanges(product, _cart[product.Id].Quantity);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    private void InvokeCartChanges(Product product, int quantity)
    {
      if(CartChange != null)
      {
        CartChange(this, new KeyValuePair<Product, int>(product, quantity));
      }
    }

    public bool UpdateItemCart(Product product, int quantity)
    {
      lock (_lockCart)
      {
        try
        {
          if (quantity <= 0)
            return RemoveItemCart(product);
          if (_cart.ContainsKey(product.Id))
            _cart[product.Id].UpdateQuantity(quantity);
          else
            _cart.Add(product.Id, new TransactionDetail(product, quantity));
          InvokeCartChanges(product, quantity);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }
    public void NewCart()
    {
      lock (_lockCart)
      {
        _cart.Clear();
      }
    }
    public bool RemoveItemCart(Product product)
    {
      lock (_lockCart)
      {
        try
        {
          if (_cart.ContainsKey(product.Id))
            _cart.Remove(product.Id);
          InvokeCartChanges(product, 0);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    public void Checkout(decimal payment, string notes, int userId, int customerId, out Transaction transaction, out List<TransactionDetail> transactionDetails)
    {
      transaction = new Transaction();
      transaction.TotalPrice = 0;
      transaction.TotalDiscount = 0;
      transaction.Total = 0;
      transaction.Notes = notes;
      transaction.Time = DateTime.Now;
      transaction.Factur = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
      transaction.Payment = payment;
      transaction.UserId = userId;
      transaction.CustomerId = customerId;
      transactionDetails =  new List<TransactionDetail>();
      foreach (KeyValuePair<int, TransactionDetail> detail in _cart)
      {
        TransactionDetail td = detail.Value;
        transaction.TotalDiscount += td.SubtotalDiscount;
        transaction.TotalPrice += td.SubtotalPrice;
        transaction.Total += (td.SubtotalPrice - td.SubtotalDiscount);
        transactionDetails.Add(td);
      }
      transaction.Exchange = transaction.Payment - transaction.Total;
      _transactionManager.SaveCompleteTransaction(transaction, transactionDetails);

    }
  }

}

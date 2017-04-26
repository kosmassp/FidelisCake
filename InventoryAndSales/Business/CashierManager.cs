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
    private static CashierManager _instance;
    public static CashierManager Instance
    {
      get
      {
        lock (_instance)
        {
          if (_instance == null)
            _instance = new CashierManager();
        }
        return _instance;
      }
    }

    private TransactionManager _transactionManager;
    public decimal GetCartTotal(out decimal totalPrice, out decimal totalDiscount)
    {
      totalPrice = 0;
      totalDiscount = 0;
      foreach (KeyValuePair<Item, int> cartItem in _cart)
      {
        totalPrice += cartItem.Key.Price * cartItem.Value;
        totalDiscount += cartItem.Key.DiscountAmount * cartItem.Value;
      }
      return totalPrice - totalDiscount;
    }
    private Dictionary<Item, int> _cart;
    private object lockCart = new object();
    
    public delegate void CartChangeDelegate(object sender, KeyValuePair<Item, int> args);
    public event CartChangeDelegate CartChange;
    public bool AddToCart(Item item, int quantity)
    {
      lock (lockCart)
      {
        try
        {
          if (_cart == null)
          {
            _cart = new Dictionary<Item, int>();
          }
          if (_cart.ContainsKey(item))
            _cart[item] = _cart[item] + quantity;
          else
            _cart.Add(item, quantity);
          InvokeCartChanges(item, _cart[item]);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    private void InvokeCartChanges(Item item, int quantity)
    {
      if(CartChange != null)
      {
        CartChange(this, new KeyValuePair<Item, int>(item, quantity));
      }
    }

    public bool UpdateItemCart(Item item, int quantity)
    {
      lock (lockCart)
      {
        try
        {
          if (_cart == null)
          {
            _cart = new Dictionary<Item, int>();
          }
          if (quantity <= 0)
            return RemoveItemCart(item);
          if (_cart.ContainsKey(item))
            _cart[item] = quantity;
          else
            _cart.Add(item, quantity);
          InvokeCartChanges(item, _cart[item]);
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
      lock (lockCart)
      {
        _cart.Clear();
      }
    }
    public bool RemoveItemCart(Item item)
    {
      lock (lockCart)
      {
        try
        {
          if (_cart == null)
          {
            _cart = new Dictionary<Item, int>();
          }
          if (_cart.ContainsKey(item))
            _cart.Remove(item);
          InvokeCartChanges(item, _cart[item]);
          return true;
        }
        catch (Exception e)
        {
          return false;
        }
      }
    }

    public void Checkout(decimal payment, string notes)
    {
      Transaction t = new Transaction();
      t.TotalPrice = 0;
      t.TotalDiscount = 0;
      t.Total = 0;
      t.Notes = notes;
      t.Time = DateTime.Now;
      t.Factur = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
      t.Payment = payment;
      foreach (KeyValuePair<Item, int> detail in _cart)
      {
        TransactionDetail td = new TransactionDetail(detail.Key, detail.Value, t);
        t.TotalDiscount += td.SubtotalDiscount;
        t.TotalPrice += td.SubtotalPrice;
        t.Total += (td.SubtotalPrice - td.SubtotalDiscount);
        t.TransactionDetails.Add(td);
      }
      _transactionManager.SaveUpdate(t);

    }
  }

}

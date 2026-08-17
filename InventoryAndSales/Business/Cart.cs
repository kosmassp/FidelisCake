using System;
using System.Collections.Generic;
using System.Linq;

using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// The basket being rung up on one screen.
  ///
  /// Each screen owns its own instance. It used to live on the CashierManager singleton, which meant
  /// opening the correction window silently replaced whatever the cashier was in the middle of
  /// ringing up, and both screens reacted to each other's changes.
  ///
  /// One entry per product - adding the same product again increases its quantity rather than
  /// creating a second line. Line arithmetic belongs to TransactionDetail.UpdateQuantity; this class
  /// only decides which lines exist.
  /// </summary>
  public class Cart
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly Dictionary<int, TransactionDetail> _items = new Dictionary<int, TransactionDetail>();
    private readonly object _lockItems = new object();

    public delegate void CartChangeDelegate(object sender, KeyValuePair<Product, int> args);

    /// <summary>Raised after every change, carrying the product and its new quantity.</summary>
    public event CartChangeDelegate CartChange;

    /// <summary>Adds to - or with a negative delta, subtracts from - the quantity of a line.</summary>
    public bool Add(Product product, int quantityDelta)
    {
      if (product == null)
        return false;

      lock (_lockItems)
      {
        try
        {
          TransactionDetail line;
          if (_items.TryGetValue(product.Id, out line))
          {
            line.UpdateQuantity(line.Quantity + quantityDelta);
          }
          else
          {
            // Nothing to subtract from a product that is not in the cart.
            if (quantityDelta <= 0)
              return true;
            line = new TransactionDetail(product, quantityDelta);
            _items.Add(product.Id, line);
          }
          RaiseCartChange(product, line.Quantity);
          return true;
        }
        catch (Exception e)
        {
          _log.Error(e);
          return false;
        }
      }
    }

    /// <summary>Sets an absolute quantity. Zero or less removes the line.</summary>
    public bool SetQuantity(Product product, int quantity)
    {
      if (product == null)
        return false;

      lock (_lockItems)
      {
        try
        {
          if (quantity <= 0)
            return RemoveInternal(product);

          TransactionDetail line;
          if (_items.TryGetValue(product.Id, out line))
            line.UpdateQuantity(quantity);
          else
            _items.Add(product.Id, new TransactionDetail(product, quantity));

          RaiseCartChange(product, quantity);
          return true;
        }
        catch (Exception e)
        {
          _log.Error(e);
          return false;
        }
      }
    }

    public bool Remove(Product product)
    {
      if (product == null)
        return false;
      lock (_lockItems)
      {
        return RemoveInternal(product);
      }
    }

    private bool RemoveInternal(Product product)
    {
      try
      {
        _items.Remove(product.Id);
        RaiseCartChange(product, 0);
        return true;
      }
      catch (Exception e)
      {
        _log.Error(e);
        return false;
      }
    }

    /// <summary>
    /// Empties the cart. Deliberately silent - the caller resets its own grid, and raising an event
    /// per removed line would make it flicker.
    /// </summary>
    public void Clear()
    {
      lock (_lockItems)
      {
        _items.Clear();
      }
    }

    /// <summary>
    /// Amount owed, with the gross and the discount handed back separately for the receipt.
    /// </summary>
    public decimal GetTotal(out decimal totalPrice, out decimal totalDiscount)
    {
      decimal price = 0;
      decimal discount = 0;
      lock (_lockItems)
      {
        foreach (KeyValuePair<int, TransactionDetail> item in _items)
        {
          price += item.Value.SubtotalPrice;
          discount += item.Value.SubtotalDiscount;
        }
      }
      totalPrice = price;
      totalDiscount = discount;
      return price - discount;
    }

    /// <summary>
    /// The lines to be sold.
    ///
    /// A copy of the list, not of the lines themselves - checkout runs synchronously on the UI
    /// thread and clears the cart straight afterwards, so the caller is free to stamp TransactionId
    /// onto them. Do not hold on to the result past the save.
    /// </summary>
    public List<TransactionDetail> GetLines()
    {
      lock (_lockItems)
      {
        return _items.Values.ToList();
      }
    }

    public bool IsEmpty
    {
      get
      {
        lock (_lockItems)
        {
          return _items.Count == 0;
        }
      }
    }

    private void RaiseCartChange(Product product, int quantity)
    {
      CartChangeDelegate handler = CartChange;
      if (handler != null)
        handler(this, new KeyValuePair<Product, int>(product, quantity));
    }
  }
}

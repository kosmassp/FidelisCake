using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  /// <summary>One basket put aside so the till can serve somebody else.</summary>
  public class HeldCart
  {
    /// <summary>1-based, as shown to the cashier.</summary>
    public int Slot { get; private set; }

    /// <summary>Whatever the cashier had typed in the notes box, to tell one hold from another.</summary>
    public string Label { get; private set; }

    public DateTime HeldAt { get; private set; }
    public List<KeyValuePair<Product, int>> Lines { get; private set; }

    public HeldCart(int slot, string label, List<KeyValuePair<Product, int>> lines, DateTime heldAt)
    {
      Slot = slot;
      Label = (label ?? string.Empty).Trim();
      Lines = lines ?? new List<KeyValuePair<Product, int>>();
      HeldAt = heldAt;
    }

    public int ItemCount
    {
      get
      {
        int count = 0;
        foreach (KeyValuePair<Product, int> line in Lines)
          count += line.Value;
        return count;
      }
    }

    public decimal Total
    {
      get
      {
        decimal total = 0;
        foreach (KeyValuePair<Product, int> line in Lines)
          total += line.Key.NetPrice * line.Value;
        return total;
      }
    }

    /// <summary>
    /// What the cashier reads in the dropdown. Leads with the slot number, then enough to recognise
    /// whose basket it is - the note if there is one, otherwise the amount and the time it was held.
    /// </summary>
    public override string ToString()
    {
      string summary = string.Format(CultureInfo.CurrentCulture, "{0} brg - {1}",
                                     ItemCount, Total.ToString("N0"));
      string who = string.IsNullOrEmpty(Label) ? HeldAt.ToString("HH:mm") : Label;
      return string.Format("{0}. {1} ({2})", Slot, who, summary);
    }
  }

  /// <summary>
  /// Baskets set aside mid-sale, so a customer who cannot pay yet does not block the queue.
  ///
  /// Deliberately in memory only and for the length of one shift: a held basket is a customer
  /// standing at the counter, not a record. <see cref="Clear"/> is called whenever the signed-in user
  /// changes, so nothing survives a logout and no cashier inherits another's holds.
  /// </summary>
  public class HeldCartService
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>How many baskets can be set aside at once.</summary>
    public const int MaxSlots = 10;

    private readonly Dictionary<int, HeldCart> _held = new Dictionary<int, HeldCart>();
    private readonly object _lock = new object();

    /// <summary>Held baskets, lowest slot first.</summary>
    public List<HeldCart> GetAll()
    {
      lock (_lock)
      {
        return _held.Values.OrderBy(h => h.Slot).ToList();
      }
    }

    public int Count
    {
      get { lock (_lock) { return _held.Count; } }
    }

    public bool IsFull
    {
      get { return Count >= MaxSlots; }
    }

    /// <summary>
    /// Sets a basket aside in the lowest free slot.
    /// </summary>
    /// <returns>The slot used, or null when all of them are taken.</returns>
    public HeldCart Hold(List<KeyValuePair<Product, int>> lines, string label)
    {
      if (lines == null || lines.Count == 0)
        return null;

      lock (_lock)
      {
        for (int slot = 1; slot <= MaxSlots; slot++)
        {
          if (_held.ContainsKey(slot))
            continue;
          HeldCart held = new HeldCart(slot, label, lines, DateTime.Now);
          _held[slot] = held;
          _log.InfoFormat("Cart held in slot {0}: {1} items.", slot, held.ItemCount);
          return held;
        }
      }
      return null;
    }

    /// <summary>
    /// Takes a basket back out. The slot is freed by doing so - a recalled basket is on the screen,
    /// not on hold, and leaving a copy behind is how a sale gets rung up twice.
    /// </summary>
    public HeldCart Recall(int slot)
    {
      lock (_lock)
      {
        HeldCart held;
        if (!_held.TryGetValue(slot, out held))
          return null;
        _held.Remove(slot);
        _log.InfoFormat("Cart recalled from slot {0}.", slot);
        return held;
      }
    }

    /// <summary>Abandons a held basket without putting it back on the screen.</summary>
    public bool Discard(int slot)
    {
      lock (_lock)
      {
        if (!_held.Remove(slot))
          return false;
        _log.InfoFormat("Held cart in slot {0} discarded.", slot);
        return true;
      }
    }

    /// <summary>Drops every hold. Called when the signed-in user changes.</summary>
    public void Clear()
    {
      lock (_lock)
      {
        if (_held.Count > 0)
          _log.InfoFormat("Discarding {0} held cart(s) - session ended.", _held.Count);
        _held.Clear();
      }
    }
  }
}

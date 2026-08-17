using System;
using System.Collections.Generic;
using System.Globalization;
using InventoryAndSales.Database.Manager;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Product and user master data. Every change here is audited, because these are the records a
  /// shop asks about afterwards: who changed that price, who gave that account its rights.
  /// </summary>
  public class MasterManager
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly ProductManager _productManager;
    private readonly UserManager _userManager;
    private readonly AuditService _audit;

    public MasterManager(ProductManager productManager, UserManager userManager, AuditService audit)
    {
      _productManager = productManager;
      _userManager = userManager;
      _audit = audit;
    }

    public List<Product> GetAllProduct()
    {
      return _productManager.GetAll();
    }

    public List<Product> GetAllAvailable(string criteria, string orderBy)
    {
      if (string.IsNullOrEmpty(orderBy))
        return _productManager.GetAllAvailable(criteria);
      return _productManager.GetAllAvailable(criteria, orderBy);
    }

    public void AddProduct(Product product)
    {
      _productManager.Save(product);
      _log.InfoFormat("Product '{0}' added.", product.Code);
      _audit.Record(AuditService.ActionCreate, AuditService.EntityProduct, product.Code, Describe(product));
    }

    public void UpdateProduct(Product product)
    {
      // Read first so the entry can say what the value *was*. One extra read on a master edit is
      // cheap; an audit trail that only records the new price answers half the question.
      Product previous = FindProduct(product.Id);
      _productManager.Update(product);
      _log.InfoFormat("Product '{0}' updated.", product.Code);
      _audit.Record(AuditService.ActionUpdate, AuditService.EntityProduct, product.Code,
                    Change(previous == null ? null : Describe(previous), Describe(product)));
    }

    public void DeleteProduct(Product product)
    {
      product.Deleted = true;
      _productManager.Update(product);
      _log.InfoFormat("Product '{0}' deleted.", product.Code);
      _audit.Record(AuditService.ActionDelete, AuditService.EntityProduct, product.Code, Describe(product));
    }

    public List<User> GetUsers()
    {
      return _userManager.GetAll();
    }

    public void UpdateUser(User user)
    {
      User previous = FindUser(user.Id);
      _userManager.Update(user);
      _log.InfoFormat("User '{0}' updated.", user.Username);
      _audit.Record(AuditService.ActionUpdate, AuditService.EntityUser, user.Username,
                    Change(previous == null ? null : Describe(previous), Describe(user)));
    }

    public void DeleteUser(User user)
    {
      user.Deleted = true;
      _userManager.Update(user);
      _log.InfoFormat("User '{0}' deleted.", user.Username);
      _audit.Record(AuditService.ActionDelete, AuditService.EntityUser, user.Username, Describe(user));
    }

    public void AddUser(User user)
    {
      _userManager.Save(user);
      _log.InfoFormat("User '{0}' added.", user.Username);
      _audit.Record(AuditService.ActionCreate, AuditService.EntityUser, user.Username, Describe(user));
    }

    #region Audit detail

    /// <summary>
    /// A product as one readable line. Amounts are formatted invariantly: an audit row is read years
    /// later, possibly on another machine, and must not depend on that machine's culture.
    /// </summary>
    private static string Describe(Product product)
    {
      return string.Format(CultureInfo.InvariantCulture,
                           "nama='{0}', barcode='{1}', harga={2}, diskon={3}, dihapus={4}",
                           product.Name, product.Barcode, product.Price, product.Discount, product.Deleted);
    }

    /// <summary>
    /// A user as one readable line. **The password hash is deliberately not included** — an audit
    /// trail is read by more people than the user table is.
    /// </summary>
    private static string Describe(User user)
    {
      return string.Format(CultureInfo.InvariantCulture, "nama='{0}', role={1}, dihapus={2}",
                           user.Name, user.Role, user.Deleted);
    }

    private static string Change(string previous, string current)
    {
      if (previous == null)
        return current;
      if (string.Equals(previous, current, StringComparison.Ordinal))
        return current + " (tidak ada perubahan)";
      return previous + " -> " + current;
    }

    /// <summary>
    /// The stored row, or null when it cannot be read. Best effort on purpose: failing to describe
    /// the previous state must not stop the edit itself.
    /// </summary>
    private Product FindProduct(int id)
    {
      try
      {
        return _productManager.FindById(id);
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not read product {0} for the audit entry.", id), e);
        return null;
      }
    }

    private User FindUser(int id)
    {
      try
      {
        return _userManager.FindById(id);
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not read user {0} for the audit entry.", id), e);
        return null;
      }
    }

    #endregion
  }
}

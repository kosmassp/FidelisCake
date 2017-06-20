using System;

namespace InventoryAndSales.Enumeration
{
  [Flags]
  enum MenuAccess
  {
    Admin = 1,
    Cashier = 2,
    Master = 4,
    Laporan = 8
  }

  public enum RoleOptions
  {
    Admin = 1023,
    Cashier = 2,
  }
}

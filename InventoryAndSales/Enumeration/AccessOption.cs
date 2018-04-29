using System;

namespace InventoryAndSales.Enumeration
{
  [Flags]
  public enum AccessOption
  {
    Admin = 1,
    Cashier = 2,
    Master = 4,
    Laporan = 8
  }

  public enum RoleOptions
  {
    Admin = 1023,
    Cashier = AccessOption.Cashier,
    Supervisor = AccessOption.Cashier | AccessOption.Master | AccessOption.Laporan,
  }
}

using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class AuditLogManager : BaseManager<AuditLog>
  {
    public AuditLogManager(AuditLogDao dao)
      : base(dao)
    {
    }
  }
}

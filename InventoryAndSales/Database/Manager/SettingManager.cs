using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataAccess;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.Manager
{
  public class SettingConfigurationManager : BaseManager<SettingConfiguration>
  {
    public SettingConfigurationManager(SettingConfigurationDao dao)
      : base(dao)
    {
    }

    /// <summary>Rows for a setting key. KEY is a reserved word, hence the brackets.</summary>
    public List<SettingConfiguration> FindByKey(string key)
    {
      return BaseDao.FindByQuery(
        "WHERE [KEY] = @key",
        string.Empty,
        new SqlParameter("@key", SqlDbType.VarChar, 80) { Value = key });
    }
  }
}

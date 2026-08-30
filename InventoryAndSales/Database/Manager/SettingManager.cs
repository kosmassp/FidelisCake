using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

    /// <summary>Rows for a setting key. Key is a reserved word everywhere, hence the quoting.</summary>
    public List<SettingConfiguration> FindByKey(string key)
    {
      return BaseDao.FindByQuery(
        string.Format("WHERE {0} = @key", Dialect.Quote("Key")),
        string.Empty,
        DbParam.AnsiText("@key", 80, key));
    }
  }
}

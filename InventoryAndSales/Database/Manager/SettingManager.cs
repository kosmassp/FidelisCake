using System;
using System.Collections.Generic;
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

    public List<SettingConfiguration> FindByKey(string key)
    {
      List<SettingConfiguration> items = BaseDao.FindByQuery(string.Format("WHERE [KEY] = '{0}'", key));
      return items;
    }
  }
}

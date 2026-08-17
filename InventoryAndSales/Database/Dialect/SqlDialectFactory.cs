using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// Chooses the dialect from configuration and resolves the matching ADO.NET provider.
  ///
  /// The provider is looked up through <see cref="DbProviderFactories"/> rather than referenced at
  /// compile time, so the application keeps no dependency on Npgsql or System.Data.SQLite. A site
  /// that wants one of them drops the assembly beside the executable and registers it in App.config;
  /// everyone else is unaffected and the shipped binary is unchanged.
  /// </summary>
  public static class SqlDialectFactory
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>App.config key naming the database product. Defaults to SQL Server.</summary>
    public const string ProviderSettingKey = "DatabaseProvider";

    public static ISqlDialect Create()
    {
      string configured = ConfigurationManager.AppSettings[ProviderSettingKey];
      return Create(configured);
    }

    public static ISqlDialect Create(string providerName)
    {
      if (string.IsNullOrWhiteSpace(providerName))
        return new SqlServerDialect();

      string requested = providerName.Trim();
      foreach (ISqlDialect dialect in All())
      {
        if (string.Equals(dialect.Name, requested, StringComparison.OrdinalIgnoreCase))
          return dialect;
      }

      _log.ErrorFormat(
        "Unknown {0} '{1}'. Expected one of: {2}. Falling back to {3}.",
        ProviderSettingKey, requested, string.Join(", ", Names()), SqlServerDialect.DialectName);
      return new SqlServerDialect();
    }

    public static List<ISqlDialect> All()
    {
      return new List<ISqlDialect>
      {
        new SqlServerDialect(),
        new PostgreSqlDialect(),
        new SqliteDialect(),
      };
    }

    private static List<string> Names()
    {
      List<string> names = new List<string>();
      foreach (ISqlDialect dialect in All())
        names.Add(dialect.Name);
      return names;
    }

    /// <summary>
    /// Resolves the ADO.NET factory for a dialect.
    /// </summary>
    /// <exception cref="ConfigurationErrorsException">
    /// The provider is not registered. The message names the assembly the site has to install, since
    /// whoever hits this is setting up a machine rather than reading source.
    /// </exception>
    public static DbProviderFactory ResolveProviderFactory(ISqlDialect dialect)
    {
      try
      {
        return DbProviderFactories.GetFactory(dialect.ProviderInvariantName);
      }
      catch (Exception e)
      {
        string message = string.Format(
          "Database provider '{0}' for {1} is not registered. Install the provider and add it to " +
          "<system.data><DbProviderFactories> in App.config.",
          dialect.ProviderInvariantName, dialect.Name);
        _log.Error(message, e);
        throw new ConfigurationErrorsException(message, e);
      }
    }
  }
}

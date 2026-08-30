using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace InventoryAndSales.Database.Dialect
{
  /// <summary>
  /// Finds a provider's <see cref="DbProviderFactory"/> without anything having to be declared in
  /// App.config.
  ///
  /// Two pieces of configuration used to be required, and both are now handled here:
  ///
  ///  - a &lt;system.data&gt;&lt;DbProviderFactories&gt; registration. Not needed: every ADO.NET
  ///    provider exposes a public static Instance field on its factory - that is the convention
  ///    DbProviderFactories itself relies on - so the factory can be read straight off the type.
  ///
  ///  - &lt;assemblyBinding&gt; redirects for the assemblies Npgsql depends on, whose file versions
  ///    move faster than the versions it was compiled against. Not needed: <see cref="Install"/>
  ///    resolves those by simple name, which is what a wildcard redirect does.
  ///
  /// A provider therefore works by being present beside the executable. Nothing to register, nothing
  /// to keep in step, and no way for a site to get the config subtly wrong.
  /// </summary>
  internal static class ProviderLoader
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly object InstallLock = new object();
    private static bool _installed;

    /// <summary>
    /// Starts resolving assemblies the runtime cannot bind by exact version, matching them by simple
    /// name against whatever is beside the executable.
    ///
    /// Called before a provider is loaded. Safe to call repeatedly.
    /// </summary>
    internal static void Install()
    {
      if (_installed)
        return;
      lock (InstallLock)
      {
        if (_installed)
          return;
        AppDomain.CurrentDomain.AssemblyResolve += ResolveBySimpleName;
        _installed = true;
      }
    }

    /// <summary>
    /// Last-chance handler: the runtime asked for a specific version and could not find it. If a
    /// file of that simple name sits beside the executable, use it.
    ///
    /// Deliberately narrow - it only ever returns an assembly whose simple name is exactly the one
    /// requested, and only from the application's own folder.
    /// </summary>
    private static Assembly ResolveBySimpleName(object sender, ResolveEventArgs args)
    {
      try
      {
        AssemblyName requested = new AssemblyName(args.Name);

        // Resource lookups miss constantly and are not our problem.
        if (requested.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
          return null;

        string candidate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, requested.Name + ".dll");
        if (!File.Exists(candidate))
          return null;

        AssemblyName available = AssemblyName.GetAssemblyName(candidate);
        if (!string.Equals(available.Name, requested.Name, StringComparison.OrdinalIgnoreCase))
          return null;

        _log.InfoFormat("Resolved {0} to {1} v{2}.", args.Name, Path.GetFileName(candidate), available.Version);
        return Assembly.LoadFrom(candidate);
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not resolve assembly '{0}'.", args.Name), e);
        return null;
      }
    }

    /// <summary>
    /// The factory for a dialect.
    ///
    /// Loading the type directly is tried first so the provider shipped beside the executable is the
    /// one used. A DbProviderFactories registration still wins if a site has added one, which leaves
    /// the config route available for anyone who wants to point at a different build.
    /// </summary>
    internal static DbProviderFactory Resolve(ISqlDialect dialect)
    {
      Install();

      DbProviderFactory factory = FromFactoryType(dialect);
      if (factory != null)
        return factory;

      factory = FromConfiguredProviders(dialect);
      if (factory != null)
        return factory;

      throw new TypeLoadException(string.Format(
        "Database provider for {0} could not be loaded. Expected '{1}' beside {2}.",
        dialect.Name, FileNameOf(dialect), AppDomain.CurrentDomain.BaseDirectory));
    }

    private static DbProviderFactory FromFactoryType(ISqlDialect dialect)
    {
      try
      {
        Type factoryType = Type.GetType(dialect.ProviderFactoryTypeName, false, true);
        if (factoryType == null)
          return null;

        // The Instance member every ADO.NET factory exposes. Field on every provider shipped here,
        // but check for a property too rather than depend on that.
        FieldInfo field = factoryType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
        if (field != null)
          return field.GetValue(null) as DbProviderFactory;

        PropertyInfo property = factoryType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        if (property != null)
          return property.GetValue(null, null) as DbProviderFactory;

        _log.WarnFormat("Type '{0}' has no static Instance member.", dialect.ProviderFactoryTypeName);
        return null;
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not load provider type '{0}'.", dialect.ProviderFactoryTypeName), e);
        return null;
      }
    }

    private static DbProviderFactory FromConfiguredProviders(ISqlDialect dialect)
    {
      try
      {
        return DbProviderFactories.GetFactory(dialect.ProviderInvariantName);
      }
      catch (Exception)
      {
        // Not registered, which is the normal case now.
        return null;
      }
    }

    /// <summary>The assembly a site would be missing, for the error message.</summary>
    private static string FileNameOf(ISqlDialect dialect)
    {
      string[] parts = dialect.ProviderFactoryTypeName.Split(',');
      return parts.Length > 1 ? parts[1].Trim() + ".dll" : dialect.ProviderFactoryTypeName;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace InventoryAndSales.Database.Model
{
  public abstract class BaseObject
  {
    /// <summary>
    /// Return dictionary of table column and it's values
    /// </summary>
    /// <returns></returns>
    [IndexerName("DataColumn")]
    public abstract object this[string columnName] { get; set; }

    #region Value coercion

    // Providers do not agree on the CLR type a column comes back as. SQLite has one integer type
    // and hands back Int64 for everything from a bit flag to an identity; PostgreSQL returns Int32
    // for integer and Boolean for boolean; SQL Server returns Int32, Boolean and Decimal.
    //
    // The setters below used to cast directly - (int)value, (bool)value - which throws on a boxed
    // Int64 and made the models silently SQL-Server-only. Converting instead costs nothing and works
    // everywhere.

    protected static int ToInt(object value)
    {
      return Convert.ToInt32(value, CultureInfo.InvariantCulture);
    }

    protected static long ToLong(object value)
    {
      return Convert.ToInt64(value, CultureInfo.InvariantCulture);
    }

    protected static decimal ToDecimal(object value)
    {
      return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
    }

    protected static bool ToBool(object value)
    {
      // SQLite stores a flag as 0/1, so a plain cast would fail there.
      return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
    }

    protected static DateTime ToDateTime(object value)
    {
      return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
    }

    protected static string ToText(object value)
    {
      string text = value as string;
      if (text != null)
        return text;
      return value == null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Dialect;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  /// <summary>
  /// Generic CRUD driven by the column metadata in <see cref="DataTableList"/>.
  ///
  /// Values are always passed as parameters, never concatenated into the statement: it keeps a
  /// product name containing an apostrophe from corrupting the SQL, and it lets the provider deal
  /// with decimal, boolean and timestamp conversion instead of relying on the thread culture.
  ///
  /// Identifiers are quoted through the dialect, which is what makes reserved words such as Key and
  /// Group usable and keeps identifier case intact on PostgreSQL.
  /// </summary>
  public class BaseDao<T> where T : BaseObject, new()
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly IDataTable _dataTable;

    public BaseDao()
    {
      _dataTable = DataTableList.Instance.GetDataTable(typeof(T));
    }

    protected static ISqlDialect Dialect
    {
      get { return DBFactory.GetInstance().Dialect; }
    }

    public virtual T FindById(int id)
    {
      var result = FindByQuery(
        string.Format("WHERE {0} = @id", Dialect.Quote(_dataTable.PrimaryKeyColumn)),
        string.Empty,
        DbParam.Of("@id", id));
      if (result.Count > 0)
        return result[0];
      return null;
    }

    private const string FIND_BY_QUERY = "SELECT * FROM {0} {1}";

    public virtual List<T> FindByQuery(string whereClause)
    {
      return FindByQuery(whereClause, string.Empty);
    }

    public virtual List<T> FindByQuery(string whereClause, string orderbyClause)
    {
      return FindByQuery(whereClause, orderbyClause, new DbParameter[0]);
    }

    /// <summary>
    /// Runs a SELECT against this DAO's table. Any value inside <paramref name="whereClause"/> must
    /// be supplied as a named parameter - do not build the clause by interpolating user input.
    /// </summary>
    public virtual List<T> FindByQuery(string whereClause, string orderbyClause, params DbParameter[] parameters)
    {
      if (!string.IsNullOrEmpty(whereClause))
      {
        whereClause = whereClause.Trim();
        if (!whereClause.StartsWith("WHERE", true, CultureInfo.InvariantCulture))
          whereClause = "WHERE " + whereClause;
      }
      if (!string.IsNullOrEmpty(orderbyClause))
      {
        orderbyClause = orderbyClause.Trim();
        if (!orderbyClause.StartsWith("ORDER BY", true, CultureInfo.InvariantCulture))
          orderbyClause = "ORDER BY " + orderbyClause;
        orderbyClause = " " + orderbyClause;
      }
      string preparedSql = string.Format(FIND_BY_QUERY, Dialect.Quote(_dataTable.TableName), whereClause + orderbyClause);
      return ExecuteReader(preparedSql, parameters);
    }

    private const string INSERT_SQL = "INSERT INTO {0}({1}) VALUES ({2})";

    public virtual bool Save(T dataObject)
    {
      StringBuilder columns = new StringBuilder();
      StringBuilder values = new StringBuilder();
      List<DbParameter> parameters = new List<DbParameter>();

      foreach (string column in _dataTable.Columns)
      {
        if (column == _dataTable.PrimaryKeyColumn)
          continue;
        string parameterName = "@p" + parameters.Count;
        if (parameters.Count > 0)
        {
          columns.Append(",");
          values.Append(",");
        }
        columns.Append(Dialect.Quote(column));
        values.Append(parameterName);
        parameters.Add(DbParam.Of(parameterName, ToParameterValue(dataObject[column])));
      }

      string insertSql = string.Format(INSERT_SQL, Dialect.Quote(_dataTable.TableName), columns, values);

      // The generated key is read back by the same statement that inserts the row. Asking for it
      // afterwards, as a separate command, is the obvious shape and is wrong: a parameterised insert
      // travels as sp_executesql on SQL Server, so a later SCOPE_IDENTITY() is outside that scope
      // and comes back NULL - leaving every new row with an id of zero and every foreign key that
      // depends on it pointing at nothing.
      string insertWithIdentity = Dialect.AppendIdentityRetrieval(insertSql, _dataTable.PrimaryKeyColumn);
      object generatedId = DBUtility.ExecuteScalar(insertWithIdentity, parameters.ToArray());
      if (generatedId == null)
      {
        _log.ErrorFormat("Insert into {0} did not return a generated key.", _dataTable.TableName);
        return false;
      }

      dataObject[_dataTable.PrimaryKeyColumn] = NormalizeIdentity(generatedId);
      return true;
    }

    private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE {2} = @id";

    public virtual int Update(T dataObject)
    {
      StringBuilder columnValuePair = new StringBuilder();
      List<DbParameter> parameters = new List<DbParameter>();

      foreach (string column in _dataTable.Columns)
      {
        if (column == _dataTable.PrimaryKeyColumn)
          continue;
        string parameterName = "@p" + parameters.Count;
        if (parameters.Count > 0)
          columnValuePair.Append(",");
        columnValuePair.AppendFormat("{0}={1}", Dialect.Quote(column), parameterName);
        parameters.Add(DbParam.Of(parameterName, ToParameterValue(dataObject[column])));
      }
      parameters.Add(DbParam.Of("@id", dataObject[_dataTable.PrimaryKeyColumn]));

      string updateSql = string.Format(UPDATE_SQL, Dialect.Quote(_dataTable.TableName), columnValuePair,
                                       Dialect.Quote(_dataTable.PrimaryKeyColumn));
      return DBUtility.ExecuteNonQuery(updateSql, parameters.ToArray());
    }

    public virtual bool Delete(T dataObject)
    {
      return DeleteById((int)dataObject[_dataTable.PrimaryKeyColumn]);
    }

    private const string DELETE_SQL = "DELETE FROM {0} WHERE {1} = @id";

    public virtual bool DeleteById(int id)
    {
      string preparedSql = string.Format(DELETE_SQL, Dialect.Quote(_dataTable.TableName),
                                         Dialect.Quote(_dataTable.PrimaryKeyColumn));
      int delete = DBUtility.ExecuteNonQuery(preparedSql, DbParam.Of("@id", id));
      return delete > 0;
    }

    /// <summary>
    /// A null string used to be written as an empty string, because the old code wrapped every value
    /// in quotes. Installations rely on that - product Code and Barcode are read back without null
    /// checks in places - so keep writing an empty string rather than NULL.
    /// </summary>
    private static object ToParameterValue(object value)
    {
      if (value == null)
        return string.Empty;
      return value;
    }

    /// <summary>
    /// Boxes a generated identity as the type the model's indexer expects.
    ///
    /// Providers disagree about what the last-identity query returns - SQL Server hands back a
    /// decimal, SQLite a long. The int keyed models unbox with a direct (int) cast, which throws on
    /// anything else, while the bigint keyed ones parse whatever they are given, so box an int
    /// whenever the value fits and fall back to long.
    /// </summary>
    private static object NormalizeIdentity(object identity)
    {
      long value = Convert.ToInt64(identity, CultureInfo.InvariantCulture);
      if (value >= int.MinValue && value <= int.MaxValue)
        return (int)value;
      return value;
    }

    protected virtual List<T> ExecuteReader(String commandText, params DbParameter[] parameters)
    {
      DbConnection connection = DBFactory.GetInstance().GetConnection();
      DbTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      bool ownsConnection = activeTransaction == null;
      if (ownsConnection)
        connection.Open();
      try
      {
        List<T> returnList = new List<T>();
        using (DbCommand command = connection.CreateCommand())
        {
          command.CommandText = commandText;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          DBUtility.AddParameters(command, parameters);
          using (DbDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              T t = new T();
              // Driven by the column map rather than the result set, so a column that exists in the
              // database but is not mapped is simply ignored.
              foreach (string columnName in _dataTable.Columns)
              {
                object value = reader[columnName];
                if (!(value is DBNull))
                  t[columnName] = value;
              }
              returnList.Add(t);
            }
          }
        }
        return returnList;
      }
      catch (Exception ex)
      {
        _log.Error(string.Format("Trying to execute: {0}", commandText), ex);
        throw;
      }
      finally
      {
        // Without this the connection opened above was never returned to the pool, and a long
        // trading day would eventually exhaust it.
        if (ownsConnection)
          connection.Close();
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  /// <summary>
  /// Generic CRUD driven by the column metadata in <see cref="DataTableList"/>.
  ///
  /// Values are always passed as SqlParameters, never concatenated into the statement: it keeps a
  /// product name containing an apostrophe from corrupting the SQL, and it lets the driver deal with
  /// decimal, bit and datetime conversion instead of relying on the thread culture.
  /// </summary>
  public class BaseDao<T> where T : BaseObject, new()
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly IDataTable _dataTable;

    public BaseDao()
    {
      _dataTable = DataTableList.Instance.GetDataTable(typeof(T));
    }

    public virtual T FindById(int id)
    {
      var result = FindByQuery(
        string.Format("WHERE [{0}] = @id", _dataTable.PrimaryKeyColumn),
        string.Empty,
        new SqlParameter("@id", id));
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
      return FindByQuery(whereClause, orderbyClause, new SqlParameter[0]);
    }

    /// <summary>
    /// Runs a SELECT against this DAO's table. Any value inside <paramref name="whereClause"/> must
    /// be supplied as a named parameter - do not build the clause by interpolating user input.
    /// </summary>
    public virtual List<T> FindByQuery(string whereClause, string orderbyClause, params SqlParameter[] parameters)
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
      string preparedSql = string.Format(FIND_BY_QUERY, _dataTable.TableName, whereClause + orderbyClause);
      return ExecuteReader(preparedSql, parameters);
    }

    private const string INSERT_SQL = "INSERT INTO {0}({1}) VALUES ({2})";

    public virtual bool Save(T dataObject)
    {
      StringBuilder columns = new StringBuilder();
      StringBuilder values = new StringBuilder();
      List<SqlParameter> parameters = new List<SqlParameter>();

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
        columns.AppendFormat("[{0}]", column);
        values.Append(parameterName);
        parameters.Add(new SqlParameter(parameterName, ToParameterValue(dataObject[column])));
      }

      string insertSql = string.Format(INSERT_SQL, _dataTable.TableName, columns, values);
      int insert = DBUtility.ExecuteNonQuery(insertSql, parameters.ToArray());
      if (insert > 0)
      {
        object lastId = DBUtility.ExecuteScalar("SELECT SCOPE_IDENTITY()");
        if (lastId != null)
          dataObject[_dataTable.PrimaryKeyColumn] = NormalizeIdentity(lastId);
      }

      return insert > 0;
    }

    private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE [{2}] = @id";

    public virtual int Update(T dataObject)
    {
      StringBuilder columnValuePair = new StringBuilder();
      List<SqlParameter> parameters = new List<SqlParameter>();

      foreach (string column in _dataTable.Columns)
      {
        if (column == _dataTable.PrimaryKeyColumn)
          continue;
        string parameterName = "@p" + parameters.Count;
        if (parameters.Count > 0)
          columnValuePair.Append(",");
        columnValuePair.AppendFormat("[{0}]={1}", column, parameterName);
        parameters.Add(new SqlParameter(parameterName, ToParameterValue(dataObject[column])));
      }
      parameters.Add(new SqlParameter("@id", dataObject[_dataTable.PrimaryKeyColumn]));

      string updateSql = string.Format(UPDATE_SQL, _dataTable.TableName, columnValuePair, _dataTable.PrimaryKeyColumn);
      return DBUtility.ExecuteNonQuery(updateSql, parameters.ToArray());
    }

    public virtual bool Delete(T dataObject)
    {
      return DeleteById((int)dataObject[_dataTable.PrimaryKeyColumn]);
    }

    private const string DELETE_SQL = "DELETE FROM {0} WHERE [{1}] = @id";

    public virtual bool DeleteById(int id)
    {
      string preparedSql = string.Format(DELETE_SQL, _dataTable.TableName, _dataTable.PrimaryKeyColumn);
      int delete = DBUtility.ExecuteNonQuery(preparedSql, new SqlParameter("@id", id));
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
    /// SCOPE_IDENTITY() comes back as decimal. The int keyed models unbox it with a direct (int)
    /// cast, which throws on a boxed decimal, while the bigint keyed ones parse whatever they are
    /// given - so box an int whenever the value fits and fall back to long.
    /// </summary>
    private static object NormalizeIdentity(object identity)
    {
      long value = Convert.ToInt64(identity, CultureInfo.InvariantCulture);
      if (value >= int.MinValue && value <= int.MaxValue)
        return (int)value;
      return value;
    }

    protected virtual List<T> ExecuteReader(String commandText, params SqlParameter[] parameters)
    {
      SqlConnection connection = DBFactory.GetInstance().GetConnection();
      SqlTransaction activeTransaction = DBFactory.GetInstance().GetActiveTransaction();
      bool ownsConnection = activeTransaction == null;
      if (ownsConnection)
        connection.Open();
      try
      {
        List<T> returnList = new List<T>();
        using (SqlCommand command = connection.CreateCommand())
        {
          command.CommandText = commandText;
          command.CommandTimeout = 600;
          command.Transaction = activeTransaction;
          AddParameters(command, parameters);
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              T t = new T();
              // Driven by the column map rather than the result set, so a column that exists in the
              // database but is not mapped is simply ignored.
              foreach (string columnName in _dataTable.Columns)
              {
                if (!(reader[columnName] is DBNull))
                  t[columnName] = reader[columnName];
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

    protected static void AddParameters(SqlCommand command, SqlParameter[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      foreach (SqlParameter parameter in parameters)
      {
        if (parameter.Value == null)
          parameter.Value = DBNull.Value;
        command.Parameters.Add(parameter);
      }
    }
  }
}

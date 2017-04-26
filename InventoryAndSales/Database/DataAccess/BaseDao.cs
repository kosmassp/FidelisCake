using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.DataTable;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataAccess
{
  public class BaseDao<T> where T : BaseObject
  {
    private readonly IDataTable _dataTable;

    public BaseDao(IDataTable dataTable)
    {
      _dataTable = dataTable;
    }

    private const string FIND_BY_ID_SQL = "SELECT * FROM {0} WHERE {1} = ?";
    public T FindById(int id)
    {
      //TODO execute sql FIND_BY_ID_SQL, WE don't not to describe this everytime
      string preparedSql = string.Format(FIND_BY_ID_SQL, _dataTable.TableName, _dataTable.PrimaryKeyColumn);
      //execute sql
      return null;
    }

    private const string FIND_BY_QUERY = "SELECT * FROM {0} WHERE {1}";
    public T FindByQuery(string whereClause)
    {
      //TODO execute sql FIND_BY_ID_SQL, WE don't not to describe this everytime
      string preparedSql = string.Format(FIND_BY_ID_SQL, _dataTable.TableName, whereClause);
      //execute sql
      return null;
    }

    private const string INSERT_SQL = "INSERT INTO {0}({1}) VALUES {2}";
    private const string UPDATE_SQL = "UPDATE {0} SET {1} WHERE {2} = ?";
    public bool SaveOrUpdate(T dataObject)
    {
      StringBuilder columns = new StringBuilder();
      StringBuilder values = new StringBuilder();
      StringBuilder columnValuePair = new StringBuilder();

      foreach (string column in _dataTable.Columns)
      {
        columns.AppendFormat(",{0}", column);
        values.AppendFormat(",{0}", dataObject.Data[column]);
        columnValuePair.AppendFormat("{0}={1}", column, dataObject.Data[column]);
      }

      //TODO execute sql INSERT_SQL
      string insertSql = string.Format(INSERT_SQL, _dataTable.TableName, columns, values);
      //update
      string updateSql = string.Format(UPDATE_SQL, _dataTable.TableName, columnValuePair, _dataTable.PrimaryKeyColumn );
      //executeUpdateSql with preparedStatement
      //dataObject.Identifier

      return false;
    }

    public bool Delete(T dataObject)
    {
      return DeleteById((int)dataObject.Data[_dataTable.PrimaryKeyColumn]);
    }

    private const string DELETE_SQL = "DELETE FROM {0} WHERE {1} = ?";
    public bool DeleteById(int id)
    {
      //TODO execute sql FIND_BY_ID_SQL, WE don't not to describe this everytime
      string preparedSql = string.Format(DELETE_SQL, _dataTable.TableName, _dataTable.PrimaryKeyColumn);
      //execute sql
      return false;
    }
  }
}

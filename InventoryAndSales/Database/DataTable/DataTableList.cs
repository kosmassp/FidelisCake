using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryAndSales.Database.Model;

namespace InventoryAndSales.Database.DataTable
{
  class DataTableList
  {
    private static readonly object LockInstance = new object();
    private static DataTableList _instance;
    public static DataTableList Instance
    {
      get
      {
        lock (LockInstance)
        {
          if (_instance == null)
            _instance = new DataTableList();
        }
        return _instance;
      }
    }

    private static readonly IDataTable ItemDataTable = new DataTable("M_PRODUCTS", "Id", "Code", "Name", "Price", "Discount");
    private static readonly IDataTable UserDataTable = new DataTable("M_USERS", "Id", "Username", "Password", "Role");
    private static readonly IDataTable TransactionDataTable = new DataTable("T_TRANSACTIONS", "Id", "Factur", "TotalPrice", "TotalDiscount", "Total", "Notes", "UserId", "TransactionTime", "Payment", "Exchange", "CustomerId");
    private static readonly IDataTable TransactionDetailDataTable = new DataTable("T_TRANSACTION_DETAILS", "Id", "ProductId", "Quantity", "ProductDiscount", "ProductPrice", "SubtotalDiscount", "SubtotalPrice", "Subtotal", "TransactionId");
    private static readonly IDataTable CustomerDataTable = new DataTable("M_CUSTOMERS", "Id", "Username", "Password", "Role");


    private Dictionary<Type, IDataTable> _dict;
    private DataTableList()
    {
      _dict = new Dictionary<Type, IDataTable>();
      _dict.Add(typeof(User), UserDataTable);
      _dict.Add(typeof(Product), ItemDataTable);
      _dict.Add(typeof(Transaction), TransactionDataTable);
      _dict.Add(typeof(TransactionDetail), TransactionDetailDataTable);
      _dict.Add(typeof(Customer), CustomerDataTable);
      _dict.Add(typeof(CustomQuery), null);
    }
 
    public IDataTable GetDataTable(Type type)
    {
      return _dict[type];
    }


  }
}

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

    private static readonly IDataTable ItemDataTable = new DataTable("M_PRODUCTS", "Id", "Code", "Barcode", "Name", "Price", "Discount", "Deleted");
    private static readonly IDataTable UserDataTable = new DataTable("M_USERS", "Id", "Username", "Password", "Name", "Role", "Deleted");
    private static readonly IDataTable TransactionDataTable = new DataTable("T_TRANSACTIONS", "Id", "Factur", "TotalPrice", "TotalDiscount", "Total", "Notes", "UserId", "TransactionTime", "Payment", "Exchange", "CustomerId");
    private static readonly IDataTable TransactionDetailDataTable = new DataTable("T_TRANSACTION_DETAILS", "Id", "ProductId", "Quantity", "ProductDiscount", "ProductPrice", "SubtotalDiscount", "SubtotalPrice", "Subtotal", "TransactionId");
    private static readonly IDataTable CustomerDataTable = new DataTable("M_CUSTOMERS", "Id", "Name",  "Address", "Phone", "MemberType");


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

/*
 

CREATE TABLE [dbo].[M_PRODUCTS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NULL,
	[Name] [varchar](70) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Discount] [decimal](18, 0) NULL,
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_M_PRODUCTS_Deleted]  DEFAULT ((0)),
	[Barcode] [varchar](20) NULL
)


CREATE TABLE [dbo].[M_USERS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[Role] [int] NULL,
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_M_USERS_Deleted]  DEFAULT ((0)),
	[Name] [varchar](50) NULL,
	[Password] [varchar](256) NULL
) 
 
 
CREATE TABLE [dbo].[T_TRANSACTION_DETAILS](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NULL,
	[ProductDiscount] [decimal](18, 0) NULL,
	[ProductPrice] [decimal](18, 0) NULL,
	[Subtotal] [decimal](18, 0) NULL,
	[TransactionId] [bigint] NULL,
	[SubtotalDiscount] [decimal](18, 0) NULL,
	[SubtotalPrice] [decimal](18, 0) NULL
) 
 
CREATE TABLE [dbo].[T_TRANSACTIONS](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalPrice] [decimal](18, 0) NULL,
	[TotalDiscount] [decimal](18, 0) NULL,
	[Total] [decimal](18, 0) NULL,
	[Notes] [varchar](100) NULL,
	[TransactionTime] [datetime] NULL,
	[Payment] [decimal](18, 0) NULL,
	[Exchange] [decimal](18, 0) NULL,
	[UserId] [int] NULL,
	[CustomerId] [int] NULL,
	[Factur] [varchar](18) NULL
) 


CREATE TABLE [dbo].[M_CUSTOMERS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Type] [int] NULL
) 

*/
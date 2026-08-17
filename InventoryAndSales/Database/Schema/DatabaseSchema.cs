using System;
using System.Collections.Generic;

namespace InventoryAndSales.Database.Schema
{
  /// <summary>Storage classes the application needs, independent of any one database.</summary>
  public enum DbColumnType
  {
    Int,
    Long,
    Decimal,
    Bool,
    String,
    Text,
    DateTime,
  }

  public class ColumnDefinition
  {
    public string Name { get; private set; }
    public DbColumnType Type { get; private set; }
    /// <summary>Only meaningful for <see cref="DbColumnType.String"/>.</summary>
    public int Length { get; private set; }
    public bool Nullable { get; private set; }
    public bool IsIdentity { get; private set; }
    /// <summary>Literal default, already written the way each dialect renders it, or null.</summary>
    public bool DefaultFalse { get; private set; }

    private ColumnDefinition(string name, DbColumnType type, int length, bool nullable, bool isIdentity, bool defaultFalse)
    {
      Name = name;
      Type = type;
      Length = length;
      Nullable = nullable;
      IsIdentity = isIdentity;
      DefaultFalse = defaultFalse;
    }

    public static ColumnDefinition Identity(string name, DbColumnType type)
    {
      return new ColumnDefinition(name, type, 0, false, true, false);
    }

    public static ColumnDefinition Column(string name, DbColumnType type, bool nullable = true)
    {
      return new ColumnDefinition(name, type, 0, nullable, false, false);
    }

    public static ColumnDefinition Text(string name, int length, bool nullable = true)
    {
      return new ColumnDefinition(name, DbColumnType.String, length, nullable, false, false);
    }

    /// <summary>A NOT NULL flag defaulting to false - the soft-delete columns.</summary>
    public static ColumnDefinition Flag(string name)
    {
      return new ColumnDefinition(name, DbColumnType.Bool, 0, false, false, true);
    }
  }

  public class TableDefinition
  {
    public string Name { get; private set; }
    public List<ColumnDefinition> Columns { get; private set; }

    public TableDefinition(string name, params ColumnDefinition[] columns)
    {
      Name = name;
      Columns = new List<ColumnDefinition>(columns);
    }
  }

  public class IndexDefinition
  {
    public string Name { get; private set; }
    public string Table { get; private set; }
    public string[] Columns { get; private set; }
    public bool Unique { get; private set; }
    public bool Descending { get; private set; }

    public IndexDefinition(string name, string table, string[] columns, bool unique, bool descending)
    {
      Name = name;
      Table = table;
      Columns = columns;
      Unique = unique;
      Descending = descending;
    }
  }

  /// <summary>
  /// A column an older installation may be missing. Applied additively at startup.
  /// </summary>
  public class ColumnAddition
  {
    public string Table { get; private set; }
    public ColumnDefinition Column { get; private set; }
    /// <summary>Value to backfill into existing rows, or null to leave them NULL.</summary>
    public string BackfillLiteral { get; private set; }

    public ColumnAddition(string table, ColumnDefinition column, string backfillLiteral = null)
    {
      Table = table;
      Column = column;
      BackfillLiteral = backfillLiteral;
    }
  }

  /// <summary>
  /// The schema the application expects, declared once and rendered per database by an
  /// <see cref="Dialect.ISqlDialect"/>.
  ///
  /// This used to be SQL Server DDL written out by hand inside DBUtility, which is why supporting a
  /// second database meant rewriting the file. Nothing here mentions a specific product.
  /// </summary>
  public static class DatabaseSchema
  {
    public static List<TableDefinition> Tables()
    {
      return new List<TableDefinition>
      {
        new TableDefinition("M_SETTINGS",
          ColumnDefinition.Identity("Id", DbColumnType.Int),
          ColumnDefinition.Text("Key", 80, nullable: false),
          ColumnDefinition.Text("Group", 80),
          ColumnDefinition.Column("Value", DbColumnType.Text),
          ColumnDefinition.Column("Default", DbColumnType.Text, nullable: false)),

        new TableDefinition("M_PRODUCTS",
          ColumnDefinition.Identity("Id", DbColumnType.Int),
          ColumnDefinition.Text("Code", 10),
          ColumnDefinition.Text("Name", 70, nullable: false),
          ColumnDefinition.Column("Price", DbColumnType.Decimal, nullable: false),
          ColumnDefinition.Column("Discount", DbColumnType.Decimal),
          ColumnDefinition.Flag("Deleted"),
          ColumnDefinition.Text("Barcode", 20)),

        new TableDefinition("M_USERS",
          ColumnDefinition.Identity("Id", DbColumnType.Int),
          ColumnDefinition.Text("Username", 50),
          ColumnDefinition.Column("Role", DbColumnType.Int),
          ColumnDefinition.Flag("Deleted"),
          ColumnDefinition.Text("Name", 50),
          ColumnDefinition.Text("Password", 256)),

        new TableDefinition("T_TRANSACTION_DETAILS",
          ColumnDefinition.Identity("Id", DbColumnType.Long),
          ColumnDefinition.Column("ProductId", DbColumnType.Int),
          ColumnDefinition.Column("Quantity", DbColumnType.Int),
          ColumnDefinition.Column("ProductDiscount", DbColumnType.Decimal),
          ColumnDefinition.Column("ProductPrice", DbColumnType.Decimal),
          ColumnDefinition.Column("Subtotal", DbColumnType.Decimal),
          ColumnDefinition.Column("TransactionId", DbColumnType.Long),
          ColumnDefinition.Column("SubtotalDiscount", DbColumnType.Decimal),
          ColumnDefinition.Column("SubtotalPrice", DbColumnType.Decimal)),

        new TableDefinition("T_TRANSACTIONS",
          ColumnDefinition.Identity("Id", DbColumnType.Long),
          ColumnDefinition.Column("TotalPrice", DbColumnType.Decimal),
          ColumnDefinition.Column("TotalDiscount", DbColumnType.Decimal),
          ColumnDefinition.Column("Total", DbColumnType.Decimal),
          ColumnDefinition.Text("Notes", 100),
          ColumnDefinition.Column("TransactionTime", DbColumnType.DateTime),
          ColumnDefinition.Column("Payment", DbColumnType.Decimal),
          ColumnDefinition.Column("Exchange", DbColumnType.Decimal),
          ColumnDefinition.Column("UserId", DbColumnType.Int),
          ColumnDefinition.Text("Factur", 20),
          ColumnDefinition.Column("CustomerId", DbColumnType.Long),
          ColumnDefinition.Text("PaymentMethod", 20),
          ColumnDefinition.Text("PaymentReference", 50)),

        new TableDefinition("M_CUSTOMERS",
          ColumnDefinition.Identity("Id", DbColumnType.Int),
          ColumnDefinition.Text("Name", 50),
          ColumnDefinition.Text("Address", 50),
          ColumnDefinition.Text("Phone", 50),
          ColumnDefinition.Column("MemberType", DbColumnType.Int)),
      };
    }

    /// <summary>
    /// Columns added after the original release. Guarded and additive, because installations sit on
    /// many different versions with no migration history.
    /// </summary>
    public static List<ColumnAddition> ColumnAdditions()
    {
      return new List<ColumnAddition>
      {
        // Revision links a superseded sale to the one that replaced it.
        // 0 = active, > 0 = replaced by that Id, -1 = cancelled.
        new ColumnAddition("T_TRANSACTIONS",
          ColumnDefinition.Column("Revision", DbColumnType.Long), "0"),

        // Who voided a sale and when. Existing rows stay NULL.
        new ColumnAddition("T_TRANSACTIONS", ColumnDefinition.Column("CancelledBy", DbColumnType.Int)),
        new ColumnAddition("T_TRANSACTIONS", ColumnDefinition.Column("CancelledAt", DbColumnType.DateTime)),

        // How a sale was paid for. Every sale before this was cash, so that is what existing rows
        // are backfilled to - leaving them blank would make historic takings unattributable.
        new ColumnAddition("T_TRANSACTIONS",
          ColumnDefinition.Text("PaymentMethod", 20), "'CASH'"),
        new ColumnAddition("T_TRANSACTIONS",
          ColumnDefinition.Text("PaymentReference", 50), "''"),
      };
    }

    public static List<IndexDefinition> Indexes()
    {
      return new List<IndexDefinition>
      {
        new IndexDefinition("IDX_T_TRANS_TRXTIME", "T_TRANSACTIONS", new[] { "TransactionTime" }, false, true),
        new IndexDefinition("IDX_T_TRANS_FACTUR", "T_TRANSACTIONS", new[] { "Factur" }, true, false),
        new IndexDefinition("IDX_T_TRDETAIL_TRX_ID", "T_TRANSACTION_DETAILS", new[] { "TransactionId" }, false, true),
      };
    }
  }
}

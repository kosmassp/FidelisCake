using System;
using System.Data.Common;

namespace InventoryAndSales.Database
{
  /// <summary>
  /// The connection and transaction one command should run on, taken as a single snapshot.
  ///
  /// Asking <see cref="DBFactory"/> for the connection and then, separately, for the active
  /// transaction is the obvious shape and is wrong: nothing stops a transaction from starting or
  /// ending between the two calls, which leaves a command holding a connection from one state and a
  /// transaction from the other - either a closed connection, or a transaction belonging to a
  /// different connection. Both come from one locked read here instead.
  ///
  /// A scope either joins the ambient transaction, in which case disposing it leaves the connection
  /// alone, or owns a connection of its own, which disposing returns to the pool. Always use it with
  /// <c>using</c>.
  /// </summary>
  public sealed class DbScope : IDisposable
  {
    /// <summary>Long enough for an end-of-year report on a slow shop PC.</summary>
    private const int COMMAND_TIMEOUT_SECONDS = 600;

    private readonly bool _ownsConnection;
    private bool _disposed;

    private DbScope(DbConnection connection, DbTransaction transaction, bool ownsConnection)
    {
      Connection = connection;
      Transaction = transaction;
      _ownsConnection = ownsConnection;
    }

    public DbConnection Connection { get; private set; }

    /// <summary>The ambient transaction, or null when this scope stands alone.</summary>
    public DbTransaction Transaction { get; private set; }

    /// <summary>A scope taking part in a transaction somebody else opened and will commit.</summary>
    internal static DbScope Joined(DbConnection connection, DbTransaction transaction)
    {
      return new DbScope(connection, transaction, false);
    }

    /// <summary>A scope owning a connection of its own, released on dispose.</summary>
    internal static DbScope Owned(DbConnection connection)
    {
      return new DbScope(connection, null, true);
    }

    /// <summary>
    /// A command already attached to this scope's transaction. Building commands any other way is
    /// how a write ends up outside the unit of work it was meant to be part of.
    /// </summary>
    public DbCommand CreateCommand(string commandText)
    {
      DbCommand command = Connection.CreateCommand();
      command.CommandText = commandText;
      command.CommandTimeout = COMMAND_TIMEOUT_SECONDS;
      command.Transaction = Transaction;
      return command;
    }

    public void Dispose()
    {
      if (_disposed)
        return;
      _disposed = true;
      if (!_ownsConnection)
        return;
      // Closing returns it to the pool; without this a long trading day eventually exhausts it.
      Connection.Close();
      Connection.Dispose();
    }
  }
}

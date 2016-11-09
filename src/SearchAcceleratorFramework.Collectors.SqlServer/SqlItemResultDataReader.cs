using System;
using System.Data;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlItemResultDataReader : ISqlItemResultDataReader
  {
    private readonly IDbCommand _command;
    private readonly int _idColumnIndex;
    private readonly IDataReader _reader;
    private readonly int _weightColumnIndex;
    private bool _disposed;

    public SqlItemResultDataReader(IDbConnection connection, string sqlQuery)
    {
      _command = connection.CreateCommand();

      _command.CommandType = CommandType.Text;
      _command.CommandText = sqlQuery;
      var parameter = _command.CreateParameter();
      parameter.Direction = ParameterDirection.Input;
      parameter.Value = sqlQuery;
      parameter.DbType = DbType.String;
      _command.Parameters.Add(parameter);

      EnsureConnectionOpen(connection);

      _reader = _command.ExecuteReader();

      _idColumnIndex = _reader.GetOrdinal("Id");
      _weightColumnIndex = _reader.GetOrdinal("Weight");
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public WeightedItemResult Current
    {
      get
      {
        return new WeightedItemResult
        {
          Id = _reader.GetInt64(_idColumnIndex),
          Weight = _reader.GetDecimal(_weightColumnIndex)
        };
      }
    }

    public bool Read()
    {
      return _reader.Read();
    }

    ~SqlItemResultDataReader()
    {
      Dispose(false);
    }

    private void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          if (!_reader.IsClosed)
          {
            _reader.Close();
          }

          _reader.Dispose();
          _command.Dispose();
        }

        _disposed = true;
      }
    }

    private static void EnsureConnectionOpen(IDbConnection connection)
    {
      if (connection.State != ConnectionState.Open)
      {
        connection.Open();
      }
    }
  }
}
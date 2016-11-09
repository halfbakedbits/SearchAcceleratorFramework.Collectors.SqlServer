using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SearchAcceleratorFramework.Collectors.Database;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlServerGateway : IDatabaseGateway
  {
    private readonly SqlConnection _connection;

    public SqlServerGateway(SqlConnection connection)
    {
      _connection = connection;
    }

    public IEnumerable<WeightedItemResult> GetItemResults(string sqlQuery, string searchTerm)
    {
      using (var command = _connection.CreateCommand())
      {
        command.CommandType = CommandType.Text;
        command.CommandText = sqlQuery;
        var parameter = command.CreateParameter();
        parameter.Direction = ParameterDirection.Input;
        parameter.ParameterName = "searchTerm";
        parameter.Value = $"%{searchTerm}%";
        command.Parameters.Add(parameter);

        EnsureOpenConnection(_connection);

        using (var dataReader = command.ExecuteReader())
        {
          var idColumnIndex = dataReader.GetOrdinal("Id");
          var weightColumnIndex = dataReader.GetOrdinal("Weight");

          while (dataReader.Read())
          {
            yield return new WeightedItemResult
            {
              Id = dataReader.GetInt64(idColumnIndex),
              Weight = dataReader.GetDecimal(weightColumnIndex)
            };
          }
        }
      }
    }

    private static void EnsureOpenConnection(IDbConnection connection)
    {
      if (connection.State != ConnectionState.Open)
      {
        connection.Open();
      }
    }
  }
}
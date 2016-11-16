using System.Data;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlDataReaderFactory : ISqlDataReaderFactory
  {
    private readonly IDbConnection _connection;

    public SqlDataReaderFactory(IDbConnection connection)
    {
      _connection = connection;
    }

    public ISqlItemResultDataReader Create(string sqlQuery, SearchParameter searchParameter)
    {
      return new SqlItemResultDataReader(_connection, sqlQuery, searchParameter);
    }
  }
}
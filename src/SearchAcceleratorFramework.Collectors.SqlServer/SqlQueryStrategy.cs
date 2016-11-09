using SearchAcceleratorFramework.Strategies.Database;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlQueryStrategy : ISqlQueryStrategy
  {
    public SqlQueryStrategy(string sqlStatement)
    {
      SqlStatement = sqlStatement;
    }

    public string SqlStatement { get; }
  }
}
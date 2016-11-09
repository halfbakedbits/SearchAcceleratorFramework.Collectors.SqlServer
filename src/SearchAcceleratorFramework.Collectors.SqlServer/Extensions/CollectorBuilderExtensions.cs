using System.Data;
using SearchAcceleratorFramework.Builders;
using SearchAcceleratorFramework.Collectors.Database;
using SearchAcceleratorFramework.Collectors.SqlServer.Builders;

namespace SearchAcceleratorFramework.Collectors.SqlServer.Extensions
{
  public static class CollectorBuilderExtensions
  {
    /// <summary>
    ///   Begins Fluent Builder chain for Sql Server searches
    /// </summary>
    /// <param name="sb">The sb.</param>
    /// <param name="connection">The connection.</param>
    /// <returns></returns>
    public static SqlServerCollectorBuilder Targeting(this SearchBuilder sb, IDbConnection connection)
    {
      return new SqlServerCollectorBuilder(connection);
    }

    /// <summary>
    ///   Begins Fluent Builder chain for Sql Server searches (fine-control over dependencies)
    /// </summary>
    /// <param name="sb">The sb.</param>
    /// <param name="dataReaderFactory">The data reader factory.</param>
    /// <param name="statementProvider">The statement provider.</param>
    /// <returns></returns>
    public static SqlServerCollectorBuilder Targeting(this SearchBuilder sb, ISqlDataReaderFactory dataReaderFactory,
      IConsolidatedSqlStatementProvider statementProvider)
    {
      return new SqlServerCollectorBuilder(dataReaderFactory, statementProvider);
    }
  }
}
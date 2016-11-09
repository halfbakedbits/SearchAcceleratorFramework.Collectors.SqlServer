using System.Collections.Generic;
using System.Data;
using System.Linq;
using SearchAcceleratorFramework.Collectors.Database;
using SearchAcceleratorFramework.Strategies.Database;

namespace SearchAcceleratorFramework.Collectors.SqlServer.Builders
{
  /// <summary>
  ///   Fluent builder for Sql Server Collector
  /// </summary>
  public class SqlServerCollectorBuilder
  {
    private readonly ISqlDataReaderFactory _dataReaderFactory;
    private readonly IConsolidatedSqlStatementProvider _statmentProvider;

    public SqlServerCollectorBuilder(IDbConnection connection) : this(new SqlDataReaderFactory(connection), new SqlServerStatementProvider())
    {
    }

    public SqlServerCollectorBuilder(ISqlDataReaderFactory dataReaderFactory, IConsolidatedSqlStatementProvider statmentProvider)
    {
      _dataReaderFactory = dataReaderFactory;
      _statmentProvider = statmentProvider;
    }

    /// <summary>
    ///   Wires search strategies for collector
    /// </summary>
    /// <param name="searchStrategies">The search strategies.</param>
    /// <returns></returns>
    public SqlServerCollectorBuilderTermination SearchUsing(IEnumerable<ISqlQueryStrategy> searchStrategies)
    {
      return new SqlServerCollectorBuilderTermination(_dataReaderFactory, _statmentProvider, searchStrategies.ToArray());
    }

    public class SqlServerCollectorBuilderTermination
    {
      private readonly ISqlDataReaderFactory _dataReaderFactory;
      private readonly ISqlQueryStrategy[] _searchStrategies;
      private readonly IConsolidatedSqlStatementProvider _statementProvider;

      public SqlServerCollectorBuilderTermination(ISqlDataReaderFactory dataReaderFactory, IConsolidatedSqlStatementProvider statementProvider,
        ISqlQueryStrategy[] searchStrategies)
      {
        _dataReaderFactory = dataReaderFactory;
        _statementProvider = statementProvider;
        _searchStrategies = searchStrategies;
      }

      public SqlServerResultCollector Create()
      {
        return new SqlServerResultCollector(_dataReaderFactory, _statementProvider, _searchStrategies);
      }
    }
  }
}
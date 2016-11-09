using System.Collections.Generic;
using SearchAcceleratorFramework.Collectors.Database;
using SearchAcceleratorFramework.Strategies.Database;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlServerResultCollector : IDatabaseResultCollector
  {
    private readonly IConsolidatedSqlStatementProvider _provider;
    private readonly ISqlDataReaderFactory _readerFactory;
    private readonly ISqlQueryStrategy[] _searchStrategies;

    public SqlServerResultCollector(ISqlDataReaderFactory readerFactory, IConsolidatedSqlStatementProvider provider, params ISqlQueryStrategy[] searchStrategies)
    {
      _readerFactory = readerFactory;
      _provider = provider;
      _searchStrategies = searchStrategies;
    }

    public IEnumerable<WeightedItemResult> GetWeightedItemsMatching(string searchTerm)
    {
      var sqlQuery = _provider.CreateSqlSearchStatement(_searchStrategies);

      using (var reader = _readerFactory.Create(sqlQuery))
      {
        while (reader.Read())
        {
          yield return reader.Current;
        }
      }
    }
  }
}
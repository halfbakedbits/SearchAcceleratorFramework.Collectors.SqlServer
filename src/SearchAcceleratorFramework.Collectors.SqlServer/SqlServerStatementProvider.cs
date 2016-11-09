using System;
using System.Text;
using SearchAcceleratorFramework.Collectors.Database;
using SearchAcceleratorFramework.Strategies.Database;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SqlServerStatementProvider : IConsolidatedSqlStatementProvider
  {
    private const string AppendingClause = "UNION ALL";
    private static readonly int AppendingClauseLength = (AppendingClause + Environment.NewLine).Length;

    /// <summary>
    ///   Joins the SQL statements provided in the search strategies with a Sql-Server specific dialect.
    /// </summary>
    /// <param name="searchStrategies">The search strategies.</param>
    /// <returns>
    ///   Properly formatted SQL statement, using the "UNION ALL" statement
    /// </returns>
    /// <exception cref="System.InvalidOperationException">Must provide at least one search strategy</exception>
    public string CreateSqlSearchStatement(ISqlQueryStrategy[] searchStrategies)
    {
      if (searchStrategies == null || searchStrategies.Length == 0)
      {
        throw new InvalidOperationException("Must provide at least one search strategy");
      }

      var sb = new StringBuilder();
      foreach (var strategy in searchStrategies)
      {
        sb.AppendLine(strategy.SqlStatement);
        sb.AppendLine(AppendingClause);
      }

      sb.Length = sb.Length - AppendingClauseLength;

      return sb.ToString();
    }
  }
}
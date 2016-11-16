using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using SearchAcceleratorFramework.Builders;
using SearchAcceleratorFramework.Collectors.Database;
using SearchAcceleratorFramework.Collectors.SqlServer.Extensions;
using SearchAcceleratorFramework.Strategies.Database;
using Shouldly;
using Xunit;

namespace SearchAcceleratorFramework.Collectors.SqlServer.Tests
{
  public class SqlServerResultCollectorExamples
  {
    [Fact]
    public void BuilderConstruction_GetsSamesResult()
    {
      // Arrange
      var searchTerm = "foobar";
      var searchStrategies = new List<ISqlQueryStrategy>
      {
        new SqlQueryStrategy(
          "SELECT CustomerId AS Id, 50 AS Weight FROM dbo.Customer WHERE CustomerName LIKE @searchTerm"),
        new SqlQueryStrategy(
          "SELECT CustomerId AS Id, 1 AS Weight FROM dbo.Customer WHERE Address1 LIKE @searchTerm"),
        new SqlQueryStrategy("SELECT CustomerId AS Id, .01 AS Weight FROM dbo.Customer WHERE City LIKE @searchTerm")
      }.ToArray();

      var readerFactory = A.Fake<ISqlDataReaderFactory>();
      var reader = A.Fake<ISqlItemResultDataReader>();
      var statementProvider = A.Fake<IConsolidatedSqlStatementProvider>();
      var sqlStatement = "SELECT * FROM BUZZ";

      var searchParameter = new SearchParameter(searchTerm);
      A.CallTo(() => readerFactory.Create(sqlStatement, searchParameter)).Returns(reader);
      A.CallTo(() => statementProvider.CreateSqlSearchStatement(A<ISqlQueryStrategy[]>.That.IsSameSequenceAs(searchStrategies))).Returns(sqlStatement);
      A.CallTo(() => reader.Read()).Returns(true).NumberOfTimes(2);
      A.CallTo(() => reader.Current).ReturnsNextFromSequence(new WeightedItemResult {Id = 1, Weight = 10}, new WeightedItemResult {Id = 2, Weight = 20});

      // Act
      var sut = new SearchBuilder()
        .Targeting(readerFactory, statementProvider)
        .SearchUsing(searchStrategies)
        .Create();

      var results = sut.GetWeightedItemsMatching(searchTerm).ToList();

      // Assert
      A.CallTo(() => statementProvider.CreateSqlSearchStatement(A<ISqlQueryStrategy[]>.That.IsSameSequenceAs(searchStrategies)))
        .MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => readerFactory.Create(sqlStatement, searchParameter)).MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => reader.Current).MustHaveHappened(Repeated.Exactly.Times(2));
      A.CallTo(() => reader.Dispose()).MustHaveHappened(Repeated.Exactly.Once);

      results.Count.ShouldBe(2);
      results[0].Id.ShouldBe(1);
      results[1].Id.ShouldBe(2);
    }

    [Fact]
    public void ExplicitConstruction_CombinesSqlStatementsAndCoordinatesReader()
    {
      // Arrange
      var searchTerm = "foobar";
      var searchStrategies = new List<ISqlQueryStrategy>
      {
        new SqlQueryStrategy(
          "SELECT CustomerId AS Id, 50 AS Weight FROM dbo.Customer WHERE CustomerName LIKE @searchTerm"),
        new SqlQueryStrategy(
          "SELECT CustomerId AS Id, 1 AS Weight FROM dbo.Customer WHERE Address1 LIKE @searchTerm"),
        new SqlQueryStrategy("SELECT CustomerId AS Id, .01 AS Weight FROM dbo.Customer WHERE City LIKE @searchTerm")
      }.ToArray();

      var readerFactory = A.Fake<ISqlDataReaderFactory>();
      var reader = A.Fake<ISqlItemResultDataReader>();
      var statementProvider = A.Fake<IConsolidatedSqlStatementProvider>();
      var sqlStatement = "SELECT * FROM BUZZ";

      var searchParameter = new SearchParameter(searchTerm);

      A.CallTo(() => readerFactory.Create(sqlStatement, searchParameter)).Returns(reader);
      A.CallTo(() => statementProvider.CreateSqlSearchStatement(searchStrategies)).Returns(sqlStatement);
      A.CallTo(() => reader.Read()).Returns(true).NumberOfTimes(2);
      A.CallTo(() => reader.Current).ReturnsNextFromSequence(new WeightedItemResult {Id = 1, Weight = 10}, new WeightedItemResult {Id = 2, Weight = 20});

      // Act
      var sut = new SqlServerResultCollector(readerFactory, statementProvider, searchStrategies);

      var results = sut.GetWeightedItemsMatching(searchTerm).ToList();

      // Assert
      A.CallTo(() => statementProvider.CreateSqlSearchStatement(searchStrategies)).MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => readerFactory.Create(sqlStatement, searchParameter)).MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => reader.Current).MustHaveHappened(Repeated.Exactly.Times(2));
      A.CallTo(() => reader.Dispose()).MustHaveHappened(Repeated.Exactly.Once);

      results.Count.ShouldBe(2);
      results[0].Id.ShouldBe(1);
      results[1].Id.ShouldBe(2);
    }
  }
}
using System.Data;
using FakeItEasy;
using Xunit;

namespace SearchAcceleratorFramework.Collectors.SqlServer.Tests
{
  public class SqlItemResultDataReaderTests
  {
    [Fact]
    public void Ctor_HappyPath_NoParameterNameClosedConnection()
    {
      // Arrange
      var searchTerm = "foo";

      var connection = A.Fake<IDbConnection>();
      var sqlQuery = "SELECT a.AnimalId AS Id, 50 AS Weight FROM dbo.Animal a WHERE CHARINDEX(@SearchTerm, a.Name) > 1";

      var command = A.Fake<IDbCommand>();
      A.CallTo(() => connection.CreateCommand()).Returns(command);
      A.CallTo(() => connection.State).Returns(ConnectionState.Closed);

      var parameter = A.Fake<IDbDataParameter>();
      A.CallTo(() => command.CreateParameter()).Returns(parameter);

      var reader = A.Fake<IDataReader>();
      A.CallTo(() => command.ExecuteReader()).Returns(reader);
      A.CallTo(() => reader.GetOrdinal("Id")).Returns(1);
      A.CallTo(() => reader.GetOrdinal("Weight")).Returns(3);
      
      // Act
      using (var sut = new SqlItemResultDataReader(connection, sqlQuery, new SearchParameter(searchTerm)))
      {
      }

      // Assert
      A.CallToSet(() => command.CommandType).To(CommandType.Text).MustHaveHappened();
      A.CallToSet(() => command.CommandText).To(sqlQuery).MustHaveHappened();
      A.CallTo(() => command.Parameters.Add(parameter)).MustHaveHappened();

      A.CallToSet(() => parameter.Direction).To(ParameterDirection.Input).MustHaveHappened();
      A.CallToSet(() => parameter.ParameterName).To(SearchParameter.DefaultParameterName).MustHaveHappened();
      A.CallToSet(() => parameter.DbType).To(DbType.String).MustHaveHappened();
      A.CallToSet(() => parameter.Value).To(searchTerm).MustHaveHappened();

      A.CallTo(() => connection.Open()).MustHaveHappened();

      A.CallTo(() => command.ExecuteReader()).MustHaveHappened();
      A.CallTo(() => reader.GetOrdinal("Id")).MustHaveHappened();
      A.CallTo(() => reader.GetOrdinal("Weight")).MustHaveHappened();
    }

    [Fact]
    public void Ctor_HappyPath_NoParameterNameOpenConnection()
    {
      // Arrange
      var searchTerm = "foo";

      var connection = A.Fake<IDbConnection>();
      var sqlQuery = "SELECT a.AnimalId AS Id, 50 AS Weight FROM dbo.Animal a WHERE CHARINDEX(@SearchTerm, a.Name) > 1";

      var command = A.Fake<IDbCommand>();
      A.CallTo(() => connection.CreateCommand()).Returns(command);
      A.CallTo(() => connection.State).Returns(ConnectionState.Open);

      var parameter = A.Fake<IDbDataParameter>();
      A.CallTo(() => command.CreateParameter()).Returns(parameter);

      var reader = A.Fake<IDataReader>();
      A.CallTo(() => command.ExecuteReader()).Returns(reader);
      A.CallTo(() => reader.GetOrdinal("Id")).Returns(1);
      A.CallTo(() => reader.GetOrdinal("Weight")).Returns(3);
      
      // Act
      using (var sut = new SqlItemResultDataReader(connection, sqlQuery, new SearchParameter(searchTerm)))
      {
      }

      // Assert
      A.CallToSet(() => command.CommandType).To(CommandType.Text).MustHaveHappened();
      A.CallToSet(() => command.CommandText).To(sqlQuery).MustHaveHappened();
      A.CallTo(() => command.Parameters.Add(parameter)).MustHaveHappened();

      A.CallToSet(() => parameter.Direction).To(ParameterDirection.Input).MustHaveHappened();
      A.CallToSet(() => parameter.ParameterName).To(SearchParameter.DefaultParameterName).MustHaveHappened();
      A.CallToSet(() => parameter.DbType).To(DbType.String).MustHaveHappened();
      A.CallToSet(() => parameter.Value).To(searchTerm).MustHaveHappened();

      A.CallTo(() => connection.Open()).MustNotHaveHappened();

      A.CallTo(() => command.ExecuteReader()).MustHaveHappened();
      A.CallTo(() => reader.GetOrdinal("Id")).MustHaveHappened();
      A.CallTo(() => reader.GetOrdinal("Weight")).MustHaveHappened();
    }

    [Fact]
    public void Ctor_NoParameterName_UsesDefault_Bug1()
    {
      // Arrange
      var connection = A.Fake<IDbConnection>();
      var sqlQuery = "bar";

      var command = A.Fake<IDbCommand>();
      A.CallTo(() => connection.CreateCommand()).Returns(command);

      var parameter = A.Fake<IDbDataParameter>();
      A.CallTo(() => command.CreateParameter()).Returns(parameter);

      // Act
      using (var sut = new SqlItemResultDataReader(connection, sqlQuery, new SearchParameter("foo")))
      {
      }

      // Assert
      A.CallToSet(() => parameter.ParameterName).To(SearchParameter.DefaultParameterName).MustHaveHappened();
    }

    [Fact]
    public void Ctor_ParameterNameSupplied_OverridesDefault()
    {
      // Arrange
      var connection = A.Fake<IDbConnection>();
      var sqlQuery = "foobar";

      var command = A.Fake<IDbCommand>();
      A.CallTo(() => connection.CreateCommand()).Returns(command);

      var parameterName = "@bar";
      var parameter = A.Fake<IDbDataParameter>();
      A.CallTo(() => command.CreateParameter()).Returns(parameter);

      // Act
      using (var sut = new SqlItemResultDataReader(connection, sqlQuery, new SearchParameter("foo", parameterName)))
      {
      }

      // Assert
      A.CallToSet(() => parameter.ParameterName).To(SearchParameter.DefaultParameterName).MustNotHaveHappened();
      A.CallToSet(() => parameter.ParameterName).To(parameterName).MustHaveHappened();
    }
  }
}
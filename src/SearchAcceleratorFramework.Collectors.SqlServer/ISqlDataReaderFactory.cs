namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public interface ISqlDataReaderFactory
  {
    ISqlItemResultDataReader Create(string sqlQuery);
  }
}
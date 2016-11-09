using System;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public interface ISqlItemResultDataReader : IDisposable
  {
    WeightedItemResult Current { get; }
    bool Read();
  }
}
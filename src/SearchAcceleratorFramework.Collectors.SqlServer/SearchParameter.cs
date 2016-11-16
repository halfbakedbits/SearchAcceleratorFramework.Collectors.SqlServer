using System;

namespace SearchAcceleratorFramework.Collectors.SqlServer
{
  public class SearchParameter : IEquatable<SearchParameter>
  {
    public const string DefaultParameterName = "@SearchTerm";

    /// <summary>
    ///   Initializes a new instance of the <see cref="SearchParameter" /> class.  Uses <see cref="DefaultParameterName" /> for
    ///   Parameter Name
    /// </summary>
    /// <param name="searchTerm">The search term used as parameter value.</param>
    public SearchParameter(string searchTerm) : this(searchTerm, DefaultParameterName)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="SearchParameter" /> class.
    /// </summary>
    /// <param name="searchTerm">The search term used as parameter value.</param>
    /// <param name="parameterName">Name of the parameter with "@" prefix.</param>
    public SearchParameter(string searchTerm, string parameterName)
    {
      if (!parameterName.StartsWith("@"))
        throw new ArgumentException("Parameter must be '@' prefixed", nameof(parameterName));

      ParameterValue = searchTerm;
      ParameterName = parameterName;
    }

    public string ParameterValue { get; }

    public string ParameterName { get; }

    public bool Equals(SearchParameter other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      return string.Equals(ParameterValue, other.ParameterValue) && string.Equals(ParameterName, other.ParameterName);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;

      return Equals((SearchParameter) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((ParameterValue != null ? ParameterValue.GetHashCode() : 0)*397) ^
               (ParameterName != null ? ParameterName.GetHashCode() : 0);
      }
    }

    public static bool operator ==(SearchParameter left, SearchParameter right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(SearchParameter left, SearchParameter right)
    {
      return !Equals(left, right);
    }
  }
}
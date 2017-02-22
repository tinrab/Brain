using System;

namespace Brain.Evolution
{
  public class Gene : IEquatable<Gene>
  {
    public object Value { get; set; }

    public Gene(object value)
    {
      Value = value;
    }

    public bool Equals(Gene other)
    {
      return Value.Equals(other);
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) {
        return false;
      }

      if (ReferenceEquals(this, obj)) {
        return true;
      }

      if (obj.GetType() != GetType()) {
        return false;
      }

      return Equals((Gene) obj);
    }

    public static bool operator ==(Gene left, Gene right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Gene left, Gene right)
    {
      return !left.Equals(right);
    }
  }
}
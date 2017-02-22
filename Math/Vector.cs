using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Brain.Math
{
  public class Vector : IEnumerable<double>
  {
    private readonly List<double> _values;

    public int Length
    {
      get { return _values.Count; }
    }

    public double this[int index]
    {
      get { return _values[index]; }
      set { _values[index] = value; }
    }

    public double SquareMagnitude
    {
      get
      {
        var m = 0.0;
        for (var i = 0; i < _values.Count; i++) {
          var v = _values[i];
          m += v * v;
        }
        return m;
      }
    }

    public double Magnitude
    {
      get { return System.Math.Sqrt(SquareMagnitude); }
    }

    public double Mean
    {
      get
      {
        var avg = 0.0;
        for (var i = 0; i < Length; i++) {
          avg += _values[i] / Length;
        }
        return avg;
      }
    }

    public double Variance
    {
      get
      {
        var mean = Mean;
        var sum = 0.0;
        for (var i = 0; i < Length; i++) {
          sum += System.Math.Pow(_values[i] - mean, 2.0);
        }
        return sum / (Length - 1);
      }
    }

    public double StandardDeviation
    {
      get { return System.Math.Sqrt(Variance); }
    }

    public double Sum
    {
      get
      {
        var sum = 0.0;
        for (var i = 0; i < Length; i++) {
          sum += _values[i];
        }
        return sum;
      }
    }

    public double Min
    {
      get
      {
        var min = double.PositiveInfinity;
        for (var i = 0; i < Length; i++) {
          var v = _values[i];
          if (v < min) {
            min = v;
          }
        }
        return min;
      }
    }

    public double Max
    {
      get
      {
        var max = double.NegativeInfinity;
        for (var i = 0; i < Length; i++) {
          var v = _values[i];

          if (v > max) {
            max = v;
          }
        }
        return max;
      }
    }

    public Vector(int length)
    {
      _values = new List<double>(length);
      for (var i = 0; i < length; i++) {
        _values.Add(0.0);
      }
    }

    public Vector(params double[] values)
    {
      _values = new List<double>();
      _values.AddRange(values);
    }

    public Vector(ICollection<double> collection)
    {
      _values = new List<double>(collection);
    }

    public Vector(Vector v) : this(v._values) {}

    public IEnumerator<double> GetEnumerator()
    {
      return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _values.GetEnumerator();
    }

    public void Add(double value)
    {
      _values.Add(value);
    }

    public void RemoveAt(int index)
    {
      _values.RemoveAt(index);
    }

    public double[] ToArray()
    {
      return _values.ToArray();
    }

    public void Normalize(double min = double.NaN, double max = double.NaN)
    {
      if (double.IsNaN(min)) {
        min = Min;
      }

      if (double.IsNaN(max)) {
        max = Max;
      }

      var a = max - min;

      for (var i = 0; i < Length; i++) {
        _values[i] = (_values[i] - min) / a;
      }
    }

    public void Standardize()
    {
      var sd = StandardDeviation;
      var m = Mean;

      for (var i = 0; i < Length; i++) {
        _values[i] = (_values[i] - m) / sd;
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("[");
      for (var i = 0; i < Length; i++) {
        sb.Append(_values[i]);

        if (i < Length - 1) {
          sb.Append(", ");
        }
      }
      sb.Append("]");
      return sb.ToString();
    }

    public static Vector operator *(Vector left, double a)
    {
      var v = new Vector(left.Length);

      for (var i = 0; i < v.Length; i++) {
        v[i] = left[i] * a;
      }

      return v;
    }

    public static Vector operator /(Vector left, double a)
    {
      var v = new Vector(left.Length);

      for (var i = 0; i < v.Length; i++) {
        v[i] = left[i] / a;
      }

      return v;
    }

    public static Vector operator +(Vector left, Vector right)
    {
      var v = new Vector(left.Length);

      for (var i = 0; i < left.Length; i++) {
        v[i] = left[i] + right[i % right.Length];
      }

      return v;
    }

    public static Vector operator -(Vector left, Vector right)
    {
      var v = new Vector(left.Length);

      for (var i = 0; i < left.Length; i++) {
        v[i] = left[i] - right[i % right.Length];
      }

      return v;
    }
  }
}
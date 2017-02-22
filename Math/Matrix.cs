namespace Brain.Math
{
  public class Matrix
  {
    private readonly double[][] _values;

    public double this[int row, int column]
    {
      get { return _values[row][column]; }
      set { _values[row][column] = value; }
    }

    public Vector this[int row]
    {
      get { return new Vector(_values[row]); }
      set { _values[row] = value.ToArray(); }
    }

    public int Rows
    {
      get { return _values.Length; }
    }

    public int Columns
    {
      get { return _values[0].Length; }
    }

    public Matrix(int rows, int columns)
    {
      _values = new double[rows][];
      for (var i = 0; i < rows; i++) {
        _values[i] = new double[columns];
      }
    }

    public Matrix(int n) : this(n, n) {}

    public Matrix(double[,] values) : this(values.GetLength(0), values.GetLength(1))
    {
      for (var i = 0; i < Rows; i++) {
        for (var j = 0; j < Columns; j++) {
          _values[i][j] = values[i, j];
        }
      }
    }

    public Matrix(double[][] values) : this(values.Length, values[0].Length)
    {
      for (var i = 0; i < Rows; i++) {
        for (var j = 0; j < Columns; j++) {
          _values[i][j] = values[i][j];
        }
      }
    }

    public Matrix(Matrix m) : this(m._values) {}

    public Vector GetRow(int row)
    {
      return new Vector(_values[row]);
    }

    public Vector GetColumn(int column)
    {
      var col = new Vector(Rows);

      for (var i = 0; i < Rows; i++) {
        col[i] = _values[i][column];
      }

      return col;
    }

    public void RemoveColumn(int index)
    {
      for (var i = 0; i < Rows; i++) {
        _values[i] = Utility.RemoveElement(_values[i], index);
      }
    }

    public Matrix Take(int rows)
    {
      var m = new Matrix(rows, Columns);

      for (var i = 0; i < rows; i++) {
        m[i] = GetRow(i);
      }

      return m;
    }

    public Matrix Skip(int rows)
    {
      var n = Rows - rows;
      var m = new Matrix(n, Columns);

      for (var i = 0; i < n; i++) {
        m[i] = GetRow(rows + i);
      }

      return m;
    }

    public void ShuffleRows()
    {
      var n = Rows;
      while (n > 1) {
        n--;
        var k = Utility.RandomInt(n + 1);
        var row = _values[k];
        _values[k] = _values[n];
        _values[n] = row;
      }
    }
  }
}
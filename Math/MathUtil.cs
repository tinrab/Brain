namespace Brain.Math
{
  public static class MathUtil
  {
    public static int RoundToInt(double x)
    {
      return (int) System.Math.Round(x);
    }

    public static double Clamp(double x, double min, double max)
    {
      return x < min ? min : x > max ? max : x;
    }
  }
}
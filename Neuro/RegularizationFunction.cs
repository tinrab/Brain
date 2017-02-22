namespace Brain.Neuro
{
  public interface IRegularizationFunction
  {
    double Compute(double weight);
    double Derivative(double weight);
  }

  public static class RegularizationFunction
  {
    public static IRegularizationFunction L1 = new L1RegularizationFunction();
    public static IRegularizationFunction L2 = new L2RegularizationFunction();
  }

  internal class L1RegularizationFunction : IRegularizationFunction
  {
    public double Compute(double weight)
    {
      return System.Math.Abs(weight);
    }

    public double Derivative(double weight)
    {
      return weight < 0.0 ? -1.0 : 1.0;
    }
  }

  internal class L2RegularizationFunction : IRegularizationFunction
  {
    public double Compute(double weight)
    {
      return weight * weight * 0.5;
    }

    public double Derivative(double weight)
    {
      return weight;
    }
  }
}
namespace Brain.Neuro.Activations
{
  public class ReLUActivationFunction : IActivationFunction
  {
    public double Compute(double x)
    {
      return System.Math.Max(0.0, x);
    }

    public double Derivative(double x)
    {
      return x <= 0.0 ? 0.0 : 1.0;
    }
  }
}
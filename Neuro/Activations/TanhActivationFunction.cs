namespace Brain.Neuro.Activations
{
  public class TanhActivationFunction : IActivationFunction
  {
    public double Compute(double x)
    {
      return System.Math.Tanh(x);
    }

    public double Derivative(double x)
    {
      var y = Compute(x);
      return 1.0 - y * y;
    }
  }
}
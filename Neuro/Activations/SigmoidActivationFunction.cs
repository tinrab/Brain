namespace Brain.Neuro.Activations
{
  public class SigmoidActivationFunction : IActivationFunction
  {
    public double Compute(double x)
    {
      return 1.0 / (1.0 + System.Math.Exp(-x));
    }

    public double Derivative(double x)
    {
      var y = Compute(x);
      return y * (1.0 - y);
    }
  }
}
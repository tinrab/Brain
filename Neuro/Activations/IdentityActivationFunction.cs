namespace Brain.Neuro.Activations
{
  public class IdentityActivationFunction : IActivationFunction
  {
    public double Compute(double x)
    {
      return x;
    }

    public double Derivative(double x)
    {
      return 1.0;
    }
  }
}
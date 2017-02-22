namespace Brain.Neuro
{
  public interface IActivationFunction
  {
    double Compute(double x);
    double Derivative(double x);
  }
}
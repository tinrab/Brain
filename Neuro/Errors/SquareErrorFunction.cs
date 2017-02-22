namespace Brain.Neuro.Errors
{
  public class SquareErrorFunction : IErrorFunction
  {
    public double Error(double prediction, double actual)
    {
      var d = prediction - actual;
      return d * d * 0.5;
    }

    public double Derivative(double prediction, double actual)
    {
      return prediction - actual;
    }
  }
}
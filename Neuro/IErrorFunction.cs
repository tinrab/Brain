namespace Brain.Neuro
{
  public interface IErrorFunction
  {
    double Error(double prediction, double actual);
    double Derivative(double prediction, double actual);
  }
}
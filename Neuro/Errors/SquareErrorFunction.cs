namespace Brain.Neuro.Errors
{
	public class SquareErrorFunction : IErrorFunction
	{
		public double Error(double prediction, double actual)
		{
			return System.Math.Pow(prediction - actual, 2.0) * 0.5;
		}

		public double Derivative(double prediction, double actual)
		{
			return prediction - actual;
		}
	}
}
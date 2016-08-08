namespace Brain.NeuralNetwork
{
	public interface IErrorFunction
	{
		double Error(double prediction, double actual);
		double Derivative(double prediction, double actual);
	}

	public static class ErrorFunction
	{
		public static IErrorFunction Linear = new LinearErrorFunction();
		public static IErrorFunction Square = new SquareErrorFunction();
	}

	internal class LinearErrorFunction : IErrorFunction
	{
		public double Error(double prediction, double actual)
		{
			return actual - prediction;
		}

		public double Derivative(double prediction, double actual)
		{
			return 1.0;
		}
	}

	internal class SquareErrorFunction : IErrorFunction
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
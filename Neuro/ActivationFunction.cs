namespace Brain.Neuro
{
	public interface IActivationFunction
	{
		double Compute(double x);
		double Derivative(double x);
	}

	public static class ActivationFunction
	{
		public static IActivationFunction Tanh = new TanhActivationFunction();
		public static IActivationFunction ReLu = new ReLUActivationFunction();
		public static IActivationFunction Sigmoid = new SigmoidActivationFunction();
		public static IActivationFunction Linear = new LinearActivationFunction();
	}

	internal class TanhActivationFunction : IActivationFunction
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

	internal class ReLUActivationFunction : IActivationFunction
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

	internal class SigmoidActivationFunction : IActivationFunction
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

	internal class LinearActivationFunction : IActivationFunction
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
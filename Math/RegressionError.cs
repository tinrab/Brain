using Brain.Math;

namespace Brain.Math
{
	public static class RegressionError
	{
		/// <summary>
		///    Mean absolute error
		/// </summary>
		/// <param name="real">Real values</param>
		/// <param name="predicted">Predictions</param>
		/// <returns></returns>
		public static double MAE(Vector real, Vector predicted)
		{
			var sum = 0.0;

			for (var i = 0; i < real.Length; i++) {
				sum += System.Math.Abs(real[i] - predicted[i]);
			}

			return sum / real.Length;
		}

		/// <summary>
		///    Relative mean absolute error
		/// </summary>
		/// <param name="real"></param>
		/// <param name="predicted"></param>
		/// <returns></returns>
		public static double RMAE(Vector real, Vector predicted)
		{
			var realMean = real.Mean();
			var sum = 0.0;

			for (var i = 0; i < real.Length; i++) {
				sum += System.Math.Abs(real[i] - realMean);
			}

			return real.Length * MAE(real, predicted) / sum;
		}

		/// <summary>
		///    Mean squared error
		/// </summary>
		/// <param name="real"></param>
		/// <param name="predicted"></param>
		/// <returns></returns>
		public static double MSE(Vector real, Vector predicted)
		{
			var sum = 0.0;

			for (var i = 0; i < real.Length; i++) {
				sum += System.Math.Pow(real[i] - predicted[i], 2.0);
			}

			return sum / real.Length;
		}

		/// <summary>
		///    Relative mean squared error
		/// </summary>
		/// <param name="real"></param>
		/// <param name="predicted"></param>
		/// <returns></returns>
		public static double RMSE(Vector real, Vector predicted)
		{
			var realMean = real.Mean();
			var sum = 0.0;

			for (var i = 0; i < real.Length; i++) {
				sum += System.Math.Pow(real[i] - realMean, 2.0);
			}

			return real.Length * MSE(real, predicted) / sum;
		}
	}
}
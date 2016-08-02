using System;

namespace Brain
{
    public static class RegressionError
    {
        /// <summary>
        ///     Mean absolute error
        /// </summary>
        /// <param name="real">Real values</param>
        /// <param name="predicted">Predictions</param>
        /// <returns></returns>
        public static double MAE(double[] real, double[] predicted)
        {
            var sum = 0.0;

            for (var i = 0; i < real.Length; i++) {
                sum += Math.Abs(real[i] - predicted[i]);
            }

            return sum / real.Length;
        }

        /// <summary>
        ///     Relative mean absolute error
        /// </summary>
        /// <param name="real"></param>
        /// <param name="predicted"></param>
        /// <returns></returns>
        public static double RMAE(double[] real, double[] predicted)
        {
            var realAvg = Util.Average(real);
            var sum = 0.0;

            for (var i = 0; i < real.Length; i++) {
                sum += Math.Abs(real[i] - realAvg);
            }

            return real.Length * MAE(real, predicted) / sum;
        }

        /// <summary>
        ///     Mean squared error
        /// </summary>
        /// <param name="real"></param>
        /// <param name="predicted"></param>
        /// <returns></returns>
        public static double MSE(double[] real, double[] predicted)
        {
            var sum = 0.0;

            for (var i = 0; i < real.Length; i++) {
                sum += Math.Pow(real[i] - predicted[i], 2.0);
            }

            return sum / real.Length;
        }

        /// <summary>
        ///     Relative mean squared error
        /// </summary>
        /// <param name="real"></param>
        /// <param name="predicted"></param>
        /// <returns></returns>
        public static double RMSE(double[] real, double[] predicted)
        {
            var realAvg = Util.Average(real);
            var sum = 0.0;

            for (var i = 0; i < real.Length; i++) {
                sum += Math.Pow(real[i] - realAvg, 2.0);
            }

            return real.Length * MSE(real, predicted) / sum;
        }
    }
}
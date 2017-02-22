using System;

namespace Brain.Math
{
  public static class StatisticsUtil
  {
    public static double ClassificationAccuracy(int[] actual, int[] predictions)
    {
      if (actual.Length == 0 || actual.Length != predictions.Length) {
        throw new Exception("Invalid vector length");
      }

      var correct = 0;

      for (var i = 0; i < actual.Length; i++) {
        if (actual[i] == predictions[i]) {
          correct++;
        }
      }

      return correct / (double) actual.Length;
    }

    public static double Mae(Vector actual, Vector predicted)
    {
      var sum = 0.0;

      for (var i = 0; i < actual.Length; i++) {
        sum += System.Math.Abs(actual[i] - predicted[i]);
      }

      return sum / actual.Length;
    }

    public static double Rmae(Vector actual, Vector predicted)
    {
      var actualMean = actual.Mean;
      var sum = 0.0;

      for (var i = 0; i < actual.Length; i++) {
        sum += System.Math.Abs(actual[i] - actualMean);
      }

      return actual.Length * Mae(actual, predicted) / sum;
    }

    public static double Mse(Vector actual, Vector predicted)
    {
      var sum = 0.0;

      for (var i = 0; i < actual.Length; i++) {
        sum += System.Math.Pow(actual[i] - predicted[i], 2.0);
      }

      return sum / actual.Length;
    }

    public static double Rmse(Vector actual, Vector predicted)
    {
      var actualMean = actual.Mean;
      var sum = 0.0;

      for (var i = 0; i < actual.Length; i++) {
        sum += System.Math.Pow(actual[i] - actualMean, 2.0);
      }

      return actual.Length * Mse(actual, predicted) / sum;
    }
  }
}
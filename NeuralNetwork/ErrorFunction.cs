using System;

namespace Brain.NeuralNetwork
{
    public interface IErrorFunction
    {
        double Error(double x, double target);
        double Derivative(double x, double target);
    }

    public static class ErrorFunction
    {
        public static IErrorFunction Square = new SquareErrorFunction();
    }

    internal class SquareErrorFunction : IErrorFunction
    {
        public double Error(double x, double target)
        {
            return Math.Pow(x - target, 2.0) * 0.5;
        }

        public double Derivative(double x, double target)
        {
            return x - target;
        }
    }
}
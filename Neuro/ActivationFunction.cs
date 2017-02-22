using Brain.Neuro.Activations;

namespace Brain.Neuro
{
  public static class ActivationFunction
  {
    public static IActivationFunction Identity = new IdentityActivationFunction();
    public static IActivationFunction ReLU = new ReLUActivationFunction();
    public static IActivationFunction Sigmoid = new SigmoidActivationFunction();
    public static IActivationFunction Tanh = new TanhActivationFunction();
  }
}
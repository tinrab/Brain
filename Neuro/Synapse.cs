namespace Brain.Neuro
{
  public class Synapse
  {
    public Neuron Source { get; set; }
    public Neuron Destination { get; set; }
    public double Weight { get; set; }
    public double ErrorDerivative { get; set; }
    public double ErrorDerivativeSum { get; set; }
    public int DerivativeCount { get; set; }
    public IRegularizationFunction Regularization { get; set; }
  }
}
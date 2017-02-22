namespace Brain.Neuro
{
  public interface IParameterGenerator
  {
    double GenerateSynapseWeight();
    double GenerateNeuronBias();
  }
}
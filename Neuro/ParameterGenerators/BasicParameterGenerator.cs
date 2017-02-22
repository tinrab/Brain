namespace Brain.Neuro.ParameterGenerators
{
  public class BasicParameterGenerator : IParameterGenerator
  {
    public double GenerateSynapseWeight()
    {
      return Utility.RandomDouble() - 0.5;
    }

    public double GenerateNeuronBias()
    {
      return 0.1;
    }
  }
}
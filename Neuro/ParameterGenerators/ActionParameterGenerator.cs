namespace Brain.Neuro.ParameterGenerators
{
  public class ActionParameterGenerator : IParameterGenerator
  {
    public delegate T Func<T>();

    private readonly Func<double> _generateNeuronBias;
    private readonly Func<double> _generateSynapseWeight;

    public ActionParameterGenerator(Func<double> generateNeuronBias, Func<double> generateSynapseWeight)
    {
      _generateNeuronBias = generateNeuronBias;
      _generateSynapseWeight = generateSynapseWeight;
    }

    public double GenerateSynapseWeight()
    {
      return _generateSynapseWeight();
    }

    public double GenerateNeuronBias()
    {
      return _generateNeuronBias();
    }
  }
}